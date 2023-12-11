using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork1
{
    class ImprovedLoader
    {
        public bool[] img48 = new bool[48 * 48];
        private Random rand = new Random();

        public FigureType currentFigure = FigureType.Undef;

        public int FigureCount { get; set; } = 10;

        // Создание обучающей выборки
        public SamplesSet samples = new SamplesSet();
        // Создание тестовой выборки
        public SamplesSet samplesTest = new SamplesSet();

        private Controller controller = null;

        public ImprovedLoader(Controller _controller)
        {
            controller = _controller;
        }

        // функция очистки изображений
        private void ClearImage()
        {
            for (int i = 0; i < 48; ++i)
                for (int j = 0; j < 48; ++j)
                    img48[i * 48 + j] = false;
        }

        // геттер обучающей выборки
        public SamplesSet LoadSampleSet()
        {
            return samples;
        }

        // геттер тестовой выборки
        public SamplesSet LoadSampleSetTest()
        {
            return samplesTest;
        }

        // Функция создание файла с векторами признаков для всех методов
        public void CreateDataset()
        {
            // Выборка для метода сумм
            SamplesSet MethodSumSamples = new SamplesSet();
            // Выборка для метода шакала
            SamplesSet MethodShakalSamples = new SamplesSet();
            // Выборка для метода чередования
            SamplesSet MethodAltSamples = new SamplesSet();
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
                    controller.processor.ProcessImage(bmp, true);
                    Bitmap bmp48 = controller.processor.processed;

                    currentFigure = (FigureType)i;

                    // получение изображения
                    for (int x = 0; x < 48; x++)
                    {
                        for (int y = 0; y < 48; y++)
                        {
                            Color newColor = bmp48.GetPixel(x, y);
                            if (newColor.R < 50 || newColor.G < 50 || newColor.B < 50)
                            {
                                img48[x * 48 + y] = true;
                            }
                        }
                    }

                    // добавление в выборку метода суммирования
                    MethodSumSamples.AddSample(MethodSum());

                    // добавление в выборку метода шакала
                    MethodShakalSamples.AddSample(MethodShakal());

                    // добавление в выборку метода чередования
                    MethodAltSamples.AddSample(MethodAlt());
                }
            }

            // запись в файл метода суммирования
            SaveSamples("methodSum.txt", MethodSumSamples);

            // запись в файл метода шакала
            SaveSamples("methodShakal.txt", MethodShakalSamples);

            // запись в файл метода чередования
            SaveSamples("methodAlt.txt", MethodAltSamples);
        }

        // Функция хэширования выборки в файл
        private void SaveSamples(string nameFile, SamplesSet MethodSamples)
        {
            // путь к dataset
            string path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\Dataset\\numbers\\";
            string pathFile = path + nameFile;
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

        // Функция загрузки файла с векторами признаков для разных методов
        public void LoadDataset(int method)
        {
            string nameFile = "";
            int size = 0;
            switch (method)
            {
                case 0: // метод сложение пикселей по строке и столбцу
                    nameFile = "methodSum.txt";
                    size = 96;
                    break;
                case 1: // метод сохранения черных пикселей
                    nameFile = "methodShakal.txt";
                    size = 2304;
                    break;
                case 2: // метод чередования пикселей
                    nameFile = "methodAlt.txt";
                    size = 96;
                    break;
                default:
                    break;
            }

            SamplesSet MethodSamples = new SamplesSet();
            SamplesSet MethodSamplesTest = new SamplesSet();
            // получение директории,  где хранится файл с векторами признаков
            string path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\Dataset\\numbers\\";
            string pathFile = path + nameFile;
            int c = 1;
            using (StreamReader sr = File.OpenText(pathFile))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    List<string> splitSep = s.Split(';').ToList();
                    // загружаем из файла все классы, пока не достигли выбранного
                    if (Int32.Parse(splitSep[0]) == FigureCount)
                    {
                        break;
                    }
                    List<string> splitSpace = splitSep[1].Split(' ').ToList();

                    double[] input = new double[size];
                    for (int k = 0; k < size; k++)
                        input[k] = 0;

                    for (int i = 0; i < splitSpace.Count(); i++)
                    {
                        input[i] = double.Parse(splitSpace[i]);
                    }
                    currentFigure = (FigureType)Int32.Parse(splitSep[0]);
                    if (MethodSamples.samples.Count() < 15 * c)
                        MethodSamples.AddSample(new Sample(input, FigureCount, currentFigure)); // берем по 15 векторов для обучения
                    else
                    {
                        MethodSamplesTest.AddSample(new Sample(input, FigureCount, currentFigure)); // берем по 5 векторов для обучения
                        if (MethodSamplesTest.samples.Count() > 5 * c) c += 1;
                    }
                }
            }

            samples = MethodSamples;
            samplesTest = MethodSamplesTest;
        }

        // Функция загрузки случайного изображения для тестирования в зависимости от метода(при нажатии на экран)
        public Sample LoadImage(int method, bool isInput = false)
        {
            ClearImage();
            
            Bitmap bmp48 = !isInput ? GetRandomImage() : GetInputImage();
            // получение изображения
            for (int x = 0; x < 48; x++)
            {
                for (int y = 0; y < 48; y++)
                {
                    Color newColor = bmp48.GetPixel(x, y);
                    if (newColor.R < 50 || newColor.G < 50 || newColor.B < 50)
                    {
                        img48[x * 48 + y] = true;
                    }
                }
            }

            switch (method)
            {
                case 0:
                    return MethodSum(); // создание Sample с помощью метода суммирования
                case 1:
                    return MethodShakal(); // создание Sample с помощью метода шакала
                case 2:
                    return MethodAlt(); // создание Sample с помощью метода чередования
                default:
                    return null;
            }
        }

        // Функция получения случайного изображения
        private Bitmap GetRandomImage()
        {
            // путь к dataset
            string path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\Dataset\\numbers";
            // получение всех директорий
            List<string> directories = Directory.GetDirectories(path).ToList();

            // получение случайной директории(класса)
            int randomDirectory = rand.Next(0, FigureCount);
            // получение всех файлов
            List<string> files = Directory.GetFiles(directories[randomDirectory]).ToList();

            // получение случайной файла
            int randomFile = rand.Next(0, files.Count());

            currentFigure = (FigureType)randomDirectory;

            // загрузка изображения
            Bitmap bmp = new Bitmap(Image.FromFile(files[randomFile]));
            controller.processor.ProcessImage(bmp, true);
            return controller.processor.processed;
        }

        // Функция для получения зафиксированного изображения
        private Bitmap GetInputImage()
        {
            // путь к фото
            string path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\Dataset\\numbers";
            string pathFile = path + "\\input.jpg";

            currentFigure = FigureType.Undef;

            // загрузка изображения
            Bitmap bmp = new Bitmap(Image.FromFile(pathFile));
            controller.processor.ProcessImage(bmp, true);
            return controller.processor.processed;
        }

        // метод сумм
        private Sample MethodSum()
        {
            // input для метода суммирования
            double[] inputSum = new double[96];
            for (int k = 0; k < 96; k++)
                inputSum[k] = 0;

            // получение вектора признаков метода суммирования
            for (int x = 0; x < 48; x++)
                for (int y = 0; y < 48; y++)
                {
                    if (img48[x * 48 + y])
                    {
                        inputSum[x] += 1;
                        inputSum[48 + y] += 1;
                    }
                }

            // добавление в выборку метода суммирования
            return new Sample(inputSum, FigureCount, currentFigure);
        }

        // метод шакала
        private Sample MethodShakal()
        {
            // input для метода шакала
            double[] inputShakal = new double[2304];
            for (int k = 0; k < 2304; k++)
                inputShakal[k] = 0;

            // получение вектора признаков для метода шакала
            for (int x = 0; x < 48; x++)
            {
                for (int y = 0; y < 48; y++)
                {
                    if (img48[x * 48 + y])
                    {
                        inputShakal[x * 48 + y] = 1;
                    }
                }
            }

            // добавление в выборку метода шакала
            return new Sample(inputShakal, FigureCount, currentFigure);
        }

        // метод чередования
        private Sample MethodAlt()
        {
            // input для метода чередования
            double[] inputAlt = new double[96];
            for (int k = 0; k < 96; k++)
                inputAlt[k] = 0;


            // получение вектора признаков метода чередования
            for (int x = 0; x < 48; x++)
                for (int y = 0; y < 48; y++)
                    if (x - 1 > 0 && img48[x * 48 + y] != img48[(x - 1) * 48 + y])
                    {
                        inputAlt[x] += 1;
                    }

            for (int x = 0; x < 48; x++)
                for (int y = 0; y < 48; y++)
                    if (y - 1 > 0 && img48[x * 48 + y] != img48[x * 48 + y - 1])
                    {
                        inputAlt[48 + y] += 1;
                    }

            // добавление в выборку метода чередования
            return new Sample(inputAlt, FigureCount, currentFigure);
        }

        // Функция отрисовки изображения на pictureBox
        public Bitmap GenImage()
        {
            Bitmap drawArea = new Bitmap(48, 48);
            for (int i = 0; i < 48; ++i)
                for (int j = 0; j < 48; ++j)
                    if (!img48[i * 48 + j]) drawArea.SetPixel(i, j, Color.Black);
            return drawArea;
        }
    }
}
