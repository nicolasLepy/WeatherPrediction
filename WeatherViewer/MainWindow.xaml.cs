using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeatherPrediction;

namespace WeatherViewer
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Database _database;
        private ForecastGenerator _forecast;

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }



        private void UpdateChart()
        {
            SeriesCollection.Clear();
            foreach (City city in _database.Cities)
            {
                string title = city.Name + " max";
                List<double> reports = new List<double>();
                foreach (Report rp in city.Forecast)
                {
                    reports.Add(rp.TMax);
                }
                LineSeries ls = new LineSeries();
                ls.Title = title;
                ChartValues<double> cv = new ChartValues<double>(reports);
                ls.Values = cv;
                ls.Stroke = Brushes.IndianRed;
                ls.Fill = Brushes.Transparent;
                SeriesCollection.Add(ls);

                title = city.Name + " min";
                reports = new List<double>();
                foreach (Report rp in city.Forecast)
                {
                    reports.Add(rp.TMin);
                }
                ls = new LineSeries();
                ls.Title = title;
                cv = new ChartValues<double>(reports);
                ls.Values = cv;
                ls.Stroke = Brushes.LightSkyBlue;
                ls.Fill = Brushes.Transparent;
                SeriesCollection.Add(ls);
            }

            List<string> labelsList = new List<string>();
            foreach(Report rp in _database.Cities[0].Forecast)
            {
                labelsList.Add(rp.Day.ToShortDateString());
            }

            Labels = labelsList.ToArray();
            YFormatter = value => value.ToString("0.0°");
            Console.WriteLine("hello");
            DataContext = this;

        }

        public MainWindow()
        {
            InitializeComponent();

            img1.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\img\\sun.png"));
            img2.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\img\\sun.png"));
            img3.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\img\\sun.png"));
            img4.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\img\\sun.png"));
            img5.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\img\\sun.png"));
            img6.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\img\\sun.png"));
            img7.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\img\\sun.png"));


            _database = new Database();
            //Loader ld = new Loader(_database);
            //ld.LoadDB();
            //_forecast = new ForecastGenerator(_database, new DateTime(281, 5, 31), 0.3, 0.1);
            //for (int i = 0; i < 30; i++)
            //    _forecast.GenerateDay();

            SeriesCollection = new SeriesCollection();

        }

        private void UpdateWidgets()
        {
            lbCities.Items.Clear();
            foreach (City city in _database.Cities)
            {
                lbCities.Items.Add(city);
            }

        }

        private void RestartComputation()
        {
            _database.ResetForecast();
            _forecast = new ForecastGenerator(_database, new DateTime(281, 5, 31), Double.Parse(tbAlpha.Text, CultureInfo.InvariantCulture), Double.Parse(tbBeta.Text, CultureInfo.InvariantCulture));
            for (int i = 0; i < 30; i++)
                _forecast.GenerateDay();
            UpdateChart();
        }

        private void TbAlpha_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void TbBeta_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void BtnRestart_Click(object sender, RoutedEventArgs e)
        {
            RestartComputation();
        }

        private void LbCities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            City city = lbCities.SelectedItem as City;
            if(city != null && city.Forecast.Count > 6)
            {
                lbCityName.Content = city.Name;
                lb1Min.Content = city.Forecast[0].TMin.ToString("0°");
                lb1Max.Content = city.Forecast[0].TMax.ToString("0°");
                lb2Min.Content = city.Forecast[1].TMin.ToString("0°");
                lb2Max.Content = city.Forecast[1].TMax.ToString("0°");
                lb3Min.Content = city.Forecast[2].TMin.ToString("0°");
                lb3Max.Content = city.Forecast[2].TMax.ToString("0°");
                lb4Min.Content = city.Forecast[3].TMin.ToString("0°");
                lb4Max.Content = city.Forecast[3].TMax.ToString("0°");
                lb5Min.Content = city.Forecast[4].TMin.ToString("0°");
                lb5Max.Content = city.Forecast[4].TMax.ToString("0°");
                lb6Min.Content = city.Forecast[5].TMin.ToString("0°");
                lb6Max.Content = city.Forecast[5].TMax.ToString("0°");
                lb7Min.Content = city.Forecast[6].TMin.ToString("0°");
                lb7Max.Content = city.Forecast[6].TMax.ToString("0°");
                
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Loader ld = new Loader(_database);
                ld.LoadDB(openFileDialog.FileName);
                UpdateWidgets();
            }
        }
    }
}
