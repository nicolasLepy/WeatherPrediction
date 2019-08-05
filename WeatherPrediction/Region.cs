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
        private Matrix _waterMap;
        private List<RegionalReport> _cloudinessReports;

        public string Name { get => _name; }
        public double MapSizeX { get => _mapSizeX; }
        public double MapSizeY { get => _mapSizeY; }
        public string MapPath { get => _mapPath; }
        public List<City> Cities { get => _cities; }
        public Matrix WaterMap { get => _waterMap; }
        public List<RegionalReport> CloudinessReports { get => _cloudinessReports; }

        public Region(string name, double mapSizeX, double mapSizeY, string mapPath, Matrix waterMap)
        {
            _name = name;
            _mapSizeX = mapSizeX;
            _mapSizeY = mapSizeY;
            _mapPath = mapPath;
            _cities = new List<City>();
            _waterMap = waterMap;
            _cloudinessReports = new List<RegionalReport>();
        }

        public Matrix LastCloudinessReport()
        {
            Matrix res;
            if (CloudinessReports.Count > 0)
                res = CloudinessReports[CloudinessReports.Count - 1].Matrix;
            else
                res = new Matrix(Utils.MATRIX_SIZE, Utils.MATRIX_SIZE, 0);
            return res;
        }

        public Matrix GetCloudinessReport(DateTime date, int hour)
        {
            Matrix res = null;
            foreach(RegionalReport rr in _cloudinessReports)
            {
                if (rr.Date.Year == date.Year && rr.Date.Month == date.Month && rr.Date.Day == date.Day && rr.Hour == hour) res = rr.Matrix;
            }
            return res;
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
                
                if(yesturday == null)
                {
                    yesturday = today;
                }
                if(tomorrow == null)
                {
                    tomorrow = today;
                }

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

        public double Cloudiness(double x, double y, DateTime day, int hour, double p, int widthMap, int heightMap)
        {
            int intX = (int)x;
            int intY = (int)y;
            int indiceX = (int)((Utils.MATRIX_SIZE / (widthMap+0.0))*intX);
            int indiceY = (int)((Utils.MATRIX_SIZE / (heightMap + 0.0)) * intY);

            Matrix cloudiness = GetCloudinessReport(day, hour);
            return cloudiness.Get(indiceX,indiceY);
        }

        public override string ToString()
        {
            return _name;
        }

    }
}