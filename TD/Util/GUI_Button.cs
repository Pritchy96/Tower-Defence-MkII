using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tower_Defence.Util
{
    public abstract class GUI_Button
    {
        private Bitmap buttonNormTex, buttonPressTex;
        protected Rectangle rectangle;
        private bool pressed = false;

        public Bitmap GetButtonNormTex
        {
            get { return buttonNormTex; }
        }
        protected Bitmap SetButtonNormTex
        {
            set { buttonNormTex = value; }
        }

        public Bitmap ButtonPressTex
        {
            get { return buttonPressTex; }
        }

        public Rectangle Rectangle
        {
            get { return rectangle; }
        }

        public bool Pressed
        {
            get { return pressed; }
            set { pressed = value; }
        }

        public GUI_Button(Bitmap buttonNormalTexture, Bitmap buttonPressedTexture, int x, int y)
        {
            buttonNormTex = buttonNormalTexture;
            buttonPressTex = buttonPressedTexture;

            rectangle = new Rectangle(x, y, buttonNormalTexture.Width, buttonNormalTexture.Height);
        }

        public abstract void Press(MouseEventArgs e);

        public virtual void Redraw(PaintEventArgs e)
        {
            if (!pressed)
            {
                e.Graphics.DrawImage(GetButtonNormTex, Rectangle);
            }
            else
            {
                e.Graphics.DrawImage(ButtonPressTex, Rectangle);
            }
         }
    }
}
