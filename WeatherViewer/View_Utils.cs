using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WeatherViewer
{
    public class View_Utils
    {

        public static Color Cloudiness2Color(double cloudiness)
        {
            Color res = new Color();
            res.R = 255;
            res.G = 255;
            res.B = 255;
            float value = (float)cloudiness * 3;
            if (value > 1)
                value = 1;
            res.ScA = value;

            return res;
        }

        public static Color Wind2Color(double cloudiness)
        {
            Color res = new Color();
            res.R = 255;
            res.G = 255;
            res.B = 255;
            float value = (float)cloudiness * 3;
            if (value > 1)
                value = 1;
            if (value < 0)
                value = 0;
            res.ScA = value;

            return res;
        }

        public static Color PressureToColor(double pressure)
        {
            Color res = new Color();
            res.A = 190;
            res.G = 0;
            res.R = 0;

            if (pressure < 990) pressure = 990;
            pressure -= 990;
            byte blue = (byte)(pressure * 4);
            if (blue > 255) blue = 255;
            blue = (byte)(255 - blue);

            res.B = blue;
            

            return res;
        }

        public static Color Temperature2Color(double temperature)
        {
            Color res = new Color();
            res.A = 140;
            
            double red = 0;
            double blue = 20;
            double green = 20;

            if(temperature > 25)
            {
                red = (temperature - 25) * 17;
                if (red > 255) red = 255;
            }
            else
            {
                red = 0;
            }

            if (temperature < 10)
            {
                blue = (30-(temperature + 20)) * 8.5;
                if (blue > 255) blue = 255;
            }
            else
                blue = 0;

            if(temperature > 0 && temperature < 30)
            {
                double tp = 15 - Math.Abs(temperature - 15);
                green = tp * 10;
                if (green > 150) green = 150;
            }

            

            res.R = (byte)red;
            res.B = (byte)blue;
            res.G = (byte)green;
            return res;
        }

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
