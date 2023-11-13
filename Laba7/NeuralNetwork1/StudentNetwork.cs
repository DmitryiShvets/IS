using System;

namespace NeuralNetwork1
{
    public class StudentNetwork : BaseNetwork
    {
        private IActivationFunction function = new SigmoidActivationFunction();

        private StudentNeuralNetwork network;

        

        public StudentNetwork(int[] structure)
        {
            network = new StudentNeuralNetwork(structure, function);
        }


        private double GetLossFunctionValue(double[] actualResult,double[] TartgetResults)
        {
            double res = 0;
            for (int i = 0; i < TartgetResults.Length; i++)
            {
                res += Math.Pow(TartgetResults[i] - actualResult[i], 2);
            }

            return 0.5 * res;
        }

        public void backwardPropagation(Sample sample,double learningRate = 0.1)
        {
            var targets = sample.targetValues;
            //бежим от последнего слоя к первому
            for(int i=network.GetLayersCount()-1; i >= 1; i--)
            {
                for(int j = 0; j < network.layers[i].perceptrons.Length; j++)
                {
                    var current = network.layers[i].perceptrons[j];

                    if(i == network.GetLayersCount()-1)
                    {
                        current.Error = targets[j] - current.ApplyActivationFunction();
                    }

                    double error = current.GetError();

                    current.Bias += learningRate * error*current.Bias;

                    for(int k = 0; k < current.weights.Length; k++)
                    {
                        network.layers[i - 1].perceptrons[k].Error += error * current.weights[k];
                        current.weights[k] += learningRate * error * network.layers[i-1].perceptrons[k].ApplyActivationFunction();
                    }
                    current.Error = 0;
                }
            }   
        }


        public override int Train(Sample sample, double acceptableError, bool parallel)
        {
            int count = 1;
            
            var lossFunctionResult = GetLossFunctionValue(network.Compute(sample.input),sample.targetValues);
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
    }
}