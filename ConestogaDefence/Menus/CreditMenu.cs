using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConestogaDefence.Menus
{
    public class CreditMenu : Menu
    {
        private static readonly string msContent = 
            "Developer\nGyeongsik Lee\n\n" +
            "Graphic Images\nKenney - www.kenney.nl\n" +
            "Lucy Morris - www.lucyamorris.com\n\n" +
            "Sound\nopening music\n" +
            "www.matthewpablo.com/contact\n\n" +
            "playing music\n" +
            "www.matthewpablo.com/contact\n\n" +
            "Font\nKenney or www.kenney.nl";

        public CreditMenu(in Texture2D panelTexture, in Texture2D cancelTexture, 
            in SpriteFont font) : base(panelTexture, cancelTexture, font)
        {
        }

        public override void Draw(in SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.DrawString(Font, msContent, new Vector2(690, 260), 
                Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
