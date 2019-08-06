using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WeatherPrediction
{
    public class Matrix
    {

        private double[,] _array;

        public int Width { get => _array.GetLength(0); }
        public int Height { get => _array.GetLength(1); }

        public Matrix(int width, int height, double initValue)
        {
            _array = new double[width, height];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    _array[i, j] = initValue;
        }
        
        public double Get(int line, int column)
        {
            return _array[line, column];
        }

        public void Set(int line, int column, double value)
        {
            _array[line, column] = value;
        }

        public bool ValidPosition(int x, int y)
        {
            bool res = false;
            if (x < Width && y < Height && x>0 && y>0) res = true;
            return res;
        }


        private void ComputeCloudinessStep(int i, int j, int ni, int nj, Matrix copy, Matrix pressures)
        {
            double ratioPressures = Math.Pow(pressures.Get(i, j)/pressures.Get(ni, nj),30);
            //Console.WriteLine(pressures.Get(i, j) + "->" + pressures.Get(ni,nj) + " : " + ratioPressures.ToString("0.00"));
            double newValue = Get(i, j) * 0.6 * ratioPressures;
            if (newValue < 0.01) newValue = 0;
            copy.Set(ni,nj, newValue);
        }

        public void GenerateClouds(Matrix waterMap)
        {
            Random r = new Random();

            for(int i = 0;i<Width; i++)
            {
                for(int j = 0;j<Height; j++)
                {
                    if (waterMap.Get(i,j) == 1 && r.Next(1, 4) == 2)
                    {
                        Set(i, j, Get(5, 8) + r.NextDouble() * 0.4 + 0.1);
                    }
                }
            }            
        }

        /// <summary>
        /// Edge detection to estimate the wind thanks to cloudiness
        /// </summary>
        /// <param name="cloudiness"></param>
        public void EdgeDetection(Matrix cloudiness)
        {
            for(int i = 1; i<Width-1; i++)
            {
                for(int j = 1; j<Height-1; j++)
                {
                    double c00 = cloudiness.Get(i - 1, j - 1) * -1;
                    double c01 = cloudiness.Get(i - 1, j) * -1;
                    double c02 = cloudiness.Get(i - 1, j + 1) * -1;
                    double c10 = cloudiness.Get(i, j - 1) * -1;
                    double c11 = cloudiness.Get(i, j) * 8;
                    double c12 = cloudiness.Get(i, j + 1) * -1;
                    double c20 = cloudiness.Get(i + 1, j - 1) * -1;
                    double c21 = cloudiness.Get(i + 1, j) * -1;
                    double c22 = cloudiness.Get(i + 1, j + 1) * -1;
                    double value = c00 + c01 + c02 + c10 + c11 + c12 + c20 + c21 + c22;
                    if (value < 0) value = 0;
                    Set(i, j, value);
                }
            }
        }

        public void ComputeCloudiness(Matrix pressures, Matrix waterMap)
        {
            if (pressures.Width != Width || pressures.Height != Height)
                throw new Exception("The pressure matrix must have the same size");

            GenerateClouds(waterMap);
            Matrix copy = Copy();

            for(int i = 0;i<Width; i++)
            {
                for(int j = 0; j<Height; j++)
                {
                    if(Get(i,j)>0)
                    {

                        if (ValidPosition(i + 1, j - 1)) ComputeCloudinessStep(i, j, i + 1, j - 1, copy, pressures);
                        if (ValidPosition(i + 1, j)) ComputeCloudinessStep(i, j, i + 1, j, copy, pressures);
                        if (ValidPosition(i + 1, j + 1)) ComputeCloudinessStep(i, j, i + 1, j + 1, copy, pressures);
                        if (ValidPosition(i, j - 1)) ComputeCloudinessStep(i, j, i, j - 1, copy, pressures);
                        if (ValidPosition(i, j + 1)) ComputeCloudinessStep(i, j, i, j + 1, copy, pressures);
                        if (ValidPosition(i - 1, j - 1)) ComputeCloudinessStep(i, j, i - 1, j - 1, copy, pressures);
                        if (ValidPosition(i - 1, j)) ComputeCloudinessStep(i, j, i - 1, j, copy, pressures);
                        if (ValidPosition(i - 1, j + 1)) ComputeCloudinessStep(i, j, i - 1, j + 1, copy, pressures);

                    }
                }
            }
            Clone(copy);
        }

        public void ApplyConvolution(Matrix kernel)
        {
            throw new NotImplementedException();
        }

        

        public Matrix Copy()
        {
            Matrix res = new Matrix(Width, Height, 0);
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    res.Set(i, j, Get(i, j));
            return res;
        }

        public void Clone(Matrix matrix)
        {
            if(matrix.Width != Width && matrix.Height != Height)
                throw new Exception("The matrix must have the same size");
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    Set(i, j, matrix.Get(i, j));
        }

        public void PrintMatrix()
        {
            Console.WriteLine("----\n");
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Console.Write(Get(i, j).ToString("0.00") + "  ");
                }
                Console.WriteLine("");
            }
        }

    }
}