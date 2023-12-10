using System.Diagnostics;
using System.IO;
using System.Linq;
using Accord.Neuro;
using Accord.Neuro.Learning;

namespace NeuralNetwork1
{
    class AccordNet : BaseNetwork
    {
      
        // Реализация нейронной сети из Accord.NET
        private ActivationNetwork network;

        //  Секундомер 
        public Stopwatch stopWatch = new Stopwatch();

   
        // "structure" - Массив с указанием нейронов на каждом слое (включая сенсорный)
        public AccordNet(int[] structure)
        {
            // Создаём сеть - вроде того
            network = new ActivationNetwork(new SigmoidFunction(2.0), structure[0], structure.Skip(1).ToArray());

            //  Встряска "мозгов" сети - рандомизируем веса связей
            new NguyenWidrow(network).Randomize();
        }

        // Обучение сети одному образу  
        public override int Train(Sample sample, double acceptableError, bool parallel)
        {
            var teacher = MakeTeacher(parallel);

            int iters = 1;
            while (teacher.Run(sample.input, sample.Output) > acceptableError)
            {
                ++iters;
            }

            return iters;
        }

        //  Создаём "обучателя" - либо параллельного, либо последовательного  
        private ISupervisedLearning MakeTeacher(bool parallel)
        {
            if (parallel)
                return new ParallelResilientBackpropagationLearning(network);
            return new ResilientBackpropagationLearning(network);
        }

        public override double TrainOnDataSet(SamplesSet samplesSet, int epochsCount, double acceptableError,
            bool parallel)
        {
            //  Сначала надо сконструировать массивы входов и выходов
            double[][] inputs = new double[samplesSet.Count][];
            double[][] outputs = new double[samplesSet.Count][];

            //  Теперь массивы из samplesSet группируем в inputs и outputs
            for (int i = 0; i < samplesSet.Count; ++i)
            {
                inputs[i] = samplesSet[i].input;
                outputs[i] = samplesSet[i].Output;
            }

            //  Текущий счётчик эпох
            int epoch_to_run = 0;

            //  Создаём "обучателя" - либо параллельного, либо последовательного  
            var teacher = MakeTeacher(parallel);

            double error = double.PositiveInfinity;

            stopWatch.Restart();

            while (epoch_to_run < epochsCount && error > acceptableError)
            {
                epoch_to_run++;
                error = teacher.RunEpoch(inputs, outputs);

                OnTrainProgress((epoch_to_run * 1.0) / epochsCount, error, stopWatch.Elapsed);
            }

            OnTrainProgress(1.0, error, stopWatch.Elapsed);

            stopWatch.Stop();

            return error;
        }

        public override double[] Compute(double[] input)
        {
            return network.Compute(input);
        }

        public override void Print()
        {

        }
    }
}