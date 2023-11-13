using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork1
{
    class StudentNeuralNetwork
    {
        public StudentNetworkLayer[] layers;

        private double[] output;

        public StudentNeuralNetwork(int[] structure,IActivationFunction function)
        {
            layers = new StudentNetworkLayer[structure.Length-1];
            for(int i = 0; i < structure.Length - 1; i++)
            {
                layers[i] = new StudentNetworkLayer(structure[i], structure[i + 1], function);
            }
        }

        public double[] Compute(double[] input)
        {
            for(int i = 0; i < layers.Length; i++)
            {
                input = layers[i].GetLayerOutput(input);
            }

            return input;
        }

        public int GetLayersCount()
        {
            return layers.Length;
        }
    }
}
