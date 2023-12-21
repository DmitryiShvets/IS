using AForge.Imaging;
using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Imaging;
using System.IO;

namespace NeuralNetwork1
{
    internal class Settings
    {
        private int _border = 10;

        public int border
        {
            get
            {
                return _border;
            }
            set
            {
                if ((value > 0) && (value < height / 3))
                {
                    _border = value;
                    if (top > 2 * _border) top = 2 * _border;
                    if (left > 2 * _border) left = 2 * _border;
                }
            }
        }

        public int width = 640;
        public int height = 640;

        /// <summary>
        /// Размер сетки для сенсоров по горизонтали
        /// </summary>
        public int blocksCount = 10;

        /// <summary>
        /// Желаемый размер изображения до обработки
        /// </summary>
        public Size orignalDesiredSize = new Size(500, 500);
        /// <summary>
        /// Желаемый размер изображения после обработки
        /// </summary>
        public Size processedDesiredSize = new Size(500, 500);

        public int margin = 10;
        public int top = 20;
        public int left = 20;

        /// <summary>
        /// Второй этап обработки
        /// </summary>
        public bool processImg = true;

        /// <summary>
        /// Порог при отсечении по цвету 
        /// </summary>
        public byte threshold = 20;
        public float differenceLim = (float)20.0 / 255;

        public void incTop() { if (top < 2 * _border) ++top; }
        public void decTop() { if (top > 0) --top; }
        public void incLeft() { if (left < 2 * _border) ++left; }
        public void decLeft() { if (left > 0) --left; }
    }

    internal class MagicEye
    {
        /// <summary>
        /// Обработанное изображение
        /// </summary>
        public Bitmap processed;
        /// <summary>
        /// Оригинальное изображение после обработки
        /// </summary>
        public Bitmap original;

        /// <summary>
        /// Класс настроек
        /// </summary>
        public Settings settings = new Settings();

        private bool[] img48 = new bool[48 * 48];

        public MagicEye()
        {
        }

        public bool ProcessImage(Bitmap bitmap, bool checkAspectRatio)
        {
            // На вход поступает необработанное изображение с веб-камеры

            //  Минимальная сторона изображения (обычно это высота)
            if (checkAspectRatio)
            {
                //if (bitmap.Height > bitmap.Width) throw new Exception("К такой забавной камере меня жизнь не готовила!");
                //  Можно было, конечено, и не кидаться эксепшенами в истерике, но идите и купите себе нормальную камеру!
                int side = System.Math.Min(bitmap.Height, bitmap.Width);

                AForge.Imaging.Filters.Crop cropFilter = new AForge.Imaging.Filters.Crop(new Rectangle((bitmap.Width - bitmap.Height) / 2, 0, side, side));
                original = cropFilter.Apply(bitmap);
            }
            else
            {
                original = bitmap;
            }

             //  Теперь всю эту муть пилим в обработанное изображение
            AForge.Imaging.Filters.Grayscale grayFilter = new AForge.Imaging.Filters.Grayscale(0.2125, 0.7154, 0.0721);
            var uProcessed = grayFilter.Apply(AForge.Imaging.UnmanagedImage.FromManagedImage(original));


            //  Масштабируем изображение до 500x500 - этого достаточно
            AForge.Imaging.Filters.ResizeBilinear scaleFilter = new AForge.Imaging.Filters.ResizeBilinear(settings.orignalDesiredSize.Width, settings.orignalDesiredSize.Height);
            uProcessed = scaleFilter.Apply(uProcessed);
            original = scaleFilter.Apply(original);
            Graphics g = Graphics.FromImage(original);
            //  Пороговый фильтр применяем. Величина порога берётся из настроек, и меняется на форме
            AForge.Imaging.Filters.BradleyLocalThresholding threshldFilter = new AForge.Imaging.Filters.BradleyLocalThresholding();
            threshldFilter.PixelBrightnessDifferenceLimit = settings.differenceLim;
            threshldFilter.ApplyInPlace(uProcessed);


            if (settings.processImg)
            {

                string info = processSample(ref uProcessed);
                Font f = new Font(FontFamily.GenericSansSerif, 20);
                g.DrawString(info, f, Brushes.Black, 30, 30);
            }
            processed = uProcessed.ToManagedImage();

            return true;
        }

        private void ClearImage()
        {
            for (int i = 0; i < 48; ++i)
                for (int j = 0; j < 48; ++j)
                    img48[i * 48 + j] = false;
        }

        public Sample LoadImage()
        {
            ClearImage();

            // путь к фото
            string path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\processed_input.png";

            // загрузка изображения
            Bitmap bmp = new Bitmap(System.Drawing.Image.FromFile(path));

            // получение изображения
            for (int x = 0; x < 48; x++)
            {
                for (int y = 0; y < 48; y++)
                {
                    Color newColor = bmp.GetPixel(x, y);
                    if (newColor.R < 50 || newColor.G < 50 || newColor.B < 50)
                    {
                        img48[x * 48 + y] = true;
                    }
                }
            }
            return MethodShakal(); // создание Sample с помощью метода шакала  
        }


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
                    if (!img48[x * 48 + y])
                    {
                        inputShakal[x * 48 + y] = 1;
                    }
                }
            }

            // добавление в выборку метода шакала
            return new Sample(inputShakal, 10, FigureType.Undef);
        }

        /// <summary>
        /// Обработка одного сэмпла
        /// </summary>
        /// <param name="index"></param>
        private string processSample(ref UnmanagedImage unmanaged)
        {
            string rez = "Обработка";

            ///  Инвертируем изображение
            AForge.Imaging.Filters.Invert InvertFilter = new AForge.Imaging.Filters.Invert();
            InvertFilter.ApplyInPlace(unmanaged);

            ///    Создаём BlobCounter, выдёргиваем самый большой кусок, масштабируем, пересечение и сохраняем
            ///    изображение в эксклюзивном использовании
            AForge.Imaging.BlobCounterBase bc = new AForge.Imaging.BlobCounter();

            bc.FilterBlobs = true;
            bc.MinWidth = 3;
            bc.MinHeight = 3;
            // Упорядочиваем по размеру
            bc.ObjectsOrder = AForge.Imaging.ObjectsOrder.Size;
            // Обрабатываем картинку

            bc.ProcessImage(unmanaged);

            Rectangle[] rects = bc.GetObjectsRectangles();
            rez = "Насчитали " + rects.Length.ToString() + " прямоугольников!";
            //if (rects.Length == 0)
            //{
            //    finalPics[r, c] = AForge.Imaging.UnmanagedImage.FromManagedImage(new Bitmap(100, 100));
            //    return 0;
            //}

            // К сожалению, код с использованием подсчёта blob'ов не работает, поэтому просто высчитываем максимальное покрытие
            // для всех блобов - для нескольких цифр, к примеру, 16, можем получить две области - отдельно для 1, и отдельно для 6.
            // Строим оболочку, включающую все блоки. Решение плохое, требуется доработка
            int lx = unmanaged.Width;
            int ly = unmanaged.Height;
            int rx = 0;
            int ry = 0;
            for (int i = 0; i < rects.Length; ++i)
            {
                if (lx > rects[i].X) lx = rects[i].X;
                if (ly > rects[i].Y) ly = rects[i].Y;
                if (rx < rects[i].X + rects[i].Width) rx = rects[i].X + rects[i].Width;
                if (ry < rects[i].Y + rects[i].Height) ry = rects[i].Y + rects[i].Height;
            }
            if (rx <= lx || ry <= ly)
            {
                rx = unmanaged.Width;
                ry = unmanaged.Height;
                lx = 0;
                ly = 0;
            }
            // Обрезаем края, оставляя только центральные блобчики
            AForge.Imaging.Filters.Crop cropFilter = new AForge.Imaging.Filters.Crop(new Rectangle(lx, ly, rx - lx, ry - ly));
            unmanaged = cropFilter.Apply(unmanaged);

            //  Масштабируем до 100x100
            AForge.Imaging.Filters.ResizeBilinear scaleFilter = new AForge.Imaging.Filters.ResizeBilinear(100, 100);
            unmanaged = scaleFilter.Apply(unmanaged);

            lx = unmanaged.Width;
            ly = unmanaged.Height;
            rx = 0;
            ry = 0;
            unsafe
            {
                byte* ptr = (byte*)unmanaged.ImageData.ToPointer();

                int width = unmanaged.Width;
                int height = unmanaged.Height;
                int stride = unmanaged.Stride;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        byte pixelValue = ptr[y * stride + x];
                        if (lx > x && pixelValue > 200) lx = x;
                        if (ly > y && pixelValue > 200) ly = y;
                        if (rx < x && pixelValue > 200) rx = x;
                        if (ry < y && pixelValue > 200) ry = y;
                    }
                }
            }
            if (rx <= lx || ry <= ly)
            {
                rx = unmanaged.Width;
                ry = unmanaged.Height;
                lx = 0;
                ly = 0;
            }
            if (rx - lx < ry - ly)
            {
                int centerX = (rx + lx) / 2;
                rx = centerX + (ry - ly) / 2;
                lx = centerX - (ry - ly) / 2;
            }
            else
            {
                int centerY = (ry + ly) / 2;
                ry = centerY + (rx - lx) / 2;
                ly = centerY - (rx - lx) / 2;
            }
            cropFilter = new AForge.Imaging.Filters.Crop(new Rectangle(lx, ly, rx - lx, ry - ly));
            unmanaged = cropFilter.Apply(unmanaged);
            scaleFilter = new AForge.Imaging.Filters.ResizeBilinear(48, 48);
            unmanaged = scaleFilter.Apply(unmanaged);
            Threshold thresholdFilter = new Threshold(90);
            unmanaged = thresholdFilter.Apply(unmanaged);

            return rez;
        }

    }
}

