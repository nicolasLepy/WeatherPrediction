﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace WeatherPrediction
{
    public class RegionalReport
    {
        private readonly Matrix<double> _matrix;
        private readonly DateTime _date;
        private readonly int _hour;

        public Matrix<double> Matrix { get => _matrix; }
        public DateTime Date { get => _date; }
        public int Hour { get => _hour; }

        public RegionalReport(DateTime date, int hour, Matrix<double> matrix)
        {
            _matrix = matrix;
            _date = date;
            _hour = hour;
        }
    }
}
