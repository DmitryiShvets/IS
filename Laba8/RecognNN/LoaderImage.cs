using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork1
{
    /// <summary>
    /// Тип цифры
    /// </summary>
    public enum FigureType : byte { Zero = 0, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Undef };

    class LoaderImage
    {
        public bool[,] img = new bool[300, 300];

        //  private int margin = 50;
        private Random rand = new Random();

        public FigureType currentFigure = FigureType.Undef;

        public int FigureCount { get; set; } = 10;

        // Создание обучающей выборки
        public SamplesSet samples = new SamplesSet();
        // Создание тестовой выборки
        public SamplesSet samplesTest = new SamplesSet();

        public void ClearImage()
        {
            for (int i = 0; i < 300; ++i)
                for (int j = 0; j < 300; ++j)
                    img[i, j] = false;
        }

        public void CreateDataset(int method)
        {
            switch (method)
            {
                case 0:
                    MethodSumCreate();
                    break;
                default:
                    break;
            }
        }

        public void MethodSumCreate()
        {
            SamplesSet MethodSamples = new SamplesSet();
            // путь к dataset
            string path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\Dataset\\numbers";
            // получение всех директорий
            List<string> directories = Directory.GetDirectories(path).ToList();

            for (int i = 0; i < FigureCount; i++)
            {
                // получение всех файлов из директории
                List<string> files = Directory.GetFiles(directories[i]).ToList();
                for (int j = 0; j < files.Count(); j++)
                {
                    ClearImage();

                    // загрузка изображения
                    Bitmap bmp = new Bitmap(Image.FromFile(files[j]));

                    double[] input = new double[600];
                    for (int k = 0; k < 600; k++)
                        input[k] = 0;

                    currentFigure = (FigureType)i;

                    for (int x = 0; x < 300; x++)
                        for (int y = 0; y < 300; y++)
                        {
                            Color color = bmp.GetPixel(x, y);
                            if (color.R < 50) img[x, y] = true;
                            if (img[x, y])
                            {
                                input[x] += 1;
                                input[300 + y] += 1;
                            }
                        }
                    string text = ((int)currentFigure).ToString() + ";";
                    for (int w = 0; w < input.Length; w++)
                    {
                        if (w != input.Length - 1)
                        {
                            text += input[w].ToString() + " ";
                        }
                        else
                        {
                            text += input[w].ToString();
                        }
                    }
                    MethodSamples.AddSample(new Sample(input, FigureCount, currentFigure));
                }
            }

            // запись в файл
            string pathFile = path + "\\methodSum.txt";

            if (!File.Exists(pathFile))
            {
                using (StreamWriter sw = File.CreateText(pathFile))
                {
                    foreach (Sample sample in MethodSamples)
                    {
                        string text = ((int)sample.actualClass).ToString() + ";";
                        for (int i = 0; i < sample.input.Length; i++)
                        {
                            if (i != sample.input.Length - 1)
                            {
                                text += sample.input[i].ToString() + " ";
                            }
                            else
                            {
                                text += sample.input[i].ToString();
                            }
                        }
                        sw.WriteLine(text);
                    }
                }
            }
        }

        public void LoadDataset(int method)
        {
            switch (method)
            {
                case 0:
                    MethodSumLoad();
                    break;
                default:
                    break;
            }
        }

        public void MethodSumLoad()
        {
            SamplesSet MethodSamples = new SamplesSet();
            SamplesSet MethodSamplesTest = new SamplesSet();
            string path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\Dataset\\numbers";
            string pathFile = path + "\\methodSum.txt";
            int c = 1;
            using (StreamReader sr = File.OpenText(pathFile))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    List<string> splitSep = s.Split(';').ToList();
                    if (Int32.Parse(splitSep[0]) == FigureCount)
                    {
                        break;
                    }
                    List<string> splitSpace = splitSep[1].Split(' ').ToList();
                    double[] input = new double[600];
                    for (int k = 0; k < 600; k++)
                        input[k] = 0;
                    for (int i = 0; i < splitSpace.Count(); i++)
                    {
                        input[i] = double.Parse(splitSpace[i]);
                    }
                    currentFigure = (FigureType)Int32.Parse(splitSep[0]);
                    if (MethodSamples.samples.Count() < 130 * c)
                        MethodSamples.AddSample(new Sample(input, FigureCount, currentFigure));
                    else
                    {
                        MethodSamplesTest.AddSample(new Sample(input, FigureCount, currentFigure));
                        if (MethodSamplesTest.samples.Count() > 21 * c) c += 1;
                    }
                }
            }

            samples = MethodSamples;
            samplesTest = MethodSamplesTest;
        }

        public SamplesSet LoadSampleSet()
        {
            return samples;
        }

        public SamplesSet LoadSampleSetTest()
        {
            return samplesTest;
        }

        public Sample LoadImage()
        {
            // путь к dataset
            string path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\Dataset\\numbers";
            // получение всех директорий
            List<string> directories = Directory.GetDirectories(path).ToList();

            int randomDirectory = rand.Next(0, FigureCount);
            List<string> files = Directory.GetFiles(directories[randomDirectory]).ToList();

            int randomFile = rand.Next(0, files.Count());

            ClearImage();

            // загрузка изображения
            Bitmap bmp = new Bitmap(Image.FromFile(files[randomFile]));

            double[] input = new double[600];
            for (int k = 0; k < 600; k++)
                input[k] = 0;

            currentFigure = (FigureType)randomDirectory;

            for (int x = 0; x < 300; x++)
                for (int y = 0; y < 300; y++)
                {
                    Color color = bmp.GetPixel(x, y);
                    if (color.R < 50) img[x, y] = true;
                    if (img[x, y])
                    {
                        input[x] += 1;
                        input[300 + y] += 1;
                    }
                }
            return new Sample(input, FigureCount, currentFigure);
        }

        public Bitmap GenBitmap()
        {
            Bitmap drawArea = new Bitmap(300, 300);
            for (int i = 0; i < 300; ++i)
                for (int j = 0; j < 300; ++j)
                    if (img[i, j])
                        drawArea.SetPixel(i, j, Color.Black);
            return drawArea;
        }
    }
}
