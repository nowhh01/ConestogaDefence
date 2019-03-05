using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConestogaDefence.Enemies
{
    public class Enemy1 : Enemy
    {
        private static readonly float msSpeed = 3f;

        public Enemy1(int level, in List<Vector2> waypoints, in Texture2D texture, 
            in Vector2 position) : base(waypoints, texture, position)
        {
            Speed = msSpeed;
            StartHealth = 50 * level;
            CurrentHealth = StartHealth;
            RewardGiven = 1;
        }
    }
}