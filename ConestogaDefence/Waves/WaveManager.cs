using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ConestogaDefence.Maps;
using ConestogaDefence.Enemies;

namespace ConestogaDefence.Waves
{
    public class WaveManager
    {
        private static readonly int msNumberOfWaves = 20;

        private readonly Texture2D[] mEnemyTextures;
        private readonly int mLevel;

        private Queue<Wave> mWaves = new Queue<Wave>();
        private Player mPlayer;

        public List<Enemy> Enemies => CurrentWave.Enemies;
        public Wave CurrentWave => mWaves.Peek();
        public int Round => CurrentWave.RoundNumber + 1;
        public int NumberOfWaves => msNumberOfWaves;
        public bool AreAllWavesOver => mWaves.Count == 0;
        
        public WaveManager(int level, in Map map, in Player player,
            in Texture2D[] enemyTextures)
        {
            mLevel = level;
            mEnemyTextures = enemyTextures;
            mPlayer = player;

            int initialNumerOfEnemies;
            int numberModifier;
            int modifier;
            int additionalNumberOfEnemies;
            for (int i = 0; i < msNumberOfWaves; i++)
            {
                initialNumerOfEnemies = 10;
                numberModifier = (i / 6) + 1;
                modifier = i / 5 + 1;
                additionalNumberOfEnemies = level * 2;

                Wave wave = new Wave(mPlayer, map, enemyTextures[0], level, 
                    i, initialNumerOfEnemies * modifier + additionalNumberOfEnemies);

                mWaves.Enqueue(wave);
            }

            startNextWave();
        }

        public void Update(in GameTime gameTime)
        {
            CurrentWave.Update(gameTime);

            if (CurrentWave.IsRoundOver)
            {
                mWaves.Dequeue();
                startNextWave();
            }
        }

        public void Draw(in SpriteBatch spriteBatch)
        {
            CurrentWave.Draw(spriteBatch);
            CurrentWave.DrawEnemiesHealthBar(spriteBatch);
        }

        private void startNextWave()
        {
            mPlayer.RoundNumber++;

            if (mWaves.Count > 0)
            {
                mWaves.Peek().Start();
            }
        }
    }
}
