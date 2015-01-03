using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsBusinessObjects.Objects
{
    public class SaintBuff
    {
        public string Name { get; private set; }
        public string Value { get; private set; }

        public SaintBuff(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return Name + ": " + Value;
        }
    }
}
