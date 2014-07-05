using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tower_Defence;
using Tower_Defence.Properties;

    class MainState : BasicState
    {
        public float speedCoef = 1;
        Level level = new Level();  //DEBUG
        Player player;
        Wave wave;

        public MainState(Manager manager) : base(manager) 
        {
            player = new Player(level);
            wave = new Wave(10, 10, 10, 10, level, Resources.En_Basic, Resources.En_Basic, player);
        }

        public override void Update()
        {
            wave.Update();
        }

        public override void MouseMoved(MouseEventArgs e)
        {
        }

        public override void MouseClicked(MouseEventArgs e)
        {
        }

        public override void Redraw(PaintEventArgs e)
        {
            level.Draw(e);
            wave.Draw(e);
        }
    }

