using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba5
{
    public partial class Form1 : Form
    {
        private ProductionModel model;
        private string target_fact = "f18";
        public Form1()
        {
            InitializeComponent();
            model = new ProductionModel();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btn_load_db_Click(object sender, EventArgs e)
        {
            model.Init("../../facts.txt", "../../rules.txt");
            //model.Init("../../facts_test.txt", "../../rules_test.txt");
            lst_resolve.Clear();
            lst_facts.Items.Clear();
            lst_rules.Clear();
            foreach (var item in model.KnowledgeBaseFacts)
            {
                lst_facts.Items.Add(item);
            }
            foreach (var item in model.KnowledgeBaseProduction)
            {
                lst_rules.Items.Add(item.ToString());
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < lst_facts.Items.Count; i++) lst_facts.SetSelected(i, false);
            lst_resolve.Clear();
        }

        private void btn_forvard_resolve_Click(object sender, EventArgs e)
        {
            lst_resolve.Clear();
            List<string> list_base_facts = new List<string>();
            var selected = lst_facts.SelectedItems;
            foreach (var item in selected)
            {
                Fact fact = item as Fact;
                list_base_facts.Add(fact.Id);
            }

            Resolve resolve = model.ForwardChaining(list_base_facts, target_fact);
            ProccesForwardResolve(resolve);
        }

        private void btn_back_resolve_Click(object sender, EventArgs e)
        {
            lst_resolve.Clear();
            List<string> list_base_facts = new List<string>();
            var selected = lst_facts.SelectedItems;
            foreach (var item in selected)
            {
                Fact fact = item as Fact;
                list_base_facts.Add(fact.Id);
            }

            Resolve resolve = model.BackChaining(list_base_facts, target_fact);
            ProccesBackResolve(resolve);
        }
        private void ProccesBackResolve(Resolve resolve)
        {
            if (resolve.successful && resolve.bd_rules != null)
            {
                lst_resolve.Items.Add("Целевой факт удалось вывести. Порядок вывода:\n");
                lst_resolve.Items.Add("Целевой факт:\n");
                string cur_facts = "";
                for (int j = 0; j < resolve.bd_facts[0].Count; j++)
                {
                    cur_facts += resolve.bd_facts[0][j].Id + ", ";
                }
                lst_resolve.Items.Add(cur_facts + '\n');

                for (int i = 0; i < resolve.bd_rules.Count; i++)
                {
                    lst_resolve.Items.Add("Получен из правила:\n");
                    lst_resolve.Items.Add(resolve.bd_rules[i].ToString());
                    lst_resolve.Items.Add("Примененные факты:\n");
                    cur_facts = "";
                    for (int j = 0; j < resolve.bd_facts[i + 1].Count; j++)
                    {
                        cur_facts += resolve.bd_facts[i + 1][j].Id + ", ";
                    }
                    lst_resolve.Items.Add(cur_facts + '\n');
                }
            }

            else if (resolve.successful && resolve.bd_rules == null)
            {
                lst_resolve.Items.Add("Целевой факт уже является аксиомой\n");
                lst_resolve.Items.Add("Известные факты:\n");
                string cur_facts = "";
                for (int j = 0; j < resolve.bd_facts[0].Count; j++)
                {
                    cur_facts += resolve.bd_facts[0][j].Id + ", ";
                }
                lst_resolve.Items.Add(cur_facts + '\n');
            }
            else
            {
                lst_resolve.Items.Add("Целевой факт не удалось вывести. Порядок вывода:");
                if (resolve.bd_rules.Count > 0)
                {
                    for (int i = 0; i < resolve.bd_rules.Count; i++)
                    {
                        lst_resolve.Items.Add("Известные факты:\n");
                        string cur_facts = "";
                        for (int j = 0; j < resolve.bd_facts[i].Count; j++)
                        {
                            cur_facts += resolve.bd_facts[i][j].Id + ", ";
                        }
                        lst_resolve.Items.Add(cur_facts + '\n');
                        lst_resolve.Items.Add("Примененное правило:\n");
                        lst_resolve.Items.Add(resolve.bd_rules[i].ToString());

                    }
                }
            }

        }
        private void ProccesForwardResolve(Resolve resolve)
        {
            if (resolve.successful && resolve.bd_rules != null)
            {
                lst_resolve.Items.Add("Целевой факт удалось вывести. Порядок вывода:\n");
                lst_resolve.Items.Add("Известные факты:\n");
                string cur_facts = "";
                for (int j = 0; j < resolve.bd_facts[0].Count; j++)
                {
                    cur_facts += resolve.bd_facts[0][j].Id + ", ";
                }
                lst_resolve.Items.Add(cur_facts + '\n');

                for (int i = 0; i < resolve.bd_rules.Count; i++)
                {
                    lst_resolve.Items.Add("Примененное правило:\n");
                    lst_resolve.Items.Add(resolve.bd_rules[i].ToString());
                    lst_resolve.Items.Add("Известные факты:\n");
                    cur_facts = "";
                    for (int j = 0; j < resolve.bd_facts[i + 1].Count; j++)
                    {
                        cur_facts += resolve.bd_facts[i + 1][j].Id + ", ";
                    }
                    lst_resolve.Items.Add(cur_facts + '\n');
                }
            }

            else if (resolve.successful && resolve.bd_rules == null)
            {
                lst_resolve.Items.Add("Целевой факт уже является аксиомой\n");
                lst_resolve.Items.Add("Известные факты:\n");
                string cur_facts = "";
                for (int j = 0; j < resolve.bd_facts[0].Count; j++)
                {
                    cur_facts += resolve.bd_facts[0][j].Id + ", ";
                }
                lst_resolve.Items.Add(cur_facts + '\n');
            }
            else
            {
                lst_resolve.Items.Add("Целевой факт не удалось вывести. Порядок вывода:");
                if (resolve.bd_rules.Count > 0)
                {
                    for (int i = 0; i < resolve.bd_rules.Count; i++)
                    {
                        lst_resolve.Items.Add("Известные факты:\n");
                        string cur_facts = "";
                        for (int j = 0; j < resolve.bd_facts[i].Count; j++)
                        {
                            cur_facts += resolve.bd_facts[i][j].Id + ", ";
                        }
                        lst_resolve.Items.Add(cur_facts + '\n');
                        lst_resolve.Items.Add("Примененное правило:\n");
                        lst_resolve.Items.Add(resolve.bd_rules[i].ToString());

                    }
                }
                else
                {
                    lst_resolve.Items.Add("Известные факты:\n");
                    string cur_facts = "";
                    for (int j = 0; j < resolve.bd_facts[0].Count; j++)
                    {
                        cur_facts += resolve.bd_facts[0][j].Id + ", ";
                    }
                    lst_resolve.Items.Add(cur_facts + '\n');
                }
            }
        }
    }
}
