using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConestogaDefence
{
    // Singleton
    public sealed class Cursor
    {
        private static readonly Cursor msInstance = new Cursor();

        private MouseState mState;
        private MouseState mOldState;
        private Texture2D mTexture;

        private int x;
        private int y;

        public static MouseState State => msInstance.mState;
        public static MouseState OldState => msInstance.mOldState;
        public static int X => msInstance.x;
        public static int Y => msInstance.y;
        
        static Cursor()
        {
            msInstance.mTexture =
                ContentHolder.LoadTexture("Etc/cursor_pointerFlat_shadow");
        }
        
        // Prevent from instantiating
        private Cursor() { }

        public static void Update()
        {
            msInstance.mOldState = State;
            msInstance.mState = Mouse.GetState();
            msInstance.x = msInstance.mState.X;
            msInstance.y = msInstance.mState.Y;
        }

        public static void Draw(in SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(msInstance.mTexture, new Vector2(msInstance.x, msInstance.y), 
                Color.White);
        }
    }
}
