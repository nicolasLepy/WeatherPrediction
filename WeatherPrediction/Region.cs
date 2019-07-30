using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WeatherPrediction
{
    public class Region
    {

        private string _name;
        private double _mapSizeX;
        private double _mapSizeY;
        private string _mapPath;
        private List<City> _cities;

        public string Name { get => _name; }
        public double MapSizeX { get => _mapSizeX; }
        public double MapSizeY { get => _mapSizeY; }
        public string MapPath { get => _mapPath; }
        public List<City> Cities { get => _cities; }

        public Region(string name, double mapSizeX, double mapSizeY, string mapPath)
        {
            _name = name;
            _mapSizeX = mapSizeX;
            _mapSizeY = mapSizeY;
            _mapPath = mapPath;
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

        private double wk(double xx, double xy, double xkx, double xky)
        {
            return 1.0 / Utils.Distance(xx, xy, xkx, xky);
        }

        public double Indicator(double x, double y, DateTime day, int hour, double p, int indicator_number)
        {
            double numerator = 0;
            double denomiator = 0;
            foreach (City city in _cities)
            {
                Report yesturday = city.GetReport(day.AddDays(-1));
                Report today = city.GetReport(day);
                Report tomorrow = city.GetReport(day.AddDays(1));

                IInterpolation interpolation;
                if(indicator_number == 1)
                    interpolation = Utils.TemperaturesInterpolation(yesturday.TMax, today.TMin, today.TMax, tomorrow.TMin);
                else
                    interpolation = Utils.PressuresInterpolation(yesturday.LastPressure(), today.Pressures, tomorrow.FirstPressure());

                double cityVariable = interpolation.Interpolate(hour);

                double weight = Math.Pow(wk(x, y, city.X, city.Y), p);
                denomiator += weight;
                numerator += weight * cityVariable;
            }

            double localTemp = (numerator + 0.0) / (denomiator + 0.0);
            return localTemp;
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
            return Indicator(x, y, day, hour, p, 1);
        }

        public double Pressure(double x, double y, DateTime day, int hour, double p)
        {
            return Indicator(x, y, day, hour, p, 2);
        }

    }
}