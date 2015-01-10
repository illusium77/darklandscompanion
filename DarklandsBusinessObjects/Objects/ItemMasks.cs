using System;

namespace DarklandsBusinessObjects.Objects
{
    [Flags]
    public enum ItemMaskA
    {
        IsEdged = 1 << 0,
        IsImpact = 1 << 1,
        IsPolearm = 1 << 2,
        IsFlail = 1 << 3,
        IsThrown = 1 << 4,
        IsBow = 1 << 5,
        IsMetalArmor = 1 << 6,
        IsShield = 1 << 7
    }

    [Flags]
    public enum ItemMaskB
    {
        UnknownA = 1 << 0,
        UnknownB = 1 << 1,
        IsComponent = 1 << 2,
        IsPotion = 1 << 3,
        IsRelic = 1 << 4,
        IsHorse = 1 << 5,
        IsQuestIndoor = 1 << 6,
        Constant0A = 1 << 7
    }

    [Flags]
    public enum ItemMaskC
    {
        IsLockpicks = 1 << 0,
        IsLight = 1 << 1,
        IsArrow = 1 << 2,
        Constant0B = 1 << 3,
        IsQuarrel = 1 << 4,
        IsBall = 1 << 5,
        Constant0C = 1 << 6,
        IsQuestOutdoor = 1 << 7
    }

    [Flags]
    public enum ItemMaskD
    {
        IsThrowPotion = 1 << 0,
        Constant0D = 1 << 1,
        IsNoMetalArmor = 1 << 2,
        IsMissileWeapon = 1 << 3,
        Constant0E = 1 << 4,
        UnknownC = 1 << 5,
        IsMusic = 1 << 6,
        Constant0F = 1 << 7
    }

    [Flags]
    public enum ItemMaskE
    {
        UnknownD = 1 << 0,
        UnknownE = 1 << 1,
        Constant0G = 1 << 2,
        Constant0H = 1 << 3,
        Constant0I = 1 << 4,
        Constant0J = 1 << 5,
        Constant0K = 1 << 6,
        UnknownF = 1 << 7
    }

    //0x20: bitmask[1]
    //bit 1:	is_edged:	Item is an edged weapon.
    //bit 2:	is_impact:	Item is an impact weapon.
    //bit 3:	is_polearm:	Item is an polearm.
    //bit 4:	is_flail:	Item is a flail.
    //bit 5:	is_thrown:	Item is a thrown weapon.
    //bit 6:	is_bow:	Item is a bow.
    //bit 7:	is_etal_armor:	Item is metal armor.
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
    //bit 4:	is_issile_weapon:	Item is a missile weapon.
    //bit 5:	[constant: 0]	
    //bit 6:	is_unknown_1: unknown	
    //Set only for: great hammer, maul, military hammer, leather armor, pure gold, manganes, zincblende, antimoni, orpiment, white cinnibar, nikel, pitchblende, zinken, and brimstone.
    //Best guess is "items found in chests in the mines", but it's a wild guess.
    //bit 7:	is_usic:	Item is a musical instrument.
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
}