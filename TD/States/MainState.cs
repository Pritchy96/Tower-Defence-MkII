using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tower_Defence;

    class MainState : BasicState
    {
        public float speedCoef = 1;
        Level level = new Level();  //DEBUG

        public MainState(Manager manager) : base(manager) { }

        public override void Update()
        {
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
        }
    }

