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
using System.Windows.Shapes;
using WeatherPrediction;

namespace WeatherViewer
{
    /// <summary>
    /// Logique d'interaction pour WeatherForecast_Window.xaml
    /// </summary>
    public partial class WeatherForecast_Window : Window
    {
        private Database _database;

        public WeatherForecast_Window(Database database)
        {
            _database = database;

            InitializeComponent();
            foreach(Region region in _database.Regions)
            {
                foreach (City city in region.Cities)
                {
                    cbCities.Items.Add(city);
                }
            }
        }

        private void CbCities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            City city = cbCities.SelectedItem as City;
            if (city != null && city.Forecast.Count > 6)
            {
                spWeather.Children.Clear();

                string[] timeString = dpDate.Text.Split('/');
                DateTime date = new DateTime(int.Parse(timeString[2]), int.Parse(timeString[1]), int.Parse(timeString[0]));

                for (int i = 0; i < 7; i++)
                {
                    Report report = city.GetReport(date);

                    Label lbDate = new Label();
                    lbDate.Style = Application.Current.FindResource("StyleLabel") as Style;
                    lbDate.Content = report.Day.ToShortDateString();

                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri(View_Utils.IconPath(View_Utils.WeatherToIcon(report.Weather[12]))));
                    image.Width = 50;
                    image.Height = 50;

                    Label lbMin = new Label();
                    lbMin.Style = Application.Current.FindResource("StyleLabelTempMin") as Style;
                    lbMin.Content = report.TMin.ToString("0°");

                    Label lbMax = new Label();
                    lbMax.Style = Application.Current.FindResource("StyleLabelTempMax") as Style;
                    lbMax.Content = report.TMax.ToString("0°");

                    Label lbPression = new Label();
                    lbPression.Style = Application.Current.FindResource("StyleLabel") as Style;
                    lbPression.Content = report.FirstPressure().Value.ToString("0 hp");

                    StackPanel sp = new StackPanel();
                    sp.Orientation = Orientation.Vertical;

                    StackPanel sp_temp = new StackPanel();
                    sp_temp.Orientation = Orientation.Horizontal;

                    sp_temp.Children.Add(lbMin);
                    sp_temp.Children.Add(lbMax);

                    sp.Children.Add(lbDate);
                    sp.Children.Add(image);
                    sp.Children.Add(sp_temp);
                    sp.Children.Add(lbPression);

                    spWeather.Children.Add(sp);

                    date = date.AddDays(1);
                }
            }
        }
    }
}