using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConestogaDefence.Menus
{
    public class LoadMenu : Menu
    {
        public LoadMenu(in Texture2D panelTexture, in Texture2D cancelTexture, 
            in SpriteFont font) : base(panelTexture, cancelTexture, font)
        {
        }

        public override void Draw(in SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(in GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
