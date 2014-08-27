using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Tower_Defence.Properties;

namespace Tower_Defence.States.Ingame
{
    public class Wave_Manager
    {
        #region Variables
        private int numberOfWaves; //How many waves in this level?
        private int currentWaveNum = 0; //How many waves are we up to?
        private Queue<Wave> waves = new Queue<Wave>(); //A queue to hold our waves.
        private System.Timers.Timer waveTimer = new System.Timers.Timer();  //Timer for waves
        private Bitmap enemyTexture = Resources.En_Basic; //Texture of enemy in the wave.
        private Main_State mainState; //Reference to level.
        public List<Enemy> enemies = new List<Enemy>(); //List of Enemies in the wave
        #endregion

        #region Properties.
        public Wave CurrentWave    //Get current wave in Queue
        {
            get { return waves.Peek(); }
        }
        public List<Enemy> Enemies //Get current enemy list
        {
            get { return enemies; }
        }
        public int Round //Round/Wave number.
        {
            get { return CurrentWave.RoundNumber + 1; }
        }
        #endregion

        public Wave_Manager(Main_State mainState, int numberOfWaves)
        {
            this.numberOfWaves = numberOfWaves;
            this.mainState = mainState;

            AddWaves();
            waveTimer.Interval = 10000;
            waveTimer.Elapsed += SendNextWave;
            waveTimer.Start();
        }

        private void AddWaves()
        {
            if (numberOfWaves == -1)    //Unlimited Waves.
            {
                #region Adding Waves.
                for (int i = currentWaveNum; i < currentWaveNum + 50; i++)
                {
                    int initalNumberOfEnemies = 6;
                    int initalHealth = 3;
                    int cashDrop = 2;

                    //Modifier to add 6 enemies, Every 6 waves.
                    //Is an integer so rounds down. (wave 1, 1/6 = 0 as it rounds down.)
                    int enemyNumberModifier = (i / initalNumberOfEnemies) + 1;

                    //Adds 3 to health every three waves.
                    int healthHumberModifier = (i / 1) + 2;

                    //Adds 2 to cashDrop every 6 waves.
                    int cashDropModifier = (i / 6) + 1;

                    //Initialising new wave.
                    Wave wave = new Wave(this, mainState, i, initalNumberOfEnemies * enemyNumberModifier, initalHealth * healthHumberModifier,
                        cashDrop * cashDropModifier, enemyTexture);

                    //Adding wave to Queue.
                    waves.Enqueue(wave);
                }
                #endregion
            }
            else
            {
                #region Adding Waves.
                for (int i = 0; i < numberOfWaves; i++)
                {
                    int initalNumberOfEnemies = 6;
                    int initalHealth = 3;
                    int cashDrop = 2;

                    //Modifier to add 6 enemies, Every 6 waves.
                    //Is an integer so rounds down. (wave 1, 1/6 = 0 as it rounds down.)
                    int enemyNumberModifier = (i / initalNumberOfEnemies) + 1;

                    //Adds 3 to health every three waves.
                    int healthHumberModifier = (i / 1) + 2;

                    //Adds 2 to cashDrop every 6 waves.
                    int cashDropModifier = (i / 6) + 1;

                    //Initialising new wave.
                    Wave wave = new Wave(this, mainState, i, initalNumberOfEnemies * enemyNumberModifier, initalHealth * healthHumberModifier,
                        cashDrop * cashDropModifier, enemyTexture);

                    //Adding wave to Queue.
                    waves.Enqueue(wave);
                }
                #endregion
            }
        }

        //Event
        private void SendNextWave(Object source, ElapsedEventArgs e)
        {
            waves.Dequeue();    //Remove finished wave.

            if (waves.Count > 0)    //If there are waves left.
            {
                waves.Peek().Start();   //Start the next wave.
            }
            else
            {
                NoMoreWaves();
            }
        }

