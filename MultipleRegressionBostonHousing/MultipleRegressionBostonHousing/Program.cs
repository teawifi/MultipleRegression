using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleRegressionBostonHousing
{
    class Program
    {        
        static void Main(string[] args)
        {
            var x_trainSet = File.ReadAllLines("boston_x_train.csv").Select(line => line.Split(','));
            var y_trainSet = File.ReadAllLines("boston_y_train.csv").Select(line => line.Split(','));

            var x_testSet = File.ReadAllLines("boston_x_test.csv").Select(line => line.Split(','));
            var y_testSet = File.ReadAllLines("boston_y_test.csv").Select(line => line.Split(','));

            double[,] x_train = TransformTo2DArray(x_trainSet);
            double[] y_train = TransformTo1DArray(y_trainSet);
            double[,] x_test = TransformTo2DArray(x_testSet);
            double[] y_test = TransformTo1DArray(y_testSet);

            double[] mean = x_train.Mean();
            double[] std = x_train.StandardDeviation(mean);
            x_train = x_train.MeanNormalization(mean, std);
            x_test = x_test.MeanNormalization(mean, std);

            var regression = new Regression();
            regression.Fit(x_train, y_train);
            Console.WriteLine("Train result");
            Console.WriteLine("R-Squared: {0}", regression.RSquared);
            Console.WriteLine("Adjusted R-Squared: {0}", regression.AdjustedRSquared);
            Console.WriteLine("Fitted coefficients");
            foreach (var cof in regression.FittedCoefficients)
            {
                Console.Write("" + cof + " ");
            }
            Console.WriteLine();

            regression.Predict(x_test, y_test);
            Console.WriteLine("Test result");
            Console.WriteLine("R-Squared: {0}", regression.RSquared);
            Console.WriteLine("Adjusted R-Squared: {0}", regression.AdjustedRSquared);
            Console.WriteLine("Predicted Y");
            foreach (var y in regression.PredictedY)
            {
                Console.WriteLine("" + y);
            }

            Console.ReadKey();
        }

        private static double[,] TransformTo2DArray(IEnumerable<string[]> set)
        {
            double[,] newSet = new double[set.Count(), set.ElementAt(0).Count()];
            for (int i = 0; i < set.Count(); i++)
            {
                for (int j = 0; j < set.ElementAt(0).Count(); j++)
                {
                    var item = set.ElementAt(i)[j];
                    var num = Double.Parse(item, CultureInfo.InvariantCulture);
                    newSet[i, j] = num;
                }
            }

            return newSet;
        }

        private static double[] TransformTo1DArray(IEnumerable<string[]> set)
        {
            double[] newSet = new double[set.Count()];
            for (int i = 0; i < set.Count(); i++)
            {
                newSet[i] = Double.Parse(set.ElementAt(i)[0], CultureInfo.InvariantCulture);
            }

            return newSet;
        }
    }
}
