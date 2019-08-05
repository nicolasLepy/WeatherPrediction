using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherPrediction
{
    public class RegionalReport
    {
        private Matrix _matrix;
        private DateTime _date;
        private int _hour;

        public Matrix Matrix { get => _matrix; }
        public DateTime Date { get => _date; }
        public int Hour { get => _hour; }

        public RegionalReport(DateTime date, int hour, Matrix matrix)
        {
            _matrix = matrix;
            _date = date;
            _hour = hour;
        }
    }
}
