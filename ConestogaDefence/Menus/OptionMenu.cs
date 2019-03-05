using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using ConestogaDefence.Buttons;

namespace ConestogaDefence.Menus
{
    public enum eOptionButtonType
    {
        Background,
        Effect
    }

    public class OptionMenu : Menu
    {
        private OptionButton[] mButtons;
        private OptionButton mButtonClicked;
        private Texture2D mChangingButtonTexture;
        private Texture2D mButtonBarTexture;
        private int mIndexOfButtonClicked;

        public OptionMenu(in Texture2D changingButtonTexture,
            in Texture2D buttonBarTexture, in Texture2D panelTexture,
            in Texture2D cancelTexture, in SpriteFont font) 
            : base(panelTexture, cancelTexture, font)
        {
            mChangingButtonTexture = changingButtonTexture;
            mButtonBarTexture = buttonBarTexture;

            MediaPlayer.Volume = 0.5f;
            Shared.SoundEffectVolume = 0.5f;

            mButtons = new OptionButton[]
            {
                new OptionButton(new Vector2(1010, 450), changingButtonTexture),
                new OptionButton(new Vector2(1010, 500), changingButtonTexture)
            };
        }

        public override void Draw(in SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.DrawString(Font, "Display", new Vector2(680, 250), 
                Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(Font, "Mode", new Vector2(700, 300), 
                Color.White);
            spriteBatch.DrawString(Font, "Sound", new Vector2(680, 400), 
                Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(Font, "Background", new Vector2(700, 450), 
                Color.White);
            spriteBatch.Draw(mButtonBarTexture, new Vector2(870, 465), 
                Color.White);
            spriteBatch.DrawString(Font, 
                mButtons[(int)eOptionButtonType.Background].Volume.ToString(),
                new Vector2(1210, 450), Color.White);
            spriteBatch.DrawString(Font, "Effect", new Vector2(700, 500), 
                Color.White);
            spriteBatch.Draw(mButtonBarTexture, new Vector2(870, 515), 
                Color.White);
            spriteBatch.DrawString(Font,
                mButtons[(int)eOptionButtonType.Effect].Volume.ToString(),
                new Vector2(1210, 500), Color.White);
            spriteBatch.DrawString(Font, "Click", new Vector2(700, 550), 
                Color.White);

            if (mButtonClicked != null)
            {
                mButtonClicked.MoveButton(860, 860 + 300, 300);
                controlVolume(mIndexOfButtonClicked);
            }

            foreach (OptionButton btn in mButtons)
            {
                btn.Draw(spriteBatch);
            }
        }

        public override void Update(in GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0; i < mButtons.Count(); i++)
            {
                mButtons[i].Update(gameTime);

                if (mButtons[i].IsDragged && mButtonClicked == null)
                {
                    mButtonClicked = mButtons[i];
                    mIndexOfButtonClicked = i;
                }
            }

            if (mButtonClicked != null)
            {
                if (!mButtonClicked.IsDragged)
                {
                    mButtonClicked = null;
                }
            }
        }

        private void controlVolume(int indexOfButtonClicked)
        {
            switch (indexOfButtonClicked)
            {
                case (int)eOptionButtonType.Background:
                    MediaPlayer.Volume = 
                        (float)mButtons[indexOfButtonClicked].Volume / 100;
                    break;
                case (int)eOptionButtonType.Effect:
                    Shared.SoundEffectVolume =
                        (float)mButtons[indexOfButtonClicked].Volume / 100;
                    break;
                default:
                    break;
            }
        }
    }
}
