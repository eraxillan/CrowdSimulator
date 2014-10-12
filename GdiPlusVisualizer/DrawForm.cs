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

        class UniqueLineCollection
        {
            SortedSet<LineF> m_lines = new SortedSet<LineF>();

            public void Add( LineF line )
            {
                if ( !Exists( line ) )
                    m_lines.Add( line );
            }

            public bool Exists( LineF line )
            {
                return m_lines.Contains( line );
            }

            public void Draw( Graphics g, Pen p )
            {
                //System.Diagnostics.Debug.Assert( m_lines.Count % 4 == 0 );

                foreach ( var line in m_lines )
                {
                    g.DrawLine( p, line.X1, line.Y1, line.X2, line.Y2 );

                    /*var pnts = new[] 
                    { 
                        new PointF { X = line.X1, Y = line.Y1 },
                        new PointF { X = line.X1, Y = line.Y2 },
                        new PointF { X = line.X2, Y = line.Y2 },
                        new PointF { X = line.X2, Y = line.Y1 },
                        new PointF { X = line.X1, Y = line.Y1 }
                    };

                    g.DrawLines( p, pnts );*/
                }
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
            var range = m_building.GetExtent();

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

            // Smooth graphics output and scale
            g.SmoothingMode = SmoothingMode.HighQuality;
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

        private void cmbFloor_SelectedIndexChanged( object sender, EventArgs e )
        {
            int newFloorNumber = cmbFloor.SelectedIndex + 1;
            System.Diagnostics.Debug.Assert( newFloorNumber >= m_building.FirstFloorNumber && newFloorNumber <= m_building.FloorCount );

            m_building.CurrentFloorNumber = newFloorNumber;
            m_panPoint = new PointF();
            m_boxMap = m_building.CurrentFloor.GetBoxMap();
            m_currentBoxExtents = RectangleF.Empty;
            m_fixedBoxExtents = RectangleF.Empty;

            pbVisualizator.Refresh();
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
            if ( dlgDataDir.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                // Get the last directory of the path
                m_currentDir = new DirectoryInfo( dlgDataDir.SelectedPath ).Name;
                this.Text = "Building schema: " + m_currentDir;

                // Load building data from the XML file
                var inputParser = new InputDataParser.Parser();
                var building = inputParser.LoadGeometryXMLRoot( dlgDataDir.SelectedPath + @"\geometry.xml" );
                if ( building.FloorList.Count() == 0 )
                {
                    MessageBox.Show( "Building has no floors", "Visualizer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
                    return;
                }

                // Load apertures
                // FIXME:
                //

                // Load furniture
                // FIXME:

                // Load people
                // FIXME:

                m_building = new BuildingWrapper( building );
                foreach ( var floor in building.FloorList )
                    cmbFloor.Items.Add( floor.Name );
                cmbFloor.SelectedIndex = 0;

                lblBuildingExtent.Text = "Building extent (world): " + RectFToString( m_building.GetExtent() );
                m_boxMap = m_building.CurrentFloor.GetBoxMap();

                // Updata data files load status
                lstDataFiles.Items[ 0 ].ImageIndex = System.IO.File.Exists( dlgDataDir.SelectedPath + @"\geometry.xml" ) ? 0 : 1;
                lstDataFiles.Items[ 1 ].ImageIndex = System.IO.File.Exists( dlgDataDir.SelectedPath + @"\apertures.xml" ) ? 0 : 1;
                lstDataFiles.Items[ 2 ].ImageIndex = System.IO.File.Exists( dlgDataDir.SelectedPath + @"\furniture.xml" ) ? 0 : 1;
                lstDataFiles.Items[ 3 ].ImageIndex = System.IO.File.Exists( dlgDataDir.SelectedPath + @"\people.xml" ) ? 0 : 1;

                lstDataFiles.Items[ 0 ].SubItems[ 1 ].Text = @"geometry.xml";
                lstDataFiles.Items[ 1 ].SubItems[ 1 ].Text = @"apertures.xml";
                lstDataFiles.Items[ 2 ].SubItems[ 1 ].Text = @"furniture.xml";
                lstDataFiles.Items[ 3 ].SubItems[ 1 ].Text = @"people.xml";

                lstDataFiles.Items[ 0 ].ToolTipText = dlgDataDir.SelectedPath + @"\geometry.xml";
                lstDataFiles.Items[ 1 ].ToolTipText = dlgDataDir.SelectedPath + @"\apertures.xml";
                lstDataFiles.Items[ 2 ].ToolTipText = dlgDataDir.SelectedPath + @"\furniture.xml";
                lstDataFiles.Items[ 3 ].ToolTipText = dlgDataDir.SelectedPath + @"\people.xml";
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
    }

    // -------------------------------------------------------------------------------------------------------

    interface IDrawable
    {
        void Draw( Graphics g );
    }

    interface IExtentOwner
    {
        RectangleF GetExtent();
    }

    class Point3F
    {
        float m_X = float.NaN;
        float m_Y = float.NaN;
        float m_Z = float.NaN;

        public Point3F( float X, float Y, float Z = 0 )
        {
            m_X = X;
            m_Y = Y;
            m_Z = Z;
        }

        public bool IsNull
        {
            get { return ( float.IsNaN( m_X ) || float.IsNaN( m_Y ) || float.IsNaN( m_Z ) ); }
        }

        public float X
        {
            get { return m_X; }
        }

        public float Y
        {
            get { return m_Y; }
        }

        public float Z
        {
            get { return m_Z; }
        }

        public override string ToString()
        {
            if ( IsNull )
                return "<Invalid>";

            string pntString = "{ ";
            pntString += X.ToString( "F3" );
            pntString += "; ";
            pntString += Y.ToString( "F3" );
            pntString += "; ";
            pntString += Z.ToString( "F3" );
            pntString += " }";
            return pntString;
        }
    }

    [System.ComponentModel.DefaultProperty( "Id" )]
    class BoxWrapper : IExtentOwner, IDrawable
    {
        GeometryTypes.TBox m_box = null;
        RectangleF m_extent = new RectangleF();
        Font m_textFont = null;
        RectangleF m_textRect;
        Point3F m_nearLeft;
        Point3F m_farRight;

        public BoxWrapper( GeometryTypes.TBox box )
        {
            m_box = box;
            m_nearLeft = new Point3F( m_box.X1, m_box.Y1, m_box.Z1 );
            m_farRight = new Point3F( m_box.X2, m_box.Y2, m_box.Z2 );
        }

        [System.ComponentModel.Category("Base properties")]
        public int Id
        {
            get { return m_box.Id; }
        }

        [System.ComponentModel.Category( "Base properties" )]
        public string Name
        {
            get { return m_box.Name; }
        }

        [System.ComponentModel.Category( "Base properties" )]
        public int Type
        {
            get { return m_box.Type; }
        }

        [System.ComponentModel.Category( "Placement" )]
        public Point3F NearLeft
        {
            get { return m_nearLeft; }
        }

        [System.ComponentModel.Category( "Placement" )]
        public Point3F FarRight
        {
            get { return m_farRight; }
        }

        [System.ComponentModel.Category( "Placement" ),
         System.ComponentModel.Description( "The 2D extent of the geometry object" )]
        public RectangleF Extents
        {
            get { return GetExtent(); }
        }

        public RectangleF GetExtent()
        {
            if ( !m_extent.IsEmpty )
                return m_extent;

            float x1 = m_box.X1;
            float y1 = m_box.Y1;
            float x2 = m_box.X2;
            float y2 = m_box.Y2;

            float w = x2 - x1;
            float h = y2 - y1;
            System.Diagnostics.Debug.Assert( w > 0, "Box extent have negative width" );
            System.Diagnostics.Debug.Assert( h > 0, "Box extent have negative height" );

            m_extent = new RectangleF( x1, y1, w, h );
            return m_extent;
        }

        public void Draw( Graphics g )
        {
            switch ( m_box.Type )
            {
                case 0:
                    {
                        // Draw box rectangle
                        var extent = GetExtent();
                        using ( var brownPen = new Pen( Color.Brown, 1.0f / g.DpiX ) )
                        {
                            g.DrawRectangle( brownPen, extent.X, extent.Y, extent.Width, extent.Height );
                        }

                        // Draw box properties as text on it
                        var extentCenter = new PointF( ( extent.Left + extent.Right ) / 2, ( extent.Bottom + extent.Top ) / 2 );
                        DrawText( g, "ID: " + m_box.Id + "\n" + "Height: " + ( m_box.Z2 - m_box.Z1 ), extentCenter, extent );
                        break;
                    }
                default:
                    System.Diagnostics.Debug.Assert( false, "Invalid TBox type" );
                    break;
            }
        }

        private void DrawText( Graphics g, string text, PointF ptStart, RectangleF extent )
        {
            var gs = g.Save();
            // Inverse Y axis again - now it grow down;
            // if we don't do this, text will be drawn inverted
            g.ScaleTransform( 1.0f, -1.0f, MatrixOrder.Prepend );

            if ( m_textFont == null )
            {
                // Find the maximum appropriate text size to fix the extent
                float fontSize = 100.0f;
                Font fnt;
                SizeF textSize;
                do
                {
                    fnt = new Font( "Arial", fontSize / g.DpiX, FontStyle.Bold, GraphicsUnit.Pixel );
                    textSize = g.MeasureString( text, fnt );
                    m_textRect = new RectangleF( new PointF( ptStart.X - textSize.Width / 2.0f, -ptStart.Y - textSize.Height / 2.0f ), textSize );

                    var textRectInv = new RectangleF( m_textRect.X, -m_textRect.Y, m_textRect.Width, m_textRect.Height );
                    if ( extent.Contains( textRectInv ) )
                        break;

                    fontSize -= 1.0f;
                    if ( fontSize <= 0 )
                    {
                        fontSize = 1.0f;
                        break;
                    }
                } while ( true );

                m_textFont = fnt;
            }

            // Create a StringFormat object with the each line of text, and the block of text centered on the page
            var stringFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString( text, m_textFont, Brushes.Black, m_textRect, stringFormat );
            stringFormat.Dispose();

            g.Restore( gs );
        }
    }

    class RoomWrapper : IExtentOwner, IDrawable
    {
        GeometryTypes.TRoom m_room = null;
        BoxWrapper[] m_boxes = null;

        public RoomWrapper( GeometryTypes.TRoom room )
        {
            m_room = room;

            m_boxes = new BoxWrapper[ room.Geometry.Count() ];
            for ( int i = 0; i < room.Geometry.Count(); ++i )
            {
                m_boxes[ i ] = new BoxWrapper( room.Geometry[ i ] );
            }
        }

        public BoxWrapper[] Boxes
        {
            get { return m_boxes; }
        }

        public RectangleF GetExtent()
        {
            RectangleF extent;
            if ( m_boxes.Count() >= 1 )
            {
                extent = m_boxes[ 0 ].GetExtent();
            }
            else
                extent = new RectangleF();

            for ( int i = 1; i < m_boxes.Count(); ++i )
            {
                extent = RectangleF.Union( extent, m_boxes[ i ].GetExtent() );
            }

            return extent;
        }

        public void Draw( Graphics g )
        {
            switch ( m_room.Type )
            {
                case 0: // Room itself
                    {
                        foreach ( var box in m_boxes )
                        {
                            box.Draw( g );
                        }
                        break;
                    }
                case 1: // Corridor
                    {
                        using ( var grayBrush = new HatchBrush( HatchStyle.DiagonalCross, Color.LightGray, Color.White ) )
                        {
                            g.FillRectangle( grayBrush, GetExtent().X, GetExtent().Y, GetExtent().Width, GetExtent().Height );
                        }
                        break;
                    }
                default:
                    System.Diagnostics.Debug.Assert( false, "Invalid TRoom type" );
                    break;
            }
        }
    }


    class FloorWrapper : IExtentOwner, IDrawable
    {
        GeometryTypes.TFloor m_floor = null;
        RoomWrapper[] m_rooms = null;

        public FloorWrapper( GeometryTypes.TFloor floor )
        {
            m_floor = floor;

            m_rooms = new RoomWrapper[ floor.RoomList.Count() ];
            for ( int i = 0; i < floor.RoomList.Count(); ++i )
            {
                m_rooms[ i ] = new RoomWrapper( floor.RoomList[ i ] );
            }
        }

        public int Number
        {
            get { return m_floor.Number; }
        }

        public Dictionary<RectangleF, BoxWrapper> GetBoxMap()
        {
            Dictionary<RectangleF, BoxWrapper> boxes = new Dictionary<RectangleF, BoxWrapper>();
            foreach ( var room in m_rooms )
            {
                foreach ( var box in room.Boxes )
                {
                    boxes.Add( box.GetExtent(), box );
                }
            }
            return boxes;
        }

        public RectangleF GetExtent()
        {
            RectangleF extent;
            if ( m_rooms.Count() >= 1 )
            {
                extent = m_rooms[ 0 ].GetExtent();
            }
            else
                extent = new RectangleF();

            for ( int i = 1; i < m_rooms.Count(); ++i )
            {
                extent = RectangleF.Union( extent, m_rooms[ i ].GetExtent() );
            }

            return extent;
        }

        public void Draw( Graphics g )
        {
            switch ( m_floor.Type )
            {
                case 0:
                    {
                        foreach ( var room in m_rooms )
                        {
                            room.Draw( g );
                        }
                        break;
                    }
                default:
                    System.Diagnostics.Debug.Assert( false, "Invalid TFloor type" );
                    break;
            }
        }
    }

    class BuildingWrapper : IExtentOwner, IDrawable
    {
        GeometryTypes.TBuilding m_building = null;
        int m_floorNumber = -1;
        FloorWrapper[] m_floors = null;

        #region Floor sorter class (by numbers of floor)
        class FloorComparer : IComparer
        {
            public int Compare( object x, object y )
            {
                if ( x == null || y == null ) return 1;

                var xx = x as GeometryTypes.TFloor;
                var yy = y as GeometryTypes.TFloor;
                if ( xx == null || yy == null )
                    throw new ArgumentException( "Object is not a TFloor" );

                if ( xx.Number == yy.Number )
                    return 0;
                if ( xx.Number > yy.Number )
                    return 1;
                return -1;
            }
        }

        void SortFloors( GeometryTypes.TBuilding building )
        {
            Array.Sort( building.FloorList, new FloorComparer() );
        }
        #endregion

        /*public BuildingWrapper( string fileName )
        {
            InputDataParser.Parser inputParser = new InputDataParser.Parser();
            System.Diagnostics.Debug.Assert( System.IO.File.Exists( @"..\..\..\Data\KinderGarten\садик17_geometry.xml" ) );
            m_building = inputParser.LoadGeometryXMLRoot( @"..\..\..\Data\KinderGarten\садик17_geometry.xml" );
            if ( m_building.FloorList.Count() == 0 )
                throw new InvalidOperationException( "Building has no floors" );

            SortFloors( m_building );
            m_floorNumber = m_building.FloorList[ 0 ].Number;
        }*/

        public BuildingWrapper( GeometryTypes.TBuilding building )
        {
            m_building = building;
            SortFloors( building );

            m_floorNumber = building.FloorList.Count() > 0 ? building.FloorList[ 0 ].Number : -1;
            m_floors = new FloorWrapper[ FloorCount ];
            for( int i = 0; i < building.FloorList.Count(); ++i )
            {
                m_floors[ i ] = new FloorWrapper( building.FloorList[ i ] );
            }
        }

        public int CurrentFloorNumber
        {
            get { return m_floorNumber; }
            set
            {
                if ( value >= FirstFloorNumber && value <= FloorCount )
                    m_floorNumber = value;
                else
                    throw new ArgumentException( "Floor number must be in range " + FirstFloorNumber + " - " + FloorCount );
            }
        }

        public int FirstFloorNumber
        {
            get
            {
                return m_building.FloorList[ 0 ].Number;
            }
        }

        public int FloorCount
        {
            get
            {
                // FIXME: think about negative floor numbers
                return m_building.FloorList.Count();
            }
        }

        public FloorWrapper CurrentFloor
        {
            get
            {
                foreach ( var floor in m_floors )
                {
                    if ( floor.Number == m_floorNumber )
                        return floor;
                }

                return null;
            }
        }

        public RectangleF GetExtent()
        {
            return CurrentFloor.GetExtent();
        }

        public void Draw( Graphics g )
        {
            CurrentFloor.Draw( g );
        }
    }
}
