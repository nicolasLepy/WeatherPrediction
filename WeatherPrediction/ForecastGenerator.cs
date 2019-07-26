using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WeatherPrediction
{
    public class ForecastGenerator
    {

        private Database _database;
        private DateTime _day;
        private double _alpha;
        private double _beta;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="day"></param>
        /// <param name="alpha"> Smoothness coefficient (between 0 and 1)
        /// 1 : no freedom, forecasts depend entirely from recorded data 
        /// 0 : total freedom </param>
        /// <param name="beta"> Influence of reports from other cities to determine report of a place
        /// from 0 (no influence) to 1 (total influence)
        /// </param>
        public ForecastGenerator(Database database, DateTime day, double alpha, double beta)
        {
            _database = database;
            _day = day;
            _alpha = alpha;
            _beta = beta;
        }

        /// <summary>
        /// Can be improved with a dictionnary [ City, distance]
        /// </summary>
        /// <param name="city"></param>
        /// <param name="max">True : consider TMax / False : consider TMin</param>
        public double InfluenceOtherCities(City city, bool max)
        {
            double tInfluence = 0;
            double distTotale = 0;
            foreach(City c in _database.Cities)
            {
                if(c != city)
                    distTotale += Utils.Distance(c, city);
            }

            foreach (City c in _database.Cities)
            {
                if(c != city)
                {
                    double cityInfluence = (Utils.Distance(c, city)) / (distTotale + 0.0);
                    tInfluence += (max ? c.LastDay(_day).TMax : c.LastDay(_day).TMin) * cityInfluence;
                }
                
            }

            return tInfluence;


        }

        public void GenerateDay()
        {
            _day = _day.AddDays(1);
            Console.WriteLine("Simulate day " + _day.ToShortDateString());
            Normal normalDistribution = new Normal(0, 3);

            foreach(City city in _database.Cities)
            {

                Report lastDay = city.LastDay(_day);
                Report normal = city.DailyNormal(_day);
                
                double tMin = lastDay.TMin + normalDistribution.Sample();
                tMin = tMin + ( _alpha * (normal.TMin - tMin));
                tMin = tMin + (_beta * (InfluenceOtherCities(city, false) - tMin));


                double tMax = lastDay.TMax + normalDistribution.Sample();
                tMax = tMax + (_alpha * (normal.TMax - tMax));
                tMax = tMax + (_beta * (InfluenceOtherCities(city, true) - tMax));

                city.Forecast.Add(new Report(_day, tMin, tMax));
            }

        }

    }
}