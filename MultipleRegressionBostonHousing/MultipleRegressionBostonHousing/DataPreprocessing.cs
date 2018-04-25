using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleRegressionBostonHousing
{
    public static class DataPreprocessing
    {
        public static double[,] MeanNormalization(this double[,] data, double[] averageValues, double[] standardDeviations)
        {
            var numberOfExamples = data.GetLength(0);
            for (int example = 0; example < numberOfExamples; example++)
            {
                for (int feature = 0; feature < data.GetLength(1); feature++)
                {
                    data[example, feature] = (data[example, feature] - averageValues[feature]) / standardDeviations[feature];
                }
            }

            return data;
        }
    }
}
