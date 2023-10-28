using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba5
{
    internal class ProductionModel
    {
        public HashSet<Fact> KnowledgeBaseFacts { get; set; }  // База знаний с фактами
        public HashSet<Rule> KnowledgeBaseProduction { get; set; }  // База знаний с правилами продукции

        public ProductionModel()
        {
            KnowledgeBaseFacts = new HashSet<Fact>();
            KnowledgeBaseProduction = new HashSet<Rule>();
        }
        public void Init(string file_facts, string file_rules)
        {
            LoadFacts(file_facts);
            LoadRules(file_rules);

        }
        // Метод прямого вывода (без изменений)
        public Resolve ForwardChaining(List<string> initialFactIds, string targetFactId)
        {
            var initialFacts = KnowledgeBaseFacts.Where(f => initialFactIds.Contains(f.Id)).ToList();
            var targetFact = KnowledgeBaseFacts.FirstOrDefault(f => f.Id == targetFactId);

            Resolve resolve = new Resolve();
            var inferredFacts = new List<Fact>(initialFacts);

            bool newFactInferred = false;
            resolve.bd_facts.Add(new List<Fact>(inferredFacts));
            if (inferredFacts.Contains(targetFact))
            {           
                resolve.bd_rules = null;
                resolve.successful = true;
                return resolve;
            }

            while (true)
            {
                newFactInferred = false;
                foreach (var rule in KnowledgeBaseProduction)
                {
                    if (inferredFacts.ContainsAll(rule.Lhs) && !inferredFacts.Contains(rule.Rhs))
                    {
                        inferredFacts.Add(rule.Rhs);
                        newFactInferred = true;

                        resolve.bd_rules.Add(rule);
                        resolve.bd_facts.Add(new List<Fact>(inferredFacts));
                    }
                }

                if (!newFactInferred || inferredFacts.Contains(targetFact))
                    break;
            }

            if (inferredFacts.Contains(targetFact))
            {
                resolve.successful = true;
                return resolve;
            }
            else return resolve; // Невозможно вывести целевой факт
        }

        private void LoadFacts(string file_name)
        {
            HashSet<Fact> facts = new HashSet<Fact>();
            try
            {
                foreach (string line in File.ReadLines(file_name))
                {
                    if (line == "") continue;
                    string[] parts = line.Split(';');
                    if (parts.Length >= 3)
                    {
                        string id = parts[0];
                        double ratio;
                        if (double.TryParse(parts[1], NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out ratio))
                        {
                            string description = parts[2];
                            Fact fact = new Fact(id, ratio, description);
                            facts.Add(fact);
                        }
                        else
                        {
                            Console.WriteLine($"Ошибка парсинга Ratio в строке: {line} в файле фактов");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка формата строки: {line} в файле фактов");
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine($"Ошибка чтения файла: {e.Message}");
            }

            KnowledgeBaseFacts = facts;
        }

        private void LoadRules(string file_name)
        {

            HashSet<Rule> rules = new HashSet<Rule>();
            try
            {
                foreach (string line in File.ReadLines(file_name))
                {
                    if (line == "") continue;
                    string[] parts = line.Split(';');
                    if (parts.Length >= 4)
                    {
                        string id = parts[0];
                        double ratio;
                        if (double.TryParse(parts[1], NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out ratio))
                        {
                            List<Fact> lhs = new List<Fact>();

                            Fact rhs = null;
                            // Парсинг левой части правила (lhs)
                            string[] lhsIds = parts[2].Split(',');
                            foreach (string lhsId in lhsIds)
                            {
                                lhs.Add(KnowledgeBaseFacts.FirstOrDefault(f => f.Id == lhsId));
                            }

                            // Парсинг правой части правила (rhs)
                            string[] rhsIds = parts[3].Split(',');
                            foreach (string rhsId in rhsIds)
                            {
                                // Fact fact = new Fact(rhsId, 0.0, ""); // Ratio и Description не используются в правой части
                                rhs = KnowledgeBaseFacts.FirstOrDefault(f => f.Id == rhsId);
                                // rhs = fact;
                            }

                            string description = "если: ";
                            int count = 0;
                            foreach (Fact fact in lhs)
                            {
                                // Fact fact = new Fact(rhsId, 0.0, ""); // Ratio и Description не используются в правой части
                                description += fact.Description;
                                count++;
                                if (count < lhs.Count) description += " и ";
                            }
                            description += " то: " + rhs.Description;
                            Rule rule = new Rule(id, ratio, lhs, rhs, description);
                            rules.Add(rule);
                        }
                        else
                        {
                            Console.WriteLine($"Ошибка парсинга Ratio в строке: {line}  в файле фактов");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка формата строки: {line} в файле правил");
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine($"Ошибка чтения файла: {e.Message}");
            }
            KnowledgeBaseProduction = rules;

        }

        public string GetFacts()
        {
            string result = "";
            foreach (var item in KnowledgeBaseFacts)
            {
                result += item.ToString();
            }
            return result;
        }

        public string[] GetFactsArray()
        {
            string[] result = new string[KnowledgeBaseFacts.Count];
            int i = 0;
            foreach (var item in KnowledgeBaseFacts)
            {
                result[i++] = item.ToString();
            }
            return result;
        }

        public string GetRules()
        {
            string result = "";
            foreach (var item in KnowledgeBaseProduction)
            {
                result += item.ToString();
            }
            return result;
        }
    }

    public static class Extensions
    {
        // Пользовательское расширение для проверки, содержатся ли все элементы коллекции в другой коллекции
        public static bool ContainsAll<T>(this IEnumerable<T> collection, IEnumerable<T> items)
        {
            return items.All(collection.Contains);
        }
    }

    public class Resolve
    {
        internal List<List<Fact>> bd_facts;
        internal List<Rule> bd_rules;
        internal bool successful;
        public Resolve()
        {
            bd_facts = new List<List<Fact>>();
            bd_rules = new List<Rule>();
            successful = false;
        }
    }
}
