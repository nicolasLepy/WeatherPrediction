using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WeatherPrediction
{
    public class Database
    {
        private List<City> _cities;

        public List<City> Cities { get => _cities; }

        public Database()
        {
            _cities = new List<City>();
        }

        public string ShowForecast()
        {
            string res = "\n\n";
            foreach (City city in _cities)
            {
                res += city.Name + "\n";
                foreach (Report rp in city.Forecast)
                {
                    res += rp.Day.ToShortDateString() + " Min : " + rp.TMin.ToString("0.0") + "°, TMax : " + rp.TMax.ToString("0.0") + "° \t Normales : " + city.DailyNormal(rp.Day).TMin + "-" + city.DailyNormal(rp.Day).TMax + "\n";
                }
            }
            return res;
        }

        public void ResetForecast()
        {
            foreach(City city in Cities)
            {
                city.Forecast.Clear();
            }
        }
    }



    public class Report
    {
        private DateTime _day;
        private double _tMin;
        private double _tMax;

        public DateTime Day { get => _day; }
        public double TMin { get => _tMin; }
        public double TMax { get => _tMax; }

        public Report(DateTime day, double tmin, double tmax)
        {
            _day = day;
            _tMax = tmax;
            _tMin = tmin;
        }
    }
}