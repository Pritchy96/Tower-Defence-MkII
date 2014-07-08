using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tower_Defence.Properties;
using Tower_Defence.Util;

namespace Tower_Defence
{
    public class Level
    {
        #region Variables
        private Queue<Vector2> waypoints = new Queue<Vector2>();
        Bitmap[] textures = { Resources.Placeable_Tile, Resources.Top_Right, Resources.Bottom_Right, Resources.Top_Left, 
                                Resources.Bottom_Left, Resources.Horizontal, Resources.Vertical, Resources.Cross };
        public static int TileWidth = 40;

        #region Map Array Code
        int[,] map = 
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,5,5,1,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0,6,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0,6,0},
            {5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,7,5,5,2,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0,0,0},
            {0,0,0,0,0,0,3,5,5,5,5,5,5,5,5,7,5,5,1,0},
            {0,0,0,0,0,0,6,0,0,0,0,0,0,0,0,6,0,0,6,0},
            {0,0,0,0,0,0,6,0,0,0,0,0,0,0,0,6,0,0,6,0},
            {5,5,5,5,5,5,2,0,0,0,0,0,0,0,0,4,5,5,2,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}

        };
        #endregion
        #endregion

        #region Properties
        public int Width
        {
            get { return map.GetLength(0); }
        }

        public int Height
        {
            get { return map.GetLength(1); }
        }

        public Queue<Vector2> Waypoints
        {
            get { return waypoints; }
        }
        #endregion

        public Level()
        {
            waypoints.Enqueue(MultiplyPoint(new Vector2(0, 4), 40));
            waypoints.Enqueue(MultiplyPoint(new Vector2(18, 4), 40));
            waypoints.Enqueue(MultiplyPoint(new Vector2(18, 1), 40));
            waypoints.Enqueue(MultiplyPoint(new Vector2(15, 1), 40));
            waypoints.Enqueue(MultiplyPoint(new Vector2(15, 10), 40));
            waypoints.Enqueue(MultiplyPoint(new Vector2(18, 10), 40));
            waypoints.Enqueue(MultiplyPoint(new Vector2(18, 7), 40));
            waypoints.Enqueue(MultiplyPoint(new Vector2(6, 7), 40));
            waypoints.Enqueue(MultiplyPoint(new Vector2(6, 10), 40));
            waypoints.Enqueue(MultiplyPoint(new Vector2(0, 10), 40));
        }

        Vector2 MultiplyPoint(Vector2 point, int coefficient)
        {
            return new Vector2(point.X * coefficient, point.Y * coefficient);
        }

        public int GetIndex(int cellX, int cellY)
        {
            //If the requested cell is out of bounds, return 0
            if (cellX < 0 || cellX > Width || cellY < 0 || cellY > Height)
                return 0;
            //Otherwise return the index of the cell.
            else
                try
                {
                    return map[cellY, cellX];
                }
                catch (IndexOutOfRangeException)
                {
                    return 0;
                }
        }

        public void Draw(PaintEventArgs e)
        {

            //Draw the tiles
            for (int x = 0; x < Height; x++)
            {
                for (int y = 0; y < Width; y++)
                {
                    Bitmap tileTexture = textures[map[y, x]];

                    Rectangle tileRectangle = new Rectangle(x * TileWidth, y * TileWidth, TileWidth, TileWidth);
                    e.Graphics.DrawImage((Image)(tileTexture), tileRectangle);
                }
            }
        }
    }
}



