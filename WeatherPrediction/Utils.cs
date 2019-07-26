using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WeatherPrediction
{
    public class Utils
    {

        public static double Distance(City a, City b)
        {
            return Math.Sqrt( Math.Pow( b.X-a.X ,2) + Math.Pow(b.Y - a.Y, 2));
        }

    }
}