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
    class GUI_Slow_Tow_But : GUI_Button
    {
        private Main_State mainState;
        static int X = 75 - ((Resources.Tow_Basic.Width - Level.TileWidth) / 2);
        static int Y = 625 - ((Resources.Tow_Basic.Height - Level.TileWidth) / 2);

        public GUI_Slow_Tow_But(Main_State mainState)
            : base(Resources.Tow_Slow, Resources.Tow_Slow, X, Y)
        {
            this.mainState = mainState;
        }

        public override void Press(System.Windows.Forms.MouseEventArgs e)
        {
            //Setting tower to add to a new basic tower. 
            //The rest of the placing code is handled with events in the Main state.
            mainState.TowerToAdd = new Tow_Slow();
        }

    }
}
