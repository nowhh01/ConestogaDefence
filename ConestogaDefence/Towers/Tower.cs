using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using ConestogaDefence.Enemies;
using ConestogaDefence.Buttons;

namespace ConestogaDefence.Towers
{
    enum eUIButtonType
    {
        Upgrade,
        Sell
    }

    public class Tower : Sprite
    {
        private readonly static Color msRangeColor = Color.DodgerBlue;
        private readonly static int msSides = 100;

        private readonly Texture2D mLowerBodyTexture;
        private readonly Texture2D mLevelTexture;

        private Rectangle mTowerSize;
        private float mSmallestRange;
        private float mBulletTimer;

        public TowerUIButton[] UIButtons { get; private set; }
        public Enemy Target { get; protected set; }
        public float Radius { get; protected set; }
        public float BulletDelay { get; protected set; }
        public float TimeSinceSold { get; private set; }
        public int Row => (int)Position.Y / Shared.CellHeight;
        public int Column => (int)Position.X / Shared.CellWidth;
        public int Cost { get; protected set; }
        public int Damage { get; protected set; }
        public int Level { get; set; } = 1;
        public int CostUpgrade => Cost * (Level + 1);
        public int CostSell => Cost * Level / 2;
        public bool IsAddedOntoMap { get; set; }
        public bool IsBulletReady { get; protected set; } = true;
        public bool IsDamageApplied { get; set; }
        public bool HasTarget => Target != null;
        public bool IsUIShowed { get; set; }
        public bool IsUpgraded
        {
            get => UIButtons[(int)eUIButtonType.Upgrade].IsClicked;
            set => UIButtons[(int)eUIButtonType.Upgrade].IsClicked = value;
        }
        public bool IsSold
        {
            get => UIButtons[(int)eUIButtonType.Sell].IsClicked;
            set => UIButtons[(int)eUIButtonType.Sell].IsClicked = value;
        }
        public bool IsDead { get; private set; }

        public Tower(in Texture2D upperBodyTexture, in Vector2 position,
            in Texture2D lowerBodyTexture = null)
            : base(upperBodyTexture, position)
        {
            mLevelTexture = ContentHolder.LoadTexture("Etc/star");

            if (lowerBodyTexture != null)
            {
                mLowerBodyTexture = lowerBodyTexture;
            }
        }

        public override void Update(in GameTime gameTime)
        {
            base.Update(gameTime);

            if(IsUIShowed)
            {
                for (int i = 0; i < UIButtons.Length; i++)
                {
                    UIButtons[i].Update(gameTime);
                }
            }
            
            CheckClicked();
            
            if(!IsSold)
            {
                if (Target != null)
                {
                    Vector2 direction = Center - Target.Center;
                    direction.Normalize();

                    CurrentRotation = (float)Math.Atan2(-direction.X, direction.Y);

                    if (!IsInRange(Target.Center) || Target.IsDead)
                    {
                        Target = null;
                    }
                }

                if (!IsBulletReady)
                {
                    if (mBulletTimer >= BulletDelay)
                    {
                        IsBulletReady = true;
                        mBulletTimer = 0;
                    }
                    else
                    {
                        mBulletTimer += 
                            (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }
            }
        }

        public override void Draw(in SpriteBatch spriteBatch)
        {
            if (mLowerBodyTexture != null)
            {
                spriteBatch.Draw(mLowerBodyTexture, Position, Color.White);
            }

            base.Draw(spriteBatch);

            Vector2 v = new Vector2(0, 15);
            for (int i = 0; i < Level; i++)
            {
                spriteBatch.Draw(mLevelTexture, Position + i * v,
                    null, Color.White, 0f, Vector2.Zero, .4f, SpriteEffects.None, 0f);
            }
        }

        public override void Draw(in SpriteBatch spriteBatch, in Color color)
        {
            if (mLowerBodyTexture != null)
            {
                spriteBatch.Draw(mLowerBodyTexture, Position, color);
                spriteBatch.Draw(mLevelTexture, Position,
                    null, color, 0f, Vector2.Zero, .4f, SpriteEffects.None, 0f);
            }

            base.Draw(spriteBatch, color);
        }

        public void ShowRange(in SpriteBatch spriteBatch, in Color color)
        {
            for (float i = Radius; i > 0; i--)
            {
                spriteBatch.DrawCircle(Center.X, Center.Y, i, msSides, color * 0.3f);
            }
        }

        public bool IsInRange(in Vector2 position)
        {
            return Vector2.Distance(Center, position) <= Radius;
        }

        public void GetClosestEnemy(in List<Enemy> enemies)
        {
            mSmallestRange = Radius;

            foreach (Enemy enemy in enemies)
            {
                if (Vector2.Distance(Center, enemy.Center) < mSmallestRange)
                {
                    mSmallestRange = Vector2.Distance(Center, enemy.Center);
                    Target = enemy;
                }
            }
        }

        public void DrawUI(in SpriteBatch spriteBatch)
        {
            if (IsUIShowed)
            {
                ShowRange(spriteBatch, msRangeColor);

                for (int i = 0; i < UIButtons.Length; i++)
                {
                    if (Level == 3 && i == 0)
                    {
                        continue;
                    }
                    UIButtons[i].Draw(spriteBatch);
                }
            }
        }

        public virtual void LevelUp()
        {
            Level++;

            UIButtons[(int)eUIButtonType.Upgrade].Cost = CostUpgrade;
            UIButtons[(int)eUIButtonType.Sell].Cost = CostSell;
        }

        public void CheckClicked()
        {
            if (mTowerSize == Rectangle.Empty)
            {
                mTowerSize = new Rectangle((int)Position.X, (int)Position.Y,
                    Shared.CellWidth, Shared.CellHeight);
            }

            if (IsAddedOntoMap)
            {
                if (Cursor.OldState.LeftButton == ButtonState.Pressed
                && Cursor.State.LeftButton == ButtonState.Released)
                {
                    if (mTowerSize.Contains(Cursor.X, Cursor.Y))
                    {
                        IsUIShowed = true;
                    }
                    else
                    {
                        IsUIShowed = false;
                    }
                }
            }
        }

        public void CreateUIButtons()
        {
            UIButtons = new TowerUIButton[]
            {
                new TowerUIButton("UPGRADE", CostUpgrade,
                    Position + new Vector2(-75, 0),
                    ContentHolder.LoadTexture("Buttons/UpgradeButton")),
                new TowerUIButton("SELL", CostSell,
                    Position + new Vector2(75, 0),
                    ContentHolder.LoadTexture("Buttons/SellButton"))
            };
        }
    }
}