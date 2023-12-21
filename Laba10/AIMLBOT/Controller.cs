using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace NeuralNetwork1
{

    /// <summary>
    /// Класс-диспетчер, управляющий всеми остальными и служащий для связи с формой
    /// </summary>
    class Controller
    {
        //  Технически должен запускаться в отдельном потоке

        /// <summary>
        /// Возможные состояния - ожидание кадра, распознавание, анализ, движение.
        /// Их бы в какой-нибудь класс-диспетчер засунуть
        /// </summary>
        enum Stage { Idle, Watching, Processing, Recognizing, Responding };
        
        /// <summary>
        /// Текущее состояние
        /// </summary>
        private Stage currentState = Stage.Idle;
        
        /// <summary>
        /// Это флажок для индикации от главной формы о том, что она уехала
        /// и не обещала вернуться - то есть когда закрывается, и оставьте себе свои картинки
        /// </summary>
        private bool workNeeded { get; set; } = true;


        private FormUpdateDelegate formUpdateDelegate = null;

        /// <summa>
        /// Анализатор изображения - выполняет преобразования изображения с камеры и сопоставление с шаблонами
        /// </summary>
        public MagicEye processor = new MagicEye();
        
        /// <summary>
        /// Проверить, работает ли это
        /// </summary>
        /// <returns></returns>
        public Settings settings
        {
            get { return processor.settings; }
            set
            {
                processor.settings = value;
            }
        }

        private bool _imageProcessed = true;

        /// <summary>
        /// Готов ли процессор к обработке нового изображения
        /// </summary>
        public bool Ready { get { return _imageProcessed; } }

        /// <summary>
        /// Класс чтобы править ими всеми - и художником, и певцом, и мудрецом
        /// </summary>
        /// <param name="updater"></param>
        public Controller(FormUpdateDelegate updater)
        {
            formUpdateDelegate = updater;
        }


        
        /// <summary>
        /// Задаёт изображение для обработки
        /// </summary>
        /// <param name="image">Собственно изображение для обработки</param>
        /// <returns></returns>
        async public Task<bool> ProcessImage(Bitmap image)
        {
            if (!Ready) return false;
            _imageProcessed = false;

            bool processResult = await Task.Run(() => processor.ProcessImage(image, true));

            //  Более того, проверяем, не сдох ли султан, пока мы ишака дрессировали
            if (!workNeeded) return false;

            formUpdateDelegate();
            //  Этот блок сработает только по завершению обработки изображения
            //  Устанавливаем значение флажка о том, что мы закончили с обработкой изображения
            _imageProcessed = true;

            return true;
        }

        public Sample getSampleFromCurrent()
        {
            Bitmap processed = processor.processed;
            Rectangle rect = new Rectangle(0, 0, processed.Width, processed.Height);
            BitmapData bmpData = processed.LockBits(rect, ImageLockMode.ReadOnly, processed.PixelFormat);
            double[] input = new double[48 * 48];
            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0;
                int heightInPixels = bmpData.Height;
                int widthInBytes = bmpData.Stride;
                for (int y = 0; y < heightInPixels; y++)
                {
                    for (int x = 0; x < widthInBytes; x = x + 1)
                    {
                        double grayValue = ptr[(y * bmpData.Stride) + x] / 255.0;
                        input[(y * bmpData.Stride) + x] = grayValue;

                    }
                }
            }
            Sample sample = new Sample(input, 10);
            return sample;
        }

        /// <summary>
        /// Получает обработанное изображение
        /// </summary>
        /// <returns></returns>
        public Bitmap GetOriginalImage()
        {
            return processor.original;
        }

        /// <summary>
        /// Получает обработанное изображение
        /// </summary>
        /// <returns></returns>
        public Bitmap GetProcessedImage()
        {
            return processor.processed;
        }

        public Bitmap prepareRawImage(Bitmap rawImage)
        {
            processor.ProcessImage(rawImage, false);
            return processor.processed;
        }
    }
}
