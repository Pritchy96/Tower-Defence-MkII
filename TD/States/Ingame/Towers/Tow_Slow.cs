﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Tower_Defence.Util;

namespace Tower_Defence.States.Ingame.Towers
{
    public class Tow_Slow : Tower
    {
        //How fast an enemy will move when hit.
        private float speedModifier;
        //How long the effect will last.
        private float modifierDuration;

        //Constructor.
        public Tow_Slow(Bitmap baseTexture, Bitmap upgradedTexture, Bitmap bulletTexture, Vector2 position)
            : base(baseTexture, upgradedTexture, bulletTexture, position)    //Inheriting the Tower class & providing it's constructors.
        {
            //setting range, cost, damage.
            this.damage = 0;
            this.cost = 30;
            this.range = 80;
            this.RoF = 500f;
            this.speedModifier = 0.2f;
            this.modifierDuration = 1000f;

            base.bulletTimer.Elapsed += fire;
        }

        //Upgrading the towers values.
        public override void Upgrade()
        {
            damage *= 2f;
            range *= 1.1f;
            base.Upgrade();
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

                    //If the speed modifier is better than anything affecting the target, apply it.
                    if (target.SpeedModifier <= speedModifier)
                    {
                        target.SpeedModifier = speedModifier;
                        target.SpeedModifierDuration = modifierDuration;
                    }

                    //if so, damage and slow the enemy, and destroy the bullet.
                    target.CurrentHealth -= bullet.Damage;
                    target.Slow(speedModifier, modifierDuration);
                    bullet.Kill();
                }
                // Removing bullet from the game. But not really.
                if (bullet.isDead())
                {
                    bulletList.Remove(bullet);
                    i--;
                }
            }

            #region Only targetting enemies without a modifier. TEMP
            
            if (target != null)
            {
                if (target.SpeedModifier != 1)
                {
                    target = null;
                }
            }
             
            #endregion

            base.Update();
        }

        public override void fire(Object source, ElapsedEventArgs e)
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