/* DrawForm.cs - the main GUI logic
 * 
 * Copyright (C) 2014 Alexander Kamyshnikov
 *
 * This file is part of CrowdSimulator.
 *
 * CrowdSimulator is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as
 * published by the Free Software Foundation; either version 2.1 of
 * the License, or (at your option) any later version.
 *
 * CrowdSimulator is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this program; if not, see <http://www.gnu.org/licenses/>.
 */

// FIXME: Get rid of hard-coded type numbers - make them named constants

//#define DEBUG_DRAW

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

using SigmaDC.Types;

namespace GdiPlusVisualizer
{
    public partial class DrawForm : Form
    {
        string m_currentDir;
        BuildingWrapper m_building = null;
        float m_scale = 1.0f;
        PointF m_panPoint;
        Dictionary<RectangleF, BoxWrapper> m_boxMap = null;
        RectangleF m_currentBoxExtents;
        RectangleF m_fixedBoxExtents;

        public DrawForm()
        {
            InitializeComponent();

            pbVisualizator.MouseWheel += this.pbVisualizator_MouseWheel;

            foreach ( ListViewItem item in lstDataFiles.Items )
            {
                item.SubItems.Add( "<None>" );
            }
        }

        static string PointFToString( PointF pnt )
        {
            if ( pnt.IsEmpty )
                return "<Empty point>";

            string pntString = "{ ";
            pntString += pnt.X.ToString( "F3" );
            pntString += "; ";
            pntString += pnt.Y.ToString( "F3" );
            pntString += " }";
            return pntString;
        }

        string RectFToString( RectangleF rect )
        {
            if ( rect.IsEmpty )
                return "<Empty rect>";

            string rectString = "{ ";
            rectString += "( ";
            rectString += rect.Left.ToString( "F3" );
            rectString += "; ";
            rectString += rect.Top.ToString( "F3" );
            rectString += " )";
            rectString += ", ";
            rectString += "( ";
            rectString += rect.Right.ToString( "F3" );
            rectString += "; ";
            rectString += rect.Bottom.ToString( "F3" );
            rectString += ") }";
            return rectString;
        }

        void pbVisualizator_MouseWheel( object sender, MouseEventArgs e )
        {
            float zoom = e.Delta > 0 ? 0.1f : -0.1f;
            m_scale += zoom;
            if ( Math.Abs( m_scale ) <= 0.1 )
                m_scale = 0.1f;

            lblScale.Text = "Scale: " + Math.Round( m_scale * 100 ) + "%";
            pbVisualizator.Refresh();
        }

        struct LineF : IComparable<LineF>
        {
            public float X1;
            public float Y1;
            public float X2;
            public float Y2;

            public LineF( float X1, float Y1, float X2, float Y2 )
            {
                this.X1 = X1;
                this.Y1 = Y1;
                this.X2 = X2;
                this.Y2 = Y2;
            }

            bool FloatsAreEqual( float f1, float f2, float accuracy = 0.00001f )
            {
                // Define the tolerance for variation in their values 
                float difference = Math.Abs( f1 * accuracy );
                return ( Math.Abs( f1 - f2 ) <= difference );
            }

            public int CompareTo( LineF other )
            {
                if ( FloatsAreEqual( X1, other.X1 ) && FloatsAreEqual( X2, other.X2 )
                  && FloatsAreEqual( Y1, other.Y1 ) && FloatsAreEqual( Y2, other.Y2 ) )
                    return 0;

                if ( ( X1 < other.X1 ) && ( X2 < other.X2 ) && ( Y1 < other.Y1 ) && ( Y2 < other.Y2 ) )
                    return -1;

                return 1;
            }
        }

        /// <summary>
        /// Scales the Graphics to fit a Control, given a domain of x,y values and side margins in pixels
        /// </summary>
        /// <param name="g">The Graphics object</param>
        /// <param name="control">The Control</param>
        /// <param name="domain">The value domain</param>
        /// <param name="margin">The margin</param>
        void ScaleGraphics( Graphics g, Control control, RectangleF domain, Margins margin )
        {
            // Find the drawable area in pixels (control-margins)
            int W = control.Width - margin.Left - margin.Right;
            int H = control.Height - margin.Bottom - margin.Top;

            // Ensure drawable area is at least 1 pixel wide
            W = Math.Max( 1, W );
            H = Math.Max( 1, H );

            // Find the origin (0,0) in pixels
            float OX = margin.Left - W * ( domain.Left / domain.Width );
            float OY = margin.Top + H * ( 1 + domain.Top / domain.Height );

            // Find the scale to fit the control
            float SX = W / domain.Width;
            float SY = H / domain.Height;

            // Transform the Graphics scene
            if ( m_panPoint.IsEmpty )
            {
                m_panPoint = new PointF( OX, OY );
                lblPan.Text = "Pan (device): " + PointFToString( m_panPoint );
            }

            g.TranslateTransform( m_panPoint.X, m_panPoint.Y, MatrixOrder.Append );
            g.ScaleTransform( SX * m_scale, -SY * m_scale );
        }

