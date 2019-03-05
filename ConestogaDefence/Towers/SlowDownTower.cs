using System;
using System.Collections.Generic;
using ConestogaDefence.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConestogaDefence.Towers
{
    class SlowDownTower : Tower
    {
        private static readonly float msTransparencyRatio = .3f;
        private static readonly int msSides = 100;

        private float mSlowDownRatio;
        
        public List<Enemy> Targets = new List<Enemy>();

        public SlowDownTower(in Texture2D upperBodyTexture, in Vector2 position) 
            : base(upperBodyTexture, position)
        {
            Cost = TowerInfo.SlowDownTowerCost;
            Radius = TowerInfo.SlowDownTowerRadius;
            mSlowDownRatio = TowerInfo.SlowDownTowerRatio;
        }
        
        public override void Update(in GameTime gameTime)
        {
            CheckClicked();

            Enemy target;
            for(int i = 0; i < Targets.Count; i++)
            {
                target = Targets[i];
                if(IsInRange(target.Center) && !target.IsDead)
                {
                    target.SpeedModifier = mSlowDownRatio;
                }
                else
                {
                    target.SpeedModifier = 1f;
                    Targets.Remove(target);
                }
            }
        }

        public override void Draw(in SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            for(int i = 0; i < Targets.Count; i++)
            {
                createRoundAndRound(1, msSides, out Vector2[] points);
                drawPolygon(spriteBatch, Targets[i].Center, points,
                    Color.White * msTransparencyRatio);
            }
        }

        public override void LevelUp()
        {
            base.LevelUp();
            Radius += 10f;
            mSlowDownRatio -= .2f;
        }
        
        private void drawPolygon(in SpriteBatch spriteBatch, in Vector2 offset,
            in Vector2[] points, in Color color, in float thickness = 4f)
        {
            var texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, 
                SurfaceFormat.Color);
            texture.SetData(new[] { Color.White });

            Vector2 p1;
            Vector2 p2;
            for (int i = 0; i < points.Length - 1; i++)
            {
                p1 = points[i] + offset;
                p2 = points[i + 1] + offset;
                var length = Vector2.Distance(p1, p2);
                var angle = (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
                var scale = new Vector2(length, thickness);
                spriteBatch.Draw(texture, p1, null, color, angle, Vector2.Zero,
                    scale, SpriteEffects.None, 0f);
            }
        }

        private void createRoundAndRound(in float radius, int sides, 
            out Vector2[] points)
        {
            float max = (float)(5.0 * Math.PI);
            float step = max / sides;
            float theta = .0f;

            points = new Vector2[sides];

            for (var i = 0; i < sides; i++)
            {
                points[i] = new Vector2(
                    (float)((radius + i * .18f) * Math.Cos(theta)), 
                    (float)((radius + i * .18f) * Math.Sin(theta)));
                theta += step;
            }
        }
    }
}