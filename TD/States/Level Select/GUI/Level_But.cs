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

        public Level_But(Manager manager, Bitmap level)
            : base(Resources.Menu_Play_But, Resources.Menu_Play_But, 0, 0)  //Placeholder, actually set dynamically in constructor
        {
            this.manager = manager;
            base.SetButtonNormTex = level;

        }

        public void Initialise(int numberOfTowers)
        {
            int numberPerRow = 5;
            int row = (int)Math.Floor((double)(manager.Buttons.Count() / numberPerRow));   //Finds row to place button on (4 per row)
            int column = manager.Buttons.Count() - (row * numberPerRow);


            //55 + 152
            int x = 55 + ((column) * 150);
            int y = 139 + ((row) * 114); ;

            
            base.rectangle = new Rectangle(x, y, GetButtonNormTex.Width * 3, GetButtonNormTex.Height * 3);
        }

        public override void Press(System.Windows.Forms.MouseEventArgs e)
        {
            manager.ChangeState(new Main_State(manager, base.GetButtonNormTex));
        }

    }
}
