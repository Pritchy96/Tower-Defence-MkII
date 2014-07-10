using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using Tower_Defence.Properties;
using Tower_Defence;

    class MenuState : BasicState
    {
        //Positioning Rectangle for play button.
        Rectangle playButton = new Rectangle(
            Screen.WIDTH / 2 - (Resources.Menu_Play_But.Width / 2), 
            Screen.HEIGHT - (Resources.Menu_Play_But.Height + 200),
            Resources.Menu_Play_But.Width, Resources.Menu_Play_But.Height);

        public MenuState(Manager manager) : base(manager) 
        { 
        }

        public override void Update()
        {
        }

        public override void Redraw(PaintEventArgs e)
        {
            e.Graphics.DrawImage(Resources.Menu_Background, new Rectangle(0, 0, Screen.WIDTH, Screen.HEIGHT));
            e.Graphics.DrawImage(Resources.Menu_Play_But, playButton);
        } 

        public override void MouseMoved(MouseEventArgs e)
        {
        }

        public override void MouseClicked(MouseEventArgs e)
        {
            //Play button clicking
            if (playButton.Contains(e.Location))
            {
                base.manager.CurrentState = new MainState(base.manager);
            }
        }
    }

