using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleRegressionBostonHousing
{
    public static class Statistics
    {
        public static double Mean(this double[] vector)
        {
            if (vector == null)
                throw new ArgumentNullException("vector");

            return vector.Sum() / vector.Count();
        }

        public static double[] Mean(this double[,] matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix");

            int examples = matrix.GetLength(0);
            int features = matrix.GetLength(1);

            double[] averageValues = new double[features];

            double[] sumArray = matrix.Sum(dimension: 0);

            for (int i = 0; i < features; i++)
            {
                averageValues[i] = sumArray[i] / examples;
            }

            return averageValues;
        }

        public static double[] StandardDeviation(this double[,] data, double[] averageValues)
        {
            var numberOfExamples = data.GetLength(0);
            var standardDeviations = new double[data.GetLength(1)];
            for (int feature = 0; feature < data.GetLength(1); feature++)
            {
                double sum = 0;
                for (int example = 0; example < numberOfExamples; example++)
                {
                    sum += Math.Pow(data[example, feature] - averageValues[feature], 2);
                }
                standardDeviations[feature] = Math.Sqrt(sum / (numberOfExamples - 1));
            };

            return standardDeviations;
        }
    }
}
