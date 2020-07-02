using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherPrediction
{
    public class Season
    {

        private readonly string _name;
        private readonly DateTime _begin;
        private readonly DateTime _end;
        private readonly double _averageMinPressure;
        private readonly double _averageMaxPressure;
        private readonly bool _winterSeason;

        public string Name => _name;
        public DateTime Begin => _begin;
        public DateTime End => _end;
        public double Average_Min_Pressure => _averageMinPressure;
        public double Average_Max_Pressure => _averageMaxPressure;
        public bool Winter_Season => _winterSeason;

        public Season(string name, DateTime begin, DateTime end, double averageMinPressure, double averageMaxPressure, bool winterSeason)
        {
            _name = name;
            _begin = begin;
            _end = end;
            _averageMaxPressure = averageMaxPressure;
            _averageMinPressure = averageMinPressure;
            _winterSeason = winterSeason;
        }

    }
}
