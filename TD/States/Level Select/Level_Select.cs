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

    class Level_Select : Basic_State
    {
        private List<Level_But> levelButtons = new List<Level_But>();

        public Level_Select(Manager manager)
            : base(manager) 
        {  
        }

        public override void CreateGUI()
        {  
            //We use the .initialise method so that it positions itself correctly (needs to be placed before all buttons are added.
            levelButtons.Add(new Level_But(manager, Resources.Level_1));
            levelButtons.Add(new Level_But(manager, Resources.Level_2));
            levelButtons.Add(new Level_But(manager, Resources.Level_3));
            levelButtons.Add(new Level_But(manager, Resources.Level_4));
            levelButtons.Add(new Level_But(manager, Resources.Level_5));

            for (int i = 1; i <= levelButtons.Count(); i++)
            {
                levelButtons[i - 1].Initialise(i);
                manager.Buttons.Add(levelButtons[i - 1]);
            }
        }

        public override void Update()
        {
        }

        public override void Redraw(PaintEventArgs e)
        {
            e.Graphics.DrawImage(Resources.Level_Select_Screen, new Rectangle(0, 0, Screen.WIDTH, Screen.HEIGHT));
        } 

        public override void MouseMoved(MouseEventArgs e)
        {
        }

        public override void MouseClicked(MouseEventArgs e)
        {

        }
    }

