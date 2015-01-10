using DarklandsBusinessObjects.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DarklandsServices.Saints
{
    public static class SaintBuffManager
    {
        public static IReadOnlyList<SaintBuffFilter> SaintBuffTypes = new List<SaintBuffFilter>
        {
            { new SaintBuffFilter ("Endurance", "End") },
            { new SaintBuffFilter ("Strength", "Str") },
            { new SaintBuffFilter ("Agility", "Agl", "agi") },
            { new SaintBuffFilter ("Perception", "Per") },
            { new SaintBuffFilter ("Intelligence", "Int") },
            { new SaintBuffFilter ("Charisma", "Chr") },
            { new SaintBuffFilter ("Edged Weapons", "wEdg", "edg") },
            { new SaintBuffFilter ("Impact Weapons", "wImp","imp") },
            { new SaintBuffFilter ("Flail Weapons", "wFll", "fla") },
            { new SaintBuffFilter ("Pole Weapons", "wPol", "pol") },
            { new SaintBuffFilter ("Thrown Weapons", "wThr", "thr") },
            { new SaintBuffFilter ("Bows", "wBow", "bow") },
            { new SaintBuffFilter ("Missile Devices", "wMsD", "mis") },
            { new SaintBuffFilter ("Alchemy", "Alch", "alc") },
            { new SaintBuffFilter ("Religion", "Relg", "rel") },
            { new SaintBuffFilter ("Virtue", "Virt", "vir") },
            { new SaintBuffFilter ("Speak Common", "SpkC", "speak c", "com") },
            { new SaintBuffFilter ("Speak Latin", "SpkL", "speak l", "lat") },
            { new SaintBuffFilter ("Read & Write", "R&W", "rea") },
            { new SaintBuffFilter ("Heal Skill", @"Heal \(skill\)", "heal") },
            { new SaintBuffFilter ("Artifice", "Artf", "art") },
            { new SaintBuffFilter ("Stealth", "Stlh", "ste") },
            { new SaintBuffFilter ("Streetwise", "StrW", "stre") },
            { new SaintBuffFilter ("Riding", "Ride", "rid") },
            { new SaintBuffFilter ("Woodwise", "WdWs", "woo") },

            { new SaintBuffFilter ("Weapon Skills", "weapon skill", "wea") },
            { new SaintBuffFilter ("Healing", "heals", "heali", "reg") },
            { new SaintBuffFilter ("Local Reputation", @"Local Rep (at nearest city)*", "rep") },
            { new SaintBuffFilter ("Flame Weapons", "flame weapons", "flam") },
            { new SaintBuffFilter ("Water Walking", "water", "wat") },
        };

        public static SaintBuffFilter FindFilter(string searchTerm)
        {
            var term = searchTerm.Trim().ToLower();

            return SaintBuffTypes.FirstOrDefault(
                bt => bt.SearchWords.Any(w => w.StartsWith(term)));
        }

        public static IReadOnlyList<SaintBuff> GenerateBuffsFromClue(string clue)
        {
            var buffs = new List<SaintBuff>();

            foreach (var type in SaintBuffTypes)
            {
                var groupName = "buff";

                Regex buffRegex;
                switch (type.Name)
                {
                    case "Healing":
                        buffRegex = new Regex(@"(" + type.Regex + @")\s*(?<" + groupName + @">(End|Str)\s*\d{1,2}%)",
                            RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
                        break;
                    case "Flame Weapons":
                    case "Water Walking":
                        buffRegex = new Regex(@"(?<" + groupName + @">([A-Z]?[^\\.;:,]*(" + type.Regex + @")[^\\.;]*))",
                            RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
                        break;
                    default:
                        buffRegex = new Regex(@"(" + type.Regex + @")\s*\+(?<" + groupName + @">\(\d{1,2}-\d{1,2}\))",
                            RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
                        break;
                }

                var matches = buffRegex.Matches(clue);
                if (matches.Count > 0)
                {
                    var values = from match in matches.Cast<Match>() select match.Groups[groupName];
                    buffs.Add(new SaintBuff(type.Name, string.Join(", ", values)));
                }
            }

            return buffs;
        }
    }
}
