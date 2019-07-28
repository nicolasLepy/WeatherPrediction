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
        private List<Season> _seasons;

        public List<City> Cities { get => _cities; }
        public List<Season> Seasons { get => _seasons; }

        public Database()
        {
            _cities = new List<City>();
            _seasons = new List<Season>();
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

        public Season String2Season(string name)
        {
            Season res = null;
            foreach (Season s in _seasons) if (s.Name == name) res = s;
            return res;
        }
    }

    
}