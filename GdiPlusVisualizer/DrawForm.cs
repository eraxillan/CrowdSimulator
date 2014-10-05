//#define DEBUG_DRAW

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GdiPlusVisualizer
{
    public partial class DrawForm : Form
    {
        BuildingWrapper m_building = null;
        float m_scale = 1.0f;

        public DrawForm()
        {
            InitializeComponent();

            pbVisualizator.MouseWheel += this.DrawForm_MouseWheel;

            // Load building data from the XML file
            InputDataParser.Parser inputParser = new InputDataParser.Parser();
            System.Diagnostics.Debug.Assert( System.IO.File.Exists( @"..\..\..\Data\KinderGarten\садик17_geometry.xml" ) );
            GeometryTypes.TBuilding building = inputParser.LoadGeometryXMLRoot( @"..\..\..\Data\KinderGarten\садик17_geometry.xml" );
            if ( building.FloorList.Count() == 0 )
                throw new InvalidOperationException( "Building has no floors" );

            m_building = new BuildingWrapper( building );
            foreach ( var floor in building.FloorList )
                cmbFloor.Items.Add( floor.Name );
            cmbFloor.SelectedIndex = 0;
        }

        void DrawForm_MouseWheel( object sender, MouseEventArgs e )
        {
            float zoom = e.Delta > 0 ? 0.1f : -0.1f;
            m_scale += zoom;
            if ( Math.Abs( m_scale ) <= 0.1 )
                m_scale = 0.1f;

//            Console.WriteLine("Scale: " + Math.Round(m_scale * 100) + "%");
            pbVisualizator.Invalidate();
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
            g.TranslateTransform( OX, OY );
            g.ScaleTransform( SX * m_scale, -SY * m_scale );
        }

        private void DrawForm_Resize( object sender, EventArgs e )
        {
            Refresh();
        }

        private void pbVisualizator_Paint( object sender, PaintEventArgs e )
        {
            var g = e.Graphics;
            // Smooth graphics output
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Set margins inside the control client area in pixels
            var margin = new Margins( 16, 16, 16, 16 );

            // Set the domain of (x,y) values
            var range = m_building.GetExtent();
            // Make it smaller by 5%
            range.Inflate( 0.05f * range.Width, 0.05f * range.Height );

            // Scale graphics
            ScaleGraphics( g, pbVisualizator, range, margin );

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

            var thinGrayPen = new Pen( Color.Gray, 1.0f / g.DpiX );
            thinGrayPen.DashStyle = DashStyle.Dash;
            var bluePen = new Pen( Color.Blue, 1.0f / g.DpiX );
            var grayBrush = new HatchBrush( HatchStyle.DiagonalCross, Color.LightGray, Color.White );
            var fnt = new Font( "Arial", m_scale / g.DpiX, FontStyle.Bold, GraphicsUnit.Pixel );

            // FIXME: Get rid of hard-coded type numbers - make them named constants

            m_building.Draw( g );
        }

        private void cmbFloor_SelectedIndexChanged( object sender, EventArgs e )
        {
            int newFloorNumber = cmbFloor.SelectedIndex + 1;
            System.Diagnostics.Debug.Assert( newFloorNumber >= m_building.FirstFloorNumber && newFloorNumber <= m_building.FloorCount );

            m_building.CurrentFloorNumber = newFloorNumber;
            pbVisualizator.Refresh();
        }

        private void pbVisualizator_MouseEnter( object sender, EventArgs e )
        {
            pbVisualizator.Focus();
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

    class BoxWrapper : IExtentOwner, IDrawable
    {
        GeometryTypes.TBox m_box = null;
        RectangleF m_extent = new RectangleF();
        Font m_textFont = null;
        RectangleF m_textRect;

        public BoxWrapper( GeometryTypes.TBox box )
        {
            m_box = box;
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
            var dashedGrayPen = new Pen( Color.Gray, 1.0f / g.DpiX ) { DashStyle = DashStyle.Dash };
            var brownPen = new Pen( Color.Brown, 1.0f / g.DpiX );
            switch ( m_box.Type )
            {
                case 0:
                    {
                        // Draw box rectangle
                        RectangleF extent = GetExtent();
                        g.DrawRectangle( brownPen, extent.X, extent.Y, extent.Width, extent.Height );

                        // Draw box properties as text on it
                        PointF extentCenter = new PointF( ( extent.Left + extent.Right ) / 2, ( extent.Bottom + extent.Top ) / 2 );
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

            // FIXME: remove
            /*float minScaleValue = Math.Min( g.Transform.Elements[ 0 ], g.Transform.Elements[ 3 ] );
            var fnt = new Font( "Arial", 20.0f / g.DpiX, FontStyle.Bold, GraphicsUnit.Pixel );
            var textSize = g.MeasureString( text, fnt );
            var textRect = new RectangleF( new PointF( ptStart.X - textSize.Width / 2.0f, -ptStart.Y - textSize.Height / 2.0f ), textSize );*/

            g.DrawString( text, m_textFont, Brushes.Black, m_textRect, stringFormat );

            g.Restore( gs );
        }
    }

    class RoomWrapper : IExtentOwner, IDrawable
    {
        GeometryTypes.TRoom m_room = null;

        public RoomWrapper( GeometryTypes.TRoom room )
        {
            m_room = room;
        }

        public RectangleF GetExtent()
        {
            RectangleF extent;
            if ( m_room.Geometry.Count() >= 1 )
            {
                BoxWrapper bw = new BoxWrapper( m_room.Geometry[ 0 ] );
                extent = bw.GetExtent();
            }
            else
                extent = new RectangleF();

            for ( int i = 1; i < m_room.Geometry.Count(); ++i )
            {
                var bw = new BoxWrapper( m_room.Geometry[ i ] );
                extent = RectangleF.Union( extent, bw.GetExtent() );
            }

            return extent;
        }

        public void Draw( Graphics g )
        {
            var brownPen = new Pen( Color.Brown, 1.0f / g.DpiX );
            var grayBrush = new HatchBrush( HatchStyle.DiagonalCross, Color.LightGray, Color.White );
            switch ( m_room.Type )
            {
                case 0: // Room itself
                    {
                        foreach ( var box in m_room.Geometry )
                        {
                            var bw = new BoxWrapper( box );
                            bw.Draw( g );
                        }
                        break;
                    }
                case 1: // Corridor
                    {
                        g.FillRectangle( grayBrush, GetExtent().X, GetExtent().Y, GetExtent().Width, GetExtent().Height );
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

        public FloorWrapper( GeometryTypes.TFloor floor )
        {
            m_floor = floor;
        }

        public RectangleF GetExtent()
        {
            RectangleF extent;
            if ( m_floor.RoomList.Count() >= 1 )
            {
                RoomWrapper rw = new RoomWrapper( m_floor.RoomList[ 0 ] );
                extent = rw.GetExtent();
            }
            else
                extent = new RectangleF();

            for ( int i = 1; i < m_floor.RoomList.Count(); ++i )
            {
                RoomWrapper rw = new RoomWrapper( m_floor.RoomList[ i ] );
                extent = RectangleF.Union( extent, rw.GetExtent() );
            }

            return extent;
        }

        public void Draw( Graphics g )
        {
            switch ( m_floor.Type )
            {
                case 0:
                    {
                        foreach ( var box in m_floor.RoomList )
                        {
                            RoomWrapper rw = new RoomWrapper( box );
                            rw.Draw( g );
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
                return m_building.FloorList.Count();
            }
        }

        public FloorWrapper CurrentFloor
        {
            get
            {
                foreach ( var floor in m_building.FloorList )
                {
                    if ( floor.Number == m_floorNumber )
                        return new FloorWrapper( floor );
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
