using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Tower_Defence.Util;

namespace Tower_Defence
{
    public class Sprite
    {
        //Shared variables.
        protected Bitmap texture;
        //world relative top corner
        protected Vector2 position;
        protected Vector2 velocity;
        //World relative center
        protected Vector2 center;
        //Object relative center
        protected Vector2 origin;
        protected float rotation;

        private Rectangle bounds;

        //Getters & Setters.
        public Vector2 Center
        {
            get { return center; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public Rectangle Bounds
        {
            get { return bounds; }
        }

        public Sprite(Bitmap tex, Vector2 pos)
        {
            texture = tex;

            position = pos;
            velocity = new Vector2(0, 0);

            center = new Vector2(position.X + (texture.Width / 2), position.Y + (texture.Height / 2));
            origin = new Vector2(texture.Width / 2, texture.Height / 2);

            //Initialize rectange to fit around the Sprite.
            this.bounds = new Rectangle((int)position.X, (int)Position.Y,
            texture.Width, texture.Height);
        }

        public virtual void Update()
        {
            center = new Vector2(position.X + (texture.Width / 2), position.Y + (texture.Height / 2));
        }




        private Bitmap rotateImage(Bitmap b, float angle)
        {
            //create a new empty bitmap to hold rotated image
            Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(returnBitmap);
            //move rotation point to center of image
            g.TranslateTransform((float)center.X, (float)center.Y);
            //rotate
            g.RotateTransform(angle);
            //move image back
            g.TranslateTransform(-(float)center.X, -(float)center.Y);
            //draw passed in image onto graphics object
            g.DrawImage(b, new Point(0, 0));
            return returnBitmap;
        }

        //Initial Draw method
        public virtual void Draw(PaintEventArgs e)
        {
            Bitmap textureToDraw = texture;
            textureToDraw = rotateImage(texture, rotation);

            e.Graphics.DrawImage(texture, (Point)position);
        }
           
        /*'Tinted' Draw function, override.
        public virtual void Draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(texture, center, null, color, rotation, origin, 1.0f, SpriteEffects.None, 0);
        }
         * */
    }
}




    
