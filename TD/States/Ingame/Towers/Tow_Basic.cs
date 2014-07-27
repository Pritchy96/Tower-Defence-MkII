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
    public class Tow_Basic : Tower
    {
        //For placing towers: No position
        public Tow_Basic()
            : base(Resources.Tow_Basic, Resources.Tow_Basic_Upgrade, Resources.Bul_Basic)    //Inheriting the Tower class & providing it's constructors.
        {
            //setting range, cost, damage.
            this.damage = 1;
            this.cost = 15;
            this.range = 120;
            this.RoF = 100;

            base.BulletTimer.Elapsed += Fire;
        }

        public override void Update()
        {
            foreach (Bullet b in bulletList.ToList())
            {
                //'bending' bullets toward enemys.
                b.SetRotation(rotation);
                b.Update();

                //If the bullet is out of the range of the tower, kill it.
                if (!IsInRange(b.Center))
                    b.Kill();

                //Does the bullet get close enough to the enemy to consider it a hit?
                if (target != null && Vector2.Distance(b.Center, target.Center) < 12)
                {
                    //if so, damage the enemy and destroy the bullet.
                    target.CurrentHealth -= b.Damage;
                    b.Kill();
                }
                // Removing bullet from the game. But not really.
                if (b.isDead())
                {
                    bulletList.Remove(b);
                }
            }
            base.Update();
        }

        public override void Upgrade()
        {
            damage *= 2f;
            range *= 1.1f;
            base.Upgrade();
        }

        public override void Fire(Object source, ElapsedEventArgs e)
        {
            //If we have a target..
            if (target != null)
            {
                lock (center)
                {
                    //create a bullet at the centre of the tower.
                    Bullet bullet = new Bullet(bulletTexture, center -
                        new Vector2(bulletTexture.Width / 2), rotation, 20, damage);


                    //Add bullet to list.
                    bulletList.Add(bullet);
                }
            }
        }

    }
}



