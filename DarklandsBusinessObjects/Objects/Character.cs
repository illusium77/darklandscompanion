using System.ComponentModel.DataAnnotations;
using DarklandsBusinessObjects.Streaming;

namespace DarklandsBusinessObjects.Objects
{
    public class Character : StreamObject
    {
        public const int CharacterSize = 0x80; // In memory, it does not contain saints, formulae or items.
        private AttributeSet _currentAttributes;
        private FormulaeBitmask _formulaeBitmask;
        private ItemList _itemList;
        private AttributeSet _maxAttributes;
        private SaintBitmask _saintBitmask;
        private SkillSet _skills;

        public Character(ByteStream data, int offset, int id)
            : base(data, offset, CharacterSize)
        {
            Id = id;
        }

        public int Id { get; private set; }

        [Range(1, 255, ErrorMessage = "Enter value between 1 and 255.")]
        public int Age
        {
            get { return GetWord(0x12); }
            set
            {
                SetWord(0x12, value);
                NotifyPropertyChanged();
            }
        }

        public int Shield
        {
            get { return this[0x15]; }
            set
            {
                this[0x15] = value;
                NotifyPropertyChanged();
            }
        }

        // 00 = female, 01 = male
        public int Gender
        {
            get { return this[0x17]; }
            set
            {
                this[0x17] = value;
                NotifyPropertyChanged();
            }
        }

        [MaxLength(25, ErrorMessage = "Maximum length is 25.")]
        public string FullName
        {
            get { return GetString(0x25, 25); }
            set
            {
                SetString(0x25, value, 25);
                NotifyPropertyChanged();
            }
        }

        [MaxLength(11, ErrorMessage = "Maximum length is 11.")]
        public string ShortName
        {
            get { return GetString(0x3e, 11); }
            set
            {
                SetString(0x3e, value, 11);
                NotifyPropertyChanged();
            }
        }

        public int GearWeight
        {
            get { return this[0x49]; }
        }

        public int EquippedVitalType
        {
            get { return this[0x4b]; }
        }

        public int EquippedLegType
        {
            get { return this[0x4c]; }
        }

        [Range(1, 99, ErrorMessage = "Enter value between 1 and 99.")]
        public int EquippedVitalQuality
        {
            get { return this[0x4f]; }
            set
            {
                this[0x4f] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99, ErrorMessage = "Enter value between 1 and 99.")]
        public int EquippedLegQuality
        {
            get { return this[0x50]; }
            set
            {
                this[0x50] = value;
                NotifyPropertyChanged();
            }
        }

        public int EquippedWeaponType
        {
            get { return this[0x51]; }
        }

        [Range(1, 99, ErrorMessage = "Enter value between 1 and 99.")]
        public int EquippedWeaponQuality
        {
            get { return this[0x58]; }
            set
            {
                this[0x58] = value;
                NotifyPropertyChanged();
            }
        }

        public int EquippedMissileType
        {
            get { return this[0x22]; }
        }

        [Range(1, 99, ErrorMessage = "Enter value between 1 and 99.")]
        public int EquippedMissileQuality
        {
            get { return this[0x5a]; }
            set
            {
                this[0x5a] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99, ErrorMessage = "Enter value between 1 and 99.")]
        public int EquippedShieldQuality
        {
            get { return this[0x5b]; }
            set
            {
                this[0x5b] = value;
                NotifyPropertyChanged();
            }
        }

        public int EquippedShieldType
        {
            get { return this[0x5c]; }
        }

        public int NumberOfItems
        {
            get { return GetWord(0x7e); }
            set
            {
                SetWord(0x7e, value);
                NotifyPropertyChanged();
            }
        }

        public AttributeSet CurrentAttributes
        {
            get { return _currentAttributes ?? (_currentAttributes = new AttributeSet(DataStream, BaseOffset + 0x5d)); }
        }

        public AttributeSet MaxAttributes
        {
            get { return _maxAttributes ?? (_maxAttributes = new AttributeSet(DataStream, BaseOffset + 0x64)); }
        }

        public SkillSet Skills
        {
            get { return _skills ?? (_skills = new SkillSet(DataStream, BaseOffset + 0x6b)); }
        }

        public SaintBitmask SaintBitmask
        {
            get { return _saintBitmask; }
            set
            {
                _saintBitmask = value;
                NotifyPropertyChanged();
            }
        }

        public FormulaeBitmask FormulaeBitmask
        {
            get { return _formulaeBitmask; }
            set
            {
                _formulaeBitmask = value;
                NotifyPropertyChanged();
            }
        }

        public ItemList ItemList
        {
            get { return _itemList; }
            set
            {
                _itemList = value;
                NotifyPropertyChanged();
            }
        }

        public bool HasFormula(int id)
        {
            if (FormulaeBitmask == null)
            {
                return false;
            }

            return FormulaeBitmask.FormulaeIds.Contains(id);
        }

        public bool HasSaint(int id)
        {
            if (SaintBitmask == null)
            {
                return false;
            }

            return SaintBitmask.SaintIds.Contains(id);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (_saintBitmask != null)
                {
                    _saintBitmask.Dispose();
                }
                if (_formulaeBitmask != null)
                {
                    _formulaeBitmask.Dispose();
                }
                if (_itemList != null)
                {
                    _itemList.Dispose();
                }
                if (_currentAttributes != null)
                {
                    _currentAttributes.Dispose();
                }
                if (_maxAttributes != null)
                {
                    _maxAttributes.Dispose();
                }
                if (_skills != null)
                {
                    _skills.Dispose();
                }
            }
        }

        public override string ToString()
        {
            return "['" + Id
                   + "' '" + ShortName
                   + "' '" + FullName
                   + "' '" + Age
                   + "']";
        }
    }
}