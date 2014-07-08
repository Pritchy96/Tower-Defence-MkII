using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Tower_Defence.Util;

namespace Tower_Defence.States.Ingame.Towers
{
    public class Tow_Basic : Tower
    {
        public Tow_Basic(Bitmap baseTexture, Bitmap upgradedTexture, Bitmap bulletTexture, Vector2 position)
            : base(baseTexture, upgradedTexture, bulletTexture, position)    //Inheriting the Tower class & providing it's constructors.
        {
            //setting range, cost, damage.
            this.damage = 1;
            this.cost = 15;
            this.range = 120;
            this.RoF = 100;

            base.bulletTimer.Elapsed += Fire;
        }

        public override void Update()
        {
            for (int i = 0; i < bulletList.Count; i++)
            {
                Bullet bullet = bulletList[i];

                //'bending' bullets toward enemys.
                bullet.SetRotation(rotation);
                bullet.Update();

                //If the bullet is out of the range of the tower, kill it.
                if (!IsInRange(bullet.Center))
                    bullet.Kill();

                //Does the bullet get close enough to the enemy to consider it a hit?
                if (target != null && Vector2.Distance(bullet.Center, target.Center) < 12)
                {
                    //if so, damage the enemy and destroy the bullet.
                    target.CurrentHealth -= bullet.Damage;
                    bullet.Kill();
                }
                // Removing bullet from the game. But not really.
                if (bullet.isDead())
                {
                    bulletList.Remove(bullet);
                    i--;
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
                //create a bullet at the centre of the tower.
                Bullet bullet = new Bullet(bulletTexture, center -
                    new Vector2(bulletTexture.Width / 2), rotation, 20, damage);

                //Add bullet to list.
                bulletList.Add(bullet);
            }
        }

    }
}



