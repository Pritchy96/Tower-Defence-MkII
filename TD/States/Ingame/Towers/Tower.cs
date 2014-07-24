using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Tower_Defence.Properties;
using Tower_Defence.Util;

namespace Tower_Defence.States.Ingame
{
    public abstract class Tower : Sprite
    {
        #region Variables
        //Qualities of the towers.
        protected int cost;
        protected float damage;
        protected float range;
        protected System.Timers.Timer bulletTimer = new System.Timers.Timer(); //Time since bullet was fired. this.Interval is ROF.
        protected Enemy target;  //Current Enemy object which is being targeted. 
        protected bool selected;    //Is the tower clicked?
        private bool placed = false;
        protected int maxLevel = 5;     //Maximum level of the tower.
        protected int upgradeLevel = 0;     //The actual level the tower is
        protected float upgradeAlphaAmount = 0.5f;    //How transparent the colour overlay should be (1 = fully upgraded!)
        protected int upgradeTotal;
        protected List<Bullet> bulletList = new List<Bullet>();    //List of bullets.

        protected Bitmap bulletTexture;  //Texture of the towers bullet.
        protected Bitmap upgradedTower;    //Texture for the upgraded tower (for the overlay, drawn here)
        #endregion

        #region Properties

        public int Cost
        {
            get { return cost; }
        }

        public float Damage
        {
            get { return damage; }
        }

        public float Range
        {
            get { return range; }
        }

        public float RoF
        {
            get { return (float)bulletTimer.Interval; }
            set { bulletTimer.Interval = value; }
        }

        public Enemy Target
        {
            get { return target; }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public bool Placed
        {
            get { return placed; }
            set { placed = value; }
        }

        public virtual bool HasTarget   //Check if the tower has a target.
        {
            get { return target != null; }
        }

        public int UpgradeLevel     //Returns tower level.
        {
            get { return upgradeLevel; }
        }

        public int MaxLevel     //Returns max tower level.
        {
            get { return maxLevel; }
        }

        public int UpgradeTotal  //Returns & sets how much has been spent on tower.
        {
            get { return upgradeTotal; }
            set { upgradeTotal = value; }
        }

        public System.Timers.Timer BulletTimer
        {
            get { return bulletTimer; }
        }

        #endregion

        public Tower(Bitmap baseTexture, Bitmap upgradedTower, Bitmap bulletTexture)
            : base(baseTexture)
        {
            this.bulletTexture = bulletTexture;
            this.upgradedTower = upgradedTower;
            bulletTimer.Start();
        }

        //Update loop
        public override void Update()
        {
            base.Update();

            if (target != null)
            {
                if (!target.IsDead)
                    FaceTarget();

                //If the target is out of range or dead...
                if (!IsInRange(target.Center) || target.IsDead)
                {
                    //set target to nothing(null)
                    target = null;

                    //and restart the bullet timer. This seems to break slower shooting towers.
                    //bulletTimer = 0;
                }
            }
        }

        //Check to see if the passed position is within range.
        public bool IsInRange(Vector2 position)
        {
            if (Vector2.Distance(center, position) <= range)
                return true;
            else
                return false;
        }

        //Gets the enemy closest to the tower.
        public virtual void GetClosestEnemy(List<Enemy> enemies)
        {

            target = null;
            float smallestRange = range;

            //Loops through the enemies, if the current enemy is closer to the tower
            //than the last closest enemy, that is Set as the target. 
            foreach (Enemy enemy in enemies.ToList())
            {
                    if (Vector2.Distance(center, enemy.Center) < smallestRange)
                    {
                        smallestRange = Vector2.Distance(center, enemy.Center);
                        target = enemy;
                    }
            }
        }

        //Upgrading the tower (visually, the actual changes are in the sub classes)
        public virtual void Upgrade()
        {
            //calculating upgrade cost (add one because we are checking for the NEXT level)
            int upgradeCost = cost * (upgradeLevel + 1);
            //Updates the level by 1
            upgradeLevel++;
            //Increases the alpha of the colour overlay, the visual representation of the
            //towers level (drawn in this class!) by 0.2.
            upgradeAlphaAmount = float.Parse((upgradeLevel * 0.2).ToString());
        }

        //Evil Maths stuff to rotate the image to face the enemy.
        protected void FaceTarget()
        {
            Vector2 direction = center - target.Center;
            direction.Normalise();

            rotation = (float)Math.Atan2(-direction.X, direction.Y);
        }

        //Ensures the sub classes have a fire method (So I don't forget :D)
        public abstract void Fire(Object source, ElapsedEventArgs e);

        //Drawing.
        public override void Redraw(PaintEventArgs e)
        {
            foreach (Bullet bullet in bulletList.ToList())
            {
                bullet.Redraw(e);
            }

           // base.Redraw(e);

            Bitmap overlay = new Bitmap(upgradedTower);
            //Drawing the colour Upgrade Overlay TODO
            overlay = RotateImage(overlay, (float)(rotation * (180 / Math.PI)), false, true, Color.Transparent);
            SetImageOpacity(overlay, upgradeAlphaAmount);
            
            e.Graphics.DrawImage(overlay, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height));
            // spriteBatch.Redraw(upgradedTower, center, null, Color.White * upgradeAlphaAmount, rotation, origin, 1.0f, SpriteEffects.None, 0);
        }
    }
}



