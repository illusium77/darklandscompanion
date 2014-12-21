using DarklandsBusinessObjects.Objects;
using DarklandsBusinessObjects.Streaming;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsBusinessObjects.Save
{
    public class SaveParty : StreamObject
    {
        // https://web.archive.org/web/20091112110231/http://wallace.net/darklands/formats/dksaveXX.sav.html#offset-0xef

        public const int SAVE_CHARACTER_SIZE = 0x22a; // in save file

        public int NumberOfCharacters
        {
            get 
            {
                return GetWord(0x00); // 0xef - 0xef
            }
        }

        public int NumberOfDefinedCharacters
        {
            get
            {
                return GetWord(0x02); // 0xf1 - 0xef = 0x02
            }
        }

        // Indexes into the arrays of character data for each of the five party slots.
        private IReadOnlyList<int> m_partyCharacterIds;
        public IReadOnlyList<int> PartyCharacterIds
        {
            get
            {
                if (m_partyCharacterIds == null)
                {
                    var list = new List<int>();
                    for (int i = 0; i < 5; i++)
                    {
                        var index = GetWord(0x04 + i * 2); // 0xf3 - 0xef = 0x04, 2 = size of word
                        if (index >= 0)
                        {
                            list.Add(index);
                        }
                    }

                    m_partyCharacterIds = list;
                }

                return m_partyCharacterIds;
            }
        }

        IReadOnlyList<Character> m_characters;
        public IReadOnlyList<Character> Characters
        {
            get
            {
                if (m_characters == null)
                {
                    var characters = new List<Character>();
                    foreach (var charId in PartyCharacterIds)
                    {
                        // 0x9a = character start offset in party subblock (0x189 - 0xef)
                        var offset = 0x9A + charId * SAVE_CHARACTER_SIZE;
                        characters.Add(GetCharacter(charId, offset));
                    }

                    m_characters = characters;
                }

                return m_characters;
            }
        }

        private Character GetCharacter(int charId, int offset)
        {
            var character = new Character(DataStream, BaseOffset + offset, charId);
            
            character.SaintBitmask = new SaintBitmask(
                DataStream, BaseOffset + offset + 0x80);
            character.FormulaeBitmask = new FormulaeBitmask(
                DataStream, BaseOffset + offset + 0x94);
            character.ItemList = new ItemList(DataStream, BaseOffset + offset + 0xaa);
                
            return character;
        }

        public SaveParty(ByteStream data, int offset)
            : base(data, offset, 0) // size is unknown initially
        {
        }

        public override string ToString()
        {
            return "['" + NumberOfCharacters + "/" + NumberOfDefinedCharacters
                + "' '" + string.Join(", ", Characters)
                + "']";
        }

    }
}
