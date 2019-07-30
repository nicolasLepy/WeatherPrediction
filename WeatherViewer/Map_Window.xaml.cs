using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;
using WeatherPrediction;

namespace WeatherViewer
{
    /// <summary>
    /// Logique d'interaction pour Map_Window.xaml
    /// </summary>
    public partial class Map_Window : Window
    {

        private Database _database;
        private double _width;
        private double _height;


        public Map_Window(Database database)
        {
            InitializeComponent();
            _database = database;
            imgMap.Source = new BitmapImage(new Uri(View_Utils.IconPath("tatooine_map.png")));
            _width = imgMap.Width;
            _height = imgMap.Height;

        }

        private Point Map2Screen(Point p)
        {
            double x = (p.X / 1900.0) * _width;
            double y = (p.Y / 1283.0) * _height;

            return new Point(x, y);
        }

        private Point Screen2Map(Point p)
        {
            double x = (p.X / _width) * 1900.0;
            double y = (p.Y / _height) * 1283.0;
            return new Point(x, y);
        }

        public void SetStations()
        {
            string[] timeString = dpDate.Text.Split('/');
            DateTime date = new DateTime(int.Parse(timeString[2]), int.Parse(timeString[1]), int.Parse(timeString[0]));
            int hour = int.Parse(tbHour.Text);

            foreach (City city in _database.Cities)
            {
                Report today = city.GetReport(date);
                Report yesturday = city.GetReport(date.AddDays(-1));
                Report tomorrow = city.GetReport(date.AddDays(1));
                IInterpolation interpolation = Utils.TemperaturesInterpolation(yesturday.TMax, today.TMin, today.TMax, tomorrow.TMin);

                Point p = Map2Screen(new Point(city.X, city.Y));

                double XOnMap = p.X;
                double YOnMap = p.Y;
                DrawLabel(XOnMap, YOnMap, interpolation.Interpolate(hour).ToString("0.0"));
                /*Label label = new Label();
                label.Content = interpolation.Interpolate(hour).ToString("0.0");
                label.Margin = new Thickness(XOnMap, YOnMap, 0, 0);
                label.Style = Application.Current.FindResource("StyleLabelMap") as Style;

                canvas.Children.Add(label);*/

            }
        }

        private void DrawLabel(double x, double y, string content)
        {
            Label label = new Label();
            label.Content = content;
            label.Margin = new Thickness(x, y, 0, 0);
            label.Style = Application.Current.FindResource("StyleLabelMap") as Style;

            canvas.Children.Add(label);
        }

        /// <summary>
        /// Give temperature of a zone
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private double TemperatureZone(int x, int y, int size)
        {
            double temperature = 0;

            

            return temperature;
        }

        private void ClearMap()
        {
            List<UIElement> toRemove = new List<UIElement>();
            foreach(UIElement ui in canvas.Children)
            {
                if (ui as Label != null || ui as Rectangle != null) toRemove.Add(ui);
            }
            foreach(UIElement ui in toRemove)
            {
                canvas.Children.Remove(ui);
            }
        }

        private void BtnCompute_Click(object sender, RoutedEventArgs e)
        {

            ClearMap();

            string[] timeString = dpDate.Text.Split('/');
            DateTime date = new DateTime(int.Parse(timeString[2]), int.Parse(timeString[1]), int.Parse(timeString[0]));
            int hour = int.Parse(tbHour.Text);
            double p = Double.Parse(tbP.Text, CultureInfo.InvariantCulture);

            int step = int.Parse(tbStep.Text) ;
            int nbColumn = (int)_width / step;
            int nbLines = (int)_height / step;
            for(int i = 0; i<nbLines; i++)
            {
                for(int j = 0;  j<nbColumn; j++)
                {
                    Point pt = Screen2Map(new Point(step * j, step * i));
                    double temperature = _database.Temperature(pt.X, pt.Y, date, hour,p);
                    Rectangle rect = new Rectangle();

                    Color color = View_Utils.Temperature2Color(temperature);
                    rect.Fill = new SolidColorBrush(color);
                    rect.Width = step;
                    rect.Height = step;
                    rect.Margin = new Thickness(step * j, step * i, 0, 0);
                    canvas.Children.Add(rect);
                    if(step > 25)
                        DrawLabel(step * j, step * i, temperature.ToString("0.0"));

                }
            }


            SetStations();

        }
        

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Point p = Mouse.GetPosition(canvas);
                string[] timeString = dpDate.Text.Split('/');
                DateTime date = new DateTime(int.Parse(timeString[2]), int.Parse(timeString[1]), int.Parse(timeString[0]));
                int hour = int.Parse(tbHour.Text);
                double pp = Double.Parse(tbP.Text, CultureInfo.InvariantCulture);

                Point onMap = Screen2Map(new Point(p.X, p.Y));
                double temp = _database.Temperature(onMap.X, onMap.Y, date, hour, pp);
                lbTemp.Content = temp.ToString("0.0");
            }
            catch { }
            
            //Console.WriteLine(p.X + " - " + p.Y);
        }

    }
}
