﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using DarklandsBusinessObjects.Objects;
using DarklandsBusinessObjects.Streaming;

namespace DarklandsBusinessObjects.Save
{
    public class SaveParty : StreamObject
    {
        // https://web.archive.org/web/20091112110231/http://wallace.net/darklands/formats/dksaveXX.sav.html#offset-0xef

        public const int SaveCharacterSize = 0x22a; // in save file
        private IReadOnlyList<Character> _characters;
        // Indexes into the arrays of character data for each of the five party slots.
        private List<int> _partyCharacterIds;
        private IReadOnlyDictionary<int, CharacterColors> _characterColors;

        public SaveParty(ByteStream data, int offset)
            : base(data, offset, 0) // size is unknown initially
        {
        }

        public int NumberOfCharacters
        {
            get { return GetWord(0x00); // 0xef - 0xef
            }
        }

        public int NumberOfDefinedCharacters
        {
            get { return GetWord(0x02); // 0xf1 - 0xef = 0x02
            }
        }

        public IReadOnlyList<int> PartyCharacterIds
        {
            get
            {
                if (_partyCharacterIds == null)
                {
                    var list = new List<int>();
                    for (var i = 0; i < 5; i++)
                    {
                        var index = GetWord(0x04 + i*2); // 0xf3 - 0xef = 0x04, 2 = size of word
                        if (index >= 0)
                        {
                            list.Add(index);
                        }
                    }

                    _partyCharacterIds = list;
                }

                return _partyCharacterIds.ToList();
            }
        }

        public IReadOnlyList<Character> Characters
        {
            get
            {
                if (_characters == null)
                {
                    var characters = (
                        from charId in PartyCharacterIds
                        let offset = 0x9A + charId*SaveCharacterSize 
                        select GetCharacter(charId, offset)).ToList();

                    _characters = characters;
                }

                return _characters;
            }
        }

        private Character GetCharacter(int charId, int offset)
        {
            var character = new Character(DataStream, BaseOffset + offset, charId)
            {
                SaintBitmask = new SaintBitmask(
                    DataStream, BaseOffset + offset + 0x80),
                FormulaeBitmask = new FormulaeBitmask(
                    DataStream, BaseOffset + offset + 0x94),
                ItemList = new ItemList(DataStream, BaseOffset + offset + 0xaa)
            };

            return character;
        }

        public IReadOnlyDictionary<int, CharacterColors> Colors
        {
            get
            {
                if (_characterColors != null) return _characterColors;

                var colors = new Dictionary<int, CharacterColors>();
                for (var i = 0; i < NumberOfCharacters; i++)
                {
                    colors.Add(PartyCharacterIds[i],
                        new CharacterColors(DataStream, 0x111 + i*CharacterColors.CharacterColorsSize));
                }
                _characterColors = colors;

                return _characterColors;
            }
        }

        public CharacterColors GetCharacterColors(int charId)
        {
            return Colors.ContainsKey(charId) ? Colors[charId] : null;
        }

        public string GetCharacterImage(int charId)
        {
            if (PartyCharacterIds.Contains(charId))
            {
                var index = _partyCharacterIds.FindIndex(i => i == charId);
                return GetString(0x0e + index * 4, 4); // 0xfd - 0xef = 0x0e, 4 = length of the image string
            }

            return string.Empty;
        }

        public void SetCharacterImage(int charId, string image)
        {
            if (PartyCharacterIds.Contains(charId))
            {
                var index = _partyCharacterIds.FindIndex(i => i == charId);
                SetString(0x0e + index * 4, image, 4); // 0xfd - 0xef = 0x0e, 4 = length of the image string
            }
        }

        public override string ToString()
        {
            return "['" + NumberOfCharacters + "/" + NumberOfDefinedCharacters
                   + "' '" + string.Join(", ", Characters)
                   + "']";
        }
    }
}