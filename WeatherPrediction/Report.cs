using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherPrediction
{
    public class Report
    {
        private DateTime _day;
        private double _tMin;
        private double _tMax;
        private List<KeyValuePair<int, double>> _pressures;
        private List<double> _weather;

        public DateTime Day { get => _day; }
        public double TMin { get => _tMin; }
        public double TMax { get => _tMax; }
        public List<KeyValuePair<int, double>> Pressures { get => _pressures; }
        /// <summary>
        /// Weather hour by hour
        /// </summary>
        public List<double> Weather { get => _weather; }

        public Report(DateTime day, double tmin, double tmax)
        {
            _day = day;
            _tMax = tmax;
            _tMin = tmin;
            _pressures = new List<KeyValuePair<int, double>>();
            _weather = new List<double>();
        }

        public void AddPressureValue(int hour, double pressure)
        {
            _pressures.Add(new KeyValuePair<int, double>(hour, pressure));
        }

        /// <summary>
        /// Return last pressure extremum for this report
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<int, double> LastPressure()
        {
            return _pressures[_pressures.Count - 1];
        }

        /// <summary>
        /// Return first pressure extremum expected for this report
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<int,double> FirstPressure()
        {
            return _pressures[0];
        }

        public void SetWeather(Season season, Report yesturday, double altitude)
        {

            IInterpolation interpolation_pressures = Utils.PressuresInterpolation(yesturday.LastPressure(), Pressures, LastPressure());
            IInterpolation interpolation_temperatures = Utils.TemperaturesInterpolation(yesturday.TMax, TMin, TMax, TMin);

            for(int i = 0; i<24;i++)
            {
                double pressure = interpolation_pressures.Interpolate(i);
                double temperature = interpolation_temperatures.Interpolate(i);
                double p0 = pressure * Math.Pow((1 - ((0.0065 * altitude) / (temperature + 0.0065 * altitude + 273.15))), -5.257);
                p0 *= 10;
                double morning_pressure = FirstPressure().Value;
                double evening_pressure = LastPressure().Value;
                //Pressure is rising
                double z = 0;
                if (evening_pressure - morning_pressure > 3)
                {
                    z = 179 - (2 * p0) / 129;
                }
                //Pressure is falling
                else if (evening_pressure - morning_pressure < 3)
                {
                    z = 130 - p0 / 81;
                }
                //Pressure is steady
                else
                {
                    z = 147 - (5 * p0) / 376;
                }

                if (season != null)
                {
                    if (season.Winter_Season) z--;
                    else z++;
                }
                else
                {
                    Console.WriteLine(_day.ToShortDateString() + " don't have season.");
                }
                _weather.Add(z);
                Console.WriteLine(i + "h : " + z);
            }
            Console.WriteLine("------");

            

            


            
            
        }
    }
}
