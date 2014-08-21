using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tower_Defence.Properties;
using Tower_Defence.Util;

namespace Tower_Defence.States.Main_Menu
{
    class Level_But : GUI_Button
    {
        private Manager manager;
        private Bitmap originalTexture;

        public Level_But(Manager manager, Bitmap level)
            : base(Resources.Menu_Play_But, Resources.Menu_Play_But, 0, 0)  //Placeholder, actually set dynamically in constructor
        {
            this.manager = manager;
            base.SetButtonNormTex = enhanceTexture(level);
            originalTexture = level;
        }

        public void Initialise(int numberOfTowers)
        {
            int numberPerRow = 5;
            int row = (int)Math.Floor((double)(manager.Buttons.Count() / numberPerRow));   //Finds row to place button on
            int column = manager.Buttons.Count() - (row * numberPerRow);    //Finds column position
            int x = 55 + ((column) * 150);
            int y = 139 + ((row) * 114); ;
            base.rectangle = new Rectangle(x, y, GetButtonNormTex.Width, GetButtonNormTex.Height);
        }

        /// <summary>
        /// Removes fuzzyness induced when stretching image.
        /// </summary>
        public Bitmap enhanceTexture(Bitmap texture)
        {
            Bitmap enhancedTexture = new Bitmap(texture.Width * 3, texture.Height * 3);

            using (Graphics g = Graphics.FromImage(enhancedTexture))
            {
                g.DrawImage(texture, 0, 0, texture.Width * 3, texture.Height * 3);
            }
            
            
            for (int i = 0; i < enhancedTexture.Width; i++)
            {
                for (int j = 0; j < enhancedTexture.Height; j++)
                {
                    Color pixelColour = enhancedTexture.GetPixel(i, j);

                    //If the pixel colour is black, skip the rest of the loop
                    if (pixelColour == Color.FromArgb(0,0,0))
                    {
                        continue;
                    }

                    //If the pixel colour is close enough to white, make it properly white.
                    if (( pixelColour.R > 159 &&
                          pixelColour.G > 159 &&
                          pixelColour.B > 159))
                    {
                        enhancedTexture.SetPixel(i, j, Color.White);
                    }
                    //If it's still a white/grey but not white enough, remove it (make it black)
                    else if (pixelColour.R == pixelColour.G && pixelColour.R == pixelColour.B)
                    {
                        enhancedTexture.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                    }
                    //If it's close enough to blue, make it fully blue

                }
            }
            
            return enhancedTexture;

        }
        public override void Press(System.Windows.Forms.MouseEventArgs e)
        {
            manager.ChangeState(new Main_State(manager, originalTexture));
        }

    }
}
