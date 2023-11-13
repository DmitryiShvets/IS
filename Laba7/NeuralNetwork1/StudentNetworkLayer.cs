using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork1
{
    class StudentNetworkLayer
    {
        /*
         *  input мы отправляем каждому нейрону
         *  output = массив выходов из каждого нейрона = кол-во нейронов
         *  
         */
        public StudentPerceptron[] perceptrons;
        private double[] outputs;
        
        public StudentNetworkLayer(int inputCount,int outputCount,IActivationFunction function)
        {
            perceptrons = new StudentPerceptron[outputCount];
            Random r = new Random();
            for(int i = 0; i < outputCount; i++)
            {
                perceptrons[i] = new StudentPerceptron(inputCount,r,function);
            }
            outputs = new double[outputCount];
        }

        public StudentNetworkLayer(double[] inputs, int outputCount, IActivationFunction function)
        {
            perceptrons = new StudentPerceptron[outputCount];
            Random r = new Random();
            for (int i = 0; i < outputCount; i++)
            {
                perceptrons[i] = new StudentPerceptron(inputs,1, function);
            }
            outputs = new double[outputCount];
        }

        public int GetPerceptronsCount()
        {
            return perceptrons.Length;
        }

        public double[] GetLayerOutput(double[] input,bool isParallel = false) 
        {  
            outputs = isParallel ? 
                perceptrons.AsParallel().Select(x => x.GetWeightedSum(input)).ToArray():
                 perceptrons.Select(x => x.GetWeightedSum(input)).ToArray();
            return outputs;
        }
    }
}
