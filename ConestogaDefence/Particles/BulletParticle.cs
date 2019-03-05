using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace ConestogaDefence.Particles
{
    class BulletParticle
    {
        private static readonly float msTransparencyRatio = .2f;
        private static readonly int msSides = 5;

        public Vector2 Position { get; set; }
        public Vector2 Center => 
            new Vector2(Position.X + Radius / 2, Position.Y + Radius / 2);
        public Vector2 Velocity { get; set; }
        public float Radius { get; set; }
        public float Angle { get; set; }
        public float AngularVelocity { get; set; }
        public float TimeToLive { get; set; }

        public BulletParticle(in Vector2 position, in Vector2 velocity, in float angle,
            in float angularVelocity, in float radius, in float timeToLive)
        {
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Radius = radius;
            TimeToLive = timeToLive;
        }

        public void Update(in GameTime gameTime)
        {
            Position += Velocity;
            Angle += AngularVelocity;
        }

        public void Draw(in SpriteBatch spriteBatch)
        {
            for (float radius = Radius; radius > 0; radius--)
            {
                createPolygon(radius, msSides, out Vector2[] points);
                spriteBatch.DrawPolygon(Position, points, 
                    Color.White * (float)(TimeToLive * msTransparencyRatio));
            }
        }

        private void createPolygon(in float radius, int sides, out Vector2[] points)
        {
            float max = (float)(2.0 * Math.PI);
            float step = max / sides;
            float theta = .0f;

            points = new Vector2[sides];
            for (var i = 0; i < sides; i++)
            {
                points[i] = new Vector2(
                    (float)(radius * Math.Cos(theta)), 
                    (float)(radius * Math.Sin(theta)));
                theta += step;
            }
        }
    }
}
