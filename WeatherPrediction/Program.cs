using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherPrediction
{

    

    public class Program
    {
        

        private static void PrintMatrix(Matrix matrix)
        {
            Console.WriteLine("----\n");
            for(int i = 0;i<matrix.Width; i++)
            {
                for(int j = 0;j<matrix.Height;j++)
                {
                    Console.Write(matrix.Get(i, j).ToString("0.00") + "  ");
                }
                Console.WriteLine("");
            }
        }

        static void Main(string[] args)
        {

            Matrix matrixPressures = new Matrix(10, 10, 1020);
            matrixPressures.Set(3, 3, 1000);
            matrixPressures.Set(3, 4, 1000);
            matrixPressures.Set(4, 3, 1000);
            matrixPressures.Set(4, 4, 1000);
            matrixPressures.Set(5, 3, 1000);
            matrixPressures.Set(5, 4, 1000);
            matrixPressures.Set(4, 5, 1000);
            matrixPressures.Set(3, 5, 1000);

            PrintMatrix(matrixPressures);


            Matrix matrix = new Matrix(10, 10, 0);
            matrix.Set(5, 8, 1);
            matrix.Set(6, 8, 1);
            matrix.Set(7, 8, 1);
            matrix.Set(5, 9, 1);


            for (int i = 0;i<50; i++)
            {
                PrintMatrix(matrix);
                //matrix.ComputeCloudiness(matrixPressures);

            }
            PrintMatrix(matrix);



            System.Console.ReadLine();

        }

    }
}
