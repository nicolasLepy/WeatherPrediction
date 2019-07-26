using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherPrediction
{

    

    public class Program
    {
        

        static void Main(string[] args)
        {

            Database db = new Database();
            Loader ld = new Loader(db);
            ld.LoadDB();


            ForecastGenerator fg = new ForecastGenerator(db, new DateTime(281,5,31),0.3,0.1);
            for(int i = 0; i<30;i++)
                fg.GenerateDay();

            Console.WriteLine(db.ShowForecast());
            System.Console.ReadLine();

        }

    }
}
