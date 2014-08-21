﻿using System;
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
        #region Variables
        private Main_State mainState;
        private int numOfEnemies;   //Number of Enemies to spawn.
        private int waveNumber; //Wave number.
        private int health; //Enemies health.
        private int cashDrop; //How much money a creep drops.
        private int enemiesSpawned = 0;  //How many enimies have spawned.
        public System.Timers.Timer spawnTimer = new System.Timers.Timer(); //Timer to set time between creep spawns during a wave.
        private bool enemyAtEnd; //Has an enemy reached the end of the path?
        private Bitmap enemyTexture; //Texture for the enemy.
        private Bitmap healthTexture; //a Texture for the health bar.
        public List<Enemy> enemies = new List<Enemy>(); //List of Enemies in the wave
        #endregion

        #region Properties
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
            set { enemyAtEnd = value; } 
        }

        public List<Enemy> Enemies
        {
            get { return enemies; }
        }
        #endregion

        public Wave(Main_State mainState, int waveNumber, int numOfEnemies, int health, int cashDrop,
            Bitmap enemyTexture, Bitmap healthTexture)
        {
            this.waveNumber = waveNumber;
            this.numOfEnemies = numOfEnemies;

            this.mainState = mainState;

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
        }

        public void Update()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];  
                enemy.Update();


                if (enemy.IsDead)
                {
                    //If the enemy is dead but has health, it must be at the end of the path.
                    if (enemy.CurrentHealth > 0)
                    {
                        enemyAtEnd = true;
                        mainState.Lives -= 1;
                    }
                    //Otherwise, we've killed it! Give some money!
                    else
                    {
                        mainState.Money += enemy.BountyGiven;
                    }

                    enemies.Remove(enemy);
                    i--;
                }
            }
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
            enemies.Add(enemy);
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

        public void Draw(PaintEventArgs e)
        {
            foreach (Enemy enemy in enemies.ToList())
            {
                enemy.Redraw(e);
            }

        }
    }
}




