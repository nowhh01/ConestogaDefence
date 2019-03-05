using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace ConestogaDefence
{
    // Singleton
    public sealed class ContentHolder
    {
        private static readonly ContentHolder msInstance = new ContentHolder();

        private ContentManager mContent;
        private Dictionary<string, Texture2D> mTextures;
        private Dictionary<string, Song> mSongs;
        private Dictionary<string, SpriteFont> mFonts;

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        // Static constructors are specified to execute 
        // only when an instance of the class is created
        // or a static member is referenced
        static ContentHolder()
        {
            msInstance.mTextures = new Dictionary<string, Texture2D>();
            msInstance.mSongs = new Dictionary<string, Song>();
            msInstance.mFonts = new Dictionary<string, SpriteFont>();
        }

        private ContentHolder() { }

        public static void SetContentManager(in ContentManager content)
        {
            msInstance.mContent = content;
        }
        
        public static Song LoadSong(in string assetName)
        {
            var songs = msInstance.mSongs;

            if (!songs.ContainsKey(assetName))
            {
                songs.Add(assetName, msInstance.mContent.Load<Song>(assetName));
            }

            return songs[assetName];
        }

        public static SpriteFont LoadFont(in string assetName)
        {
            var fonts = msInstance.mFonts;

            if (!fonts.ContainsKey(assetName))
            {
                fonts.Add(assetName, msInstance.mContent.Load<SpriteFont>(assetName));
            }
            
            return fonts[assetName];
        }

        public static Texture2D LoadTexture(in string assetName)
        {
            var textures = msInstance.mTextures;

            if (!textures.ContainsKey(assetName))
            {
                textures.Add(assetName, msInstance.mContent.Load<Texture2D>(assetName));
            }

            return textures[assetName];
        }
    }
}