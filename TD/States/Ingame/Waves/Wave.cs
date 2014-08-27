using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Tower_Defence.Properties;
using Tower_Defence.States.Ingame;

namespace Tower_Defence
{
    public class Wave
    {
        #region Variables
        private int numOfEnemies;   //Number of Enemies to spawn.
        private int enemiesSpawned = 0;  //How many enimies have spawned.
        private int waveNumber;

        private int health; //Enemies health when spawned.
        private int cashDrop; //How much money a creep drops.
        private Bitmap enemyTexture;
        private Bitmap healthTexture = Resources.Health_Bar;

        public System.Timers.Timer spawnTimer = new System.Timers.Timer(); //Timer to set time between creep spawns during a wave.
        private bool enemyAtEnd; //Has an enemy reached the end of the path?

        private Main_State mainState;
        private Wave_Manager waveManager;
        #endregion

        #region Properties

        public int RoundNumber
        {
            get { return waveNumber; }
        }


        #endregion

        public Wave(Wave_Manager waveManager, Main_State mainState, int waveNumber, int numOfEnemies, int health, int cashDrop,
            Bitmap enemyTexture)
        {
            this.waveNumber = waveNumber;
            this.numOfEnemies = numOfEnemies;

            this.mainState = mainState;
            this.waveManager = waveManager;

            //Setting the parameters passed by waveManager to this classes variables.
            this.enemyTexture = enemyTexture;
            this.health = health;
            this.cashDrop = cashDrop;

            //Spawntimer setup
            //Time inbetween spawns (500 ms)
            spawnTimer.Interval = 500;
            //Subscribing to the spawn event.
            spawnTimer.Elapsed += Spawn;
        }

        public void Update()
        {
        
        }

        public void Start()
        {
            spawnTimer.Start();
        }

        private void AddEnemy()
        {
            //Actually adding the enemy.
            Enemy enemy = new Enemy(enemyTexture, mainState.level.Waypoints.Peek(), health, cashDrop, 2f);
            //Set the waypoint of the enemy, so it knows where to go.
            enemy.SetWaypoints(mainState.level.Waypoints);
            //Add enemy to list.
            waveManager.enemies.Add(enemy);
            enemiesSpawned++;
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
    }
}




