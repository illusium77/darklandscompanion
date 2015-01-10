using System;
using DarklandsBusinessObjects.Streaming;

namespace DarklandsBusinessObjects.Objects
{
    public class Coordinate : StreamObject
    {
        //private const int MaxX = 0x0147;
        //private const int MaxY = 0x03a3;
        public const int CoordinateSize = 0x4;

        public Coordinate(ByteStream dataStream, int offset)
            : base(dataStream, offset, CoordinateSize)
        {
        }

        public int X
        {
            get { return GetWord(0x00); }
        }

        public int Y
        {
            get { return GetWord(0x02); }
        }

        //public void Translate(int newMaxX, int newMaxY)
        //{
        //    double newX = ((double)X / MAX_X) * newMaxX;
        //    X = (int)newX;

        //    double newY = ((double)Y / MAX_Y) * newMaxY;
        //    Y = (int)newY;
        //}

        public int DistanceTo(Coordinate other)
        {
            var dx = X - other.X;
            var dy = Y - other.Y;

            var distance = Math.Sqrt(dx*dx + dy*dy);

            return (int) Math.Round(distance);
        }

        public Bearing BearingTo(Coordinate other)
        {
            var dx = other.X - X;
            var dy = (other.Y - Y)*-1; // on map, y is inverted

            var bearing = (Math.Atan2(dy, dx)*(180/Math.PI) + 360)%360;

            return (Bearing) (Math.Round(bearing/45));
        }

        public override string ToString()
        {
            return "[x:" + X + " y:" + Y + "]";
        }

        #region Compare

        public override bool Equals(Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Coordinate return false.
            var c = obj as Coordinate;
            if ((Object) c == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (X == c.X) && (Y == c.Y);
        }

        public static bool operator ==(Coordinate lhs, Coordinate rhs)
        {
            // Check for null on left side. 
            if (ReferenceEquals(lhs, null))
            {
                if (ReferenceEquals(rhs, null))
                {
                    // null == null = true. 
                    return true;
                }

                // Only the left side is null. 
                return false;
            }
            // Equals handles case of null on right side. 
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Coordinate lhs, Coordinate rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return X ^ Y;
        }

        #endregion
    }

    //https://web.archive.org/web/20091112194440/http://wallace.net/darklands/formats/structures.html#structdef-coordinates
    //Structure: coordinates

    //Size 0x04.

    //A world map location (pair of coordinates).

    //0x00: x_coord: word
    //X-coordinate.
    //Ranges from 0x0000 - 0x0147.
    //The maximum is the first word of the file "darkland.map".
    //0x02: y_coord: word
    //Y-coordinate.
    //Ranges from 0x0000 - 0x03a3.
    //The maximum is the second word of the file "darkland.map".
}