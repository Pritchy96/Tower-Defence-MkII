using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tower_Defence;
using Tower_Defence.Properties;
using Tower_Defence.States.Ingame;
using Tower_Defence.States.Ingame.Towers;
using Tower_Defence.Util;

    class MainState : BasicState
    {
        public float speedCoef = 1;
        Level level = new Level();  //DEBUG
        Player player;
        WaveManager waveManager;
        Tow_Basic test;  //DEBUG

        public MainState(Manager manager) : base(manager) 
        {
            player = new Player(level);
            waveManager = new WaveManager(level, 10, Resources.En_Basic, Resources.Health_Bar, player);
            test = new Tow_Basic(Resources.Tow_Basic, Resources.Tow_Basic, Resources.Bul_Basic, new Vector2(40, 240));
        }

        public override void Update()
        {
            waveManager.Update();
            test.Update();
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
            waveManager.Draw(e);
            test.Draw(e);
        }
    }

