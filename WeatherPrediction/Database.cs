using MathNet.Numerics;
using MathNet.Numerics.Interpolation;
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

        private double Distance2Distance(double distance)
        {
            double res = 0;
            if (distance < 20) res = distance;
            if (distance < 50) res = Math.Pow(distance,2);
            if (distance < 100) res = Math.Pow(distance, 3);
            if (distance < 200) res = Math.Pow(distance, 4);
            if (distance < 300) res = Math.Pow(distance, 5);
            if (distance < 400) res = Math.Pow(distance, 6);
            if (distance < 500) res = Math.Pow(distance, 7);
            if (distance < 600) res = Math.Pow(distance, 8);
            if (distance < 700) res = Math.Pow(distance, 9);
            if (distance < 800) res = Math.Pow(distance, 10);
            if (distance < 900) res = Math.Pow(distance, 11);
            if (distance < 1000) res = Math.Pow(distance, 12);
            if (distance < 1500) res = Math.Pow(distance, 13);
            else Math.Pow(distance, 14);
            return res;
        }

        private double wk(double xx, double xy, double xkx, double xky)
        {
            return 1.0/Utils.Distance(xx, xy, xkx, xky);
        }

        /// <summary>
        /// Get hourly temperature of a point on map
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="hour"></param>
        /// <returns></returns>
        public double Temperature(double x, double y, DateTime day, int hour, double p)
        {

            double numerator = 0;
            double denomiator = 0;
            foreach(City city in _cities)
            {
                Report yesturday = city.GetReport(day.AddDays(-1));
                Report today = city.GetReport(day);
                Report tomorrow = city.GetReport(day.AddDays(1));

                IInterpolation interpolation = Utils.TemperaturesInterpolation(yesturday.TMax, today.TMin, today.TMax, tomorrow.TMin);

                double cityTemperature = interpolation.Interpolate(hour);

                double weight = Math.Pow(wk(x, y, city.X, city.Y),p);
                denomiator += weight;
                numerator += weight * cityTemperature;
            }

            double localTemp = (numerator+0.0) / (denomiator+0.0);
            return localTemp;

        }

        public Season String2Season(string name)
        {
            Season res = null;
            foreach (Season s in _seasons) if (s.Name == name) res = s;
            return res;
        }
    }

    
}