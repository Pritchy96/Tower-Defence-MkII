using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Tower_Defence.Util;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Tower_Defence
{
    public class Sprite
    {
        #region Variables
        private Bitmap texture;
        private Vector2 position; //World relative top corner
        private Vector2 velocity;
        private Vector2 center = new Vector2(0,0);   //World relative center
        private float rotation;
        private Rectangle bounds;
        #endregion

        #region Properties
        public Vector2 Center
        {
            get { lock (center) { return center; } }
            set { lock (center) { center = value; } }
        }

        public Vector2 Velocity
        {
            get { return velocity * Main_State.speedCoef; }
            set { velocity = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

        public Bitmap Texture
        {
            get { return texture; }
        }
        #endregion

        public Sprite(Bitmap tex, Vector2 pos)
        {
            texture = tex;
            position = pos;
            center = new Vector2(position.X + (texture.Width / 2), position.Y + (texture.Height / 2));
            bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); //Rect to fit around the Sprite.
        }

        public Sprite(Bitmap tex)
        {
            texture = tex;
        }

        public virtual void Update()
        {
            try
            {
                Center = new Vector2(position.X + (texture.Width / 2), position.Y + (texture.Height / 2));
            }
            catch(InvalidOperationException) {  }   //Variable is currently being used, will have to wait.

            try
            {
            Bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); //Rect to fit around the Sprite.
            }
            catch (InvalidOperationException) { }   //Variable is currently being used, will have to wait.
        }

        /// <summary>
        /// Method to rotate an Bitmap object. The result can be one of three cases:
        /// - upsizeOk = true: output Bitmap will be larger than the input, and no clipping occurs 
        /// - upsizeOk = false & clipOk = true: output same size as input, clipping occurs
        /// - upsizeOk = false & clipOk = false: output same size as input, Bitmap reduced, no clipping
        /// 
        /// A background color must be specified, and this color will fill the edges that are not 
        /// occupied by the rotated Bitmap. If color = transparent the output Bitmap will be 32-bit, 
        /// otherwise the output Bitmap will be 24-bit.
        /// 
        /// Note that this method always returns a new Bitmap object, even if rotation is zero - in 
        /// which case the returned object is a clone of the input object. 
        /// </summary>
        /// <param name="inputBitmap">input Bitmap object, is not modified</param>
        /// <param name="angleDegrees">angle of rotation, in degrees</param>
        /// <param name="upsizeOk">see comments above</param>
        /// <param name="clipOk">see comments above, not used if upsizeOk = true</param>
        /// <param name="backgroundColor">color to fill exposed parts of the background</param>
        /// <returns>new Bitmap object, may be larger than input Bitmap</returns>
        public static Bitmap RotateBitmap(Bitmap inputBitmap, float angleDegrees, bool upsizeOk,
                                         bool clipOk, Color backgroundColor)
        {
            // Test for zero rotation and return a clone of the input Bitmap
            if (angleDegrees == 0f)
                return (Bitmap)inputBitmap.Clone();

            // Set up old and new Bitmap dimensions, assuming upsizing not wanted and clipping OK
            int oldWidth = inputBitmap.Width;
            int oldHeight = inputBitmap.Height;
            int newWidth = oldWidth;
            int newHeight = oldHeight;
            float scaleFactor = 1f;

            // If upsizing wanted or clipping not OK calculate the size of the resulting Bitmap
            if (upsizeOk || !clipOk)
            {
                double angleRadians = angleDegrees * Math.PI / 180d;

                double cos = Math.Abs(Math.Cos(angleRadians));
                double sin = Math.Abs(Math.Sin(angleRadians));
                newWidth = (int)Math.Round(oldWidth * cos + oldHeight * sin);
                newHeight = (int)Math.Round(oldWidth * sin + oldHeight * cos);
            }

            // If upsizing not wanted and clipping not OK need a scaling factor
            if (!upsizeOk && !clipOk)
            {
                scaleFactor = Math.Min((float)oldWidth / newWidth, (float)oldHeight / newHeight);
                newWidth = oldWidth;
                newHeight = oldHeight;
            }

            // Create the new Bitmap object. If background color is transparent it must be 32-bit, 
            //  otherwise 24-bit is good enough.
            Bitmap newBitmap = new Bitmap(newWidth, newHeight, backgroundColor == Color.Transparent ?
                                             PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb);
            newBitmap.SetResolution(inputBitmap.HorizontalResolution, inputBitmap.VerticalResolution);

            // Create the Graphics object that does the work
            using (Graphics graphicsObject = Graphics.FromImage(newBitmap))
            {
                graphicsObject.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsObject.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphicsObject.SmoothingMode = SmoothingMode.HighQuality;

                // Fill in the specified background color if necessary
                if (backgroundColor != Color.Transparent)
                    graphicsObject.Clear(backgroundColor);

                // Set up the built-in transformation matrix to do the rotation and maybe scaling
                graphicsObject.TranslateTransform(newWidth / 2f, newHeight / 2f);

                if (scaleFactor != 1f)
                    graphicsObject.ScaleTransform(scaleFactor, scaleFactor);

                graphicsObject.RotateTransform(angleDegrees);
                graphicsObject.TranslateTransform(-oldWidth / 2f, -oldHeight / 2f);

                // Redraw the result 
                graphicsObject.DrawImage(inputBitmap, 0, 0);
            }
            return newBitmap;
        }

        public Bitmap SetBitmapOpacity(Bitmap Bitmap, float opacity)
        {
            try
            {
                //create a Bitmap the size of the Bitmap provided  
                Bitmap bmp = new Bitmap(Bitmap.Width, Bitmap.Height);

                //create a graphics object from the Bitmap  
                using (Graphics gfx = Graphics.FromImage(bmp))
                {

                    //create a color matrix object  
                    ColorMatrix matrix = new ColorMatrix();

                    //set the opacity  
                    matrix.Matrix33 = opacity;

                    //create Bitmap attributes  
                    ImageAttributes attributes = new ImageAttributes();

                    //set the color(opacity) of the Bitmap  
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    //now draw the Bitmap  
                    gfx.DrawImage(Bitmap, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, Bitmap.Width, Bitmap.Height, GraphicsUnit.Pixel, attributes);
                }
                return bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        } 

        public virtual void Redraw(PaintEventArgs e)
        {
            try
            {
                Bitmap textureToDraw = RotateBitmap(texture, (float)(rotation * (180 / Math.PI)), false, true, Color.Transparent);
                e.Graphics.DrawImage(textureToDraw, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height));
            }
            catch (InvalidOperationException) { }   //Variable is currently being used, will have to wait.

        }

        /*
        public virtual void Redraw(PaintEventArgs e, Color colour)
        {
            Bitmap textureToDraw;

            try
            {
                textureToDraw = RotateBitmap(texture, (float)(rotation * (180 / Math.PI)), false, true, Color.Transparent);
                //Draw Base
                Rectangle rect = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
                e.Graphics.DrawImage(textureToDraw, rect);

                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, 255, 0, 0)), rect);
                
            }
            catch (InvalidOperationException) { }   //Variable is currently being used, will have to wait.
        }
         */
    }
}





