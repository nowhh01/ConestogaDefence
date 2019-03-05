using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ConestogaDefence.Maps;
using ConestogaDefence.Enemies;

namespace ConestogaDefence.Waves
{
    public class Wave
    {
        private static readonly float msSpawningInterval = 0.5f;

        private readonly int mTotalEnemies;
        private readonly int mLevel;

        private Player mPlayer;
        private Map mMap;
        private Texture2D mEnemyTexture;

        private float mSpawningTimer;
        private int mNumberOfEnemiesSpawned;
        private bool mbSpawned;

        public List<Enemy> Enemies { get; private set; } = new List<Enemy>();
        public int RoundNumber { get; }
        public bool IsRoundOver => Enemies.Count == 0 
            && mNumberOfEnemiesSpawned == mTotalEnemies;

        public Wave(in Player player, in Map map, in Texture2D enemyTexture,
            int level, int waveNumber, int totalEnemies)
        {
            mPlayer = player;
            mMap = map;
            mEnemyTexture = enemyTexture;
            mLevel = level;
            RoundNumber = waveNumber;
            mTotalEnemies = totalEnemies;
        }

        public void Update(in GameTime gameTime)
        {
            if (mNumberOfEnemiesSpawned == mTotalEnemies)
            {
                mbSpawned = false;
            }

            if (mbSpawned)
            {
                mSpawningTimer += 
                    (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (mSpawningTimer > msSpawningInterval)
                {
                    addEnemy();
                }
            }

            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemy enemy = Enemies[i];
                enemy.Update(gameTime);

                if (enemy.IsDead)
                {
                    if (enemy.CurrentHealth > 0)
                    {
                        mPlayer.Lives -= 1;
                    }
                    else
                    {
                        mPlayer.Money += enemy.RewardGiven;
                    }

                    Enemies.Remove(enemy);
                    i--;
                }
            }
        }

        public void Draw(in SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in Enemies)
            {
                enemy.Draw(spriteBatch);
            }
        }

        public void Start()
        {
            mbSpawned = true;
        }

        public void DrawEnemiesHealthBar(in SpriteBatch spriteBatch)
        {
            foreach (var enemy in Enemies)
            {
                enemy.DrawHealthBar(spriteBatch);
            }   
        }

        private void addEnemy()
        {
            Enemy enemy = new Enemy1(mLevel, mMap.Waypoints, 
                mEnemyTexture, mMap.EnemySpawningPoint);

            Enemies.Add(enemy);

            mSpawningTimer = 0;
            mNumberOfEnemiesSpawned++;
        }
    }
}
