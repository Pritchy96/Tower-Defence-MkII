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
        protected Bitmap texture;
        protected Vector2 position; //World relative top corner
        protected Vector2 velocity;
        protected Vector2 center;   //World relative center
        protected float rotation;
        private Rectangle bounds;
        #endregion

        #region Properties
        public Vector2 Center
        {
            get { return center; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Rectangle Bounds
        {
            get { return bounds; }
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
            center = new Vector2(position.X + (texture.Width / 2), position.Y + (texture.Height / 2));
            bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); //Rect to fit around the Sprite.
        }

        /// <summary>
        /// Method to rotate an Image object. The result can be one of three cases:
        /// - upsizeOk = true: output image will be larger than the input, and no clipping occurs 
        /// - upsizeOk = false & clipOk = true: output same size as input, clipping occurs
        /// - upsizeOk = false & clipOk = false: output same size as input, image reduced, no clipping
        /// 
        /// A background color must be specified, and this color will fill the edges that are not 
        /// occupied by the rotated image. If color = transparent the output image will be 32-bit, 
        /// otherwise the output image will be 24-bit.
        /// 
        /// Note that this method always returns a new Bitmap object, even if rotation is zero - in 
        /// which case the returned object is a clone of the input object. 
        /// </summary>
        /// <param name="inputImage">input Image object, is not modified</param>
        /// <param name="angleDegrees">angle of rotation, in degrees</param>
        /// <param name="upsizeOk">see comments above</param>
        /// <param name="clipOk">see comments above, not used if upsizeOk = true</param>
        /// <param name="backgroundColor">color to fill exposed parts of the background</param>
        /// <returns>new Bitmap object, may be larger than input image</returns>
        public static Bitmap RotateImage(Image inputImage, float angleDegrees, bool upsizeOk,
                                         bool clipOk, Color backgroundColor)
        {
            // Test for zero rotation and return a clone of the input image
            if (angleDegrees == 0f)
                return (Bitmap)inputImage.Clone();

            // Set up old and new image dimensions, assuming upsizing not wanted and clipping OK
            int oldWidth = inputImage.Width;
            int oldHeight = inputImage.Height;
            int newWidth = oldWidth;
            int newHeight = oldHeight;
            float scaleFactor = 1f;

            // If upsizing wanted or clipping not OK calculate the size of the resulting bitmap
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

            // Create the new bitmap object. If background color is transparent it must be 32-bit, 
            //  otherwise 24-bit is good enough.
            Bitmap newBitmap = new Bitmap(newWidth, newHeight, backgroundColor == Color.Transparent ?
                                             PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb);
            newBitmap.SetResolution(inputImage.HorizontalResolution, inputImage.VerticalResolution);

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
                graphicsObject.DrawImage(inputImage, 0, 0);
            }
            return newBitmap;
        }

        /*
        public Image SetImageOpacity(Image image, float opacity)
        {
            try
            {
                //create a Bitmap the size of the image provided  
                Bitmap bmp = new Bitmap(image.Width, image.Height);

                //create a graphics object from the image  
                using (Graphics gfx = Graphics.FromImage(bmp))
                {

                    //create a color matrix object  
                    ColorMatrix matrix = new ColorMatrix();

                    //set the opacity  
                    matrix.Matrix33 = opacity;

                    //create image attributes  
                    ImageAttributes attributes = new ImageAttributes();

                    //set the color(opacity) of the image  
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    //now draw the image  
                    gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                }
                return bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }


        private const int bytesPerPixel = 4;

        /// <summary>
        /// Change the opacity of an image
        /// </summary>
        /// <param name="originalImage">The original image</param>
        /// <param name="opacity">Opacity, where 1.0 is no opacity, 0.0 is full transparency</param>
        /// <returns>The changed image</returns>
        public static Image SetImageOpacity(Image originalImage, double opacity)
        {
            if ((originalImage.PixelFormat & PixelFormat.Indexed) == PixelFormat.Indexed)
            {
                // Cannot modify an image with indexed colors
                return originalImage;
            }

            Bitmap bmp = (Bitmap)originalImage.Clone();

            // Specify a pixel format.
            PixelFormat pxf = PixelFormat.Format32bppArgb;

            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            // This code is specific to a bitmap with 32 bits per pixels 
            // (32 bits = 4 bytes, 3 for RGB and 1 byte for alpha).
            int numBytes = bmp.Width * bmp.Height * bytesPerPixel;
            byte[] argbValues = new byte[numBytes];

            // Copy the ARGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, argbValues, 0, numBytes);

            // Manipulate the bitmap, such as changing the
            // RGB values for all pixels in the the bitmap.
            for (int counter = 0; counter < argbValues.Length; counter += bytesPerPixel)
            {
                // argbValues is in format BGRA (Blue, Green, Red, Alpha)

                // If 100% transparent, skip pixel
                if (argbValues[counter + bytesPerPixel - 1] == 0)
                    continue;

                int pos = 0;
                pos++; // B value
                pos++; // G value
                pos++; // R value

                argbValues[counter + pos] = (byte)(argbValues[counter + pos] * opacity);
            }

            // Copy the ARGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            return bmp;
        }
        */

        public static Bitmap SetImageOpacity(Image img, float opacityvalue)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height); // Determining Width and Height of Source Image
            Graphics graphics = Graphics.FromImage(bmp);
            ColorMatrix colormatrix = new ColorMatrix();
            colormatrix.Matrix33 = opacityvalue;
            ImageAttributes imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttribute);
            graphics.Dispose();   // Releasing all resource used by graphics 
            return bmp;
        }

        public virtual void Redraw(PaintEventArgs e)
        {
            Bitmap textureToDraw = RotateImage(texture, (float)(rotation * (180 / Math.PI)), false, true, Color.Transparent);
            e.Graphics.DrawImage(textureToDraw, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height));
        }

        /*Tinted' Redraw function, override.
        public virtual void Redraw(PaintEventArgs e, Color color)
        {
            spriteBatch.Redraw(texture, center, null, color, rotation, origin, 1.0f, SpriteEffects.None, 0);
        }
         * */

    }
}





