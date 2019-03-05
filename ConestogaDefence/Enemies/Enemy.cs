using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace ConestogaDefence.Enemies
{
    public class Enemy : Sprite
    {
        private static readonly float msColorTimer = 0.1f;
        private static readonly int msHealthBarWidth = 30;
        private static readonly int msHealthBarHeight = 4;

        private Queue<Vector2> mWaypoints = new Queue<Vector2>();
        private Vector2 mCurrentDirection;
        private Vector2 mNextDirection;
        private Vector2 mCurrentDestination;
        private Vector2 mNextDestination;
        private Vector2 mCurvedDestination;
        private Vector2 mRandomPosition =
            new Vector2(Shared.Random.Next(35), Shared.Random.Next(35));

        private float mTimeSinceColorChanged;
        private float mNextRotation;
        private bool mbTurning;
        private bool mbColorChanged => CurrentColor == Color.Red;

        public Color CurrentColor { get; set; } = Color.White;
        public float DistanceToDestination => 
            Vector2.Distance(Position, mCurrentDestination);
        public float HealthPercentage => CurrentHealth / StartHealth;
        public float Speed { get; protected set; }
        public float CurrentHealth { get; set; }
        public float SpeedModifier { get; set; } = 1f;
        public float StartHealth { get; protected set; }
        public int RewardGiven { get; protected set; }
        public bool IsDead { get; protected set; }
        
        public Enemy(in List<Vector2> waypoints, in Texture2D texture, 
            in Vector2 position) : base(texture, position)
        {
            foreach (Vector2 waypoint in waypoints)
            {
                mWaypoints.Enqueue(waypoint);
            }

            CurrentHealth = StartHealth;
            Position = mWaypoints.Dequeue() + new Vector2(0, mRandomPosition.Y);
            mCurrentDestination = mWaypoints.Dequeue() + mRandomPosition;
            mNextDestination = mWaypoints.Peek() + mRandomPosition;

            mCurrentDirection = mCurrentDestination - Position;
            mCurrentDirection.Normalize();
            mNextDirection = mNextDestination - mCurrentDestination;
            mNextDirection.Normalize();

            CurrentRotation = 
                (float)Math.Atan2(mCurrentDirection.Y, mCurrentDirection.X);
            mNextRotation = (float)Math.Atan2(mNextDirection.Y, mNextDirection.X);
        }

        public override void Update(in GameTime gameTime)
        {
            base.Update(gameTime);
            
            if(mbColorChanged)
            {
                if(mTimeSinceColorChanged >= msColorTimer)
                {
                    CurrentColor = Color.White;
                    mTimeSinceColorChanged = 0;
                }
                else
                {
                    mTimeSinceColorChanged +=
                        (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }

            if (mCurrentDestination != Vector2.Zero && CurrentHealth > 0)
            {
                if (mCurrentDirection == mNextDirection)
                {
                    if (DistanceToDestination <= Speed)
                    {
                        changeDestination();

                        if (mWaypoints.Count == 0)
                        {
                            mCurrentDestination = Vector2.Zero;
                        }                       
                    }
                }
                else
                {
                    if (DistanceToDestination < Speed * 7 && !mbTurning)
                    {
                        mbTurning = true;
                    }                     
                }
                
                if (mbTurning)
                {
                    mCurvedDestination += mNextDirection * Speed * SpeedModifier;

                    var temporaryDirection = mCurvedDestination - Position;
                    temporaryDirection.Normalize();
                    temporaryDirection *= SpeedModifier;

                    CurrentRotation = 
                        (float)Math.Atan2(temporaryDirection.Y, temporaryDirection.X);
                    var angle = MathHelper.ToDegrees(CurrentRotation);

                    Position += temporaryDirection * Speed;
                    var nextAngle = MathHelper.ToDegrees(mNextRotation);

                    if (Math.Abs(angle - nextAngle) <= Speed)
                    {
                        changeDestination();
                        mbTurning = false;
                    }
                }
                else
                {
                    mCurvedDestination = mCurrentDestination;
                    Velocity = Vector2.Multiply(mCurrentDirection, 
                        Speed * SpeedModifier);

                    Position += Velocity;
                }
            }
            else
            {
                IsDead = true;
            }   
        }

        public override void Draw(in SpriteBatch spriteBatch)
        {
            if (!IsDead)
            {
                base.Draw(spriteBatch, CurrentColor);
            }
        }

        public void DrawHealthBar(in SpriteBatch spriteBatch)
        {
            ShapeExtensions.FillRectangle(spriteBatch, Position, 
                new Size2(msHealthBarWidth, msHealthBarHeight), Color.Red);
            ShapeExtensions.FillRectangle(spriteBatch, Position, 
                new Size2(msHealthBarWidth * HealthPercentage, msHealthBarHeight),
                Color.MediumSpringGreen);
        }

        private void changeDestination()
        {
            CurrentRotation = mNextRotation;
            mCurrentDestination = mWaypoints.Dequeue() + mRandomPosition;

            if (mWaypoints.Count > 0)
            {
                mNextDestination = mWaypoints.Peek() + mRandomPosition;
            }           

            mCurrentDirection = mNextDirection;
            mNextDirection = mNextDestination - mCurrentDestination;
            mNextDirection.Normalize();

            mNextRotation = (float)Math.Atan2(mNextDirection.Y, mNextDirection.X);
        }
    }
}