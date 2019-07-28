using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherViewer
{
    public class View_Utils
    {

        public static string IconPath(string iconName)
        {
            return System.IO.Directory.GetCurrentDirectory() + "\\img\\" + iconName;
        }

        public static string WeatherToIcon(double weather)
        {
            string icon = "";
            if (weather < 1.5) icon = "32.png";
            else if (weather < 2.5) icon = "32.png";
            else if (weather < 3.5) icon = "34.png";
            else if (weather < 4.5) icon = "30.png";
            else if (weather < 5.5) icon = "28.png";
            else if (weather < 6.5) icon = "28.png";
            else if (weather < 7.5) icon = "25.png";
            else if (weather < 8.5) icon = "25.png";
            else if (weather < 9.5) icon = "12.png";
            else if (weather < 10.5) icon = "32.png";
            else if (weather < 11.5) icon = "32.png";
            else if (weather < 12.5) icon = "34.png";
            else if (weather < 13.5) icon = "34.png";
            else if (weather < 14.5) icon = "30.png";
            else if (weather < 15.5) icon = "28.png";
            else if (weather < 16.5) icon = "25.png";
            else if (weather < 17.5) icon = "12.png";
            else if (weather < 18.5) icon = "12.png";
            else if (weather < 19.5) icon = "0.png";
            else if (weather < 20.5) icon = "32.png";
            else if (weather < 21.5) icon = "32.png";
            else if (weather < 22.5) icon = "34.png";
            else if (weather < 23.5) icon = "30.png";
            else if (weather < 24.5) icon = "28.png";
            else if (weather < 25.5) icon = "28.png";
            else if (weather < 26.5) icon = "28.png";
            else if (weather < 27.5) icon = "26.png";
            else if (weather < 28.5) icon = "25.png";
            else if (weather < 29.5) icon = "25.png";
            else if (weather < 30.5) icon = "0.png";
            else if (weather < 31.5) icon = "2.png";
            else icon = "3.png";

            return icon;
        }

    }
}