        void ScaleGraphics( Graphics g )
        {
            // Set margins inside the control client area in pixels
            var margin = new Margins( 16, 16, 16, 16 );

            // Set the domain of (x,y) values
            var range = m_building.Extents;

            // Make it smaller by 5%
            range.Inflate( 0.05f * range.Width, 0.05f * range.Height );

            // Scale graphics
            ScaleGraphics( g, pbVisualizator, range, margin );
        }

        private void DrawForm_Resize( object sender, EventArgs e )
        {
            Refresh();
        }

        private void pbVisualizator_Paint( object sender, PaintEventArgs e )
        {
            if ( m_building == null ) return;

            var g = e.Graphics;

            // FIXME: this "blurs" line
            // Smooth graphics output and scale
 //           g.SmoothingMode = SmoothingMode.HighQuality;
            g.SmoothingMode = SmoothingMode.None;
            ScaleGraphics( g );

#if DEBUG_DRAW
            // Draw arrow axes
            using ( var pen = new Pen( Color.Black, 0 ) )
            {
                pen.EndCap = LineCap.ArrowAnchor;
                g.DrawLine( pen, range.Left, 0.0f, range.Right, 0.0f );
                g.DrawLine( pen, 0.0f, range.Top, 0.0f, range.Bottom );
            }
            // draw bounding rectangle (on margin)
            using ( var pen = new Pen( Color.Brown, 0 ) )
            {
                pen.DashStyle = DashStyle.Dash;
                g.DrawRectangle( pen, range.X, range.Y, range.Width, range.Height );
            }
#endif
            m_building.Draw( g );

            // Highlight current box rectangle (the one under mouse cursor)
            if ( !m_currentBoxExtents.IsEmpty )
            {
                using ( var bluePen = new Pen( Color.Blue, 1.0f / g.DpiX ) )
                {
                    g.DrawRectangle( bluePen, m_currentBoxExtents.X, m_currentBoxExtents.Y, m_currentBoxExtents.Width, m_currentBoxExtents.Height );
                }
            }

            // Highlight fixed box rectangle (the one user clicks with the right mouse button)
            if ( !m_fixedBoxExtents.IsEmpty )
            {
                using ( var limePen = new Pen( Color.Lime, 2.0f / g.DpiX ) )
                {
                    g.DrawRectangle( limePen, m_fixedBoxExtents.X, m_fixedBoxExtents.Y, m_fixedBoxExtents.Width, m_fixedBoxExtents.Height );
                }
            }
        }

        private void pbVisualizator_MouseEnter( object sender, EventArgs e )
        {
            pbVisualizator.Focus();
        }

        private void pbVisualizator_MouseDown( object sender, MouseEventArgs e )
        {
            if ( m_building == null ) return;

            if ( e.Button == System.Windows.Forms.MouseButtons.Left )
            {
                m_panPoint = e.Location;

                using ( Graphics g = Graphics.FromHwnd( IntPtr.Zero ) )
                {
                    ScaleGraphics( g );

                    PointF[] pt = { m_panPoint };
                    g.TransformPoints( CoordinateSpace.World, CoordinateSpace.Device, pt );
                    lblPan.Text = "Pan (device): " + PointFToString( m_panPoint );
                }
            }
            else if ( e.Button == System.Windows.Forms.MouseButtons.Right )
            {
                // First click select ("fix") object, second - unselect
                if ( m_fixedBoxExtents.IsEmpty )
                    m_fixedBoxExtents = m_currentBoxExtents;
                else
                    m_fixedBoxExtents = RectangleF.Empty;
            }

            pbVisualizator.Refresh();
        }

        private void pbVisualizator_MouseMove( object sender, MouseEventArgs e )
        {
            if ( m_building == null ) return;

            // Convert mouse cursor coordinates (device) to world ones (Cartesian)
            // NOTE: We need separate Graphics object to do this; e.Graphics is valid only inside paint event handler
            PointF[] pt = { e.Location };
            using ( Graphics g = Graphics.FromHwnd( IntPtr.Zero ) )
            {
                ScaleGraphics( g );
    
                g.TransformPoints( CoordinateSpace.World, CoordinateSpace.Device, pt );
                lblCursorPos.Text = "Cursor position (world): " + PointFToString( pt[ 0 ] );
            }

            if ( m_currentBoxExtents.Contains( pt[ 0 ] ) || !m_fixedBoxExtents.IsEmpty )
                return;

            foreach ( var bm in m_boxMap )
            {
                if ( bm.Key.Contains( pt[ 0 ] ) )
                {
                    m_currentBoxExtents = bm.Key;

                    grdProps.SelectedObject = bm.Value;
                    grdProps.Refresh();

                    pbVisualizator.Refresh();
                    return;
                }
            }

            if ( ( grdProps.SelectedObject != null ) && m_fixedBoxExtents.IsEmpty )
            {
                grdProps.SelectedObject = null;
                m_currentBoxExtents = RectangleF.Empty;
                grdProps.Refresh();
                pbVisualizator.Refresh();
            }
        }

