﻿using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace WeatherPrediction
{
    public class ForecastGenerator
    {

        private readonly Random _random;

        private readonly Region _region;
        private DateTime _day;
        private readonly double _alpha;
        private readonly double _beta;
        private readonly double _gamma;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="day"></param>
        /// <param name="alpha"> Smoothness coefficient for temperatures (between 0 and 1)
        /// 1 : no freedom, forecasts depend entirely from recorded data 
        /// 0 : total freedom </param>
        /// <param name="beta"> Influence of reports from other cities to determine report of a place
        /// from 0 (no influence) to 1 (total influence)
        /// </param>
        /// <param name="gamma"> Smoothness coefficient for athmospheric pressure (between 0 and 1)
        /// </param>
        public ForecastGenerator(Region region, DateTime day, double alpha, double beta, double gamma)
        {
            _random = new Random();
            _region = region;
            _day = day;
            _alpha = alpha;
            _beta = beta;
            _gamma = gamma;
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
            foreach(City c in _region.Cities)
            {
                if (c != city)
                {
                    distTotale += Utils.Distance(c, city);
                }
            }

            foreach (City c in _region.Cities)
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
            //Normal distribution used for temperature variation
            Normal normalDistribution = new Normal(0, 3);
            //Normal distribution used for pressure variation
            Normal normalDistributionPressure = new Normal(0, 1);

            foreach(City city in _region.Cities)
            {

                Report lastDay = city.LastDay(_day);
                Report normal = city.DailyNormal(_day);
                
                double tMin = lastDay.TMin + normalDistribution.Sample();
                tMin = tMin + ( _alpha * (normal.TMin - tMin));
                tMin = tMin + (_beta * (InfluenceOtherCities(city, false) - tMin));


                double tMax = lastDay.TMax + normalDistribution.Sample();
                tMax = tMax + (_alpha * (normal.TMax - tMax));
                tMax = tMax + (_beta * (InfluenceOtherCities(city, true) - tMax));
                
                Report report = new Report(_day, tMin, tMax);

                //Athmospheric pressure

                double lastPressure = lastDay.LastPressure().Value;
                //Generate 3 local extremum every day (0-7h, 8-15h, 16-23h)
                for (int i = 0; i<3; i++)
                {
                    int hour = _random.Next(i * 8, ((i + 1) * 8) - 1);

                    double grossPressure = lastPressure + normalDistributionPressure.Sample();
                    //If it's the first pressure calculated for the day, applying smoothing on it
                    if (hour < 8)
                    {
                        double avgPressure = (city.GetSeason(_day).Average_Max_Pressure + city.GetSeason(_day).Average_Min_Pressure) / 2.0;
                        grossPressure = grossPressure + (_gamma * ( avgPressure - grossPressure ));
                    }
                    report.AddPressureValue(hour, grossPressure);
                    lastPressure = grossPressure;
                }
                
                report.SetWeather(city.GetSeason(_day),lastDay, city.Altitude);
                city.Forecast.Add(report);
            }

            //Regional reports
            for(int hour = 0; hour<24; hour++)
            {
                Matrix<double> pressures = Utils.GeneratePressuresMatrix(_region, _day, hour);
                Matrix<double> cloudinessMatrix = _region.LastCloudinessReport().Clone();
                Utils.ComputeCloudiness(cloudinessMatrix, pressures, _region.WaterMap);

                Matrix<double> wind = Utils.CreateMatrix(Utils.MATRIX_SIZE, Utils.MATRIX_SIZE, 0);
                Utils.EdgeDetection(wind, cloudinessMatrix);

                _region.Reports.Add(new WeatherReport(_day, hour, cloudinessMatrix, wind));
            }
        }

    }
}