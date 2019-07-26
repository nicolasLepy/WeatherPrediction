using System;
using System.Collections.Generic;
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

        public void LoadDB()
        {
            string line;
            City city = null;
            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader("db.txt");
            while ((line = file.ReadLine()) != null)
            {

                string[] split = line.Split(',');

                if(split[0] == "City")
                {
                    if (city != null) _database.Cities.Add(city);
                    string name = split[1];
                    int x = int.Parse(split[2]);
                    int y = int.Parse(split[3]);
                    city = new City(name, x, y);
                }
                else if(split.Length == 3)
                {
                    string[] sDate = split[0].Split('/');
                    int tmin = int.Parse(split[1]);
                    int tmax = int.Parse(split[2]);
                    DateTime date = new DateTime(int.Parse(sDate[2]), int.Parse(sDate[1]), int.Parse(sDate[0]));
                    city.Reports.Add(new Report(date, tmin, tmax));
                }

                
            }
            _database.Cities.Add(city);

            file.Close();
        }
        

    }
}