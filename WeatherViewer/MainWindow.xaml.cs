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

        public SeriesCollection SeriesCollection_Pressure { get; set; }
        public string[] Labels_Pressure { get; set; }
        public Func<double, string> YFormatter_Pressure { get; set; }

        private void UpdateChartPressures()
        {
            SeriesCollection_Pressure.Clear();
            foreach(City city in _database.Cities)
            {
                string title = city.Name;
                List<double> reports = new List<double>();
                foreach(Report rp in city.Forecast)
                {
                    reports.Add(rp.FirstPressure().Value);
                }
                LineSeries ls = new LineSeries();
                ls.Title = title;
                ChartValues<double> cv = new ChartValues<double>(reports);
                ls.Values = cv;
                ls.Stroke = Brushes.DarkGreen;
                ls.Fill = Brushes.Transparent;
                SeriesCollection_Pressure.Add(ls);

                
            }

            List<string> labelsList = new List<string>();
            foreach (Report rp in _database.Cities[0].Forecast)
            {
                labelsList.Add(rp.Day.ToShortDateString());
            }

            Labels_Pressure = labelsList.ToArray();
            YFormatter_Pressure = value => value.ToString("0.0 hp");
        }

        private void UpdateChartTemperatures()
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

        }

        public MainWindow()
        {
            InitializeComponent();
            
            _database = new Database();
            
            SeriesCollection = new SeriesCollection();
            SeriesCollection_Pressure = new SeriesCollection();

        }

        private void UpdateWidgets()
        {
            

        }

        private void RestartComputation()
        {
            _database.ResetForecast();
            _forecast = new ForecastGenerator(_database, new DateTime(281, 5, 31), Double.Parse(tbAlpha.Text, CultureInfo.InvariantCulture), Double.Parse(tbBeta.Text, CultureInfo.InvariantCulture), Double.Parse(tbGamma.Text, CultureInfo.InvariantCulture));
            for (int i = 0; i < 60; i++)
                _forecast.GenerateDay();
            UpdateChartTemperatures();
            UpdateChartPressures();
            DataContext = this;

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

        private void BtnForecast_Click(object sender, RoutedEventArgs e)
        {
            Report_Window rw = new Report_Window(_database);
            rw.Show();
        }

        private void BtnForecast_Click_1(object sender, RoutedEventArgs e)
        {
            WeatherForecast_Window wfw = new WeatherForecast_Window(_database);
            wfw.Show();
        }
    }
}
