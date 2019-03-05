using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ConestogaDefence.Buttons;
using ConestogaDefence.Maps;
using ConestogaDefence.Menus;
using ConestogaDefence.Waves;

namespace ConestogaDefence.Scenes
{
    public enum eState
    {
        GameOver,
        Paused,
        Playing
    }

    public enum eMaps
    {
        map0,
        map1
    }

    public class PlayScene : Scene
    {
        private static readonly string msEndingString = "Game End";
        private static readonly int msMargin = 10;
        private static readonly int msPadding = 20;

        private readonly Texture2D[] mNumbers = new Texture2D[10];
        private readonly MenuManager mMenuManager;
        private readonly int mLevel;
        private readonly int mMapInx;
        private readonly int mTowerButtonWidth;

        private TowerBuildButton[] mTowerBuildButtons;
        private Texture2D[] mEnemyTextures;
        private Texture2D[] mTowerButtonTextures;
        private Texture2D[] menuTextures;

        private WaveManager mWaveManager;
        private Player mPlayer;
        private Map mMap;
        private Texture2D mMapTexture;
        private Texture2D mDollarTexture;
        private Texture2D mLiveTexture;
        private SpriteFont mFont;
        private Vector2 mMoneyPosition;
        private Vector2 mLivePosition;
        private Vector2 mScorePosition;
        private Vector2 mRoundPostion;
        private eState mState = eState.Playing;

        public PlayScene(int mapInx, int level, in Game1 game) : base(game)
        {
            mMapInx = mapInx;
            mLevel = level;
            loadMap(mapInx);
            loadTextures();

            mTowerButtonWidth = mTowerButtonTextures[0].Width;
            mTowerBuildButtons = new TowerBuildButton[]
            {
                new TowerBuildButton(TowerInfo.GreenTowerCost, 
                    new Vector2(1500, 950), mTowerButtonTextures[0]),
                new TowerBuildButton(TowerInfo.RedTowerCost,
                    new Vector2(1500 + mTowerButtonWidth + msMargin, 950),
                    mTowerButtonTextures[1]),
                new TowerBuildButton(TowerInfo.SlowDownTowerCost,
                    new Vector2(1500 + (mTowerButtonWidth + msMargin) * 2, 950),
                    mTowerButtonTextures[2])
            };

            mPlayer =
                new Player(mMap);
            mWaveManager = new WaveManager(level, mMap, mPlayer, 
                mEnemyTextures);
            mMenuManager = game.GameMenuManager;
            mMoneyPosition = new Vector2(mDollarTexture.Width, 0);
            mLivePosition = new Vector2(200 + mLiveTexture.Width, 0);
            mScorePosition = new Vector2(510, 0);
            mRoundPostion = new Vector2(Shared.Center.X - 40, 0);
        }

        public override void Update(in GameTime gameTime)
        {
            if (Keypad.CurrentState.IsKeyDown(Keys.Escape) &&
                Keypad.OldState.IsKeyUp(Keys.Escape))
            {
                if (mPlayer.IsPreviewShowed)
                {
                    mPlayer.IsPreviewShowed = false;
                }
                else if (mMenuManager.SelectedMenu == null)
                {
                    if (mState == eState.Playing)
                    {
                        mState = eState.Paused;
                    }
                    else if (mState == eState.Paused)
                    {
                        mState = eState.Playing;
                    }
                }
            }

            if (mState == eState.Playing)
            {
                mPlayer.Update(gameTime, mWaveManager.Enemies);
                mWaveManager.Update(gameTime);

                foreach (TowerBuildButton button in mTowerBuildButtons)
                {
                    button.Update(gameTime);
                    button.CheckPlayerMoney(mPlayer.Money);
                }
            }

            if (mState == eState.Paused)
            {
                mMenuManager.Update(gameTime);

                if (mMenuManager.IsGameResumed)
                {
                    mMenuManager.IsGameResumed = false;
                    mState = eState.Playing;
                }
            }
        }

