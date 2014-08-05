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
        public static int TileWidth = 25;
        private static Bitmap[,] map = new Bitmap[32, 24];
        private Bitmap mapImage = Resources.Test_Level;
        Color[,] colourArray = new Color[32, 24];
        #endregion

        #region Properties
        public static int Width
        {
            get { return map.GetLength(0); }
        }

        public static int Height
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
            waypoints.Enqueue(MultiplyPoint(new Vector2(0, 8), TileWidth));
            waypoints.Enqueue(MultiplyPoint(new Vector2(28, 8), TileWidth));
            waypoints.Enqueue(MultiplyPoint(new Vector2(28, 2), TileWidth));
            waypoints.Enqueue(MultiplyPoint(new Vector2(19, 2), TileWidth));
            waypoints.Enqueue(MultiplyPoint(new Vector2(19, 21), TileWidth));
            waypoints.Enqueue(MultiplyPoint(new Vector2(28, 21), TileWidth));
            waypoints.Enqueue(MultiplyPoint(new Vector2(28, 15), TileWidth));
            waypoints.Enqueue(MultiplyPoint(new Vector2(6, 15), TileWidth));
            waypoints.Enqueue(MultiplyPoint(new Vector2(6, 21), TileWidth));
            waypoints.Enqueue(MultiplyPoint(new Vector2(0, 21), TileWidth));
            LoadMap();
        }

        private Bitmap LoadMap()
        {
            Point currentPos = new Point();

            for (int x = 0; x < mapImage.Width; x++)
            {
                for (int y = 0; y < mapImage.Height; y++)
                {
                    colourArray[x, y] = mapImage.GetPixel(x, y);
                    //Make all tiles placeable, the path will overwrite these tiles later.
                    map[x, y] = Resources.Placeable_Tile;

                    if (colourArray[x, y] == Color.FromArgb(0, 0, 255))
                        currentPos = new Point(x, y);   //Start position (Blue pixel)


                }
            }

            if (currentPos.Y > 0 && colourArray[currentPos.X, currentPos.Y - 1] != Color.FromArgb(255, 0, 0))
            {


            }

            //Down
            else if (currentPos.Y < 23 && colourArray[currentPos.X, currentPos.Y + 1] != Color.FromArgb(255, 0, 0))
            {

            }

            //Left
            else if (currentPos.X > 0 && colourArray[currentPos.X - 1, currentPos.Y] != Color.FromArgb(255, 0, 0))
            {

            }
    

            //Right
            else if (currentPos.X < 32 && colourArray[currentPos.X + 1, currentPos.Y] != Color.FromArgb(255, 0, 0))
            {

            }

            else
            {

            }
        }


        private Bitmap PathType(Vector2 currentPosition, Vector2 oldPosition)
        {
            Vector2 Delta = currentPosition - oldPosition;  //Opposite to the one we've come from - a straight line
            Vector2 DeltaOpposite = new Vector2(Delta.Y, Delta.X);  //At a right angle to the one we've come from.

            if (colourArray[(int)(currentPosition.X + Delta.X), (int)(currentPosition.Y + Delta.Y)] 
                == Color.FromArgb(255, 255, 255))   //If the position opposite the one we've come from is white..
            {
                if (colourArray[(int)(currentPosition.X + DeltaOpposite.X), (int)(currentPosition.Y + DeltaOpposite.Y)] 
                    == Color.FromArgb(255, 255, 255))   //If it's also going in another direction..
                {
                    return Resources.Cross; //Must be a cross piece
                }
                else if (Delta.X == 0)  //No change in X axis, must be vertical.
                {
                    return Resources.Vertical;
                }
                else
                {
                    return Resources.Horizontal;
                }
            }
            else if (colourArray[(int)(currentPosition.X + DeltaOpposite.X), (int)(currentPosition.Y + DeltaOpposite.Y)]
                == Color.FromArgb(255, 255, 255))  //If it's at a right angle to the one we've come from..
            {
                //at a right angle. Needs code to work out which of the four graphics to use.
            }
           

/*

            //Left
            if (currentPos.X > 0)
            {
                if (colourArray[currentPos.X - 1, currentPos.Y] != Color.FromArgb(255, 255, 255, 255) ||
                    colourArray[currentPos.X - 1, currentPos.Y] != Color.FromArgb(0, 0, 255))
                {
                    //Can't be these
                    textures.Remove(Resources.Top_Left);
                    textures.Remove(Resources.Bottom_Left);
                    textures.Remove(Resources.Horizontal);
                    textures.Remove(Resources.Cross);
                }
            }

            //Right
            if (currentPos.X < 32)
            {
                if (colourArray[currentPos.X + 1, currentPos.Y] != Color.FromArgb(255, 255, 255, 255) ||
                    colourArray[currentPos.X + 1, currentPos.Y] != Color.FromArgb(0, 0, 255))
                {
                    //Can't be these
                    textures.Remove(Resources.Bottom_Right);
                    textures.Remove(Resources.Top_Right);
                    textures.Remove(Resources.Horizontal);
                    textures.Remove(Resources.Cross);
                }
            }

            return textures[0];
            
 * */
        }

        private Bitmap[,] LoadMapOLD()
        {
            Color[,] colourArray = new Color[32, 24];

            for (int x = 0; x < mapImage.Width; x++)
            {
                for (int y = 0; y < mapImage.Height; y++)
                {
                    colourArray[x, y] = mapImage.GetPixel(x, y);
                }
            }

            for (int x = 0; x < mapImage.Width; x++)
            {
                for (int y = 0; y < mapImage.Height; y++)
                {
                    if (colourArray[x, y] == Color.FromArgb(255, 0, 0, 0))
                        map[x, y] = Resources.Placeable_Tile;

                    else if (colourArray[x, y] == Color.FromArgb(255, 255, 255, 255))
                    {
                        //Check each cell around it to see where it should be facing.
                        Color up = colourArray[x, y - 1];
                        Color down = colourArray[x, y + 1];
                        Color left = colourArray[x - 1, y];
                        Color right = colourArray[x + 1, y];

                        int pathCounter = 0;

                        if (up == Color.FromArgb(255, 255, 255, 255))
                        {
                            pathCounter++;
                        }
                        if (down == Color.FromArgb(255, 255, 255, 255))
                        {
                            pathCounter++;
                        }
                        if (left == Color.FromArgb(255, 255, 255, 255))
                        {
                            pathCounter++;
                        }
                        if (right == Color.FromArgb(255, 255, 255, 255))
                        {
                            pathCounter++;
                        }

                        if (pathCounter == 4)
                        {
                            map[x, y] = Resources.Cross;
                        }
                        else if (pathCounter == 2)
                        {
                            map[x, y] = Resources.Placeable_Tile;
                        }
                        else
                            map[x, y] = Resources.Placeable_Tile;
                    }
                    else map[x, y] = Resources.Placeable_Tile;

                    
                }
               

            }
            return map;
        }

        Vector2 MultiplyPoint(Vector2 point, int coefficient)
        {
            return new Vector2(point.X * coefficient, point.Y * coefficient);
        }

        public Bitmap GetTile(int cellX, int cellY)
        {
            //If the requested cell is out of bounds, return 0
            if (cellX < 0 || cellX > Width || cellY < 0 || cellY > Height)
                return null;
            //Otherwise return the index of the cell.
            else
                try
                {
                    return map[cellY, cellX];
                }
                catch (IndexOutOfRangeException)
                {
                    return null;
                }
        }

        public void Redraw(PaintEventArgs e)
        {

            //Redraw the tiles
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Rectangle tileRectangle = new Rectangle(x * TileWidth, y * TileWidth, TileWidth, TileWidth);
                    e.Graphics.DrawImage(map[x,y], tileRectangle);
                }
            }
        }
    }
}



