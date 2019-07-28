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

        public void ExportDB()
        {
            string res = "";
            foreach(City city in _database.Cities)
            {
                res += "City," + city.Name + "," + city.X + "," + city.Y + Environment.NewLine;
                foreach(Report report in city.Reports)
                {
                    res += report.Day.ToShortDateString() + "," + report.TMin + "," + report.TMax + Environment.NewLine;
                }
            }
            System.IO.File.WriteAllText("db.txt", res);

        }

        public void LoadDB(string chemin)
        {
            _database.Cities.Clear();
            _database.Seasons.Clear();
            string line;
            City city = null;
            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader(chemin);
            while ((line = file.ReadLine()) != null)
            {

                string[] split = line.Split(',');

                if(split[0] == "SeasonDef")
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
                    if (city != null) _database.Cities.Add(city);
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
            _database.Cities.Add(city);

            
            file.Close();
        }
        

    }
}
 
 