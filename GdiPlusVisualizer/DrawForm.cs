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
        PointF m_panPoint, m_panPointOrig;
        Dictionary<RectangleF, BoxWrapper> m_boxMap = null;
        RectangleF m_currentBoxExtents;
        RectangleF m_fixedBoxExtents;
        bool m_keepAspectRatio = false;
        bool m_drawBuilding = true;
        bool m_drawFurniture = true;
        bool m_drawPeople = true;
        bool m_drawGrid = false;
        bool m_imageIsPanning = false;
        float m_a = 0.1f;
        bool m_highlightBoxes = false;
        MathModel.DistanceField m_distField = null;
        MathModel.DistanceField.DrawMode m_fieldVisMode = MathModel.DistanceField.DrawMode.None;
        double[ , ] m_S = null;

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
            float SX, SY;
            if ( !m_keepAspectRatio )
            {
                // Canvas space will be fully used, but circle will looks like ellipse
                SX = W / domain.Width;
                SY = H / domain.Height;
            }
            else
            {
                // Not effective space usage, but picture looks like it should be
                SX = Math.Min( W / domain.Width, H / domain.Height );
                SY = SX;
            }

            // Transform the Graphics scene
            if ( m_panPoint.IsEmpty )
            {
                m_panPoint = new PointF( OX, OY );
                lblPan.Text = "Pan (device): " + PointFToString( m_panPoint );

                m_panPointOrig = m_panPoint;
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

        void DrawCellularGrid( Graphics g )
        {
            float w = m_building.Extents.Width;
            float h = m_building.Extents.Height;

            int M = 0;
            int N = 0;

            float x0 = m_building.Extents.Left - m_a/2;
            float y0 = m_building.Extents.Top - m_a/2;

            float x = x0;
            while ( x <= m_building.Extents.Right + m_a )
            {
                M++;
                x += m_a;
            }

            float y = y0;
            while ( y <= m_building.Extents.Bottom + m_a )
            {
                N++;
                y += m_a;
            }

            if ( m_S == null )
            {
                m_distField = new MathModel.DistanceField( m_building, m_a, M, N, x0, y0 );
                m_S = m_distField.CalcDistanceField();
            }
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

            if ( m_drawBuilding )
            {
                Dictionary<string, object> options = new Dictionary<string, object>();
                options[ "drawFurniture" ] = m_drawFurniture;
                options[ "drawPeople" ] = m_drawPeople;
                m_building.SetDrawOptions( options );
                m_building.Draw( g );
            }

            if ( m_highlightBoxes )
            {
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

            if ( m_drawGrid ) DrawCellularGrid( g );
            if ( m_fieldVisMode != MathModel.DistanceField.DrawMode.None ) m_distField.Visualize( g );
        }

        private void pbVisualizator_MouseEnter( object sender, EventArgs e )
        {
            pbVisualizator.Focus();
        }

        private void pbVisualizator_MouseDown( object sender, MouseEventArgs e )
        {
            if ( m_building == null ) return;

            if ( e.Button == MouseButtons.Left )
            {
                m_imageIsPanning = true;
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

        private void pbVisualizator_MouseUp( object sender, MouseEventArgs e )
        {
            if ( m_imageIsPanning )
            {
                m_imageIsPanning = false;
            }
        }

        private void pbVisualizator_MouseMove( object sender, MouseEventArgs e )
        {
            if ( m_building == null ) return;

            if ( m_drawGrid )
            {
                float x0 = m_building.Extents.Left - m_a/2;
                float y0 = m_building.Extents.Top - m_a/2;
                float xMax = m_building.Extents.Right + m_a;
                float yMax = m_building.Extents.Bottom + m_a;
                System.Diagnostics.Debug.Assert( m_building.Extents.Bottom > m_building.Extents.Top );
                float x;
                float y;
                using ( Graphics g = Graphics.FromHwnd( IntPtr.Zero ) )
                {
                    ScaleGraphics( g );

                    PointF[] ptDevice = { e.Location };
                    g.TransformPoints( CoordinateSpace.World, CoordinateSpace.Device, ptDevice );
//                    this.Text = PointFToString( ptDevice[ 0 ] );

                    PointF ptWorld = ptDevice[ 0 ];
                    x = ptWorld.X;
                    y = ptWorld.Y;
                }

                if ( x < x0 || y < y0 || x > xMax || y > yMax )
                {
                    lblCurrentCell.Text = "Current cell: <unknown>";
                }
                else
                {
                    int i = ( int )( ( x - x0 ) / m_a );
                    int j = ( int )( ( y - y0 ) / m_a );
                    // FIXME: why i and j are transposed?
                    lblCurrentCell.Text = "Current cell: ( " + j + ", " + i + " )";

                    if(m_S != null)
                    {
                        lblCurrentCell.Text += ", S[ " + j + ", " + i + " ] = " + m_S[ i, j ].ToString( "f3" );
                    }
                }
            }

            if ( m_imageIsPanning )
            {
                // NOTE: device coords
                m_panPoint = e.Location;
                pbVisualizator.Refresh();

                lblPan.Text = "Pan (device): " + PointFToString( m_panPoint );
            }

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

            if ( m_highlightBoxes )
            {
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

            // FIXME: uncomment
            // Let user select data directory
//            if ( dlgDataDir.ShowDialog() == DialogResult.OK )
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

                // NOTE: cellular grid step should be smaller than the smallest human diameter, i.e. a <= min( d )
                float minHumanDiameter = m_building.MinHumanDiameter;
                m_a = minHumanDiameter * 0.30f; // Default: 0.1 meter, i.e. 10 cm
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

        private void mnuVisualizationKeepAspectRatio_Click( object sender, EventArgs e )
        {
            m_keepAspectRatio = !m_keepAspectRatio;
            pbVisualizator.Refresh();
        }

        private void mnuVisualizationDrawBulding_Click( object sender, EventArgs e )
        {
            m_drawBuilding = !m_drawBuilding;
            pbVisualizator.Refresh();
        }

        private void mnuVisualizationDrawCellularGrid_Click( object sender, EventArgs e )
        {
            m_drawGrid = !m_drawGrid;
            lblCurrentCell.Visible = m_drawGrid;
            pbVisualizator.Refresh();
        }

        private void DrawForm_KeyPress( object sender, KeyPressEventArgs e )
        {
            const float zoomConst = 0.1f;
            const int moveConst = 20;
            switch ( char.ToLower( e.KeyChar ) )
            {
                case 'w':
                {
                    m_panPoint.Y -= moveConst;
                    break;
                }
                case 's':
                {
                    m_panPoint.Y += moveConst;
                    break;
                }
                case 'a':
                {
                    m_panPoint.X -= moveConst;
                    break;
                }
                case 'd':
                {
                    m_panPoint.X += moveConst;
                    break;
                }
                case 'n':
                {
                    m_panPoint = m_panPointOrig;
                    m_scale = 1;
                    break;
                }
                case '+':
                {
                    m_scale += zoomConst;
                    if ( Math.Abs( m_scale ) <= 0.1 ) m_scale = 0.1f;
                    break;
                }
                case '-':
                {
                    m_scale -= zoomConst;
                    if ( Math.Abs( m_scale ) <= 0.1 ) m_scale = 0.1f;
                    break;
                }
                case 'h':
                {
                    m_highlightBoxes = !m_highlightBoxes;
                    break;
                }
                case 'b':
                {
                    m_drawBuilding = !m_drawBuilding;
                    break;
                }
                case 'f':
                {
                    m_drawFurniture = !m_drawFurniture;
                    break;
                }
                case 'p':
                {
                    m_drawPeople = !m_drawPeople;
                    break;
                }
                case 'g':
                {
                    m_drawGrid = !m_drawGrid;
                    lblCurrentCell.Visible = m_drawGrid;
                    break;
                }
                case 'm':   // mode
                {
                    if( m_drawGrid )
                    {
                        m_fieldVisMode = ( MathModel.DistanceField.DrawMode )( ( int )m_fieldVisMode + 1 );
                        if ( ( int )m_fieldVisMode > ( int )MathModel.DistanceField.DrawMode.Count )
                        {
                            m_fieldVisMode = MathModel.DistanceField.DrawMode.None;
                        }
                        m_distField.SetDrawMode( m_fieldVisMode );
                    }
                    break;
                }
            }

            if ( e.KeyChar == 'w' || e.KeyChar == 's' || e.KeyChar == 'a' || e.KeyChar == 'd' || e.KeyChar == 'n' )
            {
                lblPan.Text = "Pan (device): " + PointFToString( m_panPoint );
                pbVisualizator.Refresh();
            }

            if ( e.KeyChar == '+' || e.KeyChar == '-' )
            {
                lblScale.Text = "Scale: " + Math.Round( m_scale * 100 ) + "%";
                pbVisualizator.Refresh();
            }

            if ( e.KeyChar == 'h' )
            {
                pbVisualizator.Refresh();
            }

            if ( e.KeyChar == 'b' || e.KeyChar == 'f' || e.KeyChar == 'p' )
            {
                pbVisualizator.Refresh();
            }

            if ( e.KeyChar == 'g' || e.KeyChar == 'm' )
            {
                pbVisualizator.Refresh();
            }
        }

        private void DrawForm_Load( object sender, EventArgs e )
        {
            // FIXME: remove
            mnuLoadData_Click( sender, e );
        }     
    }
}

// --------------------------------------------------------------------------------------------------------------------

namespace MathModel
{
    public class DistanceField
    {
        BuildingWrapper m_building = null;
        float m_a = 0.1f;
        int m_M = -1;
        int m_N = -1;
        float m_x0 = float.NaN;
        float m_y0 = float.NaN;
        DrawMode m_drawMode = DrawMode.None;
        int[ , ] m_G = null;
        double[ , ] m_S = null;

        public enum DrawMode { None = 0, Raw, G, S, Count };

        public DistanceField( BuildingWrapper building, float a, int M, int N, float x0, float y0, DrawMode dm = DrawMode.None )
        {
            m_building = building;
            m_a = a;
            m_M = M;
            m_N = N;
            m_x0 = x0;
            m_y0 = y0;
            m_drawMode = dm;
        }

        static RectangleF NormalizeRect( RectangleF src, float a )
        {
            var rect = src;
            // TODO: what is the correct inflate constant?
            if ( rect.Width < a ) rect.Inflate( a / 8, 0 );
            if ( rect.Height < a ) rect.Inflate( 0, a / 8 );
            return rect;
        }

        bool IntersectBox( RectangleF cellRect )
        {
            foreach ( var box in m_building.CurrentFloor.Geometry )
            {
                bool b = ( !box.Extents.IsEmpty && ( box.Extents.Top < box.Extents.Bottom ) );
                if ( !b ) System.Diagnostics.Debugger.Break();

                // We are interesting in those cases to check intersection presents:
                // TODO: check whether we need to detect tangent to obstacles cells
                // 1) Cell rectangle is tangent to the box one and entirely contained in it
                // 2) Cell rectange is tangent to the box one, but from the outer face of it
                /*const float EPS = 0.000001f;
                if ( MathUtils.NearlyEqual( box.Extents.Left, cellRect.Left, EPS )
                        || MathUtils.NearlyEqual( box.Extents.Right, cellRect.Right, EPS )
                        || MathUtils.NearlyEqual( box.Extents.Top, cellRect.Top, EPS )
                        || MathUtils.NearlyEqual( box.Extents.Bottom, cellRect.Bottom, EPS ) )
                    return true;*/

                // 3) Cell rectangle intersect one of the rectangle sides
                var rect = RectangleF.Intersect( box.Extents, cellRect );
                if ( ( rect.Width > 0 || rect.Height > 0 ) && !box.Extents.Contains( cellRect ) )
                {
                    return true;
                }
            }

            return false;
        }

        bool IntersectFurniture( RectangleF cellRect )
        {
            foreach ( var furn in m_building.CurrentFloor.Furniture )
            {
                bool b = ( !furn.Extents.IsEmpty && ( furn.Extents.Top < furn.Extents.Bottom ) );
                if ( !b ) System.Diagnostics.Debugger.Break();

                var rect = RectangleF.Intersect( furn.Extents, cellRect );
                if ( ( rect.Width > 0 || rect.Height > 0 ) )
                {
                    return true;
                }
            }

            return false;
        }

        bool IntersectStairway( RectangleF cellRect )
        {
            foreach ( var st in m_building.CurrentFloor.Stairways )
            {
                bool b = ( !st.Extents.IsEmpty && ( st.Extents.Top < st.Extents.Bottom ) );
                if ( !b ) System.Diagnostics.Debugger.Break();

                var rect = RectangleF.Intersect( st.Extents, cellRect );
                if ( ( rect.Width > 0 || rect.Height > 0 ) && !st.Extents.Contains( cellRect ) )
                {
                    return true;
                }
            }

            return false;
        }

        bool IntersectFakeAperture( RectangleF cellRect )
        {
            foreach ( var aper in m_building.CurrentFloor.FakeApertures )
            {
                bool b = ( ( aper.Extents.Width > 0 || aper.Extents.Height > 0 ) && ( aper.Extents.Top <= aper.Extents.Bottom ) );
                if ( !b ) System.Diagnostics.Debugger.Break();

                var rect = RectangleF.Intersect( aper.Extents, cellRect );
                if ( ( rect.Width > 0 || rect.Height > 0 ) && !aper.Extents.Contains( cellRect ) )
                {
                    if ( MathUtils.NearlyZero( rect.Height ) )
                    {
                        if ( rect.Left == aper.Extents.Left ) continue;
                        if ( rect.Right == aper.Extents.Right ) continue;
                    }

                    if ( MathUtils.NearlyZero( rect.Width ) )
                    {
                        if ( rect.Top == aper.Extents.Top ) continue;
                        if ( rect.Bottom == aper.Extents.Bottom ) continue;
                    }

                    return true;
                }
            }

            return false;
        }

        bool IntersectExitDoor( RectangleF cellRect, out int exitId )
        {
            exitId = -2;

            for ( int k = 1; k <= m_building.CurrentFloor.Exits.Count; ++k )
            {
                var ext = NormalizeRect( m_building.CurrentFloor.Exits[ k - 1 ].Extents, m_a );
                var rect = RectangleF.Intersect( ext, cellRect );
                if ( !rect.IsEmpty && !ext.Contains( cellRect ) )
                {
                    exitId = k;
                    return true;
                }
            }

            return false;
        }

        bool IntersectInnerDoor( RectangleF cellRect )
        {
            foreach ( var aper in m_building.CurrentFloor.Doors )
            {
                bool b = ( ( aper.Extents.Width > 0 || aper.Extents.Height > 0 ) && ( aper.Extents.Top <= aper.Extents.Bottom ) );
                if ( !b ) System.Diagnostics.Debugger.Break();

                var rect = RectangleF.Intersect( aper.Extents, cellRect );
                if ( ( rect.Width > 0 || rect.Height > 0 ) && !aper.Extents.Contains( cellRect ) )
                {
                    if ( MathUtils.NearlyZero( rect.Height ) )
                    {
                        if ( rect.Left == aper.Extents.Left ) continue;
                        if ( rect.Right == aper.Extents.Right ) continue;
                    }

                    if ( MathUtils.NearlyZero( rect.Width ) )
                    {
                        if ( rect.Top == aper.Extents.Top ) continue;
                        if ( rect.Bottom == aper.Extents.Bottom ) continue;
                    }

                    return true;
                }
            }

            return false;
        }

        bool IntersectWindow( RectangleF cellRect )
        {
            foreach ( var aper in m_building.CurrentFloor.Windows )
            {
                bool b = ( ( aper.Extents.Width > 0 || aper.Extents.Height > 0 ) && ( aper.Extents.Top <= aper.Extents.Bottom ) );
                if ( !b ) System.Diagnostics.Debugger.Break();

                var rect = RectangleF.Intersect( aper.Extents, cellRect );
                if ( ( rect.Width > 0 || rect.Height > 0 ) && !aper.Extents.Contains( cellRect ) )
                {
                    return true;
                }
            }

            return false;
        }

        public double[,] CalcDistanceField()
        {
            var G = new int[ m_M, m_N ];
            for ( int i = 0; i < m_M; ++i )
            {
                for ( int j = 0; j < m_N; ++j )
                {
                    G[ i, j ] = -1; // "empty" cell

                    // Calculate cell rectangle
                    float cellX = m_x0 + m_a * i;
                    float cellY = m_y0 + m_a * j;
                    RectangleF cellRect = new RectangleF( cellX, cellY, m_a, m_a );

                    // Check whether G[i,j] intersects with an building exit aperture/exit aperture part
                    int exitId;
                    if ( IntersectExitDoor( cellRect, out exitId ) )
                    {
                        G[ i, j ] = exitId;
                        continue;
                    }

                    // Check whether G[i,j] intersects with an door aperture/exit aperture part
                    if( IntersectInnerDoor( cellRect ) )
                    {
                        continue;
                    }

                    // Check whether G[i,j] intersects with a wall/wall part
                    if ( IntersectBox( cellRect ) )
                    {
                        if ( !IntersectFakeAperture( cellRect ) )
                        {
                            G[ i, j ] = 0;
                            continue;
                        }
                    }

                    // Check whether G[i,j] intersects with a stairway part
                    if ( IntersectStairway( cellRect ) )
                    {
                        if ( !IntersectFakeAperture( cellRect ) )
                        {
                            G[ i, j ] = 0;
                            continue;
                        }
                    }

                    // Check whether G[i,j] intersects with furniture/furniture part
                    if ( IntersectFurniture( cellRect ) )
                    {
                        G[ i, j ] = 0;
                        continue;
                    }

                    // Check whether G[i,j] intersects with a window/window part
                    if ( IntersectWindow( cellRect ) )
                    {
                        G[ i, j ] = 0;
                        continue;
                    }
                }
            }

            m_G = G;

            int nullCellsCount = 0;
            var S = new double[ m_M, m_N ];
            // Init distance field
            for ( int i = 0; i < m_M; ++i )
            {
                for ( int j = 0; j < m_N; ++j )
                {
                    float cellX = m_x0 + m_a * i;
                    float cellY = m_y0 + m_a * j;

                    if ( G[ i, j ] == 0 )   // obstacle
                    {
                        S[ i, j ] = m_M * m_N;
                    }
                    else if ( G[ i, j ] > 0 ) // exit
                    {
                        S[ i, j ] = 1;
                    }
                    else
                    {
                        S[ i, j ] = 0;
                        nullCellsCount++;
                    }
                }
            }

            double sqrt_2 = Math.Sqrt( 2 );
            double sqrt_5 = Math.Sqrt( 5 );

            // Field traversal
            while ( nullCellsCount >= 1 )
            {
                for ( int i = 0; i < m_M; ++i )
                {
                    for ( int j = 0; j < m_N; ++j )
                    {
                        if ( S[ i, j ] != 0 ) continue;

                        var stepValues = new List<double>();

                        //                  ( i-2, j-1 )
                        //                       ||
                        // ( i-1, j-2 ) <== ( i-1, j-1 )
                        if ( ( i >= 1 && j >= 1 ) && ( S[ i - 1, j - 1 ] != m_M * m_N ) )
                        {
                            if ( ( i >= 1 && j >= 1 ) && ( S[ i - 1, j - 1 ] != 0 ) )
                            {
                                stepValues.Add( sqrt_2 + S[ i - 1, j - 1 ] );
                            }

                            if ( ( i >= 1 && j >= 2 ) && S[ i - 1, j - 2 ] != 0 && S[ i - 1, j - 2 ] != m_M * m_N )
                            {
                                stepValues.Add( sqrt_5 + S[ i - 1, j - 2 ] );
                            }

                            if ( ( i >= 2 && j >= 1 ) && S[ i - 2, j - 1 ] != 0 && S[ i - 2, j - 1 ] != m_M * m_N )
                            {
                                stepValues.Add( sqrt_5 + S[ i - 2, j - 1 ] );
                            }
                        }

                        // ( i-2, j+1 )
                        //      ||
                        // ( i-1, j+1 ) ==> ( i-1, j+2 )
                        if ( ( i >= 1 && j <= m_N - 2 ) && ( S[ i - 1, j + 1 ] != m_M * m_N ) )
                        {
                            if ( ( i >= 1 && j <= m_N - 2 ) && ( S[ i - 1, j + 1 ] != 0 ) )
                            {
                                stepValues.Add( sqrt_2 + S[ i - 1, j + 1 ] );
                            }

                            if ( ( i >= 1 && j <= m_N - 3 ) && S[ i - 1, j + 2 ] != 0 && S[ i - 1, j + 2 ] != m_M * m_N )
                            {
                                stepValues.Add( sqrt_5 + S[ i - 1, j + 2 ] );
                            }

                            if ( ( i >= 2 && j <= m_N - 2 ) && S[ i - 2, j + 1 ] != 0 && S[ i - 2, j + 1 ] != m_M * m_N )
                            {
                                stepValues.Add( sqrt_5 + S[ i - 2, j + 1 ] );
                            }
                        }

                        // ( i+1, j+1 ) ==> ( i+1, j+2 )
                        //      ||
                        // ( i+2, j+1 )
                        if ( ( i <= m_M - 2 && j <= m_N - 2 ) && ( S[ i + 1, j + 1 ] != m_M * m_N ) )
                        {
                            if ( ( i <= m_M - 2 && j <= m_N - 2 ) && ( S[ i + 1, j + 1 ] != 0 ) )
                            {
                                stepValues.Add( sqrt_2 + S[ i + 1, j + 1 ] );
                            }

                            if ( ( i <= m_M - 2 && j <= m_N - 3 ) && S[ i + 1, j + 2 ] != 0 && S[ i + 1, j + 2 ] != m_M * m_N )
                            {
                                stepValues.Add( sqrt_5 + S[ i + 1, j + 2 ] );
                            }

                            if ( ( i <= m_M - 3 && j <= m_N - 2 ) && S[ i + 2, j + 1 ] != 0 && S[ i + 2, j + 1 ] != m_M * m_N )
                            {
                                stepValues.Add( sqrt_5 + S[ i + 2, j + 1 ] );
                            }
                        }

                        // ( i+1, j-2 ) ==> ( i+1, j-1 )
                        //                       ||
                        //                  ( i+2, j-1 )
                        if ( ( i <= m_M - 2 && j >= 1 ) && ( S[ i + 1, j - 1 ] != m_M * m_N ) )
                        {
                            if ( ( i <= m_M - 2 && j >= 1 ) && ( S[ i + 1, j - 1 ] != 0 ) )
                            {
                                stepValues.Add( sqrt_2 + S[ i + 1, j - 1 ] );
                            }

                            if ( ( i <= m_M - 2 && j >= 2 ) && S[ i + 1, j - 2 ] != 0 && S[ i + 1, j - 2 ] != m_M * m_N )
                            {
                                stepValues.Add( sqrt_5 + S[ i + 1, j - 2 ] );
                            }

                            if ( ( i <= m_M - 3 && j >= 1 ) && S[ i + 2, j - 1 ] != 0 && S[ i + 2, j - 1 ] != m_M * m_N )
                            {
                                stepValues.Add( sqrt_5 + S[ i + 2, j - 1 ] );
                            }
                        }

                        // ( i-1, j )
                        if ( ( i >= 1 ) && S[ i - 1, j ] != 0 && S[ i - 1, j ] != m_M * m_N )
                        {
                            stepValues.Add( 1 + S[ i - 1, j ] );
                        }

                        // ( i, j+1 )
                        if ( ( j <= m_N - 2 ) && S[ i, j + 1 ] != 0 && S[ i, j + 1 ] != m_M * m_N )
                        {
                            stepValues.Add( 1 + S[ i, j + 1 ] );
                        }

                        // ( i+1, j )
                        if ( ( i <= m_M - 2 ) && S[ i + 1, j ] != 0 && S[ i + 1, j ] != m_M * m_N )
                        {
                            stepValues.Add( 1 + S[ i + 1, j ] );
                        }

                        // ( i, j-1 )
                        if ( ( j >= 1 ) && S[ i, j - 1 ] != 0 && S[ i, j - 1 ] != m_M * m_N )
                        {
                            stepValues.Add( 1 + S[ i, j - 1 ] );
                        }

                        // Assign to (i,j) cell the minimum distance value
                        if ( stepValues.Count >= 1 )
                        {
                            double minStep = stepValues.Min<double>();
                            System.Diagnostics.Debug.Assert( minStep > 0 );

                            S[ i, j ] = minStep;
                            nullCellsCount--;
                        }
                    }
                }
            }

            m_S = S;
            return S;
        }

        public void SetDrawMode( DrawMode dm )
        {
            m_drawMode = dm;
        }

        public void Visualize( Graphics g )
        {
            // Field visualization
            double maxDistValue = double.NegativeInfinity;
            double minDistValue = double.PositiveInfinity;
            for ( int i = 0; i < m_M; ++i )
            {
                for ( int j = 0; j < m_N; ++j )
                {
                    if ( m_S[ i, j ] >= m_M * m_N ) continue;

                    if ( maxDistValue < m_S[ i, j ] ) maxDistValue = m_S[ i, j ];
                    if ( minDistValue > m_S[ i, j ] ) minDistValue = m_S[ i, j ];
                }
            }

            for ( int i = 0; i < m_M; ++i )
            {
                for ( int j = 0; j < m_N; ++j )
                {
                    float cellX = m_x0 + m_a * i;
                    float cellY = m_y0 + m_a * j;

                    switch ( m_drawMode )
                    {
                        case DrawMode.Raw:
                        {
                            using ( var p = new Pen( Color.LightSteelBlue, 0.5f / g.DpiX ) )
                            {
                                g.DrawRectangle( p, cellX, cellY, m_a, m_a );
                            }
                            break;
                        }
                        case DrawMode.G:
                        {
                            if ( m_G[ i, j ] == 0 )
                            {
                                using ( var p = new Pen( Color.Red, 1.0f / g.DpiX ) )
                                {
                                    g.DrawRectangle( p, cellX, cellY, m_a, m_a );
                                }
                            }
                            else if ( m_G[ i, j ] > 0 ) // exit
                            {
                                using ( var p = new Pen( Color.Blue, 1.0f / g.DpiX ) )
                                {
                                    g.DrawRectangle( p, cellX, cellY, m_a, m_a );
                                }
                            }
                            break;
                        }
                        case DrawMode.S:
                        {
                            if ( m_S[ i, j ] >= m_M * m_N ) continue;

                            float coeff = 1 - ( float )( m_S[ i, j ] / maxDistValue );
                            Color c1 = Color.Red;
                            Color t = Color.FromArgb( c1.A, ( int )( c1.R * coeff ), ( int )( c1.G * coeff ), ( int )( c1.B * coeff ) );

                            using ( var b = new SolidBrush( t ) )
                            {
                                g.FillRectangle( b, cellX, cellY, m_a, m_a );
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}
