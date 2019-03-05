using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ConestogaDefence.Buttons;

namespace ConestogaDefence.Menus
{
    public enum eMenuType
    {
        NewGameOrResume,
        LoadGame,
        Option,
        Credit,
        Exit
    }

    public class MenuManager
    {
        private static readonly string[,] msNames =
        {
            { "NEW GAME", "LOAD GAME", "OPTION", "CREDIT", "EXIT" },
            { "RESUME", "LOAD GAME", "OPTION", "CREDIT", "EXIT" }
        };
        private static readonly int msMargin = 10;
        private static readonly int msPadding = 20;

        private Button[] mButtons = new Button[5];
        private Texture2D[] mPanelTextures;
        private Texture2D[] mRegularButtonTextures;
        private Texture2D[] mPressedButtonTextures;
        private Texture2D[] mMapPreviews;
        private Texture2D[] mLevelTextures;
        private Texture2D[] mArrowTextures;
        private Vector2[] mItemPositions = new Vector2[5];

        private Texture2D mNewGameAndExitMenuPanel;
        private Texture2D mCancelTexture;
        private Texture2D mCreditOptionTexture;
        private Texture2D mChangingButtonTexture;
        private Texture2D mButtonBarTexture;
        private Texture2D mCurrentPanelTexture;
        private SpriteFont mFont;
        private Vector2 mPanelSize;
        private Vector2 mMenuSize;
        private Vector2 mPanelPosition;

        public Menu SelectedMenu { get; set; }
        public NewGameOrResumeMenu NewGame { get; set; }
        public LoadMenu LoadGame { get; set; }
        public CreditMenu Credit { get; set; }
        public OptionMenu Option { get; set; }
        public ExitMenu Exit { get; set; }
        public GameScene CurrentGameScene { get; set; }
        public bool IsGameResumed { get; set; }
        
        public MenuManager()
        {
            mPanelTextures = new Texture2D[]
            {
                ContentHolder.LoadTexture("Panels/StartMenuPanel"),
                ContentHolder.LoadTexture("Panels/PlayMenuPanel")
            };
            mRegularButtonTextures = new Texture2D[]
            {
                ContentHolder.LoadTexture("Buttons/buttonLong_beige"),
                ContentHolder.LoadTexture("Buttons/buttonLong_grey")
            };
            mPressedButtonTextures = new Texture2D[]
            {
                ContentHolder.LoadTexture("Buttons/buttonLong_beige_pressed"),
                ContentHolder.LoadTexture("Buttons/buttonLong_grey_pressed")
            };
            mMapPreviews = new Texture2D[]
            {
                ContentHolder.LoadTexture("Maps/Map0Preview")
            };
            mLevelTextures = new Texture2D[]
            {
                ContentHolder.LoadTexture("Levels/EasyLevel"),
                ContentHolder.LoadTexture("Levels/NormalLevel"),
                ContentHolder.LoadTexture("Levels/HardLevel")
            };
            mArrowTextures = new Texture2D[]
            {
                ContentHolder.LoadTexture("Buttons/arrowSilver_left"),
                ContentHolder.LoadTexture("Buttons/arrowSilver_right")
            };
            mNewGameAndExitMenuPanel
                = ContentHolder.LoadTexture("Panels/NewGameAndExitMenuPanel");
            mCreditOptionTexture = 
                ContentHolder.LoadTexture("panels/CreditOptionPanel");
            mCancelTexture = ContentHolder.LoadTexture("Buttons/XButton");
            mChangingButtonTexture = 
                ContentHolder.LoadTexture("Buttons/buttonRound_grey");
            mButtonBarTexture = ContentHolder.LoadTexture("Etc/OptionButtonbar");
            mFont = ContentHolder.LoadFont("Fonts/BlockFont");

            mPanelSize = 
                new Vector2(mPanelTextures[0].Width, mPanelTextures[0].Height);
            mMenuSize = new Vector2(mRegularButtonTextures[0].Width,
                mRegularButtonTextures[0].Height);
            mPanelPosition = Shared.Center - (mPanelSize / 2);

            for (int i = 0; i < msNames.GetLength(1); i++)
            {
                mItemPositions[i] = (mPanelPosition + 
                    new Vector2(msMargin + msPadding, 
                        msMargin * (i + 1) 
                        + mRegularButtonTextures[0].Height * i 
                        + msPadding));
            }

            for (int i = 0; i < msNames.GetLength(1); i++)
            {
                mButtons[i] = new Button(mItemPositions[i], 
                    mRegularButtonTextures[0], mPressedButtonTextures[0], mFont, 
                    msNames[0, i]);
            }

            //LoadGame = new LoadMenuItem();
            NewGame = 
                new NewGameOrResumeMenu(mArrowTextures, mLevelTextures, 
                mMapPreviews, mNewGameAndExitMenuPanel, mCancelTexture, 
                mFont);
            Credit = new CreditMenu(mCreditOptionTexture, mCancelTexture, 
                mFont);
            Option = new OptionMenu(mChangingButtonTexture, 
                mButtonBarTexture, mCreditOptionTexture, mCancelTexture, mFont);
            Exit = new ExitMenu(mRegularButtonTextures[1], 
                mPressedButtonTextures[1], mNewGameAndExitMenuPanel, 
                mCancelTexture, mFont);
            
            ChangeMenuManagerType(GameScene.Start);
        }

