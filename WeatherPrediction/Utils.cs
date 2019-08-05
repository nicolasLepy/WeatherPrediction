﻿using MathNet.Numerics;
using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WeatherPrediction
{
    public class Utils
    {
        public static int MATRIX_SIZE = 20;

        public static double Distance(double xa, double ya, double xb, double yb)
        {
            return Math.Sqrt(Math.Pow(xb - xa, 2) + Math.Pow(yb - ya, 2));
        }

        public static double Distance(City a, City b)
        {
            return Math.Sqrt( Math.Pow( b.X-a.X ,2) + Math.Pow(b.Y - a.Y, 2));
        }

        public static IInterpolation TemperaturesInterpolation(double tmax_yesturday, double tmin, double tmax, double tmin_tomorrow)
        {
            List<double> hours = new List<double>();
            hours.Add(-6);
            hours.Add(4.5);
            hours.Add(5);
            hours.Add(17);
            hours.Add(17.5);
            hours.Add(29);
            List<double> temp = new List<double>();
            temp.Add(tmax_yesturday);
            temp.Add(tmin);
            temp.Add(tmin);
            temp.Add(tmax);
            temp.Add(tmax);
            temp.Add(tmin_tomorrow);
            return Interpolate.Polynomial(hours, temp);
        }

        public static IInterpolation PressuresInterpolation(KeyValuePair<int, double> lastPressure_yesturday, List<KeyValuePair<int, double>> pressures, KeyValuePair<int, double> firstPressure_tommorow)
        {
            List<double> hourPressure = new List<double>();
            List<double> valuesPressure = new List<double>();
            hourPressure.Add(0 - (24 - lastPressure_yesturday.Key));
            hourPressure.Add(-0.1 - (24 - lastPressure_yesturday.Key));
            valuesPressure.Add(lastPressure_yesturday.Value);
            valuesPressure.Add(lastPressure_yesturday.Value);
            foreach (KeyValuePair<int, double> kvp in pressures)
            {
                hourPressure.Add(kvp.Key);
                hourPressure.Add(kvp.Key - 0.1);
                valuesPressure.Add(kvp.Value);
                valuesPressure.Add(kvp.Value);
            }
            hourPressure.Add(24 + firstPressure_tommorow.Key);
            hourPressure.Add(24.1 + firstPressure_tommorow.Key);
            valuesPressure.Add(firstPressure_tommorow.Value);
            valuesPressure.Add(firstPressure_tommorow.Value);
            return Interpolate.Polynomial(hourPressure, valuesPressure);
        }

        /// <summary>
        /// Generate a matrix with pressures of a region
        /// </summary>
        /// <param name="region"></param>
        /// <param name="date"></param>
        /// <param name="hour"></param>
        /// <returns></returns>
        public static Matrix GeneratePressuresMatrix(Region region, DateTime date, int hour)
        {   
            Matrix res = new Matrix(MATRIX_SIZE, MATRIX_SIZE, 0);

            for(int i = 0; i<MATRIX_SIZE; i++)
            {
                for(int j = 0; j<MATRIX_SIZE; j++)
                {
                    double iOnMap = (region.MapSizeX / (MATRIX_SIZE+0.0)) * i;
                    double jOnMap = (region.MapSizeY / (MATRIX_SIZE + 0.0)) * i;
                    double value = region.Indicator(iOnMap, jOnMap, date, hour, 2, 2);
                    res.Set(i,j,value);
                }
            }

            return res;
        }

        

    }
}