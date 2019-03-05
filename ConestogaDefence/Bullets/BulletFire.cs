using ConestogaDefence.Towers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConestogaDefence.Bullets
{
    class BulletFire : Sprite
    {
        private static readonly float msTimer = .2f;
        
        private Tower mTower;
        private int mWidth;
        private float mDeltaTime;
        
        public bool IsFired { get; set; }

        public BulletFire(in Tower tower, in Texture2D texture, in Vector2 position) 
            : base(texture, position)
        {
            mTower = tower;
            mWidth = texture.Width;
        }

        public override void Update(in GameTime gameTime)
        {
            Vector2 positionModifier = 
                Vector2.Transform(new Vector2(0, -(mWidth / 1.5f)), 
                Matrix.CreateRotationZ(CurrentRotation));
            Position = mTower.Center - mTower.Origin + positionModifier;
            CurrentRotation = mTower.CurrentRotation;
        
            if(IsFired && mDeltaTime < msTimer)
            {
                mDeltaTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if(IsFired && mDeltaTime > msTimer)
            {
                mDeltaTime = 0;
                IsFired = false;
            }
        }

        public override void Draw(in SpriteBatch spriteBatch)
        {
            if (IsFired)
            {
                base.Draw(spriteBatch);
            }
        }
    }
}