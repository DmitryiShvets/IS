using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork1
{
    class StudentPerceptron
    {
        private readonly double[] inputs;

        public double[] weights;

        public double Bias { get; set; }

        public double BiasError { get; set; }

       static Random r = new Random();

        public double weightedSum { get; set; }

        private IActivationFunction function;

        public int InputCount { get { return inputs.Length; } }

        private double output;

        public double Error { get; set; }

        public StudentPerceptron(double[] input,double b,IActivationFunction f)
        {
            
            inputs = input;
            weights = new double[InputCount];
            function = f;
            Bias = b;
        }

        private void InitWeights()
        {
            for (int i = 0; i < weights.Length; i++)
                weights[i] = r.NextDouble();
        }

        private  double GetRandomNumber(Random r,double minimum,double maximum)
        {
            return r.NextDouble() * (maximum - minimum) + minimum;
        }

        private void InitW(Random r)
        {
            for (int i = 0; i < weights.Length; i++)
                weights[i] = GetRandomNumber(r, -0.5, 0.5);
        }

        public StudentPerceptron(int input, double b, IActivationFunction f)
        {
            inputs = new double[input];
            weights = new double[InputCount];
            InitWeights();
            function = f;
            Bias = b;
        }

        public StudentPerceptron(int input, Random r, IActivationFunction f)
        {
            inputs = new double[input];
            weights = new double[InputCount];
            InitW(r);
            function = f;
            Bias = GetRandomNumber(r, -0.5, 0.5);
        }

        public double ApplyActivationFunction()
        {
            return function.GetActivationValue(weightedSum);
        }

        public double GetWeightedSum(double[] inputs)
        {
             weightedSum = 0;
            for(int i = 0; i < InputCount; i++)
            {
                weightedSum += inputs[i] * weights[i]; 
            }
            weightedSum += Bias;
            return ApplyActivationFunction();
        }

        public double GetError()
        {
            return Error * function.GetDerivativeValue(weightedSum);
        }

        /*
        public double GetPerceptronOutput(double value)
        {   

            return function.GetActivationValue()
        }
        */

    }
}