        //Non event method
        public void SendNextWave()
        {
            waves.Dequeue();    //Remove finished wave.

            if (waves.Count > 0)    //If there are waves left.
            {
                waves.Peek().Start();   //Start the next wave.
                waveTimer.Stop();
                waveTimer.Start();
            }
            else
            {
                NoMoreWaves();
            }
        }

        private void NoMoreWaves()
        {
            if (enemies.Count() == 0)
            {
                if (numberOfWaves == -1)
                {
                    AddWaves(); //Infinite waves
                }
                else
                {
                    mainState.manager.ChangeState(new Menu_State(mainState.manager));
                }
            }
            else
            {
                NoMoreWaves();
            }
        }

        public void AdjustGameSpeed()
        {
            waveTimer.Interval /= Main_State.speedCoef;
            foreach (Wave w in waves)
                w.spawnTimer.Interval /= Main_State.speedCoef;
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

        public void Redraw(PaintEventArgs e)
        {
            foreach (Enemy enemy in enemies.ToList())
            {
                enemy.Redraw(e);
            }
        }
    }
}





/*
    public class Wave_Manager
    {
        #region Variables
        private int numberOfWaves; //How many waves in this level?
        private Queue<Wave> waves = new Queue<Wave>(); //A queue to hold our waves.
        private System.Timers.Timer waveTimer = new System.Timers.Timer();  //Timer for waves
        private Bitmap enemyTexture = Resources.En_Basic; //Texture of enemy in the wave.
        private Main_State mainState; //Reference to level.
        #endregion

        #region Properties.
        public Wave CurrentWave    //Get current wave in Queue
        {
            get { return waves.Peek(); }
        }
        public List<Enemy> Enemies //Get current enemy list
        {
            get { return CurrentWave.Enemies; }
        }
        public int Round //Round/Wave number.
        {
            get { return CurrentWave.RoundNumber + 1; }
        }
        #endregion

        public Wave_Manager(Main_State mainState, int numberOfWaves)
        {
            this.numberOfWaves = numberOfWaves;
            this.mainState = mainState;

            waveTimer.Interval = 5000;
            waveTimer.Elapsed += NewWave;

            #region Adding Waves
            for (int i = 0; i < numberOfWaves; i++)
            {
                int initalNumberOfEnemies = 6;
                int initalHealth = 3;
                int cashDrop = 2;

                //Modifier to add 6 enemies, Every 6 waves.
                //Is an integer so rounds down. (wave 1, 1/6 = 0 as it rounds down.)
                int enemyNumberModifier = (i / initalNumberOfEnemies) + 1;

                //Adds 3 to health every three waves.
                int healthHumberModifier = (i / 1) + 2;

                //Adds 2 to cashDrop every 6 waves.
                int cashDropModifier = (i / 6) + 1;

                //Initialising new wave.
                Wave wave = new Wave(mainState, i, initalNumberOfEnemies * enemyNumberModifier, initalHealth * healthHumberModifier,
                    cashDrop * cashDropModifier, enemyTexture, Resources.Health_Bar);

                //Adding wave to Queue.
                waves.Enqueue(wave);
            }
            #endregion

            waveTimer.Start();
        }

        private void AddWaves()
        {

        }

        private void NewWave(Object source, ElapsedEventArgs e)
        {
            waves.Dequeue();    //Remove finished wave.

            if (waves.Count > 0)    //If there are waves left.
            {
                waves.Peek().Start();   //Start the next wave.
            }  

            waveTimer.Stop();   //Stop timer for wait between waves.
        }

        public void AdjustGameSpeed()
        {
            waveTimer.Interval *= Main_State.speedCoef;
            foreach (Wave w in waves)
                w.spawnTimer.Interval /= Main_State.speedCoef;
        }

        public void Update()
        {
            CurrentWave.Update();   //Update wave.

            if (CurrentWave.RoundOver)  //If the wave is over
                waveTimer.Start();
        }

        public void Redraw(PaintEventArgs e)
        {
            CurrentWave.Draw(e);
        }
    }
*/