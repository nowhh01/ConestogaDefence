using Microsoft.Xna.Framework;
using System;

namespace ConestogaDefence
{
    public struct Shared
    {
        public static Rectangle StageRectangle =>
            new Rectangle(0, 0, (int)Stage.X, (int)Stage.Y);
        public static Random Random => new Random();
        public static Vector2 Stage { get; set; }
        public static Vector2 Center => Stage / 2;
        public static float SoundEffectVolume { get; set; }
        public static int CellWidth => 64;
        public static int CellHeight => 64;
    }
}
