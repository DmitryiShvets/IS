﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralNetwork1
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new NeuralNetworksStand(new Dictionary<string, Func<int[], BaseNetwork>>
            {
                // Тут можно добавить свои нейросети
                {"Accord.Net Perseptron", structure => new AccordNet(structure)},
                {"Персептрон Швеца", structure => new StudentNet(structure)},
                {"Персептрон Березенцева", structure => new StudentNetwork(structure)},
            }));
        }
    }
}