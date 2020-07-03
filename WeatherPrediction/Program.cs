using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace WeatherPrediction
{

    

    public static class Program
    {
        
        
        static void Main(string[] args)
        {

            var a = Matrix<double>.Build;
            Matrix<double> mat = a.Dense(10, 10, 1020);
            mat.At(3, 3, 1000);
            mat.At(3, 4, 1000);
            mat.At(4, 3, 1000);
            mat.At(4, 4, 1000);
            mat.At(5, 3, 1000);
            mat.At(5, 4, 1000);
            mat.At(4, 5, 1000);
            mat.At(3, 5, 1000);

            Console.WriteLine(mat.ToString());


            Matrix<double> matrix = Utils.CreateMatrix(10, 10, 0);
            matrix.At(5, 8, 1);
            matrix.At(6, 8, 1);
            matrix.At(7, 8, 1);
            matrix.At(5, 9, 1);


            /*
            for (int i = 0;i<50; i++)
            {
                //matrix.ToString();
                //matrix.ComputeCloudiness(matrixPressures);

            }*/
            Console.WriteLine(matrix.ToString());



            System.Console.ReadLine();

        }

    }
}
