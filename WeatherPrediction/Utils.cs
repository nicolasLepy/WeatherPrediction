using MathNet.Numerics;
using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace WeatherPrediction
{
    public static class Utils
    {
        public static readonly int MATRIX_SIZE = 20;

        public static double Distance(double xa, double ya, double xb, double yb)
        {
            return Math.Sqrt(Math.Pow(xb - xa, 2) + Math.Pow(yb - ya, 2));
        }

        public static double Distance(City a, City b)
        {
            return Math.Sqrt( Math.Pow( b.X-a.X ,2) + Math.Pow(b.Y - a.Y, 2));
        }

        public static IInterpolation TemperaturesInterpolation(double tmaxYesturday, double tmin, double tmax, double tminTomorrow)
        {
            List<double> hours = new List<double>();
            hours.Add(-6);
            hours.Add(4.5);
            hours.Add(5);
            hours.Add(17);
            hours.Add(17.5);
            hours.Add(29);
            List<double> temp = new List<double>();
            temp.Add(tmaxYesturday);
            temp.Add(tmin);
            temp.Add(tmin);
            temp.Add(tmax);
            temp.Add(tmax);
            temp.Add(tminTomorrow);
            return Interpolate.Polynomial(hours, temp);
        }

        public static IInterpolation PressuresInterpolation(KeyValuePair<int, double> lastPressureYesturday, List<KeyValuePair<int, double>> pressures, KeyValuePair<int, double> firstPressureTommorow)
        {
            List<double> hourPressure = new List<double>();
            List<double> valuesPressure = new List<double>();
            hourPressure.Add(0 - (24 - lastPressureYesturday.Key));
            hourPressure.Add(-0.1 - (24 - lastPressureYesturday.Key));
            valuesPressure.Add(lastPressureYesturday.Value);
            valuesPressure.Add(lastPressureYesturday.Value);
            foreach (KeyValuePair<int, double> kvp in pressures)
            {
                hourPressure.Add(kvp.Key);
                hourPressure.Add(kvp.Key - 0.1);
                valuesPressure.Add(kvp.Value);
                valuesPressure.Add(kvp.Value);
            }
            hourPressure.Add(24 + firstPressureTommorow.Key);
            hourPressure.Add(24.1 + firstPressureTommorow.Key);
            valuesPressure.Add(firstPressureTommorow.Value);
            valuesPressure.Add(firstPressureTommorow.Value);
            return Interpolate.Polynomial(hourPressure, valuesPressure);
        }

        /// <summary>
        /// Generate a matrix with pressures of a region
        /// </summary>
        /// <param name="region"></param>
        /// <param name="date"></param>
        /// <param name="hour"></param>
        /// <returns></returns>
        public static Matrix<double> GeneratePressuresMatrix(Region region, DateTime date, int hour)
        {
            Matrix<double> res = CreateMatrix(MATRIX_SIZE, MATRIX_SIZE, 0);

            for(int i = 0; i<MATRIX_SIZE; i++)
            {
                for(int j = 0; j<MATRIX_SIZE; j++)
                {
                    double iOnMap = (region.MapSizeX / (MATRIX_SIZE+0.0)) * i;
                    double jOnMap = (region.MapSizeY / (MATRIX_SIZE + 0.0)) * i;
                    double value = region.Indicator(iOnMap, jOnMap, date, hour, 2, 2);
                    res.At(i,j,value);
                }
            }

            return res;
        }

        public static void ComputeCloudinessStep(int i, int j, int ni, int nj, Matrix<double> init, Matrix<double> copy, Matrix<double> pressures)
        {
            double ratioPressures = Math.Pow(pressures.At(i, j)/pressures.At(ni, nj),30);
            double newValue = init.At(i, j) * 0.6 * ratioPressures;
            if (newValue < 0.01)
            {
                newValue = 0;
            }
            copy.At(ni,nj, newValue);
        }
        
        public static void GenerateClouds(Matrix<double> destination, Matrix<double> waterMap)
        {
            Random r = new Random();

            for(int i = 0;i<destination.ColumnCount; i++)
            {
                for(int j = 0;j<destination.RowCount; j++)
                {
                    if (waterMap.At(i,j) == 1 && r.Next(1, 4) == 2)
                    {
                        destination.At(i, j, destination.At(i, j) + r.NextDouble() * 0.4 + 0.1);
                    }
                }
            }            
        }
        
        public static void EdgeDetection(Matrix<double> destination, Matrix<double> cloudiness)
        {
            for(int i = 1; i<destination.ColumnCount-1; i++)
            {
                for(int j = 1; j<destination.RowCount-1; j++)
                {
                    double c00 = cloudiness.At(i - 1, j - 1) * -1;
                    double c01 = cloudiness.At(i - 1, j) * -1;
                    double c02 = cloudiness.At(i - 1, j + 1) * -1;
                    double c10 = cloudiness.At(i, j - 1) * -1;
                    double c11 = cloudiness.At(i, j) * 8;
                    double c12 = cloudiness.At(i, j + 1) * -1;
                    double c20 = cloudiness.At(i + 1, j - 1) * -1;
                    double c21 = cloudiness.At(i + 1, j) * -1;
                    double c22 = cloudiness.At(i + 1, j + 1) * -1;

                    double value = c00 + c01 + c02 + c10 + c11 + c12 + c20 + c21 + c22;
                    if (value < 0)
                    {
                        value = 0;
                    }
                    destination.At(i, j, value);
                }
            }
        }


        public static Matrix<double> CreateMatrix(int rows, int columns, double defaultValue)
        {
            var a = Matrix<double>.Build;
            Matrix<double> mat = a.Dense(rows, columns, defaultValue);
            return mat;
        }
        
        public static bool ValidPosition(Matrix<double> matrix, int x, int y)
        {
            bool res = x < matrix.ColumnCount && y < matrix.RowCount && x > 0 && y > 0;
            return res;
        }

        public static void ComputeCloudiness(Matrix<double> destination, Matrix<double> pressures, Matrix<double> waterMap)
        {
            if (pressures.ColumnCount != destination.ColumnCount || pressures.RowCount != destination.RowCount)
            {
                throw new MatrixException("The pressure matrix must have the same size");
            }

            GenerateClouds(destination, waterMap);
            Matrix<double> copy = destination.Clone();// CreateMatrix(destination.RowCount, destination.ColumnCount, 0);
            //destination.CopyTo(copy);

            for(int i = 0;i<destination.ColumnCount; i++)
            {
                for(int j = 0; j<destination.RowCount; j++)
                {
                    if(destination.At(i,j)>0)
                    {

                        if (ValidPosition(destination, i + 1, j - 1))
                        {
                            ComputeCloudinessStep(i, j, i + 1, j - 1, destination,copy, pressures);
                        }

                        if (ValidPosition(destination,i + 1, j))
                        {
                            ComputeCloudinessStep(i, j, i + 1, j, destination,copy, pressures);
                        }

                        if (ValidPosition(destination,i + 1, j + 1))
                        {
                            ComputeCloudinessStep(i, j, i + 1, j + 1, destination,copy, pressures);
                        }

                        if (ValidPosition(destination,i, j - 1))
                        {
                            ComputeCloudinessStep(i, j, i, j - 1, destination,copy, pressures);
                        }

                        if (ValidPosition(destination,i, j + 1))
                        {
                            ComputeCloudinessStep(i, j, i, j + 1, destination,copy, pressures);
                        }

                        if (ValidPosition(destination,i - 1, j - 1))
                        {
                            ComputeCloudinessStep(i, j, i - 1, j - 1, destination,copy, pressures);
                        }

                        if (ValidPosition(destination,i - 1, j))
                        {
                            ComputeCloudinessStep(i, j, i - 1, j, destination,copy, pressures);
                        }

                        if (ValidPosition(destination,i - 1, j + 1))
                        {
                            ComputeCloudinessStep(i, j, i - 1, j + 1, destination,copy, pressures);
                        }

                    }
                }
            }

            copy.CopyTo(destination);
        }
        

    }
}