using AForge.Imaging;
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

    // Класс загрузчика фотографий
    class LoaderImage
    {
        public bool[,] img = new bool[300, 300];
        public bool[] img28 = new bool[28 * 28];
        private ImagePreproccessor imageProccessor = new ImagePreproccessor(new Settings1(5, 0.6f));
        //  private int margin = 50;
        private Random rand = new Random();

        public FigureType currentFigure = FigureType.Undef;

        public int FigureCount { get; set; } = 10;

        // Создание обучающей выборки
        public SamplesSet samples = new SamplesSet();
        // Создание тестовой выборки
        public SamplesSet samplesTest = new SamplesSet();

        // функция очистки изображений
        private void ClearImage()
        {
            for (int i = 0; i < 300; ++i)
                for (int j = 0; j < 300; ++j)
                    img[i, j] = false;

            for (int i = 0; i < 28; ++i)
                for (int j = 0; j < 28; ++j)
                    img28[i * 28 + j] = false;
        }

        // геттеры выборок
        public SamplesSet LoadSampleSet()
        {
            return samples;
        }

        public SamplesSet LoadSampleSetTest()
        {
            return samplesTest;
        }

        // Функция создание файла с векторами признаков для разных методов
        public void CreateDataset(int method)
        {
            switch (method)
            {
                case 0:
                    MethodSumCreate(); // метод сложение пикселей по строке и столбцу
                    break;
                case 1:
                    MethodShakalCreate(); // метод преобразование фотографии 300x300 к 28x28 и создание вектора признаков по черным пикселям
                    break;
                case 2:
                    MethodAltCreate(); // Функция загрузки файла с векторами признаков для разных методов
                    break;
                default:
                    break;
            }
        }

        // метод сложение пикселей по строке и столбцу
        private void MethodSumCreate()
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
                    Bitmap bmp = new Bitmap(System.Drawing.Image.FromFile(files[j]));

                    double[] input = new double[600];
                    for (int k = 0; k < 600; k++)
                        input[k] = 0;

                    currentFigure = (FigureType)i;

                    // получение вектора признаков
                    for (int x = 0; x < 300; x++)
                        for (int y = 0; y < 300; y++)
                        {
                            Color color = bmp.GetPixel(x, y);
                            if (color.R + color.G + color.B < 50) img[x, y] = true;
                            if (img[x, y])
                            {
                                input[x] += 1;
                                input[300 + y] += 1;
                            }
                        }
                    MethodSamples.AddSample(new Sample(input, FigureCount, currentFigure));
                }
            }

            // запись в файл
            string pathFile = path + "\\oldMethod\\methodSum.txt";

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

        // метод преобразование фотографии 300x300 к 28x28 и создание вектора признаков по черным пикселям
        private void MethodShakalCreate()
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
                    Bitmap bmp = new Bitmap(System.Drawing.Image.FromFile(files[j]));


                    double[] input = new double[900];
                    for (int k = 0; k < 900; k++)
                        input[k] = 0;

                    // получение вектора признаков
                    var resize = imageProccessor.ProcessImage(bmp);
                    currentFigure = (FigureType)i;

                    //for (int x = 0; x < 300; x++)
                    //{
                    //    for (int y = 0; y < 300; y++)
                    //    {
                    //        Color color = bmp.GetPixel(x, y);
                    //        if (color.R < 50) img[x, y] = true;
                    //    }
                    //}

                    //  Масштабируем изображение до 30x30 - этого достаточно
                    //AForge.Imaging.Filters.ResizeBilinear scaleFilter = new AForge.Imaging.Filters.ResizeBilinear(30, 30);
                    //var resized = scaleFilter.Apply(UnmanagedImage.FromManagedImage(bmp));
                    //Bitmap resized_bmp = resized.ToManagedImage();
                    //for (int x = 0; x < 30; x++)
                    //{
                    //    for (int y = 0; y < 30; y++)
                    //    {
                    //        Color color = resized_bmp.GetPixel(x, y);
                    //        if (color.R < 50) img30[x, y] = true;
                    //        if (img30[x, y])
                    //        {
                    //            input[x * 30 + y] = x * 30 + y;
                    //        }
                    //    }
                    //}
                    MethodSamples.AddSample(new Sample(resize, FigureCount, currentFigure));
                }
            }

            // запись в файл
            string pathFile = path + "\\oldMethod\\methodShakal.txt";

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

        // метод чередования пикселей
        private void MethodAltCreate()
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
                    Bitmap bmp = new Bitmap(System.Drawing.Image.FromFile(files[j]));

                    double[] input = new double[600];
                    for (int k = 0; k < 600; k++)
                        input[k] = 0;

                    currentFigure = (FigureType)i;

                    // получение битового изображения
                    for (int x = 0; x < 300; x++)
                        for (int y = 0; y < 300; y++)
                        {
                            Color color = bmp.GetPixel(x, y);
                            if (color.R + color.G + color.B < 50) img[x, y] = true;
                        }

                    // получение вектора признаков
                    for (int x = 0; x < 300; x++)
                        for (int y = 0; y < 300; y++)
                            if (x - 1 > 0 && img[x, y] != img[x - 1, y])
                            {
                                input[x] += 1;
                            }

                    for (int x = 0; x < 300; x++)
                        for (int y = 0; y < 300; y++)
                            if (y - 1 > 0 && img[x, y] != img[x, y - 1])
                            {
                                input[300 + y] += 1;
                            }

                    MethodSamples.AddSample(new Sample(input, FigureCount, currentFigure));
                }
            }

            // запись в файл
            string pathFile = path + "\\oldMethod\\methodAlt.txt";

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
            switch (method)
            {
                case 0:
                    MethodSumLoad(); // метод сложение пикселей по строке и столбцу
                    break;
                case 1:
                    MethodShakalLoad(); // метод преобразование фотографии 300x300 к 28x28 и создание вектора признаков по черным пикселям
                    break;
                case 2:
                    MethodAltLoad(); // Функция загрузки файла с векторами признаков для разных методов
                    break;
                default:
                    break;
            }
        }

        // метод сложение пикселей по строке и столбцу
        private void MethodSumLoad()
        {
            SamplesSet MethodSamples = new SamplesSet();
            SamplesSet MethodSamplesTest = new SamplesSet();
            // получение директории,  где хранится файл с векторами признаков
            string path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\Dataset\\numbers";
            string pathFile = path + "\\oldMethod\\methodSum.txt";
            int c = 1; // текущий класс, который загружаем
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
                    // parse
                    List<string> splitSpace = splitSep[1].Split(' ').ToList();
                    double[] input = new double[600];
                    for (int k = 0; k < 600; k++)
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

        // метод преобразование фотографии 300x300 к 28x28 и создание вектора признаков по черным пикселям
        private void MethodShakalLoad()
        {
            SamplesSet MethodSamples = new SamplesSet();
            SamplesSet MethodSamplesTest = new SamplesSet();
            // получение директории,  где хранится файл с векторами признаков
            string path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\Dataset\\numbers";
            string pathFile = path + "\\oldMethod\\methodShakal.txt";
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
                    double[] input = new double[784];
                    for (int k = 0; k < 784; k++)
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

            Console.WriteLine($"samples = {samples.samples.Count()}, samplesTest = {samplesTest.samples.Count()}");
        }

        // метод чередования пикселей
        private void MethodAltLoad()
        {
            SamplesSet MethodSamples = new SamplesSet();
            SamplesSet MethodSamplesTest = new SamplesSet();
            // получение директории,  где хранится файл с векторами признаков
            string path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\Dataset\\numbers";
            string pathFile = path + "\\oldMethod\\methodAlt.txt";
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
                    double[] input = new double[600];
                    for (int k = 0; k < 600; k++)
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
        public Sample LoadImage(int method)
        {
            switch (method)
            {
                case 0:
                    return Image300x300(); // загрузка изображения 300x300
                case 1:
                    return Image28x28(); // загрузка изображения 28x28
                case 2:
                    return Image300x300_2(); // загрузка изображения 300x300
                default:
                    return null;
            }
        }

        // загрузка изображения 300x300
        private Sample Image300x300()
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

            ClearImage();

            // загрузка изображения
            Bitmap bmp = new Bitmap(System.Drawing.Image.FromFile(files[randomFile]));

            double[] input = new double[600];
            for (int k = 0; k < 600; k++)
                input[k] = 0;

            currentFigure = (FigureType)randomDirectory;

            // создание вектора признаков
            for (int x = 0; x < 300; x++)
                for (int y = 0; y < 300; y++)
                {
                    Color color = bmp.GetPixel(x, y);
                    if (color.R + color.G + color.B < 50) img[x, y] = true; // создание изображения 300x300
                    if (img[x, y])
                    {
                        input[x] += 1;
                        input[300 + y] += 1;
                    }
                }
            return new Sample(input, FigureCount, currentFigure);
        }

        // загрузка изображения 28x28
        private Sample Image28x28()
        {
            // путь к dataset
            string path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\Dataset\\numbers";
            // получение всех директорий
            List<string> directories = Directory.GetDirectories(path).ToList();

            // получение случайной директории(класса)
            int randomDirectory = rand.Next(0, FigureCount);
            List<string> files = Directory.GetFiles(directories[randomDirectory]).ToList();

            // получение случайной файла
            int randomFile = rand.Next(0, files.Count());
            currentFigure = (FigureType)randomDirectory;

            ClearImage();

            // загрузка изображения
            Bitmap bmp = new Bitmap(System.Drawing.Image.FromFile(files[randomFile]));
            var resize = imageProccessor.ProcessImage(bmp); // создание вектора признаков

            // создание изображения 28x28
            for (int x = 0; x < 28; x++)
            {
                for (int y = 0; y < 28; y++)
                {
                    if (resize[x * 28 + y] > 0)
                    {
                        img28[x * 28 + y] = true;
                    }
                }
            }

            return new Sample(resize, FigureCount, currentFigure);
        }

        // загрузка изображения 300x300
        private Sample Image300x300_2()
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

            ClearImage();

            // загрузка изображения
            Bitmap bmp = new Bitmap(System.Drawing.Image.FromFile(files[randomFile]));

            double[] input = new double[600];
            for (int k = 0; k < 600; k++)
                input[k] = 0;

            currentFigure = (FigureType)randomDirectory;

            // получение битового изображения
            for (int x = 0; x < 300; x++)
                for (int y = 0; y < 300; y++)
                {
                    Color color = bmp.GetPixel(x, y);
                    if (color.R + color.G + color.B < 50) img[x, y] = true;
                }

            // получение вектора признаков
            for (int x = 0; x < 300; x++)
                for (int y = 0; y < 300; y++)
                {
                    if (x - 1 > 0 && img[x, y] != img[x - 1, y])
                    {
                        input[x] += 1;
                    }
                    if (y - 1 > 0 && img[x, y] != img[x, y - 1])
                    {
                        input[300 + y] += 1;
                    }
                }

            return new Sample(input, FigureCount, currentFigure);
        }

        // Функция отрисовки изображения на pictureBox в зависимости от метода(размера изображения)
        public Bitmap GenImage(int method)
        {
            switch (method)
            {
                case 0:
                    return GenBitmap(); // Отрисовка изображения 300x300
                case 1:
                    return GenBitmap28(); // Отрисовка изображения 28x28
                case 2:
                    return GenBitmap(); // Отрисовка изображения 300x300
                default:
                    return null;
            }
        }

        // Отрисовка изображения 300x300
        private Bitmap GenBitmap()
        {
            Bitmap drawArea = new Bitmap(300, 300);
            for (int i = 0; i < 300; ++i)
                for (int j = 0; j < 300; ++j)
                    if (img[i, j])
                        drawArea.SetPixel(i, j, Color.Black);
            return drawArea;
        }

        // Отрисовка изображения 28x28
        private Bitmap GenBitmap28()
        {
            Bitmap drawArea = new Bitmap(28, 28);
            for (int i = 0; i < 28; ++i)
                for (int j = 0; j < 28; ++j)
                    if (img28[i * 28 + j]) drawArea.SetPixel(i, j, Color.Black);
            return drawArea;
        }

        // Функция распознавания изображения в зависимости от метода
        public Sample CheckImage(int method)
        {
            switch (method)
            {
                case 0:
                    return CheckImage300x300(); // Распознавание изображения 300x300
                case 1:
                    return CheckImage48x48(); // Распознавание изображения 28x28
                case 2:
                    return CheckImage300x300_2(); // Распознавание изображения 300x300
                default:
                    return null;
            }
        }

        // Распознавание изображения 300x300
        private Sample CheckImage300x300()
        {
            // путь к фото
            string path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\Dataset\\numbers";
            string pathFile = path + "\\input.jpg";

            ClearImage();

            // загрузка изображения
            Bitmap bmp = new Bitmap(System.Drawing.Image.FromFile(pathFile));

            double[] input = new double[600];
            for (int k = 0; k < 600; k++)
                input[k] = 0;

            // создание вектора признаков
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
            return new Sample(input, FigureCount);
        }

        // Распознавание изображения 48x48
        private Sample CheckImage48x48()
        {
            // путь к фото
            string path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\Dataset\\numbers";
            string pathFile = path + "\\input.jpg";

            ClearImage();

            // загрузка изображения
            Bitmap bmp = new Bitmap(System.Drawing.Image.FromFile(pathFile));
            var resize = imageProccessor.ProcessImage(bmp);

            for (int x = 0; x < 28; x++)
            {
                for (int y = 0; y < 28; y++)
                {
                    if (bmp.GetPixel(x, y).R > 0)
                    {
                        img28[x * 28 + y] = true;
                    }
                }
            }

            return new Sample(resize, FigureCount);
        }

        // Распознавание изображения 300x300
        private Sample CheckImage300x300_2()
        {
            // путь к фото
            string path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\Dataset\\numbers";
            string pathFile = path + "\\input.jpg";

            ClearImage();

            // загрузка изображения
            Bitmap bmp = new Bitmap(System.Drawing.Image.FromFile(pathFile));

            double[] input = new double[600];
            for (int k = 0; k < 600; k++)
                input[k] = 0;

            // получение битового изображения
            for (int x = 0; x < 300; x++)
                for (int y = 0; y < 300; y++)
                {
                    Color color = bmp.GetPixel(x, y);
                    if (color.R + color.G + color.B < 50) img[x, y] = true;
                }

            // получение вектора признаков
            for (int x = 0; x < 300; x++)
                for (int y = 0; y < 300; y++)
                {
                    if (x - 1 > 0 && img[x, y] != img[x - 1, y])
                    {
                        input[x] += 1;
                    }
                    if (y - 1 > 0 && img[x, y] != img[x, y - 1])
                    {
                        input[300 + y] += 1;
                    }
                }

            return new Sample(input, FigureCount);
        }
    }
}
