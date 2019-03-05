using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ConestogaDefence.Enemies;
using ConestogaDefence.Particles;
using ConestogaDefence.Bullets;

namespace ConestogaDefence.Towers
{
    public class RedTower : Tower
    {
        private List<BulletParticleManager> mBulletParticles = 
            new List<BulletParticleManager>();
        private List<Bullet> mBullets = new List<Bullet>();
        private Texture2D mBulletTexture;

        public RedTower(in Texture2D bulletTexture, 
            in Texture2D upperBodyTexture, in Vector2 position, 
            in Texture2D lowerBodyTexture) 
            : base(upperBodyTexture, position, lowerBodyTexture)
        {
            mBulletTexture = bulletTexture;
            Damage = TowerInfo.RedTowerDamage;
            Cost = TowerInfo.RedTowerCost;
            Radius = TowerInfo.RedTowerRadius;
            BulletDelay = TowerInfo.RedTowerDelay;
        }

        public override void Update(in GameTime gameTime)
        {
            base.Update(gameTime);

            Bullet bullet;
            BulletParticleManager bulletParticle;
            if (IsBulletReady && HasTarget)
            {
                bullet = new Bullet(Target, CurrentRotation, Damage, mBulletTexture,
                Vector2.Subtract(Center, new Vector2(mBulletTexture.Width / 2)));
                bulletParticle = new BulletParticleManager(bullet);

                mBullets.Add(bullet);
                mBulletParticles.Add(bulletParticle);
                IsBulletReady = false;
            }

            for (int i = 0; i < mBullets.Count; i++)
            {
                bullet = mBullets[i];
                bullet.Update(gameTime);

                if (bullet.IsDead)
                {
                    IsDamageApplied = true;
                    mBullets.Remove(bullet);
                    i--;
                }
            }

            for (int i = 0; i < mBulletParticles.Count; i++)
            {
                bulletParticle = mBulletParticles[i];
                bulletParticle.Update(gameTime);

                if (bulletParticle.IsDead)
                {
                    mBulletParticles.Remove(bulletParticle);
                }
            }
        }

        public override void Draw(in SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (Bullet bullet in mBullets)
            {
                bullet.Draw(spriteBatch);
            }

            foreach (BulletParticleManager bulletParticle in mBulletParticles)
            {
                bulletParticle.Draw(spriteBatch);
            }
        }

        public override void Draw(in SpriteBatch spriteBatch, in Color color)
        {
            base.Draw(spriteBatch, color);
        }

        public override void LevelUp()
        {
            base.LevelUp();

            Damage += TowerInfo.RedTowerDamage / 2;
            Radius += 15;
            BulletDelay -= .15f;
        }
    }
}
