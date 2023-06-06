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
        int nextSpawnTick = 0;

        public override void Entry(IModHelper helper)
        {
            random = new Random();

            helper.Events.GameLoop.UpdateTicked += OnUpdateTick;
        }

        //randomness for the frequency
        private bool ShouldSpawn()
        {
            if (Game1.ticks > nextSpawnTick)
            {
                nextSpawnTick = Game1.ticks + random.Next(120) + 100;
                return true;
            }
            return false;
        }
        private void OnUpdateTick(object sender, UpdateTickedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;
            if (ShouldSpawn() == true) // Adjust the frequency by changing the number here
            {
                if (Game1.currentLocation.IsOutdoors == true) { 
                    //this.Monitor.Log("Trying to spawn...", LogLevel.Debug);
                    // Get a random X and Y coordinate within the farm area
                    int randomX;
                    int randomY;
                    randomX = Game1.player.getTileX() + random.Next(-5, 5);
                    randomY = Game1.player.getTileY() + random.Next(-5, 5);
                    //this.Monitor.Log("made Random number X: " + randomX + ", and Y: " + randomY + ".", LogLevel.Debug);
                    // Spawn a crow at the random location
                    if (Game1.currentLocation.isWaterTile(randomX, randomY).Equals(false)) 
                    {
                        Game1.playSound("coin");
                        Game1.currentLocation.critters.Add(new Crow(randomX, randomY));
                        //this.Monitor.Log("Spawing crow...", LogLevel.Debug);
                    }
                }
            }
        }
    }
}