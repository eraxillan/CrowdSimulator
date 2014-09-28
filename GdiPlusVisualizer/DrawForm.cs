//#define DEBUG_DRAW

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GdiPlusVisualizer
{
    public partial class DrawForm : Form
    {
        GeometryTypes.TBuilding m_building;
        GeometryTypes.TFloor m_selectedFloor;
        float m_scale = 30.0f;//1.0f;   // FIXME: replace

        public DrawForm()
        {
            InitializeComponent();

            this.MouseWheel += this.DrawForm_MouseWheel;

            // Load building data from the XML file
            InputDataParser.Parser inputParser = new InputDataParser.Parser();
            System.Diagnostics.Debug.Assert(System.IO.File.Exists(@"..\..\..\Data\KinderGarten\садик17_geometry.xml"));
            m_building = inputParser.LoadGeometryXMLRoot(@"..\..\..\Data\KinderGarten\садик17_geometry.xml");
            if (m_building.FloorList.Count() == 0)
                throw new InvalidOperationException("Building has no floors");

            SortFloors(m_building);
            m_selectedFloor = m_building.FloorList[0];
        }

        public class FloorComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null || y == null) return 1;

                var xx = x as GeometryTypes.TFloor;
                var yy = y as GeometryTypes.TFloor;
                if (xx == null || yy == null)
                    throw new ArgumentException("Object is not a Temperature");

                if (xx.Number == yy.Number)
                    return 0;
                if (xx.Number > yy.Number)
                    return 1;
                return -1;
            }
        }

        void SortFloors(GeometryTypes.TBuilding building)
        {
            Array.Sort(building.FloorList, new FloorComparer());
        }

        void DrawForm_MouseWheel(object sender, MouseEventArgs e)
        {
            float zoom = e.Delta > 0 ? 0.5f : -0.5f;
            m_scale += zoom;

//            Console.WriteLine("Scale: " + Math.Round(m_scale * 100) + "%");
            Invalidate();
        }

        void DrawLineWithArrow(Graphics g, Color c, float x1, float y1, float x2, float y2)
        {
            var endCap = new AdjustableArrowCap(5, 5, false)
            {
                BaseCap = LineCap.Flat,
                BaseInset = 5,
                WidthScale = 1.0f,
                StrokeJoin = LineJoin.Bevel
            };

            var customPen = new Pen(c, 1.0f / g.DpiX)
            {
                CustomEndCap = endCap
            };
            g.DrawLine(customPen, x1, y1, x2, y2);
        }

        void DrawDigonalString(Graphics G, string S, Font F, Brush B, PointF P, int Angle)
        {
            SizeF MySize = G.MeasureString(S, F);
            G.TranslateTransform(P.X + MySize.Width / 2, P.Y + MySize.Height / 2);
            G.RotateTransform(Angle);
            G.DrawString(S, F, B, new PointF(-MySize.Width / 2, -MySize.Height / 2));
            G.RotateTransform(-Angle);
            G.TranslateTransform(-P.X - MySize.Width / 2, -P.Y - MySize.Height / 2);
        }

        private void DrawForm_Paint(object sender, PaintEventArgs e)
        {
            float halfWidth = Width / 2.0f - 1;
            float halfHeight = Height / 2.0f - 1;

            // Setup graphics output settings
            var g = e.Graphics;
            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.PageUnit = GraphicsUnit.Pixel;

            // Setup coordinate system: move null point to form center, invert Y axis, setup user-defined zoom
            g.ScaleTransform(m_scale, -m_scale, MatrixOrder.Append);
            g.TranslateTransform(50, Height - 50, MatrixOrder.Append);
            

            // Draw null point, X and Y axes
#if DEBUG_DRAW
            g.FillEllipse(Brushes.Black, -1.0f, -1.0f, 1.0f, 1.0f);
            var scr = Screen.FromControl(this);
            float coeff = (float)scr.WorkingArea.Width / scr.WorkingArea.Height;
            DrawLineWithArrow(g, Color.Black, 0, 0, halfWidth - 0.1f * Width * coeff, 0);
            DrawLineWithArrow(g, Color.Black, 0, 0, 0, halfHeight - 0.1f * Height / coeff);
#endif
            var thinBlackPen = new Pen( Color.Gray, 1.0f / g.DpiX );
            thinBlackPen.DashStyle = DashStyle.Dash;
            var bluePen = new Pen( Color.Blue, 1.0f / g.DpiX );
            var grayBrush = new HatchBrush( HatchStyle.DiagonalCross, Color.LightGray, Color.White );
            var fnt = new Font( "Arial", m_scale / g.DpiX, FontStyle.Bold, GraphicsUnit.Pixel );

            // FIXME: Draw one line only one time

            foreach (var room in m_selectedFloor.RoomList)
            {
                var roomRect = new RectangleF();
                if ( room.Geometry.Count() >= 1 )
                {
                    GeometryTypes.TBox box = room.Geometry[ 0 ];
                    roomRect = new RectangleF( box.X1, box.Y1, ( box.X2 - box.X1 ), ( box.Y2 - box.Y1 ) );
                }

                foreach (var box in room.Geometry)
                {
                    // Calculate box rect and update the room one
                    var rect = new RectangleF(box.X1, box.Y1, (box.X2 - box.X1), (box.Y2 - box.Y1));
                    roomRect = RectangleF.Union(roomRect, rect);
                    
                    // Draw box rect
                    switch ( room.Type )
                    {
                        case 0:
                            g.DrawRectangle( thinBlackPen, box.X1, box.Y1, ( box.X2 - box.X1 ), ( box.Y2 - box.Y1 ) );
                            break;
                        case 1:
                            rect.Inflate( -1.0f / g.DpiX, -1.0f / g.DpiX );
                            g.FillRectangle( grayBrush, rect);
                            break;
                        default:
                            System.Diagnostics.Debug.Assert( false );
                            return;
                    }

                    // Create a StringFormat object with the each line of text, and the block of text centered on the page
                    var stringFormat = new StringFormat()
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    GraphicsState gs = g.Save();

                    g.ResetTransform();
                    g.ScaleTransform(m_scale, m_scale, MatrixOrder.Append);
                    g.TranslateTransform(50, Height - 100, MatrixOrder.Append);

#if DEBUG_DRAW
                    DrawLineWithArrow(g, Color.Yellow, 0, 0, 100, 0);
                    DrawLineWithArrow(g, Color.Yellow, 0, 0, 0, 100);
#endif

                    // FIXME: text still have incorrect coords
//                    DrawDigonalString(g, "ID: " + box.Id + "\n" + "Type: " + box.Type, fnt, Brushes.Black, new PointF(box.X1, -box.Y1), 0);

                    g.Restore(gs);
                }

                // Draw room rectange
                if(room.Type == 0)
                    g.DrawRectangle(bluePen, roomRect.X, roomRect.Y, roomRect.Width, roomRect.Height );
                //Console.WriteLine();
            }

            g.ResetTransform();
            g.Dispose();
        }

        private void DrawForm_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void DrawForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(System.Char.IsDigit(e.KeyChar))
            {
                int newNumber = (int)System.Char.GetNumericValue(e.KeyChar);
                foreach(var floor in m_building.FloorList)
                {
                    if (floor.Number == newNumber)
                    {
                        m_selectedFloor = floor;
                        Invalidate();

                        Console.WriteLine("Switching to floor number " + newNumber);
                    }
                }
            }
        }
    }
}
