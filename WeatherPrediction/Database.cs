using MathNet.Numerics;
using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WeatherPrediction
{
    public class Database
    {
        private readonly List<Region> _regions;
        private readonly List<Season> _seasons;

        public List<Region> Regions { get => _regions; }
        public List<Season> Seasons { get => _seasons; }

        public Database()
        {
            _regions = new List<Region>();
            _seasons = new List<Season>();
        }
        
        public void ResetForecast()
        {
            foreach(Region region in _regions)
            {
                foreach (City city in region.Cities)
                {
                    city.Forecast.Clear();
                }
            }
            
        }

        public Season String2Season(string name)
        {
            Season res = null;
            foreach (Season s in _seasons)
            {
                if (s.Name == name)
                {
                    res = s;
                }
            }
            return res;
        }
    }
}