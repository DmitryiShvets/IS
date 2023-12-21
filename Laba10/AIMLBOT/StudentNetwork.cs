using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NeuralNetwork1
{
    public class StudentNetwork : BaseNetwork
    {
        private Layer[] layers;
        private double error = 0;
        //  Секундомер спортивный, завода «Агат», измеряет время пробегания стометровки, ну и время затраченное на обучение тоже умеет
        public Stopwatch stopWatch = new Stopwatch();
        private double learning_rate = 0.25;

        public StudentNetwork(int[] structure)
        {
            layers = new Layer[structure.Length];
            layers[0] = new Layer(learning_rate, structure[0]);
            for (int i = 1; i < layers.Count(); i++)
            {
                layers[i] = new Layer(learning_rate, structure[i], layers[i - 1]);
            }
        }

        public override int Train(Sample sample, double acceptableError, bool parallel)
        {
            int epoch = 0;
            do
            {
                sample.ProcessPrediction(Compute(sample.input));
                if (sample.Correct() && sample.EstimatedError() < 0.1)
                {
                    error += sample.EstimatedError();
                    break;
                }
                backPropagation(sample, parallel);
                epoch++;
            } while (epoch < 100);

            return epoch;
        }

        public override double TrainOnDataSet(SamplesSet samplesSet, int epochsCount, double acceptableError, bool parallel)
        {
            stopWatch.Start();
            double prev_error = error;
            double sumError = 0;
            for (int epoch = 0; epoch < epochsCount; epoch++)
            {
                error = 0;
                foreach (var sample in samplesSet.samples)
                {
                    sumError += Train(sample, acceptableError, parallel);
                }
                error = sumError / ((epoch + 1) * samplesSet.Count + 1);
                OnTrainProgress((epoch * 1.0) / epochsCount, error, stopWatch.Elapsed);

                if (error <= acceptableError || Math.Abs(prev_error - error) < 1e-8)
                {
                    break;
                }
                prev_error = error;
            }

            stopWatch.Stop();
            OnTrainProgress(1, error, stopWatch.Elapsed);

            return error;
        }

        private void backPropagation(Sample sample, bool parallel = true)
        {
            for (int i = 0; i < layers[layers.Length - 1].neurons.Count(); i++)
            {
                layers[layers.Count() - 1].neurons[i].error = sample.error[i];
            }
            for (int i = layers.Count() - 1; i > 0; i--)
            {
                layers[i].backPropagation(parallel);
            }
        }

        public override double[] Compute(double[] input)
        {
            for (int i = 0; i < layers.Count(); i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < input.Length; j++)
                    {
                        layers[0].neurons[j].output = input[j];
                    }
                    continue;
                }
                layers[i].Compute();
            }
            return layers.Last().neurons.Select(x => x.output).ToArray();
        }

        public override void Print()
        {
            throw new NotImplementedException();
        }

        public class Layer
        {
            public double learningRate;
            public Neuron[] neurons;
            private Layer prevLayer;

            public Layer(double _learningRate, int length)
            {
                prevLayer = null;
                learningRate = _learningRate;
                neurons = new Neuron[length];
                for (int j = 0; j < length; j++)
                    neurons[j] = new Neuron();
            }

            public Layer(double _learningRate, int length, Layer prev)
            {
                prevLayer = prev;
                learningRate = _learningRate;
                neurons = new Neuron[length];
                for (int j = 0; j < length; j++)
                {
                    neurons[j] = new Neuron(prev.neurons);
                }
            }

            public void Compute()
            {
                foreach (Neuron n in neurons)
                {
                    n.activationFunc();
                }
            }

            public static int threads = Environment.ProcessorCount;
            public void backPropagation(bool parallel)
            {
                if (parallel)
                {
                    int perThread = prevLayer.neurons.Count() / threads;
                    foreach (var n in prevLayer.neurons) n.error = 0;
                    for (int j = 0; j < neurons.Length; j++)
                    {
                        neurons[j].setError(learningRate);
                    }
                    Parallel.For(0, threads, i =>
                    {
                        for (int j = 0; j < neurons.Length; j++)
                            neurons[j].backPropParallel(learningRate, perThread * i, i == threads ? prevLayer.neurons.Count() : perThread * (i + 1));
                    });
                }
                else
                {
                    for (int j = 0; j < neurons.Length; j++)
                        neurons[j].backProp(learningRate);
                }
            }
        }

        public class Neuron
        {
            public double[] weights;
            private Neuron[] prevLayer;
            public double bias;
            public double error = 0;
            public double output = 0;

            public static Random r = new Random();

            public Func<double, double> sigmFunc = (double x) => 1 / (1 + Math.Exp(-x));
            public Func<double, double> derivativeSigmFunc = (double x) => x * (1 - x);

            public Neuron(Neuron[] previous)
            {
                prevLayer = previous;
                weights = new double[prevLayer.Length];
                for (int i = 0; i < prevLayer.Length; i++)
                    weights[i] = r.NextDouble() - 0.5;
                bias = r.NextDouble() - 0.5;
            }

            public Neuron()
            {
                bias = r.NextDouble() - 0.5;
            }

            public void activationFunc()
            {
                double sum = 0;
                for (int i = 0; i < prevLayer.Length; i++)
                {
                    sum += prevLayer[i].output * weights[i];
                }
                output = sigmFunc(bias + sum);
            }

            public void setError(double learningRate)
            {
                error *= derivativeSigmFunc(output);
                bias -= learningRate * bias * error;
            }

            public void backProp(double learningRate)
            {
                setError(learningRate);

                for (int i = 0; i < prevLayer.Length; i++)
                    prevLayer[i].error += error * weights[i];

                errorToWeights(learningRate, 0, prevLayer.Length);
                error = 0;
            }

            public void backPropParallel(double learningRate, int from, int to)
            {
                for (int i = from; i < to; i++)
                    prevLayer[i].error += error * weights[i];
                errorToWeights(learningRate, from, to);
            }

            private void errorToWeights(double learningRate, int from, int to)
            {
                for (int i = from; i < to; i++)
                    weights[i] -= learningRate * prevLayer[i].output * error;
            }
        }
    }
}