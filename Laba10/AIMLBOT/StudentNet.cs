using Accord.Neuro;
using System;
using System.Linq;
using System.Text;

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
                BackwardPropagation(sample.targetValues, sample.input);
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

        public override double[] Compute(double[] input)
        {
            return network.Compute(input);
        }

        public void BackwardPropagation(double[] targets, double[]input, double learning_rate = 0.1)
        {
            //double[] targets = sample.targetValues;
            //бежим от последнего слоя к первому
            for (int i = network.layers.Length - 1; i >= 0; i--)
            {
                for (int j = 0; j < network.layers[i].neurons.Length; j++)
                {
                    var current_neuron = network.layers[i].neurons[j];

                    if (i == network.layers.Length - 1)
                    {
                        current_neuron.Error = targets[j] - current_neuron.ApplyActivationFunction();
                    }

                    double error = current_neuron.GetError();

                    //дельта смещения (alpha*Dj*Xi)
                    current_neuron.Bias += learning_rate * error * current_neuron.Bias;

                    for (int k = 0; k < current_neuron.weights.Length; k++)
                    {
                        //сумма (Dj*Wik)
                        double xi;
                        if (i > 0)
                        {
                            network.layers[i - 1].neurons[k].Error += error * current_neuron.weights[k];

                            xi = network.layers[i - 1].neurons[k].ApplyActivationFunction();
                        }
                        else
                        {
                            xi = input[k];
                        }
                        //дельта веса (alpha*Dj*Xi)
                        current_neuron.weights[k] += learning_rate * error * xi;
                    }
                    current_neuron.Error = 0;
                }
            }
        }

        private double Train(Sample sample)
        {
            var loss = GetLossFunctionValue(network.Compute(sample.input), sample.targetValues);
            BackwardPropagation(sample.targetValues, sample.input);
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

        public override void Print()
        {
            Console.WriteLine(network.ToString());
        }

        class Network
        {
            public Layer[] layers;

            //public int LayersCount { get { return layers.Length; } }

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

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < layers.Length; i++)
                {
                    sb.Append($"слой {i}\n" + layers[i].ToString());
                }
                return sb.ToString();
            }
        }

        class Layer
        {
            public Neuron[] neurons;

            public Layer(int inputCount, int outputCount)
            {
                neurons = new Neuron[outputCount];
                Random r = new Random();
                for (int i = 0; i < outputCount; i++)
                {
                    neurons[i] = new Neuron(inputCount, r);
                }
            }

            public double[] GetLayerOutput(double[] input, bool isParallel = false)
            {
                double[] outputs = isParallel ? neurons.AsParallel().Select(x => x.GetPerceptronOutput(input)).ToArray() :
                      neurons.Select(x => x.GetPerceptronOutput(input)).ToArray();
                return outputs;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in neurons)
                {
                    sb.Append(item);
                }
                return sb.ToString();
            }
        }

        class Neuron
        {
            public double[] weights;

            public double Bias { get; set; }

            public double WeightedSum { get; set; }

            public double Error { get; set; }

            public Neuron(int input, Random r)
            {
                weights = new double[input];
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
                for (int i = 0; i < inputs.Length; i++)
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

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("v: ");
                for (int i = 0; i < weights.Length; i++)
                {
                    sb.Append($"{weights[i]:f3} ");
                }
                sb.Append("b: " + $"{Bias:f3} ");
                sb.Append("e: " + $"{Error:f3} ");
                sb.Append("s: " + $"{WeightedSum:f3} \n");
                return sb.ToString();
            }
        }
    }
}