using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConestogaDefence.Buttons
{
    public class OptionButton : Button
    {
        private int mXDifferenceBetweenMouseAndButton;

        public int Volume { get; private set; } = 50;

        public OptionButton(in Vector2 position, in Texture2D regularTexture, 
            in Texture2D highlightTexture = null, in SpriteFont font = null, 
            in string name = null)
            : base(position, regularTexture, highlightTexture, font, name)
        {
        }

        public override void Update(in GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsClicked)
            {
                mXDifferenceBetweenMouseAndButton = Cursor.X - (int)Position.X;
            }             
        }

        public void MoveButton(int minX, int maxX, int length)
        {
            int xPosition;

            if (Cursor.X >= maxX)
            {
                xPosition = maxX;
            }             
            else if (Cursor.X <= minX)
            {
                xPosition = minX;
            }
            else
            {
                xPosition = Cursor.X - mXDifferenceBetweenMouseAndButton;
            }
                
            Position = new Vector2(xPosition, Position.Y);
            Volume = (xPosition - minX) * 100 / length;

            if (Volume < 0)
            {
                Volume = 0;
            }                
        }
    }
}
