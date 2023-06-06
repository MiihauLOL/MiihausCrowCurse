using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.BellsAndWhistles;

namespace MiihausCrowCurse
{
    internal sealed class ModEntry : Mod
    {
        private Random random;
        
        public override void Entry(IModHelper helper)
        {
            random = new Random();

            helper.Events.GameLoop.UpdateTicked += OnUpdateTick;
        }


        private void OnUpdateTick(object sender, UpdateTickedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;
            //randomness for the frequency
            uint randomTime = Convert.ToUInt32(random.Next(120)+100);
            if (e.IsMultipleOf(randomTime)) // Adjust the frequency by changing the number here
            {
                if (Game1.currentLocation.IsOutdoors == true) { 
                    //this.Monitor.Log("Trying to spawn...", LogLevel.Debug);
                    // Get a random X and Y coordinate within the farm area
                    int randomX;
                    int randomY;
                    double randomPlusMinus = random.NextDouble();

                    if (randomPlusMinus>0.5)
                    {
                        randomX = Game1.player.getTileX() + random.Next(5);
                    }
                    else { randomX = Game1.player.getTileX() - random.Next(5); }

                    randomPlusMinus = random.NextDouble();
                    if (randomPlusMinus > 0.5)
                    {
                        randomY = Game1.player.getTileY() + random.Next(8);
                    }
                    else { randomY = Game1.player.getTileY() - random.Next(8); }
                    
                    //this.Monitor.Log("made Random number X: " + randomX + ", and Y: " + randomY + ".", LogLevel.Debug);
                    // Spawn a crow at the random location
                    if (Game1.currentLocation.isWaterTile(randomX, randomY).Equals(false)) {
                        Game1.playSound("coin");
                        Game1.currentLocation.critters.Add(new Crow(randomX, randomY));
                        //this.Monitor.Log("Spawing crow...", LogLevel.Debug);

                    }
                }
            }

        }

    }
}