using System.Collections.Generic;
using DarklandsBusinessObjects.Streaming;

namespace DarklandsBusinessObjects.Objects
{
    public class Formula : StreamObject
    {
        public const int FormulaSize = 0x68;
        // private const int DESCRIPTION_SIZE = 80;
        private const int NumIngrediens = 5;

        private static readonly string[] FormulaTypes =
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

        private IReadOnlyList<Ingredient> _ingredients;

        public Formula(ByteStream dataStream, int offset, int id, string fullName, string shortName)
            : base(dataStream, offset, FormulaSize)
        {
            Id = id;
            FullName = fullName;
            ShortName = shortName;
        }

        public FormulaQuality Quality
        {
            get { return (FormulaQuality) (Id%3); }
        }

        public string Type
        {
            get { return FormulaTypes[Id/3]; }
        }

        public int Id { get; private set; }
        public string FullName { get; private set; }
        public string ShortName { get; private set; }

        public string Description
        {
            get { return GetString(0x00, 80); }
        }

        public int MysticNumber
        {
            get { return GetWord(0x50); }
        }

        public FormulaRiskFactor RiskFactor
        {
            get { return (FormulaRiskFactor) GetWord(0x52); }
        }

        public IReadOnlyList<Ingredient> Ingrediens
        {
            get
            {
                if (_ingredients == null)
                {
                    var list = new List<Ingredient>();

                    for (var i = 0; i < NumIngrediens; i++)
                    {
                        var ingredient = new Ingredient(DataStream,
                            BaseOffset + 0x54 + i*Ingredient.IngredientSize);
                        if (ingredient.Quantity > 0)
                        {
                            list.Add(ingredient);
                        }
                    }

                    _ingredients = list;
                }
                return _ingredients;
            }
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
                if (_ingredients != null)
                {
                    foreach (var i in _ingredients)
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