using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace WeatherPrediction
{
    public class Loader
    {

        private Database _database;

        public Loader(Database database)
        {
            _database = database;
        }

        public Matrix LoadWaterMap(string path)
        {
            Matrix matrix = null;
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
                    matrix = new Matrix(size, size, 0);
                }
                columnNumber = 0;
                foreach (string s in split)
                {
                    double value = Double.Parse(s, CultureInfo.InvariantCulture);
                    matrix.Set(lineNumber, columnNumber, value);
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
                    Matrix waterMap = LoadWaterMap(pathWaterMap);
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

            
            file.Close();
        }
        

    }
}
 
 