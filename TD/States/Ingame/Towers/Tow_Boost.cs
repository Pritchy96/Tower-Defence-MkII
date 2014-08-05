using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Tower_Defence.Properties;
using Tower_Defence.Util;

namespace Tower_Defence.States.Ingame.Towers
{
    public class Tow_Boost : Tower
    {
        private List<Tower> allTowers;
        private List<Tower> effectedTowers = new List<Tower>();
        private float damageBoost = 1.2f;
        private float rangeBoost = 1.1f;
        private float rofBoost = 1.5f;
        private int towerCount = 0; //COuntiung number of towers to know when one has been added or removed.

        //For placing towers: No position
        public Tow_Boost(List<Tower> towers)
            : base(Resources.Tow_Boost, Resources.Tow_Boost_Upgrade, Resources.Bul_Basic)
        {
            //setting range, cost, damage.
            this.damage = 0;
            this.cost = 15;
            this.range = 120;
            this.RoF = 1;
            base.BulletTimer.Stop();
            this.allTowers = towers;

            
        }

        //Triggers when tower is placed.
        public override void OnPlace()
        {
            AddBoosts();
            try
            {
                Center = new Vector2(Position.X + (Texture.Width / 2), Position.Y + (Texture.Height / 2));
            }
            catch (InvalidOperationException) { }   //Variable is currently being used, will have to wait.

            try
            {
                Bounds = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height); //Rect to fit around the Sprite.
            }
            catch (InvalidOperationException) { }   //Variable is currently being used, will have to wait.
        }

        //Triggers when tower is Removed.
        public override void OnRemove()
        {
            RemoveBoosts();
        }

        public override void Upgrade()
        {
            RemoveBoosts();
            damageBoost *= 1.2f;
            rangeBoost *= 1.2f;
            rofBoost /= 1.2f;
            AddBoosts();

            base.Upgrade();
        }

        public void RemoveBoosts()
        {
            foreach (Tower t in effectedTowers)
            {
                //Return towers to normal
                t.Damage /= damageBoost;
                t.Range /= rangeBoost;
                t.RoF *= rofBoost;
            }
            //Remove all towers that used to be effected.
            effectedTowers.Clear();
        }

        private void AddBoosts()
        {
            foreach (Tower t in allTowers)
            {
                //Calculating a rectangle from the range, to scale the radius texture to it.
                Vector2 radiusPosition = new Vector2(this.Position.X + 20, this.Position.Y + 20) - new Vector2(this.Range);

                Rectangle radiusRect = new Rectangle(
                (int)radiusPosition.X,
                (int)radiusPosition.Y,
                (int)this.Range * 2,
                (int)this.Range * 2);

                if (radiusRect.Contains((Point)t.Position) && !t.Equals(this))
                {
                    effectedTowers.Add(t);
                    t.Damage *= damageBoost;
                    t.Range *= rangeBoost;
                    t.RoF /= rofBoost;
                }
            }
        }

        public override void Fire(Object source, ElapsedEventArgs e)
        {
            //Does not fire.
        }

        public override void Update()
        {
            if (towerCount != allTowers.Count)
            {
                   RemoveBoosts();
                   AddBoosts();
                   towerCount = allTowers.Count();
            }

           //base.Update();    No need as tower is not moving
        }

        public override void Redraw(System.Windows.Forms.PaintEventArgs e)
        {
            foreach (Tower t in effectedTowers)
            {
                //Draw little graphic to show tower is effected.
            }

            base.Redraw(e);
        }
    }
}



