using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork1
{
    interface IActivationFunction
    {
        double GetActivationValue(double weightedSum);

        double GetDerivativeValue(double weightedSum);
    }
}
