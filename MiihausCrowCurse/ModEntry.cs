using System;
using System.Collections.Generic;
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
        private ModConfig Config;
        int spawnFreqency;
        bool makeSpawnSound;
        int spawnRadius;
        public override void Entry(IModHelper helper)
        {
            this.Config = this.Helper.ReadConfig<ModConfig>();
            spawnFreqency = this.Config.SpawnFreqencyInSeconds * 60;
            if(spawnFreqency < 60)
            {
                spawnFreqency = 60;
            }
            spawnRadius = this.Config.SpawnRadiusAroundFarmer;
            if (spawnRadius < 0)
            {
                spawnRadius = 0;
            }
            makeSpawnSound = this.Config.MakeSpawnSound;
            random = new Random();
            helper.Events.GameLoop.UpdateTicked += OnUpdateTick;
        }

        //randomness for the frequency
        private bool ShouldSpawn()
        {

            if (Game1.ticks > nextSpawnTick)
            {
                nextSpawnTick = Game1.ticks + random.Next(-60, 60) + spawnFreqency;
                return true;
            }
            return false;
        }

        private void OnUpdateTick(object sender, UpdateTickedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;
            if (ShouldSpawn() == true) //frequency of spawning
            {
                if (Game1.currentLocation.IsOutdoors == true)
                {
                    //this.Monitor.Log("Trying to spawn...", LogLevel.Debug);
                    // Get a random X and Y coordinate within the farm area
                    int randomX;
                    int randomY;
                    randomX = Game1.player.getTileX() + random.Next(-1 * spawnRadius, spawnRadius);
                    randomY = Game1.player.getTileY() + random.Next(-1 * spawnRadius, spawnRadius);
                    //this.Monitor.Log("made Random number X: " + randomX + ", and Y: " + randomY + ".", LogLevel.Debug);
                    // Spawn a crow at the random location
                    if (Game1.currentLocation.isWaterTile(randomX, randomY).Equals(false))
                    {
                        if(makeSpawnSound == true)
                        {
                            Game1.playSound("coin");
                        }
                        Game1.currentLocation.critters.Add(new Crow(randomX, randomY));
                        //this.Monitor.Log("Spawing crow...", LogLevel.Debug);
                    }
                }
            }
        }
    }
}
    