        private void mnuLoadData_Click( object sender, EventArgs e )
        {
            // Unload previous building data
            if ( m_building != null )
            {
                mnuUnloadData_Click( sender, e );
            }

            // FIXME: remove this after program release
            var absolutePath = Path.Combine( Directory.GetCurrentDirectory(), @"..\..\..\Data\KinderGarten" );
            dlgDataDir.SelectedPath = Path.GetFullPath( ( new Uri( absolutePath ) ).LocalPath );

            // Let user select data directory
            if ( dlgDataDir.ShowDialog() == DialogResult.OK )
            {
                // Get the last directory of the path
                m_currentDir = new DirectoryInfo( dlgDataDir.SelectedPath ).Name;
                this.Text = "Building schema: " + m_currentDir;

                // Load building data from the XML file
                m_building = new BuildingWrapper( dlgDataDir.SelectedPath );

                mnuVisualization.Enabled = true;
                mnuCmbCurrentFloor.Items.Clear();
                foreach ( var floor in m_building.Floors )
                    mnuCmbCurrentFloor.Items.Add( floor.Name );
                mnuCmbCurrentFloor.SelectedIndex = 0;

                lblBuildingExtent.Text = "Building extent (world): " + RectFToString( m_building.Extents );
                m_boxMap = m_building.CurrentFloor.GetBoxMap();

                // FIXME: implement property browsing of building, floors, rooms
                //mnuProperties.Enabled = true;

                // Updata data files load status
                lstDataFiles.Items[ 0 ].ImageIndex = System.IO.File.Exists( dlgDataDir.SelectedPath + Path.DirectorySeparatorChar + m_building.GetGeometryFileName() ) ? 0 : 1;
                lstDataFiles.Items[ 1 ].ImageIndex = System.IO.File.Exists( dlgDataDir.SelectedPath + Path.DirectorySeparatorChar + m_building.GetAperturesFileName() ) ? 0 : 1;
                lstDataFiles.Items[ 2 ].ImageIndex = System.IO.File.Exists( dlgDataDir.SelectedPath + Path.DirectorySeparatorChar + m_building.GetFurnitureFileName() ) ? 0 : 1;
                lstDataFiles.Items[ 3 ].ImageIndex = System.IO.File.Exists( dlgDataDir.SelectedPath + Path.DirectorySeparatorChar + m_building.GetPeopleFileName() ) ? 0 : 1;

                lstDataFiles.Items[ 0 ].SubItems[ 1 ].Text = m_building.GetGeometryFileName();
                lstDataFiles.Items[ 1 ].SubItems[ 1 ].Text = m_building.GetAperturesFileName();
                lstDataFiles.Items[ 2 ].SubItems[ 1 ].Text = m_building.GetFurnitureFileName();
                lstDataFiles.Items[ 3 ].SubItems[ 1 ].Text = m_building.GetPeopleFileName();

                lstDataFiles.Items[ 0 ].ToolTipText = dlgDataDir.SelectedPath + Path.DirectorySeparatorChar + m_building.GetGeometryFileName();
                lstDataFiles.Items[ 1 ].ToolTipText = dlgDataDir.SelectedPath + Path.DirectorySeparatorChar + m_building.GetAperturesFileName();
                lstDataFiles.Items[ 2 ].ToolTipText = dlgDataDir.SelectedPath + Path.DirectorySeparatorChar + m_building.GetFurnitureFileName();
                lstDataFiles.Items[ 3 ].ToolTipText = dlgDataDir.SelectedPath + Path.DirectorySeparatorChar + m_building.GetPeopleFileName();
            }
        }

        private void mnuReload_Click( object sender, EventArgs e )
        {
            // FIXME: implement
            throw new NotImplementedException();
        }

        private void mnuUnloadData_Click( object sender, EventArgs e )
        {
            // FIXME: implement
            throw new NotImplementedException();
        }

        private void mnuExit_Click( object sender, EventArgs e )
        {
            this.Close();
        }

        private void mnuCmbCurrentFloor_SelectedIndexChanged( object sender, EventArgs e )
        {
            int newFloorNumber = mnuCmbCurrentFloor.SelectedIndex + 1;
            System.Diagnostics.Debug.Assert( newFloorNumber >= m_building.FirstFloorNumber && newFloorNumber <= m_building.FloorCount );

            m_building.CurrentFloorNumber = newFloorNumber;
            m_panPoint = new PointF();
            m_boxMap = m_building.CurrentFloor.GetBoxMap();
            m_currentBoxExtents = RectangleF.Empty;
            m_fixedBoxExtents = RectangleF.Empty;

            lblFloor.Text = "Floor number: " + newFloorNumber.ToString();
            pbVisualizator.Refresh();
        }
    }
}
