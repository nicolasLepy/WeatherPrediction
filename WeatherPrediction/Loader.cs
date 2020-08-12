using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace WeatherPrediction
{
    public class Loader
    {

        private Database _database;

        public Loader(Database database)
        {
            _database = database;
        }

        public Matrix<double> LoadWaterMap(string path)
        {
            Matrix<double> matrix = null;
            System.IO.StreamReader file = new System.IO.StreamReader(System.IO.Directory.GetCurrentDirectory() + "\\waterMaps\\" + path);
            int size = 0;
            int lineNumber = 0;
            int columnNumber = 0;
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string[] split = line.Split(',');
                if (size == 0)
                {
                    size = split.Length;
                    matrix = Utils.CreateMatrix(size,size,0);
                }
                columnNumber = 0;
                foreach (string s in split)
                {
                    double value = Double.Parse(s, CultureInfo.InvariantCulture);
                    matrix.At(lineNumber, columnNumber, value);
                    columnNumber++;
                }
                lineNumber++;
            }
            file.Close();
            return matrix;
        }

        public void LoadDB(string path)
        {
            _database.Regions.Clear();
            _database.Seasons.Clear();
            string line;
            City city = null;
            Region region = null;
            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {

                string[] split = line.Split(',');

                if(split[0] == "Region")
                {
                    string name = split[1];
                    int x = int.Parse(split[2]);
                    int y = int.Parse(split[3]);
                    string map = split[4];
                    string pathWaterMap = split[5];
                    Matrix<double> waterMap = LoadWaterMap(pathWaterMap);
                    region = new Region(name, x, y, map,waterMap);
                }
                else if (split[0] == "EndRegion")
                {
                    _database.Regions.Add(region);
                }
                else if(split[0] == "EndCity")
                {
                    region.Cities.Add(city);
                }
                else if(split[0] == "SeasonDef")
                {
                    string name = split[1];
                    string[] str_begin = split[2].Split('/');
                    string[] str_end = split[3].Split('/');
                    DateTime begin = new DateTime(200, int.Parse(str_begin[1]), int.Parse(str_begin[0]));
                    DateTime end = new DateTime(200, int.Parse(str_end[1]), int.Parse(str_end[0]));
                    double min_pressure = Double.Parse(split[4],CultureInfo.InvariantCulture);
                    double max_pressure = Double.Parse(split[5], CultureInfo.InvariantCulture);
                    bool winter_season = split[6] == "True" ? true : false;
                    _database.Seasons.Add(new Season(name, begin, end, min_pressure, max_pressure, winter_season));
                }
                
                else if(split[0] == "City")
                {
                    string name = split[1];
                    int x = int.Parse(split[2]);
                    int y = int.Parse(split[3]);
                    double altitude = Double.Parse(split[4], CultureInfo.InvariantCulture);
                    city = new City(name, x, y,altitude);
                }
                else if(split[0] == "AddSeason")
                {
                    Season seasonToAdd = _database.String2Season(split[1]);
                    city.Seasons.Add(seasonToAdd);
                }
                else if(split.Length == 3)
                {
                    string[] sDate = split[0].Split('/');
                    int tmin = int.Parse(split[1]);
                    int tmax = int.Parse(split[2]);
                    DateTime date = new DateTime(int.Parse(sDate[2]), int.Parse(sDate[1]), int.Parse(sDate[0]));
                    Report report = new Report(date, tmin, tmax);
                    double avg_pressure = (city.GetSeason(date).Average_Min_Pressure + city.GetSeason(date).Average_Max_Pressure) / 2.0;
                    report.AddPressureValue(20, avg_pressure);
                    city.Reports.Add(report);
                }
            }

            InterpolateValues();
            file.Close();
        }

        /// <summary>
        /// If only one value per month was given, temperature for the other date of the month are interpolated
        /// </summary>
        private void InterpolateValues()
        {
            foreach (Region r in _database.Regions)
            {
                foreach (City c in r.Cities)
                {
                    //If one report per month
                    if (c.Reports.Count == 12)
                    {
                        for (int month = 0; month < 12; month++)
                        {
                            Report monthly = c.Reports[month];
                            for (int i = 2; i <= 31; i++)
                            {
                                try
                                {
                                    DateTime date = new DateTime(monthly.Day.Year, monthly.Day.Month, i);
                                    Report d = new Report(date, monthly.TMin, monthly.TMax);
                                    c.Reports.Add(d);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.ToString());
                                }
                            }
                        }
                    }
   
                }
            }
        }

    }
}
 
 