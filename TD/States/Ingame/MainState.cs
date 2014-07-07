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
        Tow_Slow test;  //DEBUG

        public MainState(Manager manager) : base(manager) 
        {
            player = new Player(level);
            waveManager = new WaveManager(level, 10, Resources.En_Basic, Resources.Health_Bar, player);
            test = new Tow_Slow(Resources.Tow_Slow, Resources.Tow_Slow, Resources.Bul_Basic, new Vector2(40, 240));
            player.addTowerToList(test);
        }

        public override void Update()
        {
            waveManager.Update();
            player.Update(waveManager.Enemies);
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

