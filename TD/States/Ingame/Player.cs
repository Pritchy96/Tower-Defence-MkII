using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tower_Defence.Properties;
using Tower_Defence.States.Ingame;
using Tower_Defence.States.Ingame.Towers;
using Tower_Defence.Util;

namespace Tower_Defence
{
    public class Player
    {
        public int money = 300;
        public int lives = 30;

        private List<Tower> towers = new List<Tower>();

        //Referencing the level we're on.
        private Level level;

        //Variables for checking position of tower.
        private int cellX;
        private int cellY;

        private int tileX;
        private int tileY;

        //Placing towers.
        private int newTowerIndex;  //index of the tower's texture.
        private string newTowerType;    //The type of tower to add.
        //The ranges of each tower. Is a bit naff, but there is no way to access
        //the towers ranges when you are previewing a tower. 
        private float[] ranges = new float[] { 80, 100, 60 };

        //Selecting Towers
       // private Bitmap radiusTexture;
        //     public Tower selectedTower;

        #region Properties.
        public int Money
        {
            get { return money; }
            set { money = value; }
        }

        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        public string NewTowerType
        {
            set { newTowerType = value; }
        }

        public int NewTowerIndex
        {
            set { newTowerIndex = value; }
        }

        //DEBUG
        public void addTowerToList(Tower tow)
        {
            towers.Add(tow);
        }

        #endregion

        //Constructor
        public Player(Level level)
        {
            //Setting the class variables to the Constructor Variables
            this.level = level;
        }

        //A method to check whether the cell is clear for tower placement
        private bool IsCellClear()
        {
            //Make sure tower is within limits of screen.
            bool inBounds = cellX >= 0 && cellY >= 0 && cellX < level.Width && cellY < level.Height;

            bool spaceClear = true;

            //Loop through each tower in the level, making sure it's position 
            //is not where we want to place the new tower.
            foreach (Tower tower in towers)
            {
                spaceClear = (tower.Position != new Vector2(tileX, tileY));
                //No point continuing the loop if a tower is found in the position.
                if (!spaceClear)
                    break;
            }

            //Check to make sure the position selected is not on a path
            //(remember if it = 1, it's a path tile, if it's 0, it's grass, etc)
            bool onPath = (level.GetIndex(cellX, cellY) == 0);

            return inBounds && spaceClear && onPath; //if these are all true, it will return true.
        }

        //Adding new towers.
        public void AddTower()
        {
            Tower towerToAdd = null;

            towerToAdd = new Tow_Basic(Resources.Tow_Basic, Resources.Tow_Basic, Resources.Bul_Basic, new Vector2(80, 200));

            //Only add tower if there is a free space and the player has enough money.
            if (IsCellClear() && towerToAdd.Cost <= money)
            {
                //Add the tower to the list of towers.
                towers.Add(towerToAdd);
                //Deduct cost from money total.
                money -= towerToAdd.Cost;
            }
        }

       /*
        //Adding new towers.
        public void AddTower()
        {
            Tower towerToAdd = null;

            switch (newTowerType)
            {
                case "MachineGun":
                    {
                        towerToAdd = new MachineGun(towerTextures[0,0], towerTextures[0,1], bulletTexture,
                            new Vector2(tileX, tileY)); 
                        break;
                    }

                case "Slow":
                    {
                        towerToAdd = new Slow(towerTextures[1, 0], towerTextures[1, 1], bulletTexture,
                            new Vector2(tileX, tileY));
                        break;
                    }

                case "Sniper":
                    {
                        towerToAdd = new Sniper(towerTextures[2, 0], towerTextures[2, 0], laserTexture,
                            new Vector2(tileX, tileY));
                        break;
                    }
            }

            //Only add tower if there is a free space and the player has enough jewgolds.
            if (IsCellClear() && towerToAdd.Cost <= money)
            {
                //Add the tower to the list of towers.
                towers.Add(towerToAdd);
                //Deduct cost from money total.
                money -= towerToAdd.Cost;

                //if shift is down, allow multiple tower placements.
                if (currentKeyState.IsKeyUp(Keys.LeftShift))
                {
                //otherwise, reset the field.
                newTowerType = string.Empty;
                }
            }
            else
            {
                newTowerType = string.Empty;
            }
        }



        //Removing towers.
        public void RemoveTower(Tower towerToRemove)
        {
            //calculating 70% of spent money, and casting to decimal ready for rounding.
            decimal sellValue = (decimal) ((towerToRemove.Cost + towerToRemove.UpgradeTotal) * 0.7);
             //Give back the money, rounded to the nearest integer.
            money += (int) Math.Round(sellValue);
            //Remove tower from the list (Stops it being drawn, shooting etc)
            towers.Remove(towerToRemove);
        }
  
        //Draws a preview of the tower when dragging and dropping.
        public void DrawPreview(SpriteBatch spriteBatch)
        {
            //If there is a new tower to be placed.
            if (string.IsNullOrEmpty(newTowerType) == false)
            {
                //Essentially this converts to the nearest cell by casting to integers when dividing 
                //by 40 and then multiplying by 40, giving the top right corner of the nearest cell.
                cellX = (int)(currentMouseState.X / 40); //Convert from Mouse position...
                cellY = (int)(currentMouseState.Y / 40); //... To array number.
                tileX = cellX * 40; //Comvert from Array number to level space.
                tileY = cellY * 40; //^

                Bitmap previewTexture = towerTextures[newTowerIndex, 0];
                
                //Actually drawing the tower preview.
                spriteBatch.Draw(previewTexture, new Rectangle(tileX, tileY,
                    previewTexture.Width, previewTexture.Height), Color.White);

                //Calculating a rectangle from the range, to scale the radius texture to it.
                Vector2 radiusPosition = new Vector2(tileX + 20, tileY + 20) - new Vector2(ranges[newTowerIndex]);

                Rectangle radiusRect = new Rectangle(
                (int)radiusPosition.X,
                (int)radiusPosition.Y,
                (int)ranges[newTowerIndex] * 2,
                (int)ranges[newTowerIndex] * 2);

                //Drawing the towers range.
                spriteBatch.Draw(radiusTexture, radiusRect, Color.White * 0.5f);
            }
        }

        */

