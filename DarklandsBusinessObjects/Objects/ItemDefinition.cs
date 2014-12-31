using DarklandsBusinessObjects.Streaming;
using DarklandsBusinessObjects.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsBusinessObjects.Objects
{
    public class ItemDefinition : StreamObject
    {
        public const int ITEM_DEFINITION_SIZE = 0x2e;

        public int Id { get; private set; }

        public string Name { get { return GetString(0x00, 20); } }
        public string ShortName { get { return GetString(0x14, 10); } }
        public int Type { get { return GetWord(0x14); } }
        public ItemMaskA MaskA { get { return (ItemMaskA) this[0x20]; } }
        public ItemMaskB MaskB { get { return (ItemMaskB) this[0x21]; } }
        public ItemMaskC MaskC { get { return (ItemMaskC) this[0x22]; } }
        public ItemMaskD MaskD { get { return (ItemMaskD) this[0x23]; } }
        public ItemMaskE MaskE { get { return (ItemMaskE) this[0x24]; } }

        public int Weight { get { return  this[0x25]; } }
        public int DefaultQualit { get { return  this[0x26]; } }
        public int UnknownW28 { get { return GetWord(0x28); } }
        public int UnknownW2a { get { return GetWord(0x2a); } }
        public int Value { get { return GetWord(0x2c); } }


        public bool IsQuestItem
        {
            get 
            { 
                return MaskB.HasFlag(ItemMaskB.IsQuestIndoor) 
                    || MaskC.HasFlag(ItemMaskC.IsQuestOutdoor);
            }
        }

        public ItemDefinition(ByteStream dataStream, int offset, int id)
            : base(dataStream, offset, ITEM_DEFINITION_SIZE)
        {
            Id = id;
        }

        public override string ToString()
        {
            var masks = new List<string>();
            if (MaskA > 0)
            {
                masks.Add(MaskA.ToString());
            }
            if (MaskB > 0)
            {
                masks.Add(MaskB.ToString());
            }
            if (MaskC > 0)
            {
                masks.Add(MaskC.ToString());
            }
            if (MaskD > 0)
            {
                masks.Add(MaskD.ToString());
            }
            if (MaskE > 0)
            {
                masks.Add(MaskE.ToString());
            }

            return "'0x" + Id.ToString("x")
                + "' '" + Name
                + "' '" + ShortName
                + "' '" + string.Join(", ", masks.ToArray())
                + "'";
        }

        // Structure: item_definition

        //Size 0x2e.

        //A definition of an item.

        //0x00: name: string(20)
        //Name.
        //0x14: short_name: string(10)
        //Full name.
        //0x1e: type: word
        //Item type.
        //TODO: add ext-reference to the UGE file for the enum data.
        //TODO: instead, make this an empty enum pointing there...
        //0x20: bitmask[1]
        //bit 1:	is_edged:	Item is an edged weapon.
        //bit 2:	is_impact:	Item is an impact weapon.
        //bit 3:	is_polearm:	Item is an polearm.
        //bit 4:	is_flail:	Item is a flail.
        //bit 5:	is_thrown:	Item is a thrown weapon.
        //bit 6:	is_bow:	Item is a bow.
        //bit 7:	is_metal_armor:	Item is metal armor.
        //bit 8:	is_shield:	Item is a shield.
        //0x21: bitmask[1]
        //bit 1:	unknown	
        //This and the next bit seem to indicate items that could be found in a pawnshop (all unequippable "normal" items).
        //0x03 for harp and flute, 0x02 for clock, grappling hook, and lockpicks, 0x01 for all other pawnshop items.
        //bit 2:	unknown	
        //bit 3:	is_component:	Item is an alchemical component.
        //bit 4:	is_potion:	Item is a potion.
        //bit 5:	is_relic:	Item is a relic.
        //bit 6:	is_horse:	Item is a horse.
        //bit 7:	is_quest_1:	
        //These seem to be the types of quest items that would be found in offices (love letters), or unused ones (treason note).
        //bit 8:	[constant: 0]	
        //0x22: bitmask[1]
        //bit 1:	is_lockpicks:	Item is lockpicks.
        //bit 2:	is_light:	Item gives light.
        //Torch, candle, and lantern.
        //Note that lights are not a factor in game play.
        //bit 3:	is_arrow:	Item is an arrow.
        //bit 4:	[constant: 0]	
        //bit 5:	is_quarrel:	Item is a quarrel.
        //bit 6:	is_ball:	Item is a ball.
        //bit 7:	[constant: 0]	
        //bit 8:	is_quest_2:	
        //These seem to be outdoors quest items (prayerbook), fortress/baphomet/dragon items (sword of war, bone, gold cup), and creature parts (tusk and wolfskin).
        //0x23: bitmask[1]
        //This is the only one of the five bitmasks where more than one bit is on for a given item: leather armor is 0x14.
        //bit 1:	is_throw_potion:	Item is a throwable potion.
        //bit 2:	[constant: 0]	
        //bit 3:	is_nonmetal_armor:	Item is a non-metal armor.
        //bit 4:	is_missile_weapon:	Item is a missile weapon.
        //bit 5:	[constant: 0]	
        //bit 6:	is_unknown_1: unknown	
        //Set only for: great hammer, maul, military hammer, leather armor, pure gold, manganes, zincblende, antimoni, orpiment, white cinnibar, nikel, pitchblende, zinken, and brimstone.
        //Best guess is "items found in chests in the mines", but it's a wild guess.
        //bit 7:	is_music:	Item is a musical instrument.
        //Harp and flute.
        //bit 8:	[constant: 0]	
        //0x24: bitmask[1]
        //bit 1:	is_unknown_2: unknown	
        //This and the next bit are set for anything that does not have the high bit is_unknown_3 set, except for: cloth armor, superb horse, and fast horse.
        //bit 2:	unknown	
        //bit 3:	[constant: 0]	
        //bit 4:	[constant: 0]	
        //bit 5:	[constant: 0]	
        //bit 6:	[constant: 0]	
        //bit 7:	[constant: 0]	
        //bit 8:	is_unknown_3: unknown	
        //Set for cloth armor, all types of quest items, relics, and creature parts.
        //This is always set if any of these three bits was set: is_relic, is_quest_1, is_quest_2.
        //0x25: weight: byte
        //Item weight (when wielded).
        //0x26: quality: byte
        //Default quality of the item.
        //Based on the values, this is probably used as a sort of "base" or "default" item quality.
        //Values are: 25 for most everyday items. 35 for composite bow. 70/40/25/15/5 for horses (superb, fast, average, pack, and mule). 99 for quest items and non-weapon relics. High (40-70) for weapon relics.
        //0x28: unknown word
        //Non-zero only for relics.
        //Ranges from 0x06 (St. Edward's Ring) to 0x50 (St. Gabriel's Horn).
        //0x2a: unknown word
        //Non-zero only for relics, and for the "residency permit" (which is unused by the game).
        //Ranges from 0x05 to 0x27 (residency permit).
        //0x2c: value: word
        //This value is high for expensive things (transformation potion is 0x4e0=1248) and low for cheap things (arrows are 2).
        //Quest items are either low values or off-the-scale high values (0x270f).
        //Relics vary but are lower than many potions.
    }
}
