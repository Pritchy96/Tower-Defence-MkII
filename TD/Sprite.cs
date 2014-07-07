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


        /// <summary>
        /// Rotates the image by angle.
        /// </summary>
        /// <param name="oldBitmap">The old bitmap.</param>
        /// <param name="angle">The angle.</param>
        /// <returns></returns>
        private static Bitmap RotateImage(Image oldBitmap, float angle)
        {
            var newBitmap = new Bitmap(oldBitmap.Width, oldBitmap.Height);
            var graphics = Graphics.FromImage(newBitmap);
            graphics.TranslateTransform((float)oldBitmap.Width / 2, (float)oldBitmap.Height / 2);
            graphics.RotateTransform(angle);
            graphics.TranslateTransform(-(float)oldBitmap.Width / 2, -(float)oldBitmap.Height / 2);
            graphics.DrawImage(oldBitmap, new Point(0, 0));
            return newBitmap;
        }


        //Initial Draw method
        public virtual void Draw(PaintEventArgs e)
        {
            Bitmap textureToDraw = RotateImage(texture, (float)(rotation *  (180/Math.PI)));

            e.Graphics.DrawImage(textureToDraw, (Point)position);
        }
           
        /*'Tinted' Draw function, override.
        public virtual void Draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(texture, center, null, color, rotation, origin, 1.0f, SpriteEffects.None, 0);
        }
         * */
    }
}




    
