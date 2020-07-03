using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace WeatherPrediction
{
    public class Region
    {

        private readonly string _name;
        private readonly double _mapSizeX;
        private readonly double _mapSizeY;
        private readonly string _mapPath;
        private readonly List<City> _cities;
        private readonly Matrix<double> _waterMap;
        private readonly List<RegionalReport> _windReports;
        private readonly List<RegionalReport> _cloudinessReports;

        public string Name => _name;
        public double MapSizeX => _mapSizeX;
        public double MapSizeY => _mapSizeY;
        public string MapPath => _mapPath;
        public List<City> Cities => _cities;
        public Matrix<double> WaterMap => _waterMap;
        public List<RegionalReport> WindReports => _windReports;
        public List<RegionalReport> CloudinessReports => _cloudinessReports;

        public Region(string name, double mapSizeX, double mapSizeY, string mapPath, Matrix<double> waterMap)
        {
            _name = name;
            _mapSizeX = mapSizeX;
            _mapSizeY = mapSizeY;
            _mapPath = mapPath;
            _cities = new List<City>();
            _waterMap = waterMap;
            _cloudinessReports = new List<RegionalReport>();
            _windReports = new List<RegionalReport>();
        }

        public Matrix<double> LastCloudinessReport()
        {
            Matrix<double> res;
            if (CloudinessReports.Count > 0)
            {
                res = CloudinessReports[CloudinessReports.Count - 1].Matrix;
            }
            else
            {
                res = Utils.CreateMatrix(Utils.MATRIX_SIZE, Utils.MATRIX_SIZE, 0);
            }
            return res;
        }

        public Matrix<double> GetCloudinessReport(DateTime date, int hour)
        {
            Matrix<double> res = null;
            foreach(RegionalReport rr in _cloudinessReports)
            {
                if (rr.Date.Year == date.Year && rr.Date.Month == date.Month && rr.Date.Day == date.Day &&
                    rr.Hour == hour)
                {
                    res = rr.Matrix;
                }
            }
            return res;
        }

        public Matrix<double> GetWindReport(DateTime date, int hour)
        {
            Matrix<double> res = null;
            foreach (RegionalReport rr in _windReports)
            {
                if (rr.Date.Year == date.Year && rr.Date.Month == date.Month && rr.Date.Day == date.Day &&
                    rr.Hour == hour)
                {
                    res = rr.Matrix;
                }
            }
            return res;
        }

        public string ShowForecast()
        {
            StringBuilder res = new StringBuilder("\n\n");
            foreach (City city in _cities)
            {
                
                res.Append(city.Name).Append("\n");
                foreach (Report rp in city.Forecast)
                {
                    res.Append(rp.Day.ToShortDateString()).Append(" Min : ").Append(rp.TMin.ToString("0.0")).Append("°, TMax : ").Append(rp.TMax.ToString("0.0")).Append("° \t Normales : ").Append(city.DailyNormal(rp.Day).TMin).Append("-").Append(city.DailyNormal(rp.Day).TMax).Append("\n");
                }
            }
            return res.ToString();
        }

        private double wk(double xx, double xy, double xkx, double xky)
        {
            return 1.0 / Utils.Distance(xx, xy, xkx, xky);
        }

        public double Indicator(double x, double y, DateTime day, int hour, double p, int indicator_number)
        {
            double numerator = 0;
            double denominator = 0;
            foreach (City city in _cities)
            {
                Report yesterday = city.GetReport(day.AddDays(-1));
                Report today = city.GetReport(day);
                Report tomorrow = city.GetReport(day.AddDays(1));
                
                if(yesterday == null)
                {
                    yesterday = today;
                }
                if(tomorrow == null)
                {
                    tomorrow = today;
                }

                IInterpolation interpolation;
                if (indicator_number == 1)
                {
                    interpolation = Utils.TemperaturesInterpolation(yesterday.TMax, today.TMin, today.TMax, tomorrow.TMin);
                }
                else
                {
                    interpolation = Utils.PressuresInterpolation(yesterday.LastPressure(), today.Pressures, tomorrow.FirstPressure());
                }


                double cityVariable = interpolation.Interpolate(hour);
                


                double weight = Math.Pow(wk(x, y, city.X, city.Y), p);
                denominator += weight;
                numerator += weight * cityVariable;
            }

            double localTemp = (numerator + 0.0) / (denominator + 0.0);
            return localTemp;
        }

        /// <summary>
        /// Get hourly temperature of a point on map
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="day"></param>
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
            int indexX = (int)((Utils.MATRIX_SIZE / (widthMap+0.0))*intX);
            int indexY = (int)((Utils.MATRIX_SIZE / (heightMap + 0.0)) * intY);

            Matrix<double> cloudiness = GetCloudinessReport(day, hour);
            return cloudiness.At(indexX,indexY);
        }

        public double Wind(double x, double y, DateTime day, int hour, double p, int widthMap, int heightMap)
        {
            int intX = (int)x;
            int intY = (int)y;
            int indexX = (int)((Utils.MATRIX_SIZE / (widthMap + 0.0)) * intX);
            int indexY = (int)((Utils.MATRIX_SIZE / (heightMap + 0.0)) * intY);

            Matrix<double> wind = GetWindReport(day, hour);

            return wind.At(indexX, indexY);
        }

        public override string ToString()
        {
            return _name;
        }

    }
}