using DarklandsBusinessObjects.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DarklandsBusinessObjects.Objects
{

    public class Saint
    {
        public const int DESCRIPTION_SIZE = 360;

        private IEnumerable<SaintBuff> m_buffs;

        public int Id { get; private set; }
        public string FullName { get; private set; }
        public string ShortName { get; private set; }
        public string Description { get; private set; }
        public string Clue { get; private set; }

        public Saint(int id, string longName, string shortName, string description, string clue, IEnumerable<SaintBuff> buffs)
        {
            Id = id;
            FullName = longName;
            ShortName = shortName;
            Description = description;
            Clue = clue;

            m_buffs = buffs;
        }

        public bool HasBuff(string buffName)
        {
            return m_buffs.FirstOrDefault(b => b.Name == buffName) != null;
        }

        public string GetBuff(string buffName)
        {
            return HasBuff(buffName) ? m_buffs.FirstOrDefault(b => b.Name == buffName).Value : null;
        }

        public override string ToString()
        {
            return "['0x" + Id.ToString("x")
                + "' '" + ShortName
                + "' '" + FullName
                //+ "' '" + Description
                + "']";
        }
    }
}
