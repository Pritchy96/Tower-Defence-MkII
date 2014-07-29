using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tower_Defence.Util;

namespace Tower_Defence.States.Ingame
{
    public class Bullet : Sprite
    {
        private float damage;
        private int age;
        private int speed;

        //Getters and setters.
        public float Damage
        {
            get { return damage; }
        }

        //if age is > 100, dead = true
        public bool isDead()
        {
            return age > 100;
        }

        //Constructor
        public Bullet(Bitmap Texture, Vector2 position, float rotation, int speed, float damage)
            : base(Texture, position)
        {
            this.Rotation = rotation;
            this.damage = damage;
            this.speed = speed;
        }

        //Another constuctor, taking a velocity instead of getting one from a Rotation.
        public Bullet(Bitmap Texture, Vector2 position, Vector2 velocity, int speed, float damage)
            : base(Texture, position)
        {
            this.damage = damage;
            this.speed = speed;
            this.Velocity = velocity * speed;
        }

        public override void Update()
        {
            //Ages the bullet, so it does not live forever, if it misses its target.
            age++;

            Position += Velocity; //Changes position by adding the calculated velocity to it.  

            base.Update();
        }

        public void SetRotation(float value)
        {
            Rotation = (float)(value * (180/Math.PI));

            //Transform rotates the vector speed to have the same Rotation as our tower.
     //       velocity = Vector2.Transform(new Vector2(0, -speed),
      //          Matrix.CreateRotationZ(Rotation));

              #region I seriously doubt this will work.
            Matrix matrix = new Matrix();

            /*Rotates an empty matrix by the Rotation. I hope this will work. Chances are it will just rotate (0, 0, 0) to give
             * (0, 0, 0). If so, we may have to create a matrix from the speed vector and then rotate that, and finally convert back.
             * According to http://msdn.microsoft.com/en-us/library/microsoft.xna.framework.vector2.transform.aspx, transform makes the vector (point)
             * a matric of (x, y, 0, 1) anyway. Maybe we can use the x and y of the vector as the x and y then rotate it?
             * */
            matrix.Rotate(Rotation);

            //Transforms the vector by the matrix we created. We have to supply it an array of points, so this may not work.
            Point[] transformArray = new Point[] { new Point(0, -speed) };
            matrix.TransformVectors(transformArray);

            Velocity = new Vector2(transformArray[0].X, transformArray[0].Y);
            #endregion
        }

        //kills the bullet.
        public void Kill()
        {
            this.age = 200;
        }
    }
}


    
