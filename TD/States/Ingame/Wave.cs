using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Tower_Defence
{
    public class Wave
    {
        private int numOfEnemies;   //Number of Enemies to spawn.
        private int waveNumber; //Wave number.
        private int health; //Enemies health.
        private int cashDrop; //How much money a creep drops.
        private int enemiesSpawned = 0;  //How many enimies have spawned.
        private System.Timers.Timer spawnTimer = new System.Timers.Timer(); //Timer to set time between creep spawns during a wave.

        private bool enemyAtEnd; //Has an enemy reached the end of the path?
       // private bool spawningEnemies; //Are we still spawning enemies?
        private Level level; //Level reference.
        private Player player; //Level reference.
        private Bitmap enemyTexture; //Texture for the enemy.
        private Bitmap healthTexture; //a Texture for the health bar.
        public List<Enemy> enemies = new List<Enemy>(); //List of Enemies


        //Getters and Setters.
        public bool RoundOver
        {
            get
            {
                //Check that all enemies have been spawned, and all have been killed.
                return enemies.Count == 0 && enemiesSpawned == numOfEnemies;
            }
        }

        public int RoundNumber
        {
            get { return waveNumber; }
        }

        public bool EnemyAtEnd
        {
            get { return enemyAtEnd; }
            set {enemyAtEnd = value; }  //?
        }

        public List<Enemy> Enemies
        {
            get {return enemies;}
        }


        //Constructor.
        public Wave(int waveNumber, int numOfEnemies, int health, int cashDrop, Level level,
            Bitmap enemyTexture, Bitmap healthTexture, Player player)
        {
            this.waveNumber = waveNumber;
            this.numOfEnemies = numOfEnemies;

            //referencing player & level.
            this.player = player;
            this.level = level;

            //Setting the parameters passed by waveManager to this classes variables.
            this.enemyTexture = enemyTexture;
            this.healthTexture = healthTexture;
            this.health = health;
            this.cashDrop = cashDrop;

            //Spawntimer setup
            //Time inbetween spawns (500 ms)
            spawnTimer.Interval = 500;
            //Subscribing to the spawn event.
            spawnTimer.Elapsed += Spawn;

            spawnTimer.Start(); //DEBUG
        }


        //Beginning the wave.
        public void Start()
        {
            spawnTimer.Start();
        }


        //Adding the enemies.
        private void AddEnemy()
        {
            //Actually adding the enemy.
            Enemy enemy = new Enemy(enemyTexture, level.Waypoints.Peek(), health, cashDrop, 2f);
            //Set the waypoint of the enemy, so it knows where to go.
            enemy.SetWaypoints(level.Waypoints);
            //Add enemy to list.
            enemies.Add(enemy);
            enemiesSpawned++;   
        }

        //Update method.
        public void Update()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];   //?
                enemy.Update();


                if (enemy.IsDead)
                {
                    //If the enemy is dead but has health, it must be at the end of the path.
                    if (enemy.CurrentHealth > 0)
                    {
                        enemyAtEnd = true;
                        player.Lives -= 1;
                    }
                    //Otherwise, we've killed it! Give some money!
                    else
                    {
                        player.Money += enemy.BountyGiven;
                    }

                    enemies.Remove(enemy);
                    i--;
                }
            }
        }


        private void Spawn(Object source, ElapsedEventArgs e)
        {
            //Check to see if we have completed spawning a wave.
            if (enemiesSpawned == numOfEnemies)
            {
                spawnTimer.Stop();

            }
            else
            {
                AddEnemy();
            }
        }


        public void Draw(PaintEventArgs e)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(e);

                //Drawing the background of the healthbar.
                Rectangle healthRectangle = new Rectangle((int)enemy.Position.X,
                    (int)enemy.Position.Y, healthTexture.Width, healthTexture.Height);


                //Need to work out how to do tinting
                /*
                e.Graphics.DrawImage(healthTexture, healthRectangle, Color.Gray);

                //Drawing the foreground of the Healthbar: the actual Health of the Enemy.
                float healthPercentage = enemy.HealthPercentage;
                float visableWidth = (float)healthTexture.Width * (healthPercentage / 100);

                healthRectangle = new Rectangle((int)enemy.Position.X,
                    (int)enemy.Position.Y, (int) visableWidth, healthTexture.Height);

                //Dark magic to get fading healthbar.
                float red = (healthPercentage < 50 ? 100 : 100 - (2 * healthPercentage - 100));
                float green = (healthPercentage > 50 ? 100 : (2 * healthPercentage - 100));

                //Creating the colour blend and then drawing the forground of the healthbar.
                Color healthColor = new Color(red, green, 0);
                spriteBatch.Draw(healthTexture, healthRectangle, healthColor);
                 */
            }
                                                                  
        }
    }
}




        