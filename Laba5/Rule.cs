using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba5
{
    internal class Rule
    {
        public string Id { get; set; }
        public double Ratio { get; set; }
        public List<Fact> Lhs { get; set; } // Левая часть правила (список фактов)
        public Fact Rhs { get; set; } // Правая часть правила (список фактов)
        public string Description { get; set; }

        public Rule(string id, double ratio, List<Fact> lhs, Fact rhs, string description)
        {
            Id = id;
            Ratio = ratio;
            Lhs = lhs;
            Rhs = rhs;
            Description = description;
        }

        public override string ToString()
        {
            string lhsString = string.Join(", ", Lhs.Select(fact => fact.Id));
            return $"Id: {Id}; Ratio: {Ratio}; {lhsString} => {Rhs.Id}; Description: {Description}\n";
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + (Id != null ? Id.GetHashCode() : 0);
            hash = hash * 23 + Ratio.GetHashCode();
            if (Lhs != null)
            {
                foreach (var fact in Lhs)
                {
                    hash = hash * 23 + (fact != null ? fact.GetHashCode() : 0);
                }
            }
            hash = hash * 23 + (Rhs != null ? Rhs.GetHashCode() : 0);
            hash = hash * 23 + (Description != null ? Description.GetHashCode() : 0);
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj is Rule other)
            {
                return string.Equals(Id, other.Id) &&
                       Ratio.Equals(other.Ratio) &&
                       Enumerable.SequenceEqual(Lhs, other.Lhs) &&
                       FactEquals(Rhs, other.Rhs) &&
                       string.Equals(Description, other.Description);
            }
            return false;
        }
        private bool FactEquals(Fact fact1, Fact fact2)
        {
            if (fact1 == null && fact2 == null)
            {
                return true;
            }
            if (fact1 == null || fact2 == null)
            {
                return false;
            }
            return fact1.Equals(fact2);
        }
    }
}
