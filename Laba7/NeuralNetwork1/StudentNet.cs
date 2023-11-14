using System;
using System.Linq;

namespace NeuralNetwork1
{
    public class StudentNet : BaseNetwork
    {
        private readonly Network network;

        public StudentNet(int[] structure)
        {
            network = new Network(structure);
        }

        public override int Train(Sample sample, double accept_rate, bool parallel)
        {
            int count = 1;

            var loss = GetLossFunctionValue(network.Compute(sample.input), sample.targetValues);
            while (loss >= accept_rate && count >= 50)
            {
                BackwardPropagation(sample);
                loss = GetLossFunctionValue(network.Compute(sample.input), sample.targetValues);
                count++;
            }

            return count;
        }

        public override double TrainOnDataSet(SamplesSet samplesSet, int epoch_count, double accept_rate, bool parallel)
        {
            var start = DateTime.Now;
            int all_samples_count = epoch_count * samplesSet.Count;
            int proc_samples_count = 0;
            double sum_error = 0;
            double avg_error = 0;
            for (int epoch = 0; epoch < epoch_count; epoch++)
            {

                for (int i = 0; i < samplesSet.samples.Count; i++)
                {
                    var sample = samplesSet.samples[i];
                    sum_error += Train(sample);

                    proc_samples_count++;
                    if (i % 100 == 0)
                    {
                        // Выводим среднюю ошибку для обработанного
                        OnTrainProgress(1.0 * proc_samples_count / all_samples_count,
                            sum_error / (epoch * samplesSet.Count + i + 1), DateTime.Now - start);
                    }
                }

                avg_error = sum_error / ((epoch + 1) * samplesSet.Count + 1);
                if (avg_error <= accept_rate)
                {
                    OnTrainProgress(1.0, avg_error, DateTime.Now - start);
                    return avg_error;
                }
            }
            avg_error = sum_error / (epoch_count * samplesSet.Count + 1);
            OnTrainProgress(1.0, avg_error, DateTime.Now - start);
            return sum_error / (epoch_count * samplesSet.Count);
        }

        protected override double[] Compute(double[] input)
        {
            return network.Compute(input);
        }

        public void BackwardPropagation(Sample sample, double learning_rate = 0.1)
        {
            var targets = sample.targetValues;
            //бежим от последнего слоя к первому
            for (int i = network.LayersCount - 1; i >= 1; i--)
            {
                for (int j = 0; j < network.layers[i].perceptrons.Length; j++)
                {
                    var current = network.layers[i].perceptrons[j];

                    if (i == network.LayersCount - 1)
                    {
                        current.Error = targets[j] - current.ApplyActivationFunction();
                    }

                    double error = current.GetError();

                    //дельта смещения (alpha*Dj*Xi)
                    current.Bias += learning_rate * error * current.Bias;

                    for (int k = 0; k < current.weights.Length; k++)
                    {
                        //сумма (Dj*Wik)
                        network.layers[i - 1].perceptrons[k].Error += error * current.weights[k];
                        
                        double xi = network.layers[i - 1].perceptrons[k].ApplyActivationFunction();

                        //дельта веса (alpha*Dj*Xi)
                        current.weights[k] += learning_rate * error * xi;
                    }
                    current.Error = 0;
                }
            }
        }

        private double Train(Sample sample)
        {
            var loss = GetLossFunctionValue(network.Compute(sample.input), sample.targetValues);
            BackwardPropagation(sample);
            return loss;
        }

        //функция ошибки квадрат разности
        private double GetLossFunctionValue(double[] actualResult, double[] TartgetResults)
        {
            double res = 0;
            for (int i = 0; i < TartgetResults.Length; i++)
            {
                res += Math.Pow(TartgetResults[i] - actualResult[i], 2);
            }

            return 0.5 * res;
        }

        class Network
        {
            public Layer[] layers;

            public int LayersCount { get { return layers.Length; } }

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
                //бежим от первого слоя к последнему
                for (int i = 0; i < layers.Length; i++)
                {
                    input = layers[i].GetLayerOutput(input);
                }

                return input;
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

            public double[] GetLayerOutput(double[] input, bool isParallel = false)
            {
                outputs = isParallel ? perceptrons.AsParallel().Select(x => x.GetPerceptronOutput(input)).ToArray() :
                     perceptrons.Select(x => x.GetPerceptronOutput(input)).ToArray();
                return outputs;
            }
        }

        class Perceptron
        {
            private readonly double[] inputs;

            public double[] weights;

            public double Bias { get; set; }

            public double WeightedSum { get; set; }

            public int InputCount { get { return inputs.Length; } }

            public double Error { get; set; }

            public Perceptron(int input, Random r)
            {
                inputs = new double[input];
                weights = new double[InputCount];
                InitW(r);
                Bias = GetRandomNumber(r, -0.5, 0.5);
            }

            private void InitW(Random r)
            {
                for (int i = 0; i < weights.Length; i++)
                    weights[i] = GetRandomNumber(r, -0.5, 0.5);
            }

            public double ApplyActivationFunction()
            {
                return GetActivationValue(WeightedSum);
            }

            public double GetPerceptronOutput(double[] inputs)
            {
                WeightedSum = 0;
                for (int i = 0; i < InputCount; i++)
                {
                    WeightedSum += inputs[i] * weights[i];
                }
                WeightedSum += Bias;
                return ApplyActivationFunction();
            }

            public double GetError()
            {
                return Error * GetDerivativeValue(WeightedSum);
            }

            public double GetActivationValue(double weightedSum)
            {
                return 1.0 / (1 + Math.Exp(-weightedSum));
            }

            public double GetDerivativeValue(double weightedSum)
            {
                double y = GetActivationValue(weightedSum);
                return y * (1 - y);
            }

            private double GetRandomNumber(Random r, double minimum, double maximum)
            {
                return r.NextDouble() * (maximum - minimum) + minimum;
            }

        }
    }
}