using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tower_Defence.Properties;
using Tower_Defence.Util;

namespace Tower_Defence.States.Ingame
{
    public class GUI_Toolbar
    {

        private Bitmap texture = Resources.GUI_Toolbar;

        //Position of toolbar & font.
        private Rectangle rect;
        private Vector2 textPosition;

        public GUI_Toolbar()
        {
            rect = new Rectangle(0, Level.Height * Level.TileWidth, 800, texture.Height);

            //Offset text to bottom right corner (Random values which work nicely are used).
            textPosition = new Vector2(20, rect.Top + 15);

        }

        public void Redraw(PaintEventArgs e)
        {
            e.Graphics.DrawImage(texture, rect);

            //The new String(' ', 125) is just inserting 125 spaces.
            //string text = string.Format("Gold: {0} {1} Lives: {2} {3}  Wave: {4}", player.Money, new String(' ', 10), player.Lives, new String(' ', 115), waveManager.Round);
            //spriteBatch.DrawString(font, text, textPosition, Color.White);
        }
    }
}