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
        private Main_State mainState;

        public GUI_Toolbar(Main_State MainState)
        {
            rect = new Rectangle(0, Level.Height * Level.TileWidth, 800, texture.Height);

            //Offset text to bottom right corner (Random values which work nicely are used).
            textPosition = new Vector2(20, rect.Top + 15);
            mainState = MainState;
        }

        public void Redraw(PaintEventArgs e)
        {
            e.Graphics.DrawImage(texture, rect);

            //The new String(' ', 125) is just inserting 125 spaces.
            //string text = string.Format("Gold: {0} {1} Lives: {2} {3}  Wave: {4}", player.Money, new String(' ', 10), player.Lives, new String(' ', 115), waveManager.Round);
            e.Graphics.DrawString(mainState.money.ToString(), SystemFonts.MenuFont, Brushes.White, new Point(720, 650));
            e.Graphics.DrawString(mainState.lives.ToString(), SystemFonts.MenuFont, Brushes.White, new Point(715, 672));
        }
    }
}