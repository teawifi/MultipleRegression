using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleRegressionBostonHousing
{
    public class Regression
    {
        private double[,] x;
        private double[] y;
        private double[] computedY;
        private List<double> fittedCoefficients;
        private double SSres; // residual sum of squares
        private double SSreg; // Regression sum of squares
        private double SStot; // Total sum of squares
        private double rSquared;
        private double adjustedRSquared;

        public double[] PredictedY { get { return computedY; } }
        public List<double> FittedCoefficients { get { return fittedCoefficients; } }
        public double RSquared { get { return rSquared; } }
        public double AdjustedRSquared { get { return adjustedRSquared; } }

        Random random = new Random();
        // hθ(x) = θ0 + θ1x1 + θ2x2 + θ3x3 + θ4x4 + …
        // J(θ0, θ1, θ2, θ3..) = 1/2m ∑ (hΘ (xi) -yi)2 


        public void Fit(double[,] explanatoryVariables, double[] dependentVariables)
        {
            Initialization(explanatoryVariables, dependentVariables);

            fittedCoefficients = GradientDescent(GenerateRandomParameters(x.GetLength(1) + 1));

            Compute();
        }

        public void Predict(double[,] explanatoryVariables, double[] dependentVariables)
        {
            Initialization(explanatoryVariables, dependentVariables);

            for (var row = 0; row < x.GetLength(0); row++)
            {
                computedY[row] = GetHypothesisValue(ref explanatoryVariables, ref fittedCoefficients, row);
            }

            Compute();
        }

        private void Initialization(double[,] explanatoryVariables, double[] dependentVariables)
        {
            x = explanatoryVariables;
            y = dependentVariables;
            computedY = new double[dependentVariables.Count()];
        }

        private void Compute()
        {
            int examplesCount = x.GetLength(0),
                featuresCount = x.GetLength(1); // deleted -1
            SStot = 0;
            SSreg = 0;
            SSres = 0;

            var meanY = y.Mean();
            // Calculate SSres and SStot
            for (int i = 0; i < y.Length; i++)
            {
                double difference;
                difference = y[i] - computedY[i];
                SSres += Math.Pow(difference, 2);

                difference = y[i] - meanY;
                SStot += Math.Pow(difference, 2);
            }

            // Calculate SSreg
            SSreg = SStot - SSres;

            // Calculate R-Squared
            rSquared = (SStot != 0) ? 1.0 - (SSres / SStot) : 1.0;

            // Calculate adjusted R-Squared
            adjustedRSquared = rSquared != 1 ?
                1.0 - (1.0 - rSquared) * ((examplesCount - 1.0) / (examplesCount - featuresCount - 1.0)) : 1;
        }

        private List<double> GenerateRandomParameters(int numberOfParameters)
        {
            int range = 5;
            var parameters = new List<double>();

            for (var i = 0; i < numberOfParameters; i++)
            {
                parameters.Add(random.NextDouble() * range);
            }

            return parameters;
        }

        private List<double> GradientDescent(List<double> parameters)
        {
            var updatedParameters = new List<double>();
            double alpha = 0.01;
            double error = 0.001;
            var previousCostFunctionValue = 0.0;
            var currentCostFunctionValue = GetCostFunctionValue(parameters);

            while (Math.Abs(currentCostFunctionValue - previousCostFunctionValue) >= error)
            {
                previousCostFunctionValue = currentCostFunctionValue;
                updatedParameters.Clear();

                for (int i = 0; i < parameters.Count; i++)
                {
                    updatedParameters.Add(parameters[i] - alpha * GetPartialDeriavationValue(parameters, i));
                }

                parameters.Clear();
                parameters.AddRange(updatedParameters);

                currentCostFunctionValue = GetCostFunctionValue(parameters);
            }

            return parameters;
        }

        private double GetCostFunctionValue(List<double> parameters)
        {
            var sumOfErrorSquares = 0.0;
            for (var row = 0; row < x.GetLength(0); row++)
            {
                computedY[row] = GetHypothesisValue(ref x, ref parameters, row);
                sumOfErrorSquares += Math.Pow(computedY[row] - y[row], 2);
            }

            return 1.0 / 2 * x.GetLength(0) * sumOfErrorSquares;
        }

        private double GetPartialDeriavationValue(List<double> parameters, int paramIndex)
        {
            var errorSum = 0.0;
            for (var row = 0; row < x.GetLength(0); row++)
            {
                errorSum += (GetHypothesisValue(ref x, ref parameters, row) - y[row]) * (paramIndex != 0 ? x[row, paramIndex - 1] : 1);// Add paramIndex-1
            }

            return 1.0 / x.GetLength(0) * errorSum;
        }

        private double GetHypothesisValue(ref double[,] x, ref List<double> parameters, int exampleIdx)
        {
            var hypothesis = parameters[0];
            for (int feature = 1; feature < x.GetLength(1); feature++)
            {
                hypothesis += parameters[feature] * x[exampleIdx, feature - 1]; // Add feature - 1
            }

            return hypothesis;
        }
    }
}
