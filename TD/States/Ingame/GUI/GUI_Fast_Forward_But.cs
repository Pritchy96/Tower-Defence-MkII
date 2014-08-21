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
    class GUI_Fast_Forward_But : GUI_Button
    {
        private Manager manager;
        static int X = Screen.WIDTH - 50;
        static int Y = 15;

        public GUI_Fast_Forward_But(Manager manager)
            : base(Resources.Fast_Forward_But, Resources.Fast_Forward_But, X, Y)
        {
            this.manager = manager;
        }

        public override void Press(System.Windows.Forms.MouseEventArgs e)
        {
            if (Main_State.speedCoef == 3f)
            {
                Main_State.speedCoef = 1f;
            }
            else
            {
                Main_State.speedCoef = 3f;
            }
        }

    }
}
