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

        private Queue<Vector2> waypoints = new Queue<Vector2>();
        //Variables used by all instances of Enemy.
        protected float startHealth;
        protected float currentHealth;
        protected bool alive = true;
        protected float speed = 2f;
        protected int bountyGiven;
        //Slowing enemies
        private float speedModifier = 1f;
        public System.Timers.Timer speedModifierTimer = new System.Timers.Timer();

        //Getters & Setters
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
                //Pythagoras to find distance from enemy position to next waypoint.
                return (float)Math.Sqrt(((Math.Pow((position.X - waypoints.Peek().X), 2f)) + (Math.Pow((position.Y - waypoints.Peek().Y), 2f))));
            }
        }

        public float SpeedModifier
        {
            get { return speedModifier; }
            set { speedModifier = value; }
        }

        public float SpeedModifierDuration
        {
            get { return (float)speedModifierTimer.Interval; }
            set
            {
                speedModifierTimer.Interval = value;
            }
        }

        // % Health.
        public float HealthPercentage
        {
            get { return (currentHealth / startHealth) * 100; }
        }

        //Constructor
        public Enemy(Bitmap texture, Vector2 position, float health, int bountyGiven, float speed)
            : base(texture, position)
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

            this.position = this.waypoints.Dequeue();
        }

        //Update loop.
        public override void Update()
        {
            base.Update();

            //If there is more than one waypoint left in the ememys path.
            if (waypoints.Count > 0 && alive)
            {
                //If the distance to waypoint is less than it's speed, place it at the waypoint and remove it from the queue?
                if (DistanceToDestination < speed)
                {
                    position = waypoints.Peek();
                    waypoints.Dequeue();
                }
                //Or if the distance to waypoint is greater than speed...
                else
                {
                    Vector2 direction = waypoints.Peek() - position;
                    direction.Normalise();

                    //Store the original speed of the enemy.
                    float temporarySpeed = speed;

                    //Modify enemy speed
                    temporarySpeed *= speedModifier;

                    //Calculate a vector for the enemy to move by.
                    velocity = Vector2.Multiply(direction, temporarySpeed);

                    position += velocity;
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

        //Resets the speed modifier Coefficient to a neutral 1;
        public void ResetSpeedModifier(object source, ElapsedEventArgs e)
        {
            speedModifier = 1f;
            speedModifierTimer.Stop();
        }

        //Drawing enemy.
        public override void Draw(PaintEventArgs e)
        {
            if (alive)
            {
                base.Draw(e);
            }
        }
    }

}






/*
        //Tinting the Enemys colour when damaged.
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                float healthPercentage = (float)currentHealth
                     / (float)startHealth;

                Color color = new Color(new Vector3(1 - healthPercentage, 
                    1 - healthPercentage, 1 - healthPercentage));

                base.Draw(spriteBatch, color);
            }
        }
*/