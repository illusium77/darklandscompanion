using DarklandsBusinessObjects.Streaming;
using DarklandsBusinessObjects.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsBusinessObjects.Objects
{
    public class Formula : StreamObject
    {
        private static string[] s_formulaTypes = new string[]
        {
            "Noxious Aroma",
            "Eyeburn",
            "Black Cloud",
            "Fleadust",
            "Eater Water",
            "Breath of Death",
            "Sunburst",
            "Thunderbolt",
            "Arabian Fire",
            "Stone-tar",
            "Deadly Blade",
            "Strongedge",
            "Greatpower",
            "Trueflight",
            "Hardarmor",
            "Transformation",
            "Truesight",
            "New-wind",
            "Ironarm",
            "Quickmove",
            "Essence of Grace",
            "Firewall"
        };

        public const int FORMULA_SIZE = 0x68;

        private const int DESCRIPTION_SIZE = 80;
        private const int NUM_INGREDIENS = 5;

        public FormulaQuality Quality { get { return (FormulaQuality)(Id % 3); } }
        public string Type { get { return s_formulaTypes[Id / 3]; } }

        public int Id { get; private set; }

        public string FullName { get; private set; }
        public string ShortName { get; private set; }
        public string Description { get { return GetString(0x00, 80); } }
        public int MysticNumber { get { return GetWord(0x50); } }
        public FormulaRiskFactor RiskFactor { get { return (FormulaRiskFactor)GetWord(0x52); } }

        private IReadOnlyList<Ingredient> m_ingredients;
        public IReadOnlyList<Ingredient> Ingrediens
        {
            get
            {
                if (m_ingredients == null)
                {
                    var list = new List<Ingredient>();

                    for (int i = 0; i < NUM_INGREDIENS; i++)
                    {
                        var ingredient = new Ingredient(DataStream,
                            BaseOffset + 0x54 + i * Ingredient.INGREDIENT_SIZE);
                        if (ingredient.Quantity > 0)
                        {
                            list.Add(ingredient);
                        }
                    }

                    m_ingredients = list;
                }
                return m_ingredients;
            }
        }

        public Formula(ByteStream dataStream, int offset, int id, string fullName, string shortName)
            : base(dataStream, offset, FORMULA_SIZE)
        {
            Id = id;
            FullName = fullName;
            ShortName = shortName;
        }

        public override string ToString()
        {
            return "['0x" + Id.ToString("x")
                + "' '" + Quality
                + "' '" + ShortName
                + "' '" + FullName
                + "' '" + Type
                + "']";
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (m_ingredients != null)
                {
                    foreach (var i in m_ingredients)
                    {
                        i.Dispose();
                    }
                }
            }
        }
    }


    //https://web.archive.org/web/20091112194425/http://wallace.net/darklands/formats/darkland.alc.html#structdef-formula_definition
    //Structure: formula_definition

    //Size 0x68.

    //A definition of an alchemical formula.

    //0x00: description: string(80)
    //Text description of the formula.
    //The last character is always a null, as is any unused space
    //0x50: mystic_number: word
    //Mystic number (the base difficulty when mixing a new potion).
    //0x52: risk_factor: word
    //Risk factor (when mixing a new potion).
    //0=low, 1=medium, 2=high.
    //0x54: ingredients: array[ 5 ] of struct ingredient (each size 4)
    //Ingredients required to mix a potion.
    //Ingredients are given in increasing item code value
    //If fewer than 5 ingredients are needed, the remaining space in the array is filled with zeroes.

}
