using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork1
{
    class SigmoidActivationFunction : IActivationFunction
    {

        public SigmoidActivationFunction() { }
        public  double GetActivationValue(double weightedSum)
        {
            return 1.0 / (1 + Math.Exp(-weightedSum));
        }

        public double GetDerivativeValue(double weightedSum)
        {
            double fval = GetActivationValue(weightedSum);
            return fval * (1 - fval);
        }
    }
}