        //Update loop
        public void Update(List<Enemy> enemies)
        {

            /*
            currentMouseState = Mouse.GetState();  //Set mousestate to state of mouse in this frame.
            currentKeyState = Keyboard.GetState(); //^

            //Essentially this converts to the nearest cell by casting to integers when dividing 
            //by 40 and then multiplying by 40, giving the top rigth corner of the nearest cell.
            cellX = (int)(currentMouseState.X / 40); //Convert from Mouse position...
            cellY = (int)(currentMouseState.Y / 40); //... To array number.
            tileX = cellX * 40; //Comvert from Array number to level space.
            tileY = cellY * 40; //^

            //Mouse click
            if (currentMouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed)
            {
                //Adding a tower to the position clicked as long as a tower is being placed 
                //(isCellClear is handled in the add tower method).
                if (string.IsNullOrEmpty(newTowerType) == false)
                {
                    AddTower();
                }
                    //if there is a tower clicked..
                else
                {
                    //Deselecting towers.
                    if (selectedTower != null)
                    {
                        // If the selected tower does not contain the mouse and there is a click, 
                        //unselect it.
                        if (selectedTower.Bounds.Contains(currentMouseState.X, currentMouseState.Y) == false)
                        {
                            selectedTower.Selected = false;
                            selectedTower = null;
                        }
                    }

                    //If a tower is found to be selected, skip the rest of the loop.
                    foreach (Tower tower in towers)
                    {
                        if (tower == selectedTower)
                        {
                            continue;
                        }

                        //If the tower contains the mouse, it is the selected tower.
                        if (tower.Bounds.Contains(currentMouseState.X, currentMouseState.Y))
                        {
                            selectedTower = tower;
                            tower.Selected = true;
                        }
                    }
                }
            }


            //Upgrading Towers
            if (currentKeyState.IsKeyUp(Keys.U) && prevKeyState.IsKeyDown(Keys.U) && selectedTower != null)
            {
                int upgradeCost;    //Cost of upgrade.

                //If there are no upgrades, set it to the tower cost.
                if (selectedTower.UpgradeTotal == 0)
                {
                    upgradeCost = selectedTower.Cost;    
                }
                    //otherwise set it to 2x the total cost of previous upgrades.
                else
                {
                    //calculating upgrade cost (add one because we are checking for the NEXT level)
                    upgradeCost = selectedTower.UpgradeTotal * 2;
                }
                
                if (money >= upgradeCost && selectedTower.UpgradeLevel < selectedTower.MaxLevel)
                {
                    money -= upgradeCost;
                    selectedTower.UpgradeTotal += upgradeCost;
                    selectedTower.Upgrade();
                }
            }

            //Selling Towers
            if (currentKeyState.IsKeyUp(Keys.S) && prevKeyState.IsKeyDown(Keys.S) && selectedTower != null)
            {
                    RemoveTower(selectedTower);
                    selectedTower = null;
            }
        */

            //Updating the towers.
            foreach (Tower tower in towers)
            {
                //Giving the tower a target if it does not have one.
                if (tower.HasTarget == false)
                {
                    tower.GetClosestEnemy(enemies);
                }

                tower.Update();
            }
            /*
                            prevMouseState = currentMouseState; //Set Oldstate to the state of the previous frame.
                            prevKeyState = currentKeyState; //Set Oldstate to the state of the previous frame.
                    }


        

                    //Drawing/Renering.
                    public void Draw(SpriteBatch spriteBatch)
                    {
                        Tower selectedTower = null;

                        foreach (Tower tower in towers)
                        {
                            //Drawing each tower.
                            tower.Draw(spriteBatch);

                            //If the tower has been clicked on..
                            if (tower.Selected)
                            {
                                //is has been selected.
                                selectedTower = tower;
                            }
                       }

                        //We move this outside the loop so that the radius is drawn after all the towers have,
                        //therefore always being on top of all towers.
                        if (selectedTower != null)
                        {
                            //Calculating a rectangle from the range of the selected tower, to scale the radius texture to it.
                            Vector2 radiusPosition = selectedTower.Center - new Vector2(selectedTower.Range);

                            Rectangle radiusRect = new Rectangle(
                            (int)radiusPosition.X,
                            (int)radiusPosition.Y,
                            (int)selectedTower.Range * 2,
                            (int)selectedTower.Range * 2);

                            //Drawing the towers range.
                            spriteBatch.Draw(radiusTexture, radiusRect, Color.White * 0.5f);
                        }
                    }
                     * */
        }
    }
}

        