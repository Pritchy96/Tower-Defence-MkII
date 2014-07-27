using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


    public partial class Screen : Form
    {
        #region Variables
        //Screen size.
        public static int HEIGHT = 700;
        public static int WIDTH = 800;

        //Thread Variables.
        Boolean running = false;
        Thread thread = null;

        public Manager manager = new Manager();
        #endregion

        #region Function Explanation
        //Constructor, sets Screen size and then begins Thread.
        #endregion
        public Screen()
        {
            InitializeComponent();
            SetScreenSize(WIDTH, HEIGHT);
            BeginThread();
        }

        #region Events
        private void OnExit(object sender, FormClosingEventArgs e)
        {
            killThread();
        }

        private void MouseClick(object sender, MouseEventArgs e)
        {
            manager.MouseClicked(e);
        }

        private void MouseMoved(object sender, MouseEventArgs e)
        {
            manager.MouseMoved(e);
        }

        public void KeyIsPress(object sender, KeyPressEventArgs e)
        {
            manager.KeyPress(e);
        }

        public void KeyIsDown(object sender, KeyEventArgs e)
        {
            manager.KeyDown(e);
        }

        public void KeyIsUp(object sender, KeyEventArgs e)
        {
            manager.KeyUp(e);
        }

        private void Redraw(object sender, PaintEventArgs e)
        {
            manager.Redraw(e);
        }
        #endregion

        #region Function Explanation
        //Creates and starts a Thread.
        #endregion
        public void BeginThread()
        {
            thread = new Thread(new ThreadStart(Update));
            thread.Start();
            running = true;
        }

        #region Function Explanation
        //Kills the thread.
        #endregion
        public void killThread()
        {
            //Simply kills off the Thread.
            running = false;
            thread.Abort();
            thread.Join();
        }

        #region Function Explanation
        //The main Update loop. Basically just updates Manager which handles
        //all Game updates.
        #endregion
        public void Update()
        {
            while (running)
            {
                manager.Update();
                
                //Cause screen to redraw.
                DrawScreen.Invalidate();

                //Basic Thread Slowing;
                Thread.Sleep(20);
            }
        }

        #region Function Explanation
        //Repaints Manager.
        #endregion
        private void Repaint(object sender, PaintEventArgs e)
        {
            manager.Redraw(e);
        }

        #region Function Explanation
        //Screen Size setting method with adjustments made for screen Border.
        #endregion
        private void SetScreenSize(int width, int height)
        {
            this.Width = width + 6;
            this.Height = height + 28;
        }
    }

