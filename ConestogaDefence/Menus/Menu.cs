using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ConestogaDefence.Buttons;

namespace ConestogaDefence.Menus
{
    public class Menu
    {
        private Texture2D mPanelTexture;
        private Vector2 mCenter;
        private Vector2 mSize;
        private Vector2 mCancelButtonPosition;

        public Button CancelButton { get; set; }
        public SpriteFont Font { get; }
        public Vector2 PanelPosition => mCenter - (mSize / 2);

        public Menu(in Texture2D panelTexture, in Texture2D cancelTexture,
            in SpriteFont font)
        {
            mPanelTexture = panelTexture;

            mCenter = Shared.Stage / 2;
            mSize = new Vector2(panelTexture.Width, panelTexture.Height);
            mCancelButtonPosition = new Vector2(
                PanelPosition.X + panelTexture.Width - (cancelTexture.Width / 2),
                PanelPosition.Y - cancelTexture.Height / 2);
            CancelButton = new Button(mCancelButtonPosition, cancelTexture);
            Font = font;
        }

        public virtual void Update(in GameTime gameTime)
        {
            CancelButton.Update(gameTime);
        }

        public virtual void Draw(in SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mPanelTexture, PanelPosition, Color.White);
            CancelButton.Draw(spriteBatch);
        }
    }
}
