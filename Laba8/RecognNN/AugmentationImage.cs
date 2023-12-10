using AForge.Imaging;
using NeuralNetwork1.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace NeuralNetwork1
{
    class AugmentationImage
    {

        public bool[,] img = new bool[300, 300];
        public bool[,] img30 = new bool[30, 30];

        //  private int margin = 50;
        private Random rand = new Random();

        public int FigureCount { get; set; } = 10;

        public FigureType currentFigure = FigureType.Undef;

        // Создание обучающей выборки
        public SamplesSet samples = new SamplesSet();
        // Создание тестовой выборки
        public SamplesSet samplesTest = new SamplesSet();


        public void ClearImage()
        {
            for (int i = 0; i < 300; ++i)
                for (int j = 0; j < 300; ++j)
                    img[i, j] = false;

            for (int i = 0; i < 30; ++i)
                for (int j = 0; j < 30; ++j)
                    img30[i, j] = false;
        }

        public Sample Get30x30()
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
            Bitmap bmp = new Bitmap(System.Drawing.Image.FromFile(files[randomFile]));

            double[] input = new double[900];
            for (int k = 0; k < 900; k++) input[k] = 0;

            currentFigure = (FigureType)randomDirectory;
            for (int x = 0; x < 300; x++)
            {
                for (int y = 0; y < 300; y++)
                {
                    Color color = bmp.GetPixel(x, y);
                    if (color.R < 50) img[x, y] = true;
                }
            }
            //var resized_bmp = new Bitmap(bmp, 30, 30);
            //  Масштабируем изображение до 30x30 - этого достаточно
            AForge.Imaging.Filters.ResizeBilinear scaleFilter = new AForge.Imaging.Filters.ResizeBilinear(30, 30);
            var resized = scaleFilter.Apply(UnmanagedImage.FromManagedImage(bmp));
            Bitmap resized_bmp = resized.ToManagedImage();
            for (int x = 0; x < 30; x++)
            {
                for (int y = 0; y < 30; y++)
                {
                    Color color = resized_bmp.GetPixel(x, y);
                    if (color.R < 50) img30[x, y] = true;
                    if (img30[x, y])
                    {
                        input[x * 30 + y] = 1;
                    }
                }
            }
            return new Sample(input, FigureCount, currentFigure);
        }


        public SamplesSet LoadSampleSet()
        {
            return samples;
        }

        public SamplesSet LoadSampleSetTest()
        {
            return samplesTest;
        }

        public void LoadDataset()
        {
            samples = new SamplesSet();
            samplesTest = new SamplesSet();
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

                    currentFigure = (FigureType)i;

                    for (int x = 0; x < 300; x++)
                    {
                        for (int y = 0; y < 300; y++)
                        {
                            Color color = bmp.GetPixel(x, y);
                            if (color.R < 50) img[x, y] = true;
                        }
                    }
                    //  Масштабируем изображение до 30x30 - этого достаточно
                    AForge.Imaging.Filters.ResizeBilinear scaleFilter = new AForge.Imaging.Filters.ResizeBilinear(30, 30);
                    var resized = scaleFilter.Apply(UnmanagedImage.FromManagedImage(bmp));
                    Bitmap resized_bmp = resized.ToManagedImage();
                    for (int x = 0; x < 30; x++)
                    {
                        for (int y = 0; y < 30; y++)
                        {
                            Color color = resized_bmp.GetPixel(x, y);
                            if (color.R < 50) img30[x, y] = true;
                            if (img30[x, y])
                            {
                                input[x * 30 + y] = x * 30 + y;
                            }
                        }
                    }


                    if (j < 130)
                    {
                        samples.AddSample(new Sample(input, FigureCount, currentFigure));
                    }
                    else
                    {
                        samplesTest.AddSample(new Sample(input, FigureCount, currentFigure));
                    }
                }
            }
        }


        public Bitmap GenBitmap()
        {
            Bitmap drawArea = new Bitmap(300, 300);
            for (int i = 0; i < 300; ++i)
                for (int j = 0; j < 300; ++j)
                    if (img[i, j]) drawArea.SetPixel(i, j, Color.Black);
            return drawArea;
        }


        public Bitmap GenBitmap30()
        {
            Bitmap drawArea = new Bitmap(30, 30);
            for (int i = 0; i < 30; ++i)
                for (int j = 0; j < 30; ++j)
                    if (img30[i, j]) drawArea.SetPixel(i, j, Color.Black);
            return drawArea;
        }

    }
}
