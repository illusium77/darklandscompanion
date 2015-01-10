using System.Collections.Generic;
using System.Linq;

namespace DarklandsBusinessObjects.Objects
{
    public class Saint
    {
        public const int DescriptionSize = 360;
        private readonly IEnumerable<SaintBuff> _buffs;

        public Saint(int id, string longName, string shortName, string description, string clue,
            IEnumerable<SaintBuff> buffs)
        {
            Id = id;
            FullName = longName;
            ShortName = shortName;
            Description = description;
            Clue = clue;

            _buffs = buffs;
        }

        public int Id { get; private set; }
        public string FullName { get; private set; }
        public string ShortName { get; private set; }
        public string Description { get; private set; }
        public string Clue { get; private set; }

        public bool HasBuff(string buffName)
        {
            return _buffs.FirstOrDefault(b => b.Name == buffName) != null;
        }

        public string GetBuff(string buffName)
        {
            var buff = _buffs.FirstOrDefault(b => b.Name == buffName);
            if (buff != null)
                return HasBuff(buffName) ? buff.Value : null;

            return "INVALID";
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