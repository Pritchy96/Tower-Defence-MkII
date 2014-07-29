using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tower_Defence.Properties;
using Tower_Defence.States.Ingame;
using Tower_Defence.States.Ingame.Towers;
using Tower_Defence.Util;

namespace Tower_Defence.States
{
    class GUI_Menu_But : GUI_Button
    {
        private Main_State mainState;
        static int X = 663;
        static int Y = Screen.HEIGHT - 110;

        public GUI_Menu_But(Main_State mainState)
            : base(Resources.GUI_But_Menu, Resources.GUI_But_Menu, X, Y)
        {}

        public override void Press(System.Windows.Forms.MouseEventArgs e)
        {

        }

    }
}
