using DarklandsBusinessObjects.Streaming;
using DarklandsBusinessObjects.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsBusinessObjects.Objects
{
    public class Character : StreamObject
    {
        public const int CHARACTER_SIZE = 0x80; // In memory, it does not contain saints, formulae or items.

        public int Id { get; private set; }

        public int Age { get { return GetWord(0x12); } }
        public string FullName { get { return GetString(0x25, 25); } }
        public string ShortName { get { return GetString(0x3e, 11); } }

        public int EquippedVitalType { get { return this[0x4b]; } }
        public int EquippedLegType { get { return this[0x4c]; } }
        public int EquippedVitalQuality { get { return this[0x4f]; } }
        public int EquippedLegQuality { get { return this[0x50]; } }
        public int EquippedWeaponType { get { return this[0x51]; } }
        public int EquippedWeaponQuality { get { return this[0x58]; } }
        public int EquippedMissileQuality { get { return this[0x5a]; } }
        public int EquippedShieldQuality { get { return this[0x5b]; } }
        public int EquippedShieldType { get { return this[0x5c]; } }
        public int NumberOfItems { get { return GetWord(0x7e); } }

        private AttributeSet m_currentAttributes;
        public AttributeSet CurrentAttributes
        {
            get
            {
                if (m_currentAttributes == null)
                {
                    m_currentAttributes = new AttributeSet(DataStream, BaseOffset + 0x5d);
                }
                return m_currentAttributes;
            }
        }

        private AttributeSet m_maxAttributes;
        public AttributeSet MaxAttributes
        {
            get
            {
                if (m_maxAttributes == null)
                {
                    m_maxAttributes = new AttributeSet(DataStream, BaseOffset + 0x64);
                }
                return m_maxAttributes;
            }
        }

        private SkillSet m_skills;
        public SkillSet Skills
        {
            get
            {
                if (m_skills == null)
                {
                    m_skills = new SkillSet(DataStream, BaseOffset + 0x6b);
                }
                return m_skills;
            }
        }

        private SaintBitmask m_saintBitmask;
        public SaintBitmask SaintBitmask
        {
            get { return m_saintBitmask; }
            set
            {
                m_saintBitmask = value;
                NotifyPropertyChanged();
            }
        }

        private FormulaeBitmask m_formulaeBitmask;
        public FormulaeBitmask FormulaeBitmask
        {
            get { return m_formulaeBitmask; }
            set
            {
                m_formulaeBitmask = value;
                NotifyPropertyChanged();
            }
        }

        private ItemList m_itemList;
        public ItemList ItemList
        {
            get { return m_itemList; }
            set
            {
                m_itemList = value;
                NotifyPropertyChanged();
            }
        }

        public Character(ByteStream data, int offset, int id)
            : base(data, offset, CHARACTER_SIZE)
        {
            Id = id;
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
                if (m_saintBitmask != null)
                {
                    m_saintBitmask.Dispose();
                }
                if (m_formulaeBitmask != null)
                {
                    m_formulaeBitmask.Dispose();
                }
                if (m_itemList != null)
                {
                    m_itemList.Dispose();
                }
                if (m_currentAttributes != null)
                {
                    m_currentAttributes.Dispose();
                }
                if (m_maxAttributes != null)
                {
                    m_maxAttributes.Dispose();
                }
                if (m_skills != null)
                {
                    m_skills.Dispose();
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

        // https://web.archive.org/web/20091112110231/http://wallace.net/darklands/formats/dksaveXX.sav.html#structdef-character
        //Structure: character

        //Size 0x22a (554).

        //A character (and all their belongings).

        //0x00: unknown (17 bytes)
        //0x12: age: word
        //Age.
        //A character's birthday is dependent on their party order (not marching order). The first character is January 1st, the second February, etc.
        //Note that the birthday effects can be exploited (by swapping characters at inns) so that some characters never age -- although since age after initial training has no deleterious effects, there's no reason to do so. It would also cost 40-50% of your on-hand gold for each swap.
        //0x14: unknown (1 bytes)
        //0x15: shield: char
        //Heraldic shield.
        //Ranges from 'A' to 'O'.
        //This corresponds to one of the "pics\shield?.pic" files.
        //TODO: is this a null-delimited string or just a char?
        //0x16: unknown (12 bytes)
        //0x22: equip_missile_type: byte
        //Item type of the currently equipped missile device.
        //This is the first of ten bytes (scattered through this structure) which indicate what a character currently has equipped.
        //Instead of offsets into the items array, the item type and quality are stored. The game does not seem to enforce that you must own the item.
        //0x23: unknown (2 bytes)
        //0x25: full_name: string(25)
        //Full name.
        //0x3e: short_name: string(11) (null-delimited)
        //Nickname.
        //Possibly longer or shorter, but seems to be 10 characters and a null.
        //0x49: unknown (2 bytes)
        //0x4b: equip_vital_type: byte
        //Item type of the currently equipped vital armor.
        //0x4c: equip_leg_type: byte
        //Item type of the currently equipped leg armor.
        //0x4d: unknown (2 bytes)
        //0x4f: equip_vital_q: byte
        //Quality of the currently equipped vital armor.
        //0x50: equip_leg_q: byte
        //Quality of the currently equipped leg armor.
        //0x51: equip_weapon_type: byte
        //Item type of the currently equipped weapon.
        //0x52: unknown (6 bytes)
        //0x58: equip_weapon_q: byte
        //Quality of the currently equipped weapon.
        //0x59: unknown (1 bytes)
        //0x5a: equip_missile_q: byte
        //Quality of the currently equipped missile device.
        //0x5b: equip_shield_q: byte
        //Quality of the currently equipped shield.
        //0x5c: equip_shield_type: byte
        //Item type of the currently equipped shield.
        //0x5d: curr_attrs: struct attribute_set
        //Current attributes.
        //If a saint is invoked or a potion is quaffed, any resulting changes to attributes are directly reflected here; events are added (for the near future) which then alter the attribute back to normal.
        //0x64: max_attrs: struct attribute_set
        //Maximum attributes (aside from temporary increases).
        //0x6b: skills: struct skill_set
        //Skills.
        //As with attributes, if a saint is invoked or a potion is quaffed, any resulting changes to skills are directly reflected here; events are added (for the near future) which then alter the skill back to normal.
        //0x7e: num_items: word
        //Number of items carried.
        //0x80: saints_known: array[ 160 bits ] of bit
        //Knowledge of saints.
        //The array spans 20 bytes, 160 bits.
        //The bit offset into this array corresponds to the the offset of the saint in the array saint_full_names (found in darkland.lst).
        //Fewer than 160 saints are actually defined; it is unknown what happens if bits past the actual number of saints are turned on.
        //Bit is on if saint is known
        //0x94: formulae_known: array[ 22 ] of bitmask[1]
        //Knowledge of alchemical formulae.
        //Byte offset into the array corresponds to the offset of the type of formula (fleadust, essence'o'grace, etc) in the array formula_full_names (found in darkland.lst).
        //It is unknown what happens if any of the first five bits are turned on; probably nothing, though.
        //bit 1:	[constant: 0]	
        //bit 2:	[constant: 0]	
        //bit 3:	[constant: 0]	
        //bit 4:	[constant: 0]	
        //bit 5:	[constant: 0]	
        //bit 6:	q45:	Knowledge of q45 formula.
        //bit 7:	q35:	Knowledge of q35 formula.
        //bit 8:	q25:	Knowledge of q25 formula.
        //0xaa: items: array[ 64 ] of struct item
        //Only num_items of the 64 items are populated; the rest are all 0x00.
    }
}
