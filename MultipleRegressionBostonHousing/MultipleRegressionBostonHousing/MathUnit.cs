using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleRegressionBostonHousing
{
    public static class MathUnit
    {
        public static double Sum(this double[] vector)
        {
            if (vector == null)
                throw new ArgumentNullException("vector");

            double sum = 0;
            for (int i = 0; i < vector.Count(); i++)
            {
                sum += vector[i];
            }

            return sum;
        }

        public static double[] Sum(this double[,] matrix, int dimension)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix");

            double[] results = { };

            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            if (dimension == 0)
            {
                results = new double[columns];

                for (int j = 0; j < columns; j++)
                {
                    double sum = 0;
                    for (int i = 0; i < rows; i++)
                    {
                        sum += matrix[i, j];
                    }

                    results[j] = sum;
                }
            }
            else if (dimension == 1)
            {
                results = new double[rows];

                for (int i = 0; i < rows; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < columns; j++)
                    {
                        sum += matrix[i, j];
                    }

                    results[i] = sum;
                }
            }

            return results;
        }
    }
}
