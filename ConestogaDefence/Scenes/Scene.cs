using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConestogaDefence.Scenes
{
    public abstract class Scene
    {
        public Game1 Game { get; }
        
        public Scene(in Game1 game)
        {
            Game = game;
        }

        public abstract void Update(in GameTime gameTime);
        public abstract void Draw(in SpriteBatch spriteBatch);
    }
}
