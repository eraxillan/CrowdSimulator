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
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using SigmaDC.Common.MathEx;
using SigmaDC.Interfaces;
using SigmaDC.MathModel;
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
        IHuman m_currentHuman;
        bool m_keepAspectRatio = false;
        bool m_drawBuilding = true;
        bool m_drawFurniture = true;
        bool m_drawPeople = true;
        bool m_drawGrid = false;
        bool m_imageIsPanning = false;
        float m_a = 0.1f;
        bool m_highlightBoxes = false;
        DistanceField m_distField = null;
        DistanceField.DrawMode m_fieldVisMode = DistanceField.DrawMode.None;
        double[][] m_S = null;
        List<HumanRuntimeInfo> m_humanRuntimeData = new List<HumanRuntimeInfo>();

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

        void InitDistanceField()
        {
            float w = m_building.Extents.Width;
            float h = m_building.Extents.Height;

            int M = 0;
            int N = 0;

            float x0 = m_building.Extents.Left - m_a/2;
            float y0 = m_building.Extents.Top - m_a/2;

            float x = x0;
            while ( x < m_building.Extents.Right )
            {
                M++;
                x += m_a;
            }

            float y = y0;
            while ( y < m_building.Extents.Bottom )
            {
                N++;
                y += m_a;
            }

            if ( m_S == null )
            {
                m_distField = new DistanceField( m_building, m_a, M, N, x0, y0 );

               /* this.UseWaitCursor = true;
                this.Enabled = false;

                Stopwatch stopwatch = Stopwatch.StartNew();

                m_S = m_distField.CalcDistanceField();

                TimeSpan ts = stopwatch.Elapsed;

                // Format and display the TimeSpan value. 
                string elapsedTime = String.Format( "{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10 );
                MessageBox.Show( "Distance field calculation took: " + elapsedTime );

                this.Enabled = true;
                this.UseWaitCursor = false;*/

                var model = new SigmaDCModel();
               // var hi = new HumanRuntimeInfo(h);
                model.SetupParameters( new Dictionary<string, object>() { { "r", 1.0f }, { "w", 0.0f }, { "deltaD", 0.0f }, { "kw", 4.0f }, { "kp", 1.0f }, { "ks", 1.0f } } );

                var obstacleExtents = new List<RectangleF>();
                foreach ( var box in m_building.CurrentFloor.Geometry )
                {
                    obstacleExtents.Add( box.Extents );
                }
                foreach ( var furn in m_building.CurrentFloor.Furniture )
                {
                    obstacleExtents.Add( furn.Extents );
                }
                // FIXME: window has null height, but should be considered as obstacle
                /*foreach ( var window in m_building.CurrentFloor.Windows )
                {
                    obstacleExtents.Add( window.Extents );
                }*/
                // TODO: smth else can be considered as obstacle?
                model.SetupObstacles( obstacleExtents );

                model.SetupPeople( m_building.CurrentFloor.People );

                model.NextStepAll( null, ref m_humanRuntimeData );

                /*var human = new Human();
                human.projectionCenter = new SigmaDC.Common.MathEx.Vector2( 1.0f, 3.0f );
                human.projectionDiameter = 2.0f;
                model.NextStep( human, m_distField, hi );*/
                return;
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

                // Highlight current human extent
                if ( m_currentHuman != null )
                {
                    using ( var blueBrush = new SolidBrush( Color.Blue ) )
                    {
                        g.FillEllipse( blueBrush, m_currentHuman.ProjectionExtent.X, m_currentHuman.ProjectionExtent.Y, m_currentHuman.ProjectionExtent.Width, m_currentHuman.ProjectionExtent.Height );
                    }
                }
            }

            if ( m_fieldVisMode != DistanceField.DrawMode.None ) m_distField.Visualize( g );

            if ( m_currentHuman == null ) return;
            foreach ( var hrt in m_humanRuntimeData )
            {
                if ( hrt.Id != m_currentHuman.Id ) continue;

                for ( int i = 0; i < hrt.RotateAngles.Count; ++i )
                {
                    var rect = hrt.VisibilityAreas[ i ];
                    var rectRot = rect.Rotate( rect.RotationCenter, rect.RotationAngle );
//                    Trace.Assert( rectRot.LeftTop.Y >= rectRot.LeftBottom.Y && rectRot.RightTop.Y >= rectRot.RightBottom.Y );
//                    Trace.Assert( rectRot.RightBottom.X >= rectRot.LeftBottom.X && rectRot.RightTop.X >= rectRot.LeftTop.X );

                    var rectVis = new SdcRectangle( rect );

                    rectVis.A = new Vector2( rectVis.C.X + hrt.MinDistToObstacle[ i ], rectVis.C.Y );
                    rectVis.B = new Vector2( rectVis.D.X + hrt.MinDistToObstacle[ i ], rectVis.D.Y );
                    var rectVisRot = rectVis.Rotate( hrt.ProjectionCenter, hrt.RotateAngles[ i ] );

                    // FIXME: add IVisualisable support to SdcRectangle
                    using ( var redPen = new Pen( Color.Red, 1.0f / g.DpiX ) )
                    {
                            PointF[] rectPts = { rectRot.A.ToPointF(), rectRot.B.ToPointF(), rectRot.C.ToPointF(), rectRot.D.ToPointF() };
                            g.DrawPolygon( redPen, rectPts );
                    }

                    // Draw distance to the nearest obstacle
                    using ( var cyanPen = new Pen( Color.Cyan, 1.0f / g.DpiX ) )
                    {
                            PointF[] rectPts = { rectVisRot.AB.P1.ToPointF(), rectVisRot.AB.P2.ToPointF(), rectVisRot.CD.P2.ToPointF(), rectVisRot.CD.P1.ToPointF() };
                            g.DrawPolygon( cyanPen, rectPts );
                    }
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
                float xMax = m_building.Extents.Right;
                float yMax = m_building.Extents.Bottom;
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
                        lblCurrentCell.Text += ", S[ " + j + ", " + i + " ] = " + m_S[ i ][ j ].ToString( "f3" );
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

            if ( m_highlightBoxes )
            {
                if ( ( m_currentHuman != null ) && m_currentHuman.ProjectionExtent.Contains( pt[ 0 ] ) ) return;

                // First search for human projection
                foreach ( var h in m_building.CurrentFloor.People )
                {
                    if ( MathUtils.Sqr( h.ProjectionCenter.X - pt[ 0 ].X ) + MathUtils.Sqr( h.ProjectionCenter.Y - pt[ 0 ].Y ) <= MathUtils.Sqr( h.ProjectionDiameter / 2 ) )
                    {
                        m_currentHuman = h;

                        grdProps.SelectedObject = h;
                        grdProps.Refresh();

                        pbVisualizator.Refresh();
                        return;
                    }
                }

                m_currentHuman = null;

                if ( m_currentBoxExtents.Contains( pt[ 0 ] ) || !m_fixedBoxExtents.IsEmpty ) return;

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

                InitDistanceField();
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
            m_currentHuman = null;

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
                // Moving scene: w, a, s, d (like FPS :))
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
                // Reset scene scale and position to default ones
                case 'n':
                {
                    m_panPoint = m_panPointOrig;
                    m_scale = 1;
                    break;
                }
                // Increase/decrease scale
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
                // Box higliting enabled/disabled
                case 'h':
                {
                    m_highlightBoxes = !m_highlightBoxes;
                    break;
                }
                // Building/furniture/people/grid drawing enabled/disabled
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
                    if ( !m_drawGrid ) m_fieldVisMode = DistanceField.DrawMode.None;

                    lblCurrentCell.Visible = m_drawGrid;
                    break;
                }
                case 'm':   // mode
                {
                    if ( m_drawGrid )
                    {
                        m_fieldVisMode = (DistanceField.DrawMode )( ( int )m_fieldVisMode + 1 );
                        if ( ( int )m_fieldVisMode > ( int )DistanceField.DrawMode.Count )
                        {
                            m_fieldVisMode = DistanceField.DrawMode.None;
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
