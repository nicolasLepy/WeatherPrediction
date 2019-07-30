using MathNet.Numerics;
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
    /// Logique d'interaction pour Report_Window.xaml
    /// </summary>
    public partial class Report_Window : System.Windows.Window
    {

        private Database _database;

        public Report_Window(Database database)
        {
            InitializeComponent();
            _database = database;

            foreach(Region region in _database.Regions)
            {
                foreach (City city in region.Cities)
                {
                    cbCities.Items.Add(city);
                }
            }
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            City city = cbCities.SelectedItem as City;
            if(city != null)
            {
                string[] timeString = dpDate.Text.Split('/');
                DateTime dateVal = new DateTime(int.Parse(timeString[2]), int.Parse(timeString[1]), int.Parse(timeString[0]));
                Report rp = city.GetReport(dateVal);
                Report rp_before = city.GetReport(dateVal.AddDays(-1));
                Report rp_after = city.GetReport(dateVal.AddDays(1));

                //Create polynomial interpolation for temperatures
                IInterpolation interpolation = Utils.TemperaturesInterpolation(rp_before.TMax, rp.TMin, rp.TMax, rp_after.TMin);

                //Create polynomial interpolation for pressures
                IInterpolation interpolation_pressure = Utils.PressuresInterpolation(rp_before.LastPressure(), rp.Pressures, rp_after.FirstPressure());

                dgForecast.Items.Clear();

                for (int i = 0; i<24; i++)
                {
                    double tempe = interpolation.Interpolate(i);
                    double pressure = interpolation_pressure.Interpolate(i);
                    double weather = rp.Weather[i];
                    dgForecast.Items.Add(new HourlyReport() { Hour = i + "h", Temperature = tempe.ToString("0.0") +"°", Color = GetTemperatureColor(tempe), Pressure = pressure.ToString("0.0 hp"), Icon=View_Utils.IconPath(View_Utils.WeatherToIcon(weather)) });
                    
                }
            }
        }

        public string GetTemperatureColor(double temperature)
        {
            string res = "0";

            if (temperature < -15) res = "0";
            else if (temperature < -5) res = "1";
            else if (temperature < 7) res = "2";
            else if (temperature < 22) res = "3";
            else if (temperature < 28) res = "4";
            else if (temperature < 34) res = "5";
            else if (temperature < 42) res = "6";
            else if (temperature < 47) res = "7";
            else res = "8";

            return res;
        }
    }

    public class HourlyReport
    {
        public string Color { get; set; }
        public string Hour { get; set; }
        public string Temperature { get; set; }
        public string Pressure { get; set; }
        public string Icon { get; set; }
    }
}
