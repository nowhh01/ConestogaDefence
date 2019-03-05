using ConestogaDefence.Bullets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ConestogaDefence.Particles
{
    public class BulletParticleManager
    {
        private readonly Bullet mBullet;

        private List<BulletParticle> mParticles = new List<BulletParticle>();
        private Vector2 mEmitterPosition;
        
        public bool IsDead => mParticles.Count == 0;

        public BulletParticleManager(in Bullet bullet)
        {
            mBullet = bullet;
        }

        public void Update(in GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            mEmitterPosition = mBullet.Center;

            if(!mBullet.IsDead)
            {
                mParticles.Add(generateNewParticle());
            }

            BulletParticle particle;
            for (int i = 0; i < mParticles.Count; i++)
            {
                particle = mParticles[i];
                particle.TimeToLive -= deltaTime;

                if(!mBullet.IsDead)
                {
                    particle.Update(gameTime);
                }

                if (particle.TimeToLive <= 0)
                {
                    mParticles.Remove(particle);
                    i--;
                }
            }
        }

        public void Draw(in SpriteBatch spriteBatch)
        {
            for (int i = 0; i < mParticles.Count; i++)
            {
                mParticles[i].Draw(spriteBatch);
            }
        }

        private BulletParticle generateNewParticle()
        {
            float angle = 0;
            float angularVelocity = 0.1f * (float)(Shared.Random.NextDouble() * 2 - 1);
            float radius = (float)Shared.Random.Next(7, 10);
            float timeToLive = (float)Shared.Random.Next(4, 8) / 10;

            Vector2 position = mEmitterPosition + 
                new Vector2(Shared.Random.Next(5));
            Vector2 velocity = new Vector2(
                    .5f * (float)(Shared.Random.NextDouble()),
                    .5f * (float)(Shared.Random.NextDouble()));

            return new BulletParticle(position, velocity, angle, angularVelocity, 
                radius, timeToLive);
        }
    }
}
