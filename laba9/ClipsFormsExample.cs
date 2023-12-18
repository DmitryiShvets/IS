using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Windows.Forms;

using CLIPSNET;
using Microsoft.Speech.Synthesis;

namespace ClipsFormsExample
{
    public partial class ClipsFormsExample : Form
    {
        private CLIPSNET.Environment clips = new CLIPSNET.Environment();

        public ClipsFormsExample()
        {
            InitializeComponent();
            cb_task.SelectedIndex = 0;
            cb_hero.SelectedIndex = 0;
        }

        private void HandleResponse()
        {
            //  Вытаскиаваем факт из ЭС
            String evalStr = "(find-fact ((?f ioproxy)) TRUE)";

            FactAddressValue fv = (FactAddressValue)((MultifieldValue)clips.Eval(evalStr))[0];

            MultifieldValue damf = (MultifieldValue)fv["messages"];

            foreach (var v in damf)
            {
                outputBox.Text += v.ToString() + System.Environment.NewLine;
            }
            clips.Eval("(assert (clearmessage))");

            FactAddressValue f = clips.FindFact("select-task");
            if (f != null)
            {
                string hero_name = f.GetSlotValue("hero-name").ToString();

                string hero_task = cb_task.GetItemText(cb_task.Items[cb_task.SelectedIndex]);
                outputBox.Text += "Выберите специализацию для героя: " + hero_name + System.Environment.NewLine;
                clips.Eval("(modify " + f.FactIndex + " (task " + hero_task + "))"); //TODO add cf
                clips.Eval("(assert(addtask " + hero_name + "))");
                clips.Run();
            }

            /*
            FactAddressValue trash = clips.FindFact("threshold");
            if (trash != null)
            {
                Console.WriteLine(trash.GetSlotValue("cf"));
            }
            */

            FactAddressValue nameHero = clips.FindFact("hero");
            if (nameHero != null)
            {
                Console.WriteLine(nameHero.GetSlotValue("cf"));
            }
        }

        private void nextBtn_Click(object sender, EventArgs e)
        {
            clips.Run();
            outputBox.Text += "Новая итерация : " + System.Environment.NewLine;
            HandleResponse();

        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            outputBox.Text = "Выполнены команды Clear и Reset." + System.Environment.NewLine;
            //  Здесь сохранение в файл, и потом инициализация через него
            clips.Clear();

            //  Так тоже можно - без промежуточного вывода в файл
            clips.LoadFromString(codeBox.Text);

            clips.Reset();
            clips.Eval("(assert (clearmessage))");
        }

        private void openFile_Click(object sender, EventArgs e)
        {
            if (clipsOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                codeBox.Text = System.IO.File.ReadAllText(clipsOpenFileDialog.FileName);
                Text = "Экспертная система \"Дота 2\" – " + clipsOpenFileDialog.FileName;
            }
        }

        private void saveAsButton_Click(object sender, EventArgs e)
        {
            clipsSaveFileDialog.FileName = clipsOpenFileDialog.FileName;
            if (clipsSaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(clipsSaveFileDialog.FileName, codeBox.Text);
            }
        }

        private void add_hero_Click(object sender, EventArgs e)
        {
            string hero = cb_hero.GetItemText(cb_hero.Items[cb_hero.SelectedIndex]);
            string cf = "0.1"; // TODO захардкодить для каждого героя
            clips.Eval("(assert(addhero " + hero + " " + cf + "))");
            //clips.Run();
            //clips.Run();
            HandleResponse();
        }

        private void btn_select_task_Click(object sender, EventArgs e)
        {
            string t = "0.2";
           
            clips.Eval($"(assert(threshold (cf (string-to-field \"{t}\"))))");
            
            clips.Run();
        }
    }
}
