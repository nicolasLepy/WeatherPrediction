using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace WeatherPrediction
{
    /// <summary>
    /// Represent a hourly/daily weather report for a map
    /// Contain matrix of cloudiness, cloudiness, temperature ...
    /// </summary>
    public class WeatherReport
    {
        private Matrix<double> _cloudinessMatrix;
        private Matrix<double> _windMatrix;
        
        private readonly DateTime _date;
        private readonly int _hour;

        public Matrix<double> CloudinessMatrix => _cloudinessMatrix;
        public Matrix<double> WindMatrix => _windMatrix;

        public DateTime Date => _date;
        public int Hour => _hour;

        public WeatherReport(DateTime date, int hour, Matrix<double> cloudinessMatrix, Matrix<double> windMatrix)
        {
            _date = date;
            _hour = hour;
            _cloudinessMatrix = cloudinessMatrix;
            _windMatrix = windMatrix;
        }
        
    }
    
}