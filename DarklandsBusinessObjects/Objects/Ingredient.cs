using DarklandsBusinessObjects.Streaming;

namespace DarklandsBusinessObjects.Objects
{
    public class Ingredient : StreamObject
    {
        public const int IngredientSize = 4;

        public Ingredient(ByteStream dataStream, int offset)
            : base(dataStream, offset, IngredientSize)
        {
        }

        public int Quantity
        {
            get { return GetWord(0x00); }
        }

        public int ItemCode
        {
            get { return GetWord(0x02); }
        }

        public override string ToString()
        {
            return "[#: " + Quantity
                   + " Id: " + ItemCode
                   + "]";
        }
    }

    //Structure: ingredient

    //Size 4.

    //An ingredient (and quantity) for an alchemical formula.

    //0x00: quantity: word
    //The amount of this ingredient needed.
    //Ranges from 1-5.
    //0x02: item_code: word
    //The item code of the ingredient.
    //Ingredients are given in increasing item code value.
    //Codes are a reference to offsets into the item_definitions array (found in darkland.lst).
}