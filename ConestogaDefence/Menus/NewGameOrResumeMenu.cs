using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ConestogaDefence.Buttons;

namespace ConestogaDefence.Menus
{
    public enum eArrow
    {
        Left,
        Right
    }
    
    public class NewGameOrResumeMenu : Menu
    {
        private static readonly int msMargin = 30;

        private Button[] mArrowButtons = new Button[2];
        private Button[] mLevelButtons = new Button[3];
        private Texture2D[] mMaps;
        private Vector2 mMapPosition;
        
        public int SelectedMapIndex { get; set; } = 0;
        public int SelectedLevel { get; set; } = 0;
        public bool IsSceneChanged { get; set; }
        
        public NewGameOrResumeMenu(in Texture2D[] arrowTextures, 
            in Texture2D[] levelTextures, in Texture2D[] mapPreviews, 
            in Texture2D panelTexture, in Texture2D cancelTexture, in SpriteFont font)
            : base(panelTexture, cancelTexture, font)
        {
            mMaps = mapPreviews;
            mMapPosition.X = Shared.Center.X - mMaps[0].Width / 2;
            mMapPosition.Y = PanelPosition.Y + msMargin;

            Vector2 position;
            for (int i = 0; i < arrowTextures.Length; i++)
            {
                position.X = PanelPosition.X + 
                    (panelTexture.Width - arrowTextures[0].Width) * i;
                position.Y = PanelPosition.Y + 
                    (panelTexture.Height - arrowTextures[0].Height) / 2;

                mArrowButtons[i] = new Button(position, arrowTextures[i]);
            }

            for (int i = 0; i < levelTextures.Length; i++)
            {
                position.X = Shared.Center.X - (levelTextures[0].Width / 2 * 3) +
                    (levelTextures[0].Width * i) - msMargin;
                position.Y = mMapPosition.Y + mapPreviews[0].Height + msMargin;

                mLevelButtons[i] = new Button(position, levelTextures[i]);
            }
        }

        public override void Draw(in SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(mMaps[SelectedMapIndex], mMapPosition, 
                Color.White);

            foreach (Button button in mArrowButtons)
            {
                button.Draw(spriteBatch);
            }
                
            foreach (Button button in mLevelButtons)
            {
                button.Draw(spriteBatch);
            }             
        }

        public override void Update(in GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0; i < mArrowButtons.Length; i++)
            {
                mArrowButtons[i].Update(gameTime);

                if (mArrowButtons[i].IsClicked)
                {
                    switch((eArrow)i)
                    {
                        case eArrow.Left:
                            if (SelectedMapIndex != 0)
                                SelectedMapIndex--;
                            break;
                        case eArrow.Right:
                            if (SelectedMapIndex != mMaps.Count() - 1)
                                SelectedMapIndex++;
                            break;
                        default:
                            break;
                    }
                }
            }

            for (int i = 0; i < mLevelButtons.Length; i++)
            {
                mLevelButtons[i].Update(gameTime);

                if (mLevelButtons[i].IsClicked)
                {
                    SelectedLevel = i + 1;
                    IsSceneChanged = true;
                    CancelButton.IsClicked = true;
                    mLevelButtons[i].IsClicked = false;
                }
            }
        }

        public void Initialize()
        {
            SelectedMapIndex = 0;
            SelectedLevel = 0;
            IsSceneChanged = false;
        }
    }
}