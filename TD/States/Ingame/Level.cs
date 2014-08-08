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
        public static int TileWidth = 25;
        private static Bitmap[,] mapTextureArray = new Bitmap[32, 24];
        private Bitmap mapImage;
        Color[,] colourArray = new Color[32, 24];
        Vector2 currentPosition, oldPosition;
        private Queue<Vector2> waypoints = new Queue<Vector2>();




        //TEMP
        bool atEnd = false;
        #endregion

        #region Properties
        public static int Width
        {
            get { return mapTextureArray.GetLength(0); }
        }

        public static int Height
        {
            get { return mapTextureArray.GetLength(1); }
        }

        public Queue<Vector2> Waypoints
        {
            get { return waypoints; }
        }
        #endregion

        public Level(Bitmap mapImage)
        {
            this.mapImage = mapImage;
            LoadMap();
        }

        private void LoadMap()
        {

            for (int x = 0; x < mapImage.Width; x++)
            {
                for (int y = 0; y < mapImage.Height; y++)
                {
                    colourArray[x, y] = mapImage.GetPixel(x, y);
                    //Make all tiles placeable, the path will overwrite these tiles later.
                    mapTextureArray[x, y] = Resources.Placeable_Tile;
                }
            }

            #region Set spawn point and current position
            for (int x = 0; x < mapImage.Width; x++)
            {
                for (int y = 0; y < mapImage.Height; y++)
                {
                    if (colourArray[x, y] == Color.FromArgb(0, 0, 255))
                    {
                        oldPosition = new Vector2(x, y);   //Start position (Blue pixel)
                        waypoints.Enqueue(MultiplyPoint(oldPosition, TileWidth));   //Adding first waypoint

                        #region Finding next position
                        if ((x - 1) > 0 && colourArray[x - 1, y] == Color.FromArgb(255, 255, 255))
                        {
                            currentPosition = new Vector2(x - 1, y);
                            mapTextureArray[x, y] = Resources.Horizontal;
                        }
                        else if ((x + 1) < 32 && colourArray[x + 1, y] == Color.FromArgb(255, 255, 255))
                        {
                            currentPosition = new Vector2(x + 1, y);
                            mapTextureArray[x, y] = Resources.Horizontal;
                        }
                        else if ((y - 1) > 0 && colourArray[x, y - 1] == Color.FromArgb(255, 255, 255))
                        {
                            currentPosition = new Vector2(x, y - 1);
                            mapTextureArray[x, y] = Resources.Vertical;
                        }
                        else if ((y + 1) < 24 && colourArray[x, y + 1] == Color.FromArgb(255, 255, 255))
                        {
                            currentPosition = new Vector2(x, y + 1);
                            mapTextureArray[x, y] = Resources.Vertical;
                        }

                        waypoints.Enqueue(MultiplyPoint(currentPosition, TileWidth));   //Adding last waypoint
                        #endregion
                    }
                }
            }
            #endregion

            while (!atEnd)
            {
                if (colourArray[(int)currentPosition.X, (int)currentPosition.Y] == Color.FromArgb(255, 0, 0))
                {

                    atEnd = true;

                    Vector2 delta = currentPosition - oldPosition;

                    if (delta.X != 0)
                    {
                        mapTextureArray[(int)currentPosition.X, (int)currentPosition.Y] = Resources.Horizontal;
                    }
                    else if (delta.Y != 0)
                    {
                        mapTextureArray[(int)currentPosition.X, (int)currentPosition.Y] = Resources.Vertical;
                    }
                }
                else
                {
                    mapTextureArray[(int)currentPosition.X, (int)currentPosition.Y] = PathType();
                }
            }

        }

        /// <summary>
        /// Returns the bitmap of a cell within the world mapTextureArray, based on the direction from 
        /// which it is going and to which it is going.
        /// </summary>
        /// <returns></returns>
        private Bitmap PathType()
        {
            Vector2 Delta = currentPosition - oldPosition;  //Opposite to the one we've come from - a straight line
            Vector2 DeltaOpposite = new Vector2(Delta.Y, Delta.X);  //At a right angle to the one we've come from.

            //If the position opposite the one we've come from is white..
            if (colourArray[(int)(currentPosition.X + Delta.X), (int)(currentPosition.Y + Delta.Y)] == Color.FromArgb(255, 255, 255)
                || colourArray[(int)(currentPosition.X + Delta.X), (int)(currentPosition.Y + Delta.Y)] == Color.FromArgb(255, 0, 0))
            {
                //If it's also going in another direction..
                if (colourArray[(int)(currentPosition.X + DeltaOpposite.X), (int)(currentPosition.Y + DeltaOpposite.Y)] == Color.FromArgb(255, 255, 255)
                    || colourArray[(int)(currentPosition.X + DeltaOpposite.X), (int)(currentPosition.Y + DeltaOpposite.Y)] == Color.FromArgb(255, 0, 0))
                {
                    oldPosition = currentPosition;
                    currentPosition += Delta;   //Actual enemies are going across though
                    return Resources.Cross; //Must be a cross piece
                }
                else if (Delta.X == 0)  //No change in X axis, must be vertical.
                {
                    oldPosition = currentPosition;
                    currentPosition += Delta;
                    return Resources.Vertical;
                }
                else
                {
                    oldPosition = currentPosition;
                    currentPosition += Delta;
                    return Resources.Horizontal;
                }
            }
            //If it's at a right angle to the one we've come from..
            else if (colourArray[(int)(currentPosition.X - DeltaOpposite.X), (int)(currentPosition.Y - DeltaOpposite.Y)] == Color.FromArgb(255, 255, 255)
                || colourArray[(int)(currentPosition.X - DeltaOpposite.X), (int)(currentPosition.Y - DeltaOpposite.Y)] == Color.FromArgb(255, 0, 0))
            {
                //Calculate position of the direction current tile is going.
                Vector2 newPos = new Vector2(currentPosition.X - DeltaOpposite.X, currentPosition.Y - DeltaOpposite.Y);
                //find the Delta between the old positionm the one current tile is going from, to the new position, the one it is going to.
                Vector2 newDelta = newPos - oldPosition;

                oldPosition = currentPosition;
                currentPosition = newPos;
                waypoints.Enqueue(MultiplyPoint(oldPosition, TileWidth));
                return ChooseRightAngle(Delta, newDelta);
            }
            //Or if it's a right angle going the other way (ie; up instead of down or vice versa)
            else if (colourArray[(int)(currentPosition.X + DeltaOpposite.X), (int)(currentPosition.Y + DeltaOpposite.Y)] == Color.FromArgb(255, 255, 255)
                || colourArray[(int)(currentPosition.X + DeltaOpposite.X), (int)(currentPosition.Y + DeltaOpposite.Y)] == Color.FromArgb(255, 0, 0))
            {
                Vector2 newPos = new Vector2(currentPosition.X + DeltaOpposite.X, currentPosition.Y + DeltaOpposite.Y);
                Vector2 newDelta = newPos - oldPosition;

                oldPosition = currentPosition;
                currentPosition = newPos;
                waypoints.Enqueue(MultiplyPoint(oldPosition, TileWidth));   //We are changing direction so we need to add a waypoint.
                return ChooseRightAngle(Delta, newDelta);

            }
            else
            {
                atEnd = true;
                return Resources.Radius_Texture;
            }
        }

        /// <summary>
        /// Finds the right angle to be used when loading a mapTextureArray and there is a change in direction. 
        /// It does this by first finding which direction from which the right angle is entered from. 
        /// This narrows it down to two potential angles (remember there are two potential points of 
        /// entry for each angle). It then looks at the direction from which the right angle is exited
        /// in order to choose between the remaining two: they will be going in opposite directions, only 
        /// one of which is the correct direction.
        /// </summary>
        /// <param name="OldToCurrentDelta"></param>
        /// <param name="CurrentToNewDelta"></param>
        /// <returns></returns>
        private Bitmap ChooseRightAngle(Vector2 OldToCurrentDelta, Vector2 CurrentToNewDelta)
        {
            if (OldToCurrentDelta.X == 0)   //We are coming up or down into the right angle
            {
                if (OldToCurrentDelta.Y == 1)   //Coming up into the right angle
                {
                    if (CurrentToNewDelta.X == 1)  //Exiting right angle to the right
                    {
                        return Resources.Bottom_Left;
                    }
                    else   //Exiting right angle to the left
                    {
                        return Resources.Bottom_Right;
                    }
                }
                else    //Coming down in to the right angle
                {
                    if (CurrentToNewDelta.X == 1)  //Exiting right angle to the right
                    {
                        return Resources.Top_Left;
                    }
                    else   //Exiting right angle to the left
                    {
                        return Resources.Top_Right;
                    }
                }
            }
            else //We are coming from the left or right into the right angle
            {
                if (OldToCurrentDelta.X == 1)   //Coming in from the left.
                {
                    if (CurrentToNewDelta.Y == 1)  //Exiting right angle downwards
                    {
                        return Resources.Top_Right;
                    }
                    else    //Exiting right angle Upwards
                    {
                        return Resources.Bottom_Right;
                    }
                }
                else    //Coming in from the right
                {
                    if (CurrentToNewDelta.Y == 1)  //Exiting right angle downwards.
                    {
                        return Resources.Top_Left;
                    }
                    else    //Exiting right angle Upwards
                    {
                        return Resources.Bottom_Left;
                    }
                }
            }
        }

        Vector2 MultiplyPoint(Vector2 point, int coefficient)
        {
            return new Vector2(point.X * coefficient, point.Y * coefficient);
        }

        /// <summary>
        /// Returns the bitmap tile of the cell requested within the level/mapTextureArray
        /// </summary>
        /// <param name="cellX"></param>
        /// <param name="cellY"></param>
        /// <returns></returns>
        public bool IsPath(int cellX, int cellY)
        {
            //If the requested cell is out of bounds, return 0
            if (cellX < 0 || cellX > Width || cellY < 0 || cellY > Height)
                return false;
            //Otherwise whether the cell is a path cell.
            else
                try
                {
                    return (mapImage.GetPixel(cellX, cellY) == Color.FromArgb(0, 0, 0)) == true;    //Check to see if the pixel on mapImage is black. If so, it's not a path.
                }
                catch (IndexOutOfRangeException)
                {
                    return false;
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
                    e.Graphics.DrawImage(mapTextureArray[x, y], tileRectangle);
                }
            }
        }
    }
}



/*

           

/*
 *             if (Delta.X == 1)
            {
                if (Delta.Y == 1)
                {
                    return Resources.Bottom_Left;
                }
                else
                {
                    return Resources.Radius_Texture;
                }
            }
            else
            {
                if (Delta.Y == 1)
                {
                    return Resources.Top_Left ;
                }
                else
                {
                    return Resources.Bottom_Right;
                }
            }*/
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