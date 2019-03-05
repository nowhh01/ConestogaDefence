using ConestogaDefence.Bullets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConestogaDefence.Towers
{
    public class GreenTower : Tower
    {
        private BulletFire mBullet;
        
        public GreenTower(in Texture2D bulletTexture,
            in Texture2D upperBodyTexture, in Vector2 position,
            in Texture2D lowerBodyTexture)
            : base(upperBodyTexture, position, lowerBodyTexture)
        {
            mBullet = new BulletFire(this, bulletTexture, position);
            Damage = TowerInfo.GreenTowerDamage;
            Cost = TowerInfo.GreenTowerCost;
            Radius = TowerInfo.GreenTowerRadius;
            BulletDelay = TowerInfo.GreenTowerDelay;
        }

        public override void Update(in GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsBulletReady && HasTarget)
            {
                mBullet.IsFired = true;
                IsBulletReady = false;
                Target.CurrentHealth -= Damage;
                Target.CurrentColor = Color.Red;
            }

            mBullet.Update(gameTime);
        }

        public override void Draw(in SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            mBullet.Draw(spriteBatch);
        }

        public override void LevelUp()
        {
            base.LevelUp();

            Damage += TowerInfo.RedTowerDamage / 2;
            Radius += 20;
            BulletDelay -= .2f;
        }
    }
}