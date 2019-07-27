using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WeatherPrediction
{
    public class City
    {
        private string _name;
        private int _x;
        private int _y;
        private List<Report> _reports;
        private List<Report> _forecast;

        public string Name { get => _name; }
        public int X { get => _x; }
        public int Y { get => _y; }
        public List<Report> Reports { get => _reports; }
        public List<Report> Forecast { get => _forecast; }


        public Report LastDay(DateTime reference)
        {
            Report res = null;
            if (Forecast.Count > 0)
                res = Forecast[Forecast.Count - 1];
            else
                foreach(Report rp in Reports)
                {
                    Console.WriteLine(rp.Day.ToShortDateString());
                    if (rp.Day.Month == reference.Month && rp.Day.Day == reference.Day)
                        res = rp;
                }
            return res;
            
        }

        public Report GetReport(DateTime date)
        {
            Report res = null;
            foreach(Report report in Forecast)
            {
                if (report.Day.Month == date.Month && report.Day.Day == date.Day && report.Day.Year == date.Year)
                    res = report;
            }
            return res;
        }

        public Report DailyNormal(DateTime date)
        {
            Report res = null;
            foreach(Report rp in Reports)
            {
                if (rp.Day.Month == date.Month && rp.Day.Day == date.Day)
                    res = rp;
            }
            return res;
        }
        

        public City(string name, int x, int y)
        {
            _name = name;
            _x = x;
            _y = y;
            _reports = new List<Report>();
            _forecast = new List<Report>();
        }

        
        public override string ToString()
        {
            return _name;
        }
    }
}