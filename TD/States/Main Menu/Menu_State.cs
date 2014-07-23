using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using Tower_Defence.Properties;
using Tower_Defence;
using Tower_Defence.States.Main_Menu;

    class Menu_State : Basic_State
    {

        public Menu_State(Manager manager) : base(manager) 
        {
            manager.Buttons.Add(new GUI_Play_But(manager));
        }

        public override void Update()
        {
        }

        public override void Redraw(PaintEventArgs e)
        {
            e.Graphics.DrawImage(Resources.Menu_Background, new Rectangle(0, 0, Screen.WIDTH, Screen.HEIGHT));
        } 

        public override void MouseMoved(MouseEventArgs e)
        {
        }

        public override void MouseClicked(MouseEventArgs e)
        {

        }
    }

