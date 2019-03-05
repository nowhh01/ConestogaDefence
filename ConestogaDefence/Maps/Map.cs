using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ConestogaDefence.Maps
{
    public class Map
    {
        public List<Vector2> Waypoints { get; protected set; } = new List<Vector2>();
        public int[,] Layout { get; protected set; }
        public Vector2 EnemySpawningPoint { get; protected set; }
        public int NumOfRow => Layout.GetLength(0);
        public int NumOfColumn => Layout.GetLength(1);
    }
}