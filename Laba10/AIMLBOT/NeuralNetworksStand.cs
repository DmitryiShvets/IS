﻿using AForge.Video.DirectShow;
using AForge.Video;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace NeuralNetwork1
{
    delegate void FormUpdateDelegate();

    public partial class NeuralNetworksStand : Form
    {
        /// <summary>
        /// Генератор изображений (образов)
        /// </summary>
        //GenerateImage generator = new GenerateImage();
        LoaderImage loader = new LoaderImage();
        ImprovedLoader impLoader = null;
        Controller controllerForLoader = null;
        //AugmentationImage augmentation = new AugmentationImage();
        /// <summary>
        /// Текущая выбранная через селектор нейросеть
        /// </summary>
        public BaseNetwork Net
        {
            get
            {
                var selectedItem = (string)netTypeBox.SelectedItem;
                if (!networksCache.ContainsKey(selectedItem))
                    networksCache.Add(selectedItem, CreateNetwork(selectedItem));

                return networksCache[selectedItem];
            }
        }

        private readonly Dictionary<string, Func<int[], BaseNetwork>> networksFabric;
        private Dictionary<string, BaseNetwork> networksCache = new Dictionary<string, BaseNetwork>();

        /// <summary>
        /// Конструктор формы стенда для работы с сетями
        /// </summary>
        /// <param name="networksFabric">Словарь функций, создающих сети с заданной структурой</param>
        public NeuralNetworksStand(Dictionary<string, Func<int[], BaseNetwork>> networksFabric)
        {
            InitializeComponent();
            this.networksFabric = networksFabric;
            netTypeBox.Items.AddRange(this.networksFabric.Keys.Select(s => (object)s).ToArray());
            netTypeBox.SelectedIndex = 0;
            //generator.FigureCount = (int)classCounter.Value;
            //augmentation.FigureCount = (int)classCounter.Value;
            button3_Click(this, null);
            pictureBox1.Image = Properties.Resources.Title;



            // Список камер получаем
            videoDevicesList = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo videoDevice in videoDevicesList)
            {
                cmbVideoSource.Items.Add(videoDevice.Name);
            }
            if (cmbVideoSource.Items.Count > 0)
            {
                cmbVideoSource.SelectedIndex = 0;
                var vcd = new VideoCaptureDevice(videoDevicesList[cmbVideoSource.SelectedIndex].MonikerString);
                resolutionsBox.Items.Clear();
                for (int i = 0; i < vcd.VideoCapabilities.Length; i++)
                    resolutionsBox.Items.Add(vcd.VideoCapabilities[i].FrameSize.ToString());
                resolutionsBox.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("А нет у вас камеры!", "Ошибочка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            controller = new Controller(new FormUpdateDelegate(UpdateFormFields));
            controllerForLoader = new Controller(new FormUpdateDelegate(UpdateFormFields));
            loader.FigureCount = (int)classCounter.Value;
            impLoader = new ImprovedLoader(controllerForLoader);
            impLoader.FigureCount = (int)classCounter.Value;
            // updateTmr = new System.Threading.Timer(Tick, evnt, 500, 100);

            cb_cur_class.SelectedIndex = 0;

            comboBoxMethod.Items.Add("Способ сложения");
            comboBoxMethod.Items.Add("Способ шакала");
            comboBoxMethod.Items.Add("Способ чередования");


            comboBoxMethod.SelectedIndex = 0;
        }

        public void UpdateLearningInfo(double progress, double error, TimeSpan elapsedTime)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new TrainProgressHandler(UpdateLearningInfo), progress, error, elapsedTime);
                return;
            }

            st_lable.Text = "Ошибка: " + error;
            int progressPercent = (int)Math.Round(progress * 100);
            progressPercent = Math.Min(100, Math.Max(0, progressPercent));
            elapsedTimeLabel.Text = "Затраченное время : " + elapsedTime.Duration().ToString(@"hh\:mm\:ss\:ff");
            progressBar1.Value = progressPercent;
        }


        private void set_result(Sample figure)
        {
            label1.ForeColor = figure.Correct() ? Color.Green : Color.Red;

            label1.Text = "Распознано : " + figure.recognizedClass;

            label8.Text = string.Join("\n", figure.Output.Select(d => $"{d:f2}"));
            //pictureBox1.Image = generator.GenBitmap();
            if (checkLoader.Checked) pictureBox1.Image = impLoader.GenImage();
            else pictureBox1.Image = loader.GenImage(comboBoxMethod.SelectedIndex);
            
            //pictureBox1.Image = augmentation.GenBitmap();
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            //Sample fig = generator.GenerateFigure();
            Sample fig = null;
            if (checkLoader.Checked) fig = impLoader.LoadImage(comboBoxMethod.SelectedIndex);
            else fig = loader.LoadImage(comboBoxMethod.SelectedIndex);
            //Sample fig = augmentation.Get30x30();

            Net.Predict(fig);

            set_result(fig);
        }

        private async Task<double> train_networkAsync(int training_size, int epoches, double acceptable_error,
            bool parallel = true)
        {
            //  Выключаем всё ненужное
            label1.Text = "Выполняется обучение...";
            label1.ForeColor = Color.Red;
            groupBox1.Enabled = false;
            pictureBox1.Enabled = false;
            trainOneButton.Enabled = false;

            //  Создаём новую обучающую выборку
            SamplesSet samples = null;
            if (checkLoader.Checked) samples = impLoader.LoadSampleSet();
            else samples = loader.LoadSampleSet();
            //SamplesSet samples = augmentation.LoadSampleSet();
            /*SamplesSet samples = new SamplesSet();

            for (int i = 0; i < training_size; i++)
                samples.AddSample(generator.GenerateFigure());*/
            try
            {
                //  Обучение запускаем асинхронно, чтобы не блокировать форму
                var curNet = Net;
                double f = await Task.Run(() => curNet.TrainOnDataSet(samples, epoches, acceptable_error, parallel));

                label1.Text = "Щелкните на картинку для теста нового образа";
                label1.ForeColor = Color.Green;
                groupBox1.Enabled = true;
                pictureBox1.Enabled = true;
                trainOneButton.Enabled = true;
                st_lable.Text = "Ошибка: " + f;
                st_lable.ForeColor = Color.Green;
                return f;
            }
            catch (Exception e)
            {
                label1.Text = $"Исключение: {e.Message}";
            }

            return 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            train_networkAsync((int)TrainingSizeCounter.Value, (int)EpochesCounter.Value,
                (100 - AccuracyCounter.Value) / 100.0, parallelCheckBox.Checked);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Enabled = false;
            //  Тут просто тестирование новой выборки
            //  Создаём новую обучающую выборку
            //SamplesSet samples = loader.LoadSampleSetTest();
            SamplesSet samples = null;
            if (checkLoader.Checked) samples = impLoader.LoadSampleSetTest();
            else samples = loader.LoadSampleSetTest();
            //SamplesSet samples = augmentation.LoadSampleSetTest();
            //SamplesSet samples = new SamplesSet();

            /*
            for (int i = 0; i < (int)TrainingSizeCounter.Value; i++)
                samples.AddSample(generator.GenerateFigure());*/

            double accuracy = samples.TestNeuralNetwork(Net);

            st_lable.Text = $"Точность на тестовой выборке : {accuracy * 100,5:F2}%";
            st_lable.ForeColor = accuracy * 100 >= AccuracyCounter.Value ? Color.Green : Color.Red;

            Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //  Проверяем корректность задания структуры сети
            int[] structure = CurrentNetworkStructure();
            //if (structure.Length < 2 || structure[0] != 400 ||
            //    structure[structure.Length - 1] != generator.FigureCount)
            //{
            //    MessageBox.Show(
            //        $"В сети должно быть более двух слоёв, первый слой должен содержать 400 нейронов, последний - ${generator.FigureCount}",
            //        "Ошибка", MessageBoxButtons.OK);
            //    return;
            //}

            // Чистим старые подписки сетей
            foreach (var network in networksCache.Values)
                network.TrainProgress -= UpdateLearningInfo;
            // Пересоздаём все сети с новой структурой
            networksCache = networksCache.ToDictionary(oldNet => oldNet.Key, oldNet => CreateNetwork(oldNet.Key));
        }

        private int[] CurrentNetworkStructure()
        {
            return netStructureBox.Text.Split(';').Select(int.Parse).ToArray();
        }

        private void classCounter_ValueChanged(object sender, EventArgs e)
        {
            //generator.FigureCount = (int)classCounter.Value;
            loader.FigureCount = (int)classCounter.Value;
            impLoader.FigureCount = (int)classCounter.Value;
            //augmentation.FigureCount = (int)classCounter.Value;
            var vals = netStructureBox.Text.Split(';');
            if (!int.TryParse(vals.Last(), out _)) return;
            vals[vals.Length - 1] = classCounter.Value.ToString();
            netStructureBox.Text = vals.Aggregate((partialPhrase, word) => $"{partialPhrase};{word}");
        }

        private void btnTrainOne_Click(object sender, EventArgs e)
        {
            if (Net == null) return;
            //Sample fig = generator.GenerateFigure();
            Sample fig = null;
            //Sample fig = augmentation.Get30x30();
            //pictureBox1.Image = generator.GenBitmap();
            if (checkLoader.Checked)
            {
                fig = impLoader.LoadImage(comboBoxMethod.SelectedIndex);
                pictureBox1.Image = impLoader.GenImage();
            }
            else
            {
                fig = loader.LoadImage(comboBoxMethod.SelectedIndex);
                pictureBox1.Image = loader.GenImage(comboBoxMethod.SelectedIndex);
            }
            //pictureBox1.Image = augmentation.GenBitmap();
            pictureBox1.Invalidate();
            Net.Train(fig, 0.00005, parallelCheckBox.Checked);
            set_result(fig);
        }

        private BaseNetwork CreateNetwork(string networkName)
        {
            var network = networksFabric[networkName](CurrentNetworkStructure());
            network.TrainProgress += UpdateLearningInfo;
            return network;
        }

        private void recreateNetButton_MouseEnter(object sender, EventArgs e)
        {
            infoStatusLabel.Text = "Заново пересоздаёт сеть с указанными параметрами";
        }

        private void netTrainButton_MouseEnter(object sender, EventArgs e)
        {
            infoStatusLabel.Text = "Обучить нейросеть с указанными параметрами";
        }

        private void testNetButton_MouseEnter(object sender, EventArgs e)
        {
            infoStatusLabel.Text = "Тестировать нейросеть на тестовой выборке такого же размера";
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            Net.Print();
        }

        /// <summary>
        /// Класс, реализующий всю логику работы
        /// </summary>
        private Controller controller = null;
        /// <summary>
        /// Событие для синхронизации таймера
        /// </summary>
        private AutoResetEvent evnt = new AutoResetEvent(false);

        /// <summary>
        /// Список устройств для снятия видео (веб-камер)
        /// </summary>
        private FilterInfoCollection videoDevicesList;

        /// <summary>
        /// Выбранное устройство для видео
        /// </summary>
        private IVideoSource videoSource;

        /// <summary>
        /// Таймер для измерения производительности (времени на обработку кадра)
        /// </summary>
        private Stopwatch sw = new Stopwatch();

        /// <summary>
        /// Таймер для обновления объектов интерфейса
        /// </summary>
        System.Threading.Timer updateTmr;

        public void UpdateFormFields()
        {
            //  Проверяем, вызвана ли функция из потока главной формы. Если нет - вызов через Invoke
            //  для синхронизации, и выход
            if (statusLabel.InvokeRequired)
            {
                this.Invoke(new FormUpdateDelegate(UpdateFormFields));
                return;
            }

            sw.Stop();
            ticksLabel.Text = "Тики : " + sw.Elapsed.ToString();
            originalImageBox.Image = controller.GetOriginalImage();
            processedImgBox.Image = controller.GetProcessedImage();
        }

        /// <summary>
        /// Обёртка для обновления формы - перерисовки картинок, изменения состояния и прочего
        /// </summary>
        /// <param name="StateInfo"></param>
        public void Tick(object StateInfo)
        {
            UpdateFormFields();
            Console.WriteLine("2");
            return;
        }


        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            //  Время засекаем
            sw.Restart();

            //  Отправляем изображение на обработку, и выводим оригинал (с раскраской) и разрезанные изображения
            if (controller.Ready)

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                controller.ProcessImage((Bitmap)eventArgs.Frame.Clone());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            //  Это выкинуть в отдельный поток!
            //  И отдать делегат? Или просто проверять значение переменной?
            //  Тут хрень какая-то

            //currentState = Stage.Thinking;
            //sage.solveState(processor.currentDeskState, 16, 7);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (videoSource == null)
            {
                var vcd = new VideoCaptureDevice(videoDevicesList[cmbVideoSource.SelectedIndex].MonikerString);
                vcd.VideoResolution = vcd.VideoCapabilities[resolutionsBox.SelectedIndex];
                Debug.WriteLine(vcd.VideoCapabilities[1].FrameSize.ToString());
                Debug.WriteLine(resolutionsBox.SelectedIndex);
                videoSource = vcd;
                videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
                videoSource.Start();
                StartButton.Text = "Стоп";
                controlPanel.Enabled = true;
                cmbVideoSource.Enabled = false;
            }
            else
            {
                videoSource.SignalToStop();
                if (videoSource != null && videoSource.IsRunning && originalImageBox.Image != null)
                {
                    originalImageBox.Image.Dispose();
                }
                videoSource = null;
                StartButton.Text = "Старт";
                controlPanel.Enabled = false;
                cmbVideoSource.Enabled = true;
            }
        }

        private void tresholdTrackBar_ValueChanged(object sender, EventArgs e)
        {
            controller.settings.threshold = (byte)tresholdTrackBar.Value;
            controller.settings.differenceLim = (float)tresholdTrackBar.Value / tresholdTrackBar.Maximum;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (updateTmr != null)
                updateTmr.Dispose();

            //  Как-то надо ещё робота подождать, если он работает

            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W: controller.settings.decTop(); Debug.WriteLine("Up!"); break;
                case Keys.S: controller.settings.incTop(); Debug.WriteLine("Down!"); break;
                case Keys.A: controller.settings.decLeft(); Debug.WriteLine("Left!"); break;
                case Keys.D: controller.settings.incLeft(); Debug.WriteLine("Right!"); break;
                case Keys.Q: controller.settings.border++; Debug.WriteLine("Plus!"); break;
                case Keys.E: controller.settings.border--; Debug.WriteLine("Minus!"); break;
                case Keys.F:
                    SaveFile();
                    break;
                case Keys.G:
                    MakePhoto();
                    break;
            }
        }

        private void cmbVideoSource_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var vcd = new VideoCaptureDevice(videoDevicesList[cmbVideoSource.SelectedIndex].MonikerString);
            resolutionsBox.Items.Clear();
            for (int i = 0; i < vcd.VideoCapabilities.Length; i++)
                resolutionsBox.Items.Add(vcd.VideoCapabilities[i].FrameSize.ToString());
            resolutionsBox.SelectedIndex = 0;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            controller.settings.processImg = checkBox1.Checked;
        }

        private void SaveFile()
        {
            if (controlPanel.Enabled)
            {
                int fclass = cb_cur_class.SelectedIndex;

                if (Directory.Exists(fclass.ToString()))
                {
                    int cfiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "/" + fclass).Length + 30;
                    controller.processor.processed.Save(fclass + "/" + fclass + "_" + cfiles + ".jpg");
                }
                else
                {
                    Directory.CreateDirectory(fclass.ToString());
                    int cfiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "/" + fclass).Length + 30;
                    controller.processor.processed.Save(fclass + "/" + fclass + "_" + cfiles + ".jpg");
                }
                Debug.WriteLine("Photo!");
            }

        }

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void LoadDataset_Click(object sender, EventArgs e)
        {
            if (checkLoader.Checked) impLoader.LoadDataset(comboBoxMethod.SelectedIndex);
            else loader.LoadDataset(comboBoxMethod.SelectedIndex);
        }

        private void createDataset_Click(object sender, EventArgs e)
        {
            if (checkLoader.Checked) impLoader.CreateDataset();
            else loader.CreateDataset(comboBoxMethod.SelectedIndex);
        }

        private void btn_30_load_Click(object sender, EventArgs e)
        {
            Random rand = new Random();

            // путь к dataset
            string path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\Dataset\\numbers";
            // получение всех директорий
            List<string> directories = Directory.GetDirectories(path).ToList();

            // получение случайной директории(класса)
            int randomDirectory = rand.Next(0, 10);
            List<string> files = Directory.GetFiles(directories[randomDirectory]).ToList();

            // получение случайной файла
            int randomFile = rand.Next(0, files.Count());


            // загрузка изображения
            Bitmap bmp = new Bitmap(System.Drawing.Image.FromFile(files[randomFile]));

            controller.processor.ProcessImage(bmp, true);

            test300_pict.Image = bmp;
            test30_pict.Image = controller.processor.processed;


            //Bitmap bmp = new Bitmap(test30_pict.Width, test30_pict.Height);
            //Graphics g = Graphics.FromImage(bmp);
            //g.Clear(Color.White);
            //g.DrawImage(test300_pict.Image, new Rectangle(0, 0, 30, 30));
            //test30_pict.Image = bmp;
        }

        private void comboBoxMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> split = netStructureBox.Text.Split(';').ToList();
            switch (comboBoxMethod.SelectedIndex)
            {
                case 0:
                    if (checkLoader.Checked) netStructureBox.Text = "96";
                    else netStructureBox.Text = "600";
                    break;
                case 1:
                    if (checkLoader.Checked) netStructureBox.Text = "2304";
                    else netStructureBox.Text = "784";
                    break;
                case 2:
                    if (checkLoader.Checked) netStructureBox.Text = "96";
                    else netStructureBox.Text = "600";
                    break;
            }
            for (int i = 1; i < split.Count(); i++)
            {
                netStructureBox.Text += ";" + split[i];
            }
        }

        private void button_F_Click(object sender, EventArgs e)
        {
            MakePhoto();
        }

        private void MakePhoto()
        {
            recognizedBox.Image = controller.processor.processed;
            string path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\Dataset\\numbers";
            controller.processor.processed.Save(path + "\\input.jpg");
        }

        private void check_photo_Click(object sender, EventArgs e)
        {
            //Sample photo = loader.CheckImage(comboBoxMethod.SelectedIndex);
            Sample photo = null;

            // отрисовка
            if (Net == null) return;
            if (checkLoader.Checked)
            {
                photo = impLoader.LoadImage(comboBoxMethod.SelectedIndex, true);
                pictureBox1.Image = impLoader.GenImage();
            }
            else
            {
                photo = loader.CheckImage(comboBoxMethod.SelectedIndex);
                pictureBox1.Image = loader.GenImage(comboBoxMethod.SelectedIndex);
            }
            pictureBox1.Invalidate();

            //
            Net.Predict(photo);
            set_result(photo);
        }

        private void button_dop_Click(object sender, EventArgs e)
        {
            if (Net == null) return;
            //Sample fig = loader.CheckImage(comboBoxMethod.SelectedIndex);
            Sample fig = null;
            if (checkLoader.Checked) fig = impLoader.LoadImage(comboBoxMethod.SelectedIndex, true);
            else fig = loader.CheckImage(comboBoxMethod.SelectedIndex);

            fig.actualClass = (FigureType)Int32.Parse(textBox_dop.Text);

            Console.WriteLine(fig.actualClass);

            Net.Train(fig, 0.00005, parallelCheckBox.Checked);
            set_result(fig);
        }
    }
}