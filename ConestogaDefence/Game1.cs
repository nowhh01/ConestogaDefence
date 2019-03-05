using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using ConestogaDefence.Menus;
using ConestogaDefence.Scenes;

namespace ConestogaDefence
{
    public enum GameScene
    {
        None = -1,
        Start,
        Play,
        Exit
    }
    
    public class Game1 : Game
    {
        private SpriteBatch mSpriteBatch;
        private GraphicsDeviceManager mGraphics;

        private Scene mCurrentScene;
        private Scene mNextScene;
        private StartScene mStartScene;
        private PlayScene mPlayScene;
        private Song mStartSceneSound;
        private Song mPlaySceneSound;

        public MenuManager GameMenuManager { get; set; }
        public int SelectedSceneIndex { get; set; }
        
        public Game1()
        {
            mGraphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            MediaPlayer.IsRepeating = true;
            ContentHolder.SetContentManager(Content);

            mGraphics.PreferredBackBufferWidth = 1920;
            mGraphics.PreferredBackBufferHeight = 1080;
            mGraphics.ToggleFullScreen();
            mGraphics.ApplyChanges();
        }

        public void ChangeScene()
        {
            GameScene gameScene = (GameScene)SelectedSceneIndex;

            switch (gameScene)
            {
                case GameScene.Start:
                    mNextScene = mStartScene;
                    GameMenuManager.ChangeMenuManagerType(GameScene.Start);
                    MediaPlayer.Play(mStartSceneSound);
                    break;
                case GameScene.Play:
                    mPlayScene = new PlayScene( 
                        GameMenuManager.NewGame.SelectedMapIndex,
                        GameMenuManager.NewGame.SelectedLevel,
                        this);
                    mNextScene = mPlayScene;
                    GameMenuManager.ChangeMenuManagerType(GameScene.Play);
                    MediaPlayer.Play(mPlaySceneSound);
                    break;
                case GameScene.Exit:
                    Exit();
                    break;
                default:
                    break;
            }

            SelectedSceneIndex = -1;
        }

        public void CreateNewGame(int mapInx, int levelInx)
        {
            mPlayScene = new PlayScene(mapInx, levelInx, this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here          
            base.Initialize();

            Shared.Stage = new Vector2(mGraphics.PreferredBackBufferWidth,
                mGraphics.PreferredBackBufferHeight);

            GameMenuManager = new MenuManager();
            mStartScene = new StartScene(this);
            mCurrentScene = mStartScene;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            mSpriteBatch = new SpriteBatch(GraphicsDevice);
            mStartSceneSound = ContentHolder.LoadSong("Sounds/StartSceneSound");
            mPlaySceneSound = ContentHolder.LoadSong("Sounds/PlaySceneSound");
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            if (GameMenuManager.NewGame.IsSceneChanged)
            {
                SelectedSceneIndex = (int)GameScene.Play;
                ChangeScene();
                GameMenuManager.NewGame.IsSceneChanged = false;
            }
            else if(mPlayScene != null && GameMenuManager.Exit.IsGameExited)
            {
                mPlayScene = null;
                SelectedSceneIndex = (int)GameScene.Start;
                GameMenuManager.Exit.IsGameExited = false;
                ChangeScene();
            }
            else if(GameMenuManager.CurrentGameScene == (int)GameScene.Start
                && GameMenuManager.Exit.IsGameExited)
            {
                Exit();
            }

            if (mNextScene != null)
            {
                mCurrentScene = mNextScene;
                mNextScene = null;
            }

            Cursor.Update();
            Keypad.Update();

            mCurrentScene.Update(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            mSpriteBatch.Begin();
            // TODO: Add your drawing code here          
            mCurrentScene.Draw(mSpriteBatch);
            Cursor.Draw(mSpriteBatch);

            mSpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
