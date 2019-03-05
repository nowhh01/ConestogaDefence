using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ConestogaDefence.Menus;

namespace ConestogaDefence.Scenes
{
    public class StartScene : Scene
    {
        private readonly Texture2D mBackground;

        private MenuManager mMenuManager;
        
        public StartScene(in Game1 game) : base(game)
        {
            mBackground = ContentHolder.LoadTexture("Preview_KenneyNL");
            mMenuManager = game.GameMenuManager;
        }

        public override void Update(in GameTime gameTime)
        {
            mMenuManager.Update(gameTime);

            if (Game.SelectedSceneIndex != -1)
            {
                Game.ChangeScene();
            }
        }

        public override void Draw(in SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mBackground, new Rectangle(0, 0, 1920, 1080), 
                Color.White);
            mMenuManager.Draw(spriteBatch);
        }
    }
}
