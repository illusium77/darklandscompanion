using DarklandsBusinessObjects.Streaming;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Range(1, 99)]
        public int EdgedWeapon
        {
            get { return this[0x00]; }
            set
            {
                this[0x00] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int ImpactWeapon
        {
            get { return this[0x01]; }
            set
            {
                this[0x01] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int FlailWeapon
        {
            get { return this[0x02]; }
            set
            {
                this[0x02] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int Polearm
        {
            get { return this[0x03]; }
            set
            {
                this[0x03] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int Thrown
        {
            get { return this[0x04]; }
            set
            {
                this[0x04] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int Bow
        {
            get { return this[0x05]; }
            set
            {
                this[0x05] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int MissileWeapon
        {
            get { return this[0x06]; }
            set
            {
                this[0x06] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int Alchemy
        {
            get { return this[0x07]; }
            set
            {
                this[0x07] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int Religion
        {
            get { return this[0x08]; }
            set
            {
                this[0x08] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int Virtue
        {
            get { return this[0x09]; }
            set
            {
                this[0x09] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int SpeakCommon
        {
            get { return this[0x0a]; }
            set
            {
                this[0x0a] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int SpeakLatin
        {
            get { return this[0x0b]; }
            set
            {
                this[0x0b] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int ReadAndWrite
        {
            get { return this[0x0c]; }
            set
            {
                this[0x0c] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int Healing
        {
            get { return this[0x0d]; }
            set
            {
                this[0x0d] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int Artifice
        {
            get { return this[0x0e]; }
            set
            {
                this[0x0e] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int Stealth
        {
            get { return this[0x0f]; }
            set
            {
                this[0x0f] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int Streetwise
        {
            get { return this[0x10]; }
            set
            {
                this[0x10] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int Riding
        {
            get { return this[0x11]; }
            set
            {
                this[0x11] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int Woodwise
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
