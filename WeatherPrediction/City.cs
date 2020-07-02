using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WeatherPrediction
{
    public class City
    {
        private readonly string _name;
        private readonly int _x;
        private readonly int _y;
        private readonly List<Report> _reports;
        private readonly List<Report> _forecast;
        private readonly List<Season> _seasons;
        private readonly double _altitude;

        public string Name => _name;
        public int X => _x;
        public int Y => _y;
        public List<Report> Reports => _reports;
        public List<Report> Forecast => _forecast;
        public List<Season> Seasons => _seasons;
        public double Altitude => _altitude;

        
        /// <summary>
        /// Get latest report for a day
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public Report LastDay(DateTime reference)
        {
            Report res = null;
            if (Forecast.Count > 0)
            {
                res = Forecast[Forecast.Count - 1];
            }
            else
            {
                foreach(Report rp in Reports)
                {
                    Console.WriteLine(rp.Day.ToShortDateString());
                    if (rp.Day.Month == reference.Month && rp.Day.Day == reference.Day)
                    {
                        res = rp;
                    }
                }
            }
            return res;
            
        }

        /// <summary>
        /// Obtain foreast report for a day
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public Report GetReport(DateTime date)
        {
            Report res = null;
            foreach(Report report in Forecast)
            {
                if (report.Day.Month == date.Month && report.Day.Day == date.Day && report.Day.Year == date.Year)
                {
                    res = report;
                }
            }
            return res;
        }

        /// <summary>
        /// Give seasonal normal temperature for a day
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public Report DailyNormal(DateTime date)
        {
            Report res = null;
            foreach(Report rp in Reports)
            {
                if (rp.Day.Month == date.Month && rp.Day.Day == date.Day)
                {
                    res = rp;
                }
            }
            return res;
        }
        
        /// <summary>
        /// Get season of a day
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public Season GetSeason(DateTime day)
        {
            Season res = null;
            foreach(Season season in _seasons)
            {
                if(day.DayOfYear >= season.Begin.DayOfYear && day.DayOfYear <= season.End.DayOfYear)
                {
                    res = season;
                }
            }
            return res;
        }

        public City(string name, int x, int y, double altitude)
        {
            _name = name;
            _x = x;
            _y = y;
            _reports = new List<Report>();
            _forecast = new List<Report>();
            _seasons = new List<Season>();
            _altitude = altitude;
        }

        
        public override string ToString()
        {
            return _name;
        }
    }
}