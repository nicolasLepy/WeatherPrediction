using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherPrediction
{
    public class Season
    {

        private string _name;
        private DateTime _begin;
        private DateTime _end;
        private double _average_min_pressure;
        private double _average_max_pressure;
        private bool _winter_season;

        public string Name { get => _name; }
        public DateTime Begin { get => _begin; }
        public DateTime End { get => _end; }
        public double Average_Min_Pressure { get => _average_min_pressure; }
        public double Average_Max_Pressure { get => _average_max_pressure; }
        public bool Winter_Season { get => _winter_season; }

        public Season(string name, DateTime begin, DateTime end, double average_min_pressure, double average_max_pressure, bool winter_season)
        {
            _name = name;
            _begin = begin;
            _end = end;
            _average_max_pressure = average_max_pressure;
            _average_min_pressure = average_min_pressure;
            _winter_season = winter_season;
        }

    }
}
