using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tower_Defence.Properties;
using Tower_Defence.Util;

namespace Tower_Defence.States.Main_Menu
{
    class GUI_Play_But : GUI_Button
    {
        private Manager manager; 

        public GUI_Play_But(Manager manager) : base (Resources.Menu_Play_But, Resources.Menu_Play_But, 
            Screen.WIDTH / 2 - Resources.Menu_Play_But.Width / 2, Screen.HEIGHT - (Resources.Menu_Play_But.Height + 200))
        {
            this.manager = manager;
        }

        public override void Press(System.Windows.Forms.MouseEventArgs e)
        {
            manager.ChangeState(new Main_State(manager));
        }

    }
}
