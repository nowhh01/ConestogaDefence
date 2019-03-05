using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConestogaDefence.Buttons
{
    public class TowerUIButton : Button
    {
        private Texture2D mDollarTexture;
        private SpriteFont mFont;
        private Vector2 mStringPosition;
        private string mName;

        public int Cost { get; set; }
        public bool IsActivated => SpriteColor == Color.White;

        public TowerUIButton(in string name, int cost, 
            in Vector2 position, in Texture2D regularTexture, 
            in Texture2D highlightTexture = null) 
            : base(position, regularTexture, highlightTexture)
        {
            mName = name;
            Cost = cost;
            mFont = ContentHolder.LoadFont("Fonts/BlockFont");
            mDollarTexture = ContentHolder.LoadTexture("Etc/Money");
            FontColor = Color.FloralWhite;
            SpriteColor = Color.White;

            Vector2 StringSize = mFont.MeasureString(mName);
            mStringPosition = Center - StringSize / 2;
        }

        public override void Update(in GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsClicked && !IsActivated)
            {
                IsClicked = false;
            }
        }

        public override void Draw(in SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.DrawString(mFont, mName, 
                mStringPosition + new Vector2(0, -45), FontColor);
            
            spriteBatch.Draw(mDollarTexture, Position + new Vector2(-8, 60), null,
                FontColor, 0f, Vector2.Zero, .6f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(mFont, Cost.ToString(), 
                Position + new Vector2(20, 60), FontColor);
        }

        public void CheckPlayerMoney(int money)
        {
            if(money >= Cost)
            {
                SpriteColor = Color.White;
            }
            else
            {
                SpriteColor = Color.Red;
            }
        }
    }
}