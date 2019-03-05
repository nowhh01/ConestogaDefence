using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConestogaDefence
{
    public class Sprite
    {
        private readonly int mWidth;
        private readonly int mHeight;

        private Texture2D mTexture;

        public Vector2 Center => 
            new Vector2(Position.X + mWidth / 2, Position.Y + mHeight / 2);
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; protected set; }
        public Vector2 Velocity { get; protected set; }
        public float CurrentRotation { get; protected set; }
        public float RotationModifier { get; protected set; }
        public int RotationModifierTimes { get; protected set; }
        
        public Sprite(in Texture2D texture, in Vector2 position)
        {
            mTexture = texture;
            mWidth = texture.Width;
            mHeight = texture.Height;
            Position = position;
            Origin = new Vector2(mWidth / 2, mHeight / 2);
        }

        public virtual void Update(in GameTime gameTime)
        {
        }

        public virtual void Draw(in SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, Center, null, Color.White, CurrentRotation, 
                Origin, 1.0f, SpriteEffects.None, 0);
        }

        public virtual void Draw(in SpriteBatch spriteBatch, in Color color)
        {
            spriteBatch.Draw(mTexture, Center, null, color, CurrentRotation, Origin, 1.0f, 
                SpriteEffects.None, 0);
        }
    }
}
