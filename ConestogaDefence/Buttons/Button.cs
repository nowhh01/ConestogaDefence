using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConestogaDefence.Buttons
{
    public class Button
    {
        private readonly SpriteFont mFont;

        private Texture2D mShowedTexture;
        private Rectangle mButtonSize;
        private Vector2 mStringSize;
        private Vector2 mStringPosition;
        private Vector2 mPositionDifference;
        private float mScale;

        public Texture2D RegularTexture { get; set; }
        public Texture2D PressedTexture { get; set; }
        public Color FontColor { get; protected set; } = Color.Black;
        public Color SpriteColor { get; protected set; } = Color.White;
        public Vector2 Center { get; set; }
        public Vector2 Position { get; set; }
        public string Name { get; set; }
        public bool IsClicked { get; set; }
        public bool IsDragged { get; set; }
        public bool IsPressed { get; protected set; }

        public Button(in Vector2 position, in Texture2D regularTexture,
            in Texture2D highlightTexture = null, in SpriteFont font = null, 
            in string name = null)
        {
            Position = position;
            RegularTexture = regularTexture;
            PressedTexture = highlightTexture;
            mFont = font;
            Center = new Vector2(
                position.X + regularTexture.Width / 2,
                position.Y + regularTexture.Height / 2);
            Name = name;

            mButtonSize = new Rectangle((int)position.X, (int)position.Y, 
                regularTexture.Width, regularTexture.Height);
            mShowedTexture = regularTexture;
        }

        public virtual void Update(in GameTime gameTime)
        {
            mShowedTexture = RegularTexture;
            mPositionDifference = Vector2.Zero;
            mScale = 1f;

            if (mButtonSize.Contains(Cursor.X, Cursor.Y))
            {
                if (mFont != null)
                {
                    mScale = 1.1f;
                }

                if (Cursor.OldState.LeftButton == ButtonState.Pressed)
                {
                    IsPressed = true;

                    if (PressedTexture != null)
                    {
                        mShowedTexture = PressedTexture;
                        mPositionDifference.Y = 4;
                    }

                    if (Cursor.State.LeftButton == ButtonState.Released)
                    {
                        IsClicked = true;
                    }
                }
            }

            if (IsPressed)
            {
                IsDragged = true;
            }

            if (Cursor.State.LeftButton == ButtonState.Released)
            {
                IsPressed = false;
                IsDragged = false;
            }

            if (Name != null)
            {
                mStringSize = mFont.MeasureString(Name);
                mStringPosition = Center - mStringSize / 2 * mScale;
            }
        }

        public virtual void Draw(in SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mShowedTexture, Position + mPositionDifference, 
                SpriteColor);

            if (mFont != null)
            {
                spriteBatch.DrawString(mFont, Name, 
                    mStringPosition + mPositionDifference, FontColor, 0f, 
                    Vector2.Zero, mScale, SpriteEffects.None, 0f);
            }
        }
    }
}
