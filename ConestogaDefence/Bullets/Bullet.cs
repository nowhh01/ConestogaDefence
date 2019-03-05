using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ConestogaDefence.Enemies;

namespace ConestogaDefence.Bullets
{
    public class Bullet : Sprite
    {
        private static readonly int msSpeed = 7;

        private Enemy mTarget;
        private int mDamage;
        private bool mbDamaged;
        
        public bool IsDead { get; set; }
        
        public Bullet(in Enemy target, in float rotation, int damage,
            in Texture2D texture, in Vector2 position) : base(texture, position)
        {
            mTarget = target;
            CurrentRotation = rotation;
            mDamage = damage;
        }

        public override void Update(in GameTime gameTime)
        {
            Vector2 direction = Center - mTarget.Center;
            direction.Normalize();

            CurrentRotation = (float)Math.Atan2(-direction.X, direction.Y);

            Velocity = Vector2.Transform(new Vector2(0, -msSpeed), 
                Matrix.CreateRotationZ(CurrentRotation));

            Position += Velocity;

            if (mTarget.IsDead)
            {
                IsDead = true;
            }

            if (Vector2.Distance(Center, mTarget.Center) < 20 && !mbDamaged)
            {
                mTarget.CurrentHealth -= mDamage;
                mTarget.CurrentColor = Color.Red;
                IsDead = true;
                mbDamaged = true;
            }

            base.Update(gameTime);
        }

        public override void Draw(in SpriteBatch spriteBatch)
        {
            if (!IsDead)
            {
                base.Draw(spriteBatch);
            }
        }
    }
}