        public void ChangeMenuManagerType(in GameScene gameScene)
        {
            int gameSceneIndex = (int)gameScene;

            CurrentGameScene = gameScene;
            mButtons[0].Name = msNames[gameSceneIndex, 0];
            mCurrentPanelTexture = mPanelTextures[gameSceneIndex];

            for (int i = 0; i < msNames.GetLength(1); i++)
            {
                mButtons[i].RegularTexture 
                    = mRegularButtonTextures[gameSceneIndex];
                mButtons[i].PressedTexture 
                    = mPressedButtonTextures[gameSceneIndex];
            }
        }

        public void Update(in GameTime gameTime)
        {
            if (SelectedMenu == null)
            {
                foreach (Button button in mButtons)
                {
                    button.Update(gameTime);
                }
            }
            else
            {
                SelectedMenu.Update(gameTime);

                if (SelectedMenu.CancelButton.IsClicked
                    || Keypad.CurrentState.IsKeyDown(Keys.Escape))
                {
                    SelectedMenu.CancelButton.IsClicked = false;
                    SelectedMenu = null;

                    if (SelectedMenu is NewGameOrResumeMenu)
                    {
                        NewGame.Initialize();
                    }
                }
            }
        }

        public void Draw(in SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mCurrentPanelTexture, mPanelPosition, Color.White);

            for (int i = 0; i < mButtons.Length; i++)
            {
                mButtons[i].Draw(spriteBatch);

                if (mButtons[i].IsClicked)
                {
                    checkButton((eMenuType)i, CurrentGameScene);
                    mButtons[i].IsClicked = false;
                    break;
                }
            }

            if (SelectedMenu != null)
            {
                SelectedMenu.Draw(spriteBatch);
            }
        }

        private void checkButton(in eMenuType menuType, 
            in GameScene gameScene)
        {
            switch (menuType)
            {
                case eMenuType.NewGameOrResume:
                    if (gameScene == GameScene.Start)
                    {
                        NewGame.Initialize();
                        SelectedMenu = NewGame;
                    }
                    else
                    {
                        IsGameResumed = true;
                    }
                    break;
                case eMenuType.LoadGame:
                    SelectedMenu = LoadGame;
                    break;
                case eMenuType.Option:
                    SelectedMenu = Option;
                    break;
                case eMenuType.Credit:
                    SelectedMenu = Credit;
                    break;
                case eMenuType.Exit:
                    SelectedMenu = Exit;
                    break;
                default:
                    break;
            }
        }
    }
}