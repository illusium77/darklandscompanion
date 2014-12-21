using DarklandsBusinessObjects.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsBusinessObjects.Objects
{
    //public enum ShortSkills
    //{
    //    wEdg,
    //    wImp,
    //    wFll,
    //    wPol,
    //    wThr,
    //    wBow,
    //    wMsD,
    //    Alch,
    //    Relg,
    //    Virt,
    //    SpkC,
    //    SpkL,
    //    RW,
    //    Heal,
    //    Artf,
    //    Stlh,
    //    StrW,
    //    Ride,
    //    WdWs
    //}

    public class SkillSet : StreamObject
    {
        // https://web.archive.org/web/20091112194440/http://wallace.net/darklands/formats/structures.html#structdef-skill_set

        public const int SKILL_SET_SIZE = 0x13;

        public byte EdgedWeapon
        {
            get { return this[0x00]; }
            set
            {
                this[0x00] = value;
                NotifyPropertyChanged();
            }
        }

        public byte ImpactWeapon
        {
            get { return this[0x01]; }
            set
            {
                this[0x01] = value;
                NotifyPropertyChanged();
            }
        }

        public byte FlailWeapon
        {
            get { return this[0x02]; }
            set
            {
                this[0x02] = value;
                NotifyPropertyChanged();
            }
        }

        public byte Polearm
        {
            get { return this[0x03]; }
            set
            {
                this[0x03] = value;
                NotifyPropertyChanged();
            }
        }

        public byte Thrown
        {
            get { return this[0x04]; }
            set
            {
                this[0x04] = value;
                NotifyPropertyChanged();
            }
        }

        public byte Bow
        {
            get { return this[0x05]; }
            set
            {
                this[0x05] = value;
                NotifyPropertyChanged();
            }
        }

        public byte MissileWeapon
        {
            get { return this[0x06]; }
            set
            {
                this[0x06] = value;
                NotifyPropertyChanged();
            }
        }

        public byte Alchemy
        {
            get { return this[0x07]; }
            set
            {
                this[0x07] = value;
                NotifyPropertyChanged();
            }
        }

        public byte Religion
        {
            get { return this[0x08]; }
            set
            {
                this[0x08] = value;
                NotifyPropertyChanged();
            }
        }

        public byte Virtue
        {
            get { return this[0x09]; }
            set
            {
                this[0x09] = value;
                NotifyPropertyChanged();
            }
        }

        public byte SpeakCommon
        {
            get { return this[0x0a]; }
            set
            {
                this[0x0a] = value;
                NotifyPropertyChanged();
            }
        }

        public byte SpeakLatin
        {
            get { return this[0x0b]; }
            set
            {
                this[0x0b] = value;
                NotifyPropertyChanged();
            }
        }

        public byte ReadAndWrite
        {
            get { return this[0x0c]; }
            set
            {
                this[0x0c] = value;
                NotifyPropertyChanged();
            }
        }

        public byte Healing
        {
            get { return this[0x0d]; }
            set
            {
                this[0x0d] = value;
                NotifyPropertyChanged();
            }
        }

        public byte Artifice
        {
            get { return this[0x0e]; }
            set
            {
                this[0x0e] = value;
                NotifyPropertyChanged();
            }
        }

        public byte Stealth
        {
            get { return this[0x0f]; }
            set
            {
                this[0x0f] = value;
                NotifyPropertyChanged();
            }
        }

        public byte Streetwise
        {
            get { return this[0x10]; }
            set
            {
                this[0x10] = value;
                NotifyPropertyChanged();
            }
        }

        public byte Riding
        {
            get { return this[0x11]; }
            set
            {
                this[0x11] = value;
                NotifyPropertyChanged();
            }
        }

        public byte Woodwise
        {
            get { return this[0x12]; }
            set
            {
                this[0x12] = value;
                NotifyPropertyChanged();
            }
        }

        public SkillSet(ByteStream data, int offset)
            : base(data, offset, SKILL_SET_SIZE)
        {
        }
    }

}
