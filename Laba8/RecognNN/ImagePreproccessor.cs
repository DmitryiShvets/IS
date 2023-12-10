using AForge.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork1
{
    public class Settings1 {

        public byte threshold = 120;
        public int border = 20;
        public int top = 40;
        public int left = 40;
        public float differenceLim = 0.15f;

        public Settings1(byte treshold,float differenceLim)
        {
            this.threshold = treshold;
            this.differenceLim = differenceLim;
        }

    }

    public class ImagePreproccessor
    {
        private Settings1 settings;
        public ImagePreproccessor(Settings1 settings)
        {
            this.settings = settings;
        }


        private UnmanagedImage GetGreyScaledImage(UnmanagedImage image)
        {
            AForge.Imaging.Filters.Grayscale grayFilter = new AForge.Imaging.Filters.Grayscale(0.2125, 0.7154, 0.0721);
            return grayFilter.Apply(image);
        }

        private UnmanagedImage GetFilteredImage(UnmanagedImage image)
        {
            AForge.Imaging.Filters.BradleyLocalThresholding threshldFilter = new AForge.Imaging.Filters.BradleyLocalThresholding();
            threshldFilter.PixelBrightnessDifferenceLimit = settings.differenceLim;
            return threshldFilter.Apply(image);
        }

        private double[] ConvertImageToArray(Bitmap processed)
        {
            double[] input = new double[processed.Width * processed.Height];
            int size = 28;
            for (int i = 0; i < processed.Height; i++)
            {
                for (int j = 0; j < processed.Width; j++)
                {
                    input[i * size + j] = processed.GetPixel(j, i).R;
                }

            }
            return input;
        }

        public double[] ProcessImage(Bitmap original)
        {
            
            //  Можно было, конечено, и не кидаться эксепшенами в истерике, но идите и купите себе нормальную камеру!
            int side = original.Height;

            //  Отпиливаем границы, но не более половины изображения
            if (side < 4 * settings.border) settings.border = side / 4;
            side -= 2 * settings.border;

            
            var uProcessed = UnmanagedImage.FromManagedImage(original);

            uProcessed = GetGreyScaledImage(uProcessed);
            //пороговое отсечение 
            uProcessed = GetFilteredImage(uProcessed);
            // получаем самый большой блоб - нашу циферку, которую будем распознавать
            uProcessed  = GetMaxBlob(uProcessed);
            uProcessed = GetScaledImage(uProcessed, 28, 28);
           var  processed = uProcessed.ToManagedImage();
           
            /*
            using (StreamWriter sw = new StreamWriter("test_output.txt"))
            {
                sw.WriteLine("start");
                for (int i = 0; i < processed.Width; i++)
                {
                    for (int j = 0; j < processed.Height; j++)
                    {
                        sw.Write(processed.GetPixel(i, j).R + " ");
                    }
                    sw.WriteLine();
                }
                sw.WriteLine("end");
            }
              */
            return ConvertImageToArray(processed);
        }


        public Bitmap GetProcessedImage(Bitmap original)
        {
            //  Можно было, конечено, и не кидаться эксепшенами в истерике, но идите и купите себе нормальную камеру!
            int side = original.Height;

            //  Отпиливаем границы, но не более половины изображения
            if (side < 4 * settings.border) settings.border = side / 4;
            side -= 2 * settings.border;

            //  Мы сейчас занимаемся тем, что красиво оформляем входной кадр, чтобы вывести его на форму
            Rectangle cropRect = new Rectangle((original.Width - original.Height) / 2 + settings.left + settings.border, settings.top + settings.border, side, side);


            var uProcessed = UnmanagedImage.FromManagedImage(original);

            uProcessed = GetGreyScaledImage(uProcessed);
            //пороговое отсечение 
            uProcessed = GetFilteredImage(uProcessed);
            // получаем самый большой блоб - нашу циферку, которую будем распознавать
            uProcessed = GetMaxBlob(uProcessed);
            uProcessed = GetScaledImage(uProcessed, 28, 28);
            var processed = uProcessed.ToManagedImage();

            /*
            using (StreamWriter sw = new StreamWriter("test_output.txt"))
            {
                sw.WriteLine("start");
                for (int i = 0; i < processed.Width; i++)
                {
                    for (int j = 0; j < processed.Height; j++)
                    {
                        sw.Write(processed.GetPixel(i, j).R + " ");
                    }
                    sw.WriteLine();
                }
                sw.WriteLine("end");
            }
              */
            return processed;
        }


        private UnmanagedImage GetScaledImage(UnmanagedImage unmanaged,int w,int h)
        {
            AForge.Imaging.Filters.ResizeBilinear scaleFilter = new AForge.Imaging.Filters.ResizeBilinear(w, h);
            return scaleFilter.Apply(unmanaged);
        }

        private UnmanagedImage GetMaxBlob(UnmanagedImage unmanaged)
        {
           

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

            var maxRect = rects.FirstOrDefault();

          /* 
           * 
           * здесь по сути мы из найденных блобов делаем один большой блоб - может понадобится если захотим сразу несколько цифр поймать
           * 
           * 
           * уверен? 
              нейронка же может только одну цифру распознать 

            а, ну да 
          можно выпиливать
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
          */

            var beforeCutting  = unmanaged.ToManagedImage();
            beforeCutting.Save(@"../../beforeBlobbing.png");
            // Обрезаем края, оставляя только центральные блобчики
            AForge.Imaging.Filters.Crop cropFilter = new AForge.Imaging.Filters.Crop(new Rectangle(maxRect.X, maxRect.Y, maxRect.X + maxRect.Width, maxRect.Y + maxRect.Height));
            unmanaged = cropFilter.Apply(unmanaged);
            var afterCutting = unmanaged.ToManagedImage();
            afterCutting.Save(@"../../afterBlobbing.png");
            // InvertFilter.ApplyInPlace(unmanaged);
            return unmanaged;
        }

    }
}
