using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Defence.Util
{
    public class Vector2
    {
        private float x, y;

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2(float xy)
        {
            X = xy;
            Y = xy;
        }

        public Vector2 Normalise()
        {
            float length = DistanceTo(new Vector2(0, 0));
            X = (X / length);
            Y = (Y / length);
            return new Vector2(X / length, Y / length);
        }

        public float DistanceTo(Vector2 point)
        {
            //Pythagoras to find distance from enemy position to next waypoint.
            return (float)Math.Sqrt(((Math.Pow((X - point.X), 2f)) + (Math.Pow((Y - point.Y), 2f))));
        }

        public static float Distance(Vector2 point1, Vector2 point2)
        {
            //Pythagoras to find distance from enemy position to next waypoint.
            return (float)Math.Sqrt(((Math.Pow((point1.X - point2.X), 2f)) + (Math.Pow((point1.Y - point2.Y), 2f))));
        }

        //Overloading casting to Point data type so it works.
        public static explicit operator Point(Vector2 Vector2)
        {
            return new Point() { X = (int)Vector2.X, Y = (int)Vector2.Y};
        }

        //Overloading adding two Vector2's so the program knows how to handle it.
        public static Vector2 operator +(Vector2 V1, Vector2 V2)
        {
            return new Vector2(V1.X + V2.X, V1.Y + V2.Y);
        }

        //Overloading minusing two Vector2's
        public static Vector2 operator -(Vector2 V1, Vector2 V2)
        {
            return new Vector2(V1.X - V2.X, V1.Y - V2.Y);
        }

        //Method to multiply two Vector2's using operators
        public static Vector2 operator *(Vector2 V1, int num)
        {
            return new Vector2(V1.X * num, V1.Y * num);
        }

        //Method to multiply two Vector2's
        public static Vector2 Multiply (Vector2 V1, Vector2 V2)
        {
            return new Vector2(V1.X * V2.X, V1.Y * V2.Y);
        }

        //Multiplying a Vector2 by a scalar float
        public static Vector2 Multiply(Vector2 V1, float num)
        {
            return new Vector2(V1.X * num, V1.Y * num);
        }
    }
}
