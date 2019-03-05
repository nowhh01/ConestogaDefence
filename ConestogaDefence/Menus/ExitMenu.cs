using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ConestogaDefence.Buttons;

namespace ConestogaDefence.Menus
{
    public enum eYesOrNo
    {
        Yes,
        No
    }

    public class ExitMenu : Menu
    {
        private static readonly string[] msButtonNames = { "YES", "NO" };
        private static readonly string msMessage = "Do you want to exit this game?";
        private static readonly int msMargin = 20;

        private Button[] mButtons = new Button[2];
        private Texture2D mRegularButtonTexture;
        private Texture2D mPressedButtonTexture;
        private Vector2 mMessagePosition;
        
        public bool IsGameExited { get; set; }

        public ExitMenu(in Texture2D regularButtonTexture, 
            in Texture2D pressedButtonTexture, in Texture2D panelTexture,
            in Texture2D cancelTexture, in SpriteFont font) 
            : base(panelTexture, cancelTexture, font)
        {
            mRegularButtonTexture = regularButtonTexture;
            mPressedButtonTexture = pressedButtonTexture;
            mMessagePosition = Shared.Center - new Vector2(200, 100);

            Vector2 position;
            for (int i = 0; i < mButtons.Length; i++)
            {
                position.X = Shared.Center.X - regularButtonTexture.Width +
                    (regularButtonTexture.Width * i) - (msMargin * (int)Math.Pow(-1, i));
                position.Y = Shared.Center.Y + msMargin * 2;

                mButtons[i] = new Button(position, regularButtonTexture, 
                    pressedButtonTexture, font, msButtonNames[i]);
            }
        }

        public override void Draw(in SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.DrawString(Font, msMessage, mMessagePosition, 
                Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            
            foreach(Button button in mButtons)
            {
                button.Draw(spriteBatch);
            }
        }

        public override void Update(in GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0; i < mButtons.Length; i++)
            {
                mButtons[i].Update(gameTime);

                if(i == 0 && mButtons[i].IsClicked)
                {
                    IsGameExited = true;
                    mButtons[i].IsClicked = false;
                    CancelButton.IsClicked = true;
                }
                else if(i == 1 && mButtons[i].IsClicked)
                {
                    CancelButton.IsClicked = true;
                    mButtons[i].IsClicked = false;
                }
            }
        }
    }
}
