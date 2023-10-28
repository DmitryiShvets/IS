using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba5
{
    internal class Fact
    {
        public string Id { get; set; }
        public double Ratio { get; set; }
        public string Description { get; set; }

        public Fact(string id, double ratio, string description)
        {
            Id = id;
            Ratio = ratio;
            Description = description;
        }

        public override string ToString()
        {
            return $"Id: {Id}; Ratio: {Ratio}; Description: {Description}\n";
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + (Id != null ? Id.GetHashCode() : 0);
            hash = hash * 23 + Ratio.GetHashCode();
            hash = hash * 23 + (Description != null ? Description.GetHashCode() : 0);
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj is Fact other)
            {
                return string.Equals(Id, other.Id) &&
                       Ratio.Equals(other.Ratio) &&
                       string.Equals(Description, other.Description);
            }
            return false;
        }
    }
}