        public override void Draw(in SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mMapTexture, Vector2.Zero, null, Color.White, 0f, 
                Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(mDollarTexture, Vector2.Zero, Color.White);
            spriteBatch.Draw(mLiveTexture, new Vector2(200, 0), Color.White);
            spriteBatch.DrawString(mFont, "Score", new Vector2(400, 0), 
                Color.GreenYellow, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(mFont, "Round", new Vector2(800, 0), 
                Color.GreenYellow, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(mFont, "/ " + mWaveManager.NumberOfWaves,
                new Vector2(1000, 0), Color.GreenYellow, 0f, Vector2.Zero, 1.5f, 
                SpriteEffects.None, 0f);

            displayHud(spriteBatch);

            if (mPlayer.Lives >= 0)
            {
                mWaveManager.Draw(spriteBatch);
                mPlayer.Draw(spriteBatch);
                
                for (int i = 0; i < mTowerBuildButtons.Length; i++)
                {
                    mTowerBuildButtons[i].Draw(spriteBatch);

                    if (mTowerBuildButtons[i].IsClicked 
                        && mTowerBuildButtons[i].IsActivated)
                    {
                        mPlayer.SelectedTowerTypeIndex = i;
                        mPlayer.IsPreviewShowed = true;
                        mTowerBuildButtons[i].IsClicked = false;
                        mPlayer.CreateTower();
                        break;
                    }
                }
            }
            else if (mPlayer.Lives <= 0 || mWaveManager.AreAllWavesOver)
            {
                spriteBatch.DrawString(mFont, msEndingString, 
                    new Vector2(800, 100), Color.Black, 0f, Vector2.Zero, 3f, 
                    SpriteEffects.None, 0f);
            }

            if (mState == eState.Paused)
            {
                mMenuManager.Draw(spriteBatch);
            }
        }

        private void displayHud(in SpriteBatch spriteBatch)
        {
            var money = mPlayer.Money.ToString();
            var live = mPlayer.Lives.ToString();
            var score = mPlayer.Score.ToString();
            var round = mPlayer.RoundNumber.ToString();

            drawStatusWIthNumbers(spriteBatch, money, mMoneyPosition);
            drawStatusWIthNumbers(spriteBatch, live, mLivePosition);
            drawStatusWIthNumbers(spriteBatch, score, mScorePosition);
            drawStatusWIthNumbers(spriteBatch, round, mRoundPostion);
        }

        private void drawStatusWIthNumbers(in SpriteBatch spriteBatch,
            in string status, in Vector2 position)
        {
            Texture2D numberTexture;

            for (int i = 0; i < status.Length; i++)
            {
                numberTexture = mNumbers[status[i] - 48];
                spriteBatch.Draw(numberTexture, position +
                    new Vector2(numberTexture.Width - msPadding, 0) * i, Color.Yellow);
            }
        }

        private void loadMap(int mapIndex)
        {
            switch (mapIndex)
            {
                case 0:
                    mMap = new Map0();
                    break;
                default:
                    break;
            }
        }

        private void loadTextures()
        {
            for (int i = 0; i < 10; i++)
            {
                mNumbers[i] = ContentHolder.LoadTexture("Numbers/" + i);
            }
            mEnemyTextures = new Texture2D[]
            {
                ContentHolder.LoadTexture("Enemies/Enemy1"),
                ContentHolder.LoadTexture("Enemies/Enemy2"),
                ContentHolder.LoadTexture("Enemies/Enemy3"),
                ContentHolder.LoadTexture("Enemies/Enemy4")
            };
            mTowerButtonTextures = new Texture2D[]
            {
                ContentHolder.LoadTexture("Buttons/GreenTowerButton"),
                ContentHolder.LoadTexture("Buttons/RedTowerButton"),
                ContentHolder.LoadTexture("Buttons/SlowDownTowerButton")
            };
            menuTextures = new Texture2D[]
            {
                ContentHolder.LoadTexture("Panels/PlayMenuPanel"),
                ContentHolder.LoadTexture("Buttons/buttonLong_grey"),
                ContentHolder.LoadTexture("Buttons/buttonLong_grey_pressed")
            };
            mMapTexture = ContentHolder.LoadTexture("Maps/Map" + mMapInx);
            mDollarTexture = ContentHolder.LoadTexture("Etc/Money");
            mLiveTexture = ContentHolder.LoadTexture("Etc/Heart");
            mFont = ContentHolder.LoadFont("Fonts/BlockFont");
        }
    }
}