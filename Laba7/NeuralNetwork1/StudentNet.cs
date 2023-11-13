using System;
using System.Linq;

namespace NeuralNetwork1
{
    public class StudentNet : BaseNetwork
    {
        private Network network;

        public StudentNet(int[] structure)
        {
            network = new Network(structure);
        }


        private double GetLossFunctionValue(double[] actualResult, double[] TartgetResults)
        {
            double res = 0;
            for (int i = 0; i < TartgetResults.Length; i++)
            {
                res += Math.Pow(TartgetResults[i] - actualResult[i], 2);
            }

            return 0.5 * res;
        }

        public void backwardPropagation(Sample sample, double learningRate = 0.1)
        {
            var targets = sample.targetValues;
            //бежим от последнего слоя к первому
            for (int i = network.GetLayersCount() - 1; i >= 1; i--)
            {
                for (int j = 0; j < network.layers[i].perceptrons.Length; j++)
                {
                    var current = network.layers[i].perceptrons[j];

                    if (i == network.GetLayersCount() - 1)
                    {
                        current.Error = targets[j] - current.ApplyActivationFunction();
                    }

                    double error = current.GetError();

                    current.Bias += learningRate * error * current.Bias;

                    for (int k = 0; k < current.weights.Length; k++)
                    {
                        network.layers[i - 1].perceptrons[k].Error += error * current.weights[k];
                        current.weights[k] += learningRate * error * network.layers[i - 1].perceptrons[k].ApplyActivationFunction();
                    }
                    current.Error = 0;
                }
            }
        }


        public override int Train(Sample sample, double acceptableError, bool parallel)
        {
            int count = 1;

            var lossFunctionResult = GetLossFunctionValue(network.Compute(sample.input), sample.targetValues);
            while (lossFunctionResult >= acceptableError && count >= 50)
            {
                backwardPropagation(sample);
                lossFunctionResult = GetLossFunctionValue(network.Compute(sample.input), sample.targetValues);
                count++;
            }

            return count;
        }

        private double Train(Sample sample)
        {
            var loss = GetLossFunctionValue(network.Compute(sample.input), sample.targetValues);
            backwardPropagation(sample);
            return loss;
        }


        public override double TrainOnDataSet(SamplesSet samplesSet, int epochsCount, double acceptableError, bool parallel)
        {
            var start = DateTime.Now;
            int totalSamplesCount = epochsCount * samplesSet.Count;
            int processedSamplesCount = 0;
            double sumError = 0;
            double mean;
            for (int epoch = 0; epoch < epochsCount; epoch++)
            {
                for (int i = 0; i < samplesSet.samples.Count; i++)
                {
                    var sample = samplesSet.samples[i];
                    sumError += Train(sample);

                    processedSamplesCount++;
                    if (i % 100 == 0)
                    {
                        // Выводим среднюю ошибку для обработанного
                        OnTrainProgress(1.0 * processedSamplesCount / totalSamplesCount,
                            sumError / (epoch * samplesSet.Count + i + 1), DateTime.Now - start);
                    }
                }

                mean = sumError / ((epoch + 1) * samplesSet.Count + 1);
                if (mean <= acceptableError)
                {
                    OnTrainProgress(1.0,
                        mean, DateTime.Now - start);
                    return mean;
                }
            }
            mean = sumError / (epochsCount * samplesSet.Count + 1);
            OnTrainProgress(1.0,
                       mean, DateTime.Now - start);
            return sumError / (epochsCount * samplesSet.Count);
        }

        protected override double[] Compute(double[] input)
        {
            return network.Compute(input);
        }



        class Network
        {
            public Layer[] layers;

            private double[] output;

            public Network(int[] structure)
            {
                layers = new Layer[structure.Length - 1];
                for (int i = 0; i < structure.Length - 1; i++)
                {
                    layers[i] = new Layer(structure[i], structure[i + 1]);
                }
            }

            public double[] Compute(double[] input)
            {
                for (int i = 0; i < layers.Length; i++)
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

        class Layer
        {
            public Perceptron[] perceptrons;

            private double[] outputs;

            public Layer(int inputCount, int outputCount)
            {
                perceptrons = new Perceptron[outputCount];
                Random r = new Random();
                for (int i = 0; i < outputCount; i++)
                {
                    perceptrons[i] = new Perceptron(inputCount, r);
                }
                outputs = new double[outputCount];
            }

            public Layer(double[] inputs, int outputCount)
            {
                perceptrons = new Perceptron[outputCount];
                Random r = new Random();
                for (int i = 0; i < outputCount; i++)
                {
                    perceptrons[i] = new Perceptron(inputs, 1);
                }
                outputs = new double[outputCount];
            }

            public int GetPerceptronsCount()
            {
                return perceptrons.Length;
            }

            public double[] GetLayerOutput(double[] input, bool isParallel = false)
            {
                outputs = isParallel ?
                    perceptrons.AsParallel().Select(x => x.GetWeightedSum(input)).ToArray() :
                     perceptrons.Select(x => x.GetWeightedSum(input)).ToArray();
                return outputs;
            }
        }

        class Perceptron
        {
            private readonly double[] inputs;

            public double[] weights;

            public double Bias { get; set; }

            public double weightedSum { get; set; }

            public int InputCount { get { return inputs.Length; } }

            public double Error { get; set; }

            public Perceptron(double[] input, double b)
            {
                inputs = input;
                weights = new double[InputCount];
                Bias = b;
            }

            public Perceptron(int input, Random r)
            {
                inputs = new double[input];
                weights = new double[InputCount];
                InitW(r);
                Bias = GetRandomNumber(r, -0.5, 0.5);
            }

            private double GetRandomNumber(Random r, double minimum, double maximum)
            {
                return r.NextDouble() * (maximum - minimum) + minimum;
            }

            private void InitW(Random r)
            {
                for (int i = 0; i < weights.Length; i++)
                    weights[i] = GetRandomNumber(r, -0.5, 0.5);
            }

            public double ApplyActivationFunction()
            {
                return GetActivationValue(weightedSum);
            }

            public double GetWeightedSum(double[] inputs)
            {
                weightedSum = 0;
                for (int i = 0; i < InputCount; i++)
                {
                    weightedSum += inputs[i] * weights[i];
                }
                weightedSum += Bias;
                return ApplyActivationFunction();
            }

            public double GetActivationValue(double weightedSum)
            {
                return 1.0 / (1 + Math.Exp(-weightedSum));
            }

            public double GetError()
            {
                return Error * GetDerivativeValue(weightedSum);
            }

            public double GetDerivativeValue(double weightedSum)
            {
                double fval = GetActivationValue(weightedSum);
                return fval * (1 - fval);
            }
        }
    }
}