using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConestogaDefence.Buttons
{
    public class TowerBuildButton : Button
    {
        private readonly int mCost;

        public bool IsActivated => SpriteColor == Color.White;

        public TowerBuildButton(int cost, in Vector2 position, 
            in Texture2D regularTexture, in Texture2D highlightTexture = null, 
            in SpriteFont font = null, in string name = null) 
            : base(position, regularTexture, highlightTexture, font, name)
        {
            FontColor = Color.White;
            mCost = cost;
        }

        public override void Update(in GameTime gameTime)
        {
            base.Update(gameTime);
            
            if(IsClicked && !IsActivated)
            {
                IsClicked = false;
            }
        }

        public void CheckPlayerMoney(int money)
        {
            if (money >= mCost)
            {
                SpriteColor = Color.White;
            }                
            else
            {
                SpriteColor = Color.Gray;
            }
        }
    }
}
