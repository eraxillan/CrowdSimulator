using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Visualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Variables
        private readonly Visualizer.ViewModel.BuildingViewModel m_buildingViewModel;
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

            myCanvas.RenderTransform = new ScaleTransform(1.0f, 1.0f);

            m_buildingViewModel = new Visualizer.ViewModel.BuildingViewModel();
            DataContext = m_buildingViewModel;
        }
        #endregion

        private void cmbFloors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            myCanvas.Children.Clear();

            foreach(var room in m_buildingViewModel.SelectedFloor.RoomList)
            {
                var g = new Grid();
//                g.MaxWidth = room.Geometry[0].X2 - room.Geometry[0].X1;
//                g.MaxWidth = room.Geometry[0].Y2 - room.Geometry[0].Y1;

                // Draw room ID and room type
               /* var tbId = new TextBlock();
                tbId.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                tbId.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                tbId.Text = "ID: " + room.Id.ToString() + "\n";
                tbId.Text += "Type: " + room.Type.ToString();
               //  Width="{Binding ElementName=cnvMain, Path=ActualWidth}"
               // Height="{Binding ElementName=cnvMain, Path=ActualHeight}"
                var bndW = new Binding("ActualWidth");
                var bndH = new Binding("ActualHeight");
                bndW.Source = myCanvas;
                bndH.Source = myCanvas;
                tbId.SetBinding(TextBlock.WidthProperty, bndW);
                tbId.SetBinding(TextBlock.HeightProperty, bndH);
                g.Children.Add(tbId);*/

                //myCanvas.Children.Add(tbId);

                // Draw room geometry
                foreach(var box in room.Geometry)
                {
                    var poly = new Polygon
                    {
                        Stroke = Brushes.Black,
                        StrokeThickness = 0.05
                    };
                    poly.Points.Add(new Point(box.X1, box.Y1));
                    poly.Points.Add(new Point(box.X2, box.Y1));
                    poly.Points.Add(new Point(box.X2, box.Y2));
                    poly.Points.Add(new Point(box.X1, box.Y2));
                    myCanvas.Children.Add(poly);
                    //g.Children.Add(poly);

                    var tbId = new TextBlock();
                    tbId.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    tbId.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tbId.Text = "ID: " + room.Id.ToString() + "\n";
                    tbId.Text += "Type: " + room.Type.ToString();
                    var bndW = new Binding("ActualWidth");
                    var bndH = new Binding("ActualHeight");
                    bndW.Source = poly;
                    bndH.Source = poly;
                    //tbId.SetBinding(TextBlock.WidthProperty, bndW);
                    //tbId.SetBinding(TextBlock.HeightProperty, bndH);
                    Canvas.SetLeft(tbId, box.X1);
                    Canvas.SetTop(tbId, box.Y1);
                    tbId.Width = box.X2 - box.X1;
                    tbId.Height = box.Y2 - box.Y1;
                    myCanvas.Children.Add(tbId);
                    //g.Children.Add(tbId);
                }

                //myCanvas.Children.Add(g);
            }
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var st = myCanvas.RenderTransform as ScaleTransform;
            double zoom = e.Delta > 0 ? .2 : -.2;
            st.ScaleX += zoom;
            st.ScaleY += zoom;

            if (Math.Abs(st.ScaleX - 1) > 0)
                Title = "Crowd Visualizer: scale " + st.ScaleX.ToString();
            else
                Title = "Crowd Visualizer";
        }
    }
}
