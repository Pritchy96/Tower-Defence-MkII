using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Tower_Defence.Util;


namespace Tower_Defence
{
    public class Enemy : Sprite
    {
        #region Variables
        private Queue<Vector2> waypoints = new Queue<Vector2>();
        //Variables used by all instances of Enemy.
        protected float startHealth;
        protected float currentHealth;
        protected bool alive = true;
        protected float speed = 2f;
        protected int bountyGiven;
        //Slowing enemies
        private float speedCoef = 1f;   //Speed Coefficient
        public System.Timers.Timer speedModifierTimer = new System.Timers.Timer();  //Timer for speed change
        #endregion

        #region Properties
        public float CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        public bool IsDead
        {
            get { return !alive; }
        }

        public int BountyGiven
        {
            get { return bountyGiven; }
        }

        //Distance from next Waypoint.
        public float DistanceToDestination
        {
            get
            {
                //Pythagoras to find distance from enemy Position to next waypoint.
                return (float)Math.Sqrt(((Math.Pow((Position.X - waypoints.Peek().X), 2f)) + (Math.Pow((Position.Y - waypoints.Peek().Y), 2f))));
            }
        }

        public float SpeedCoef
        {
            get { return speedCoef; }
            set { speedCoef = value; }
        }

        public float SpeedModifierDuration
        {
            get { return (float)speedModifierTimer.Interval; }
            set
            {
                speedModifierTimer.Interval = value;
            }
        }

        public float HealthPercentage
        {
            get { return (currentHealth / startHealth) * 100; }
        }
        #endregion

        public Enemy(Bitmap texture, Vector2 Position, float health, int bountyGiven, float speed)
            : base(texture, Position)
        {
            this.startHealth = health;
            this.currentHealth = startHealth;
            this.bountyGiven = bountyGiven;
            this.speed = speed;
            speedModifierTimer.Elapsed += ResetSpeedModifier;
        }

        //Getting the Waypoints from Level class.
        public void SetWaypoints(Queue<Vector2> waypoints)
        {
            foreach (Vector2 waypoint in waypoints)
                this.waypoints.Enqueue(waypoint);

            this.Position = this.waypoints.Dequeue();
        }

        public override void Update()
        {
            base.Update();

            //If there is more than one waypoint left in the ememys path.
            if (waypoints.Count > 0 && alive)
            {
                //If the distance to waypoint is less than it's speed, place it at the waypoint and remove it from the queue?
                if (DistanceToDestination < speed)
                {
                    Position = waypoints.Peek();
                    waypoints.Dequeue();
                }
                //Or if the distance to waypoint is greater than speed...
                else
                {
                    Vector2 direction = waypoints.Peek() - Position;
                    direction.Normalise();

                    //Store the original speed of the enemy.
                    float temporarySpeed = speed;

                    //Modify enemy speed
                    temporarySpeed *= speedCoef;

                    //Calculate a vector for the enemy to move by.
                    Velocity = Vector2.Multiply(direction, temporarySpeed);

                    Position += Velocity;
                }
                //If there are no more waypoints, it must be at the end of it's path, therefore remove it from the game/screen.
            }
            else
            {
                alive = false;
            }


            if (currentHealth <= 0)
            {
                alive = false;
            }
        }

        public void Slow(float speedCoef, float modifierDuration)
        {
            this.SpeedCoef = speedCoef;
            speedModifierTimer.Interval = modifierDuration;
            speedModifierTimer.Start();
        }

        //Resets the speed modifier Coefficient to a neutral 1;
        public void ResetSpeedModifier(object source, ElapsedEventArgs e)
        {
            speedCoef = 1f;
            speedModifierTimer.Stop();
        }

        public override void Redraw(PaintEventArgs e)
        {
            if (alive)
            {
                base.Redraw(e);
            }
        }
    }
}


/*
        //Tinting the Enemys colour when damaged.
        public override void Redraw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                float healthPercentage = (float)currentHealth
                     / (float)startHealth;

                Color color = new Color(new Vector3(1 - healthPercentage, 
                    1 - healthPercentage, 1 - healthPercentage));

                base.Redraw(spriteBatch, color);
            }
        }
*/