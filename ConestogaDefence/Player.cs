using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ConestogaDefence.Maps;
using ConestogaDefence.Towers;
using ConestogaDefence.Enemies;
using System;

namespace ConestogaDefence
{
    enum eTowerType
    {
        Green,
        Red,
        SlowDown
    }

    public class Player
    {
        private static readonly int msStartingMoney = 50;
        private static readonly int msStartingLives = 30;

        private readonly Texture2D[] mBulletTextures;
        private readonly Texture2D[] mTowerLowerTextures;
        private readonly Texture2D[] mTowerUpperTextures;

        private List<Tower> mTowers = new List<Tower>();
        private Tower mSelectedTower;
        private Map mMap;
        private Color mPreviewTowerColor;
        private Color mRangeColor;
        private Vector2 mPreviewPosition;
        private int mRow;
        private int mColumn;
        private int mPreviewX;
        private int mPreviewY;

        public int Money { get; set; } = msStartingMoney;
        public int Lives { get; set; } = msStartingLives;
        public int Score { get; set; }
        public int RoundNumber { get; set; }
        public int SelectedTowerTypeIndex { get; set; }
        public bool IsPreviewShowed { get; set; }

        public Player(in Map map)
        {
            mMap = map;
            mBulletTextures = new Texture2D[]
            {
                ContentHolder.LoadTexture("Bullets/SmallFire"),
                ContentHolder.LoadTexture("Bullets/SmallMissile"),
                ContentHolder.LoadTexture("Bullets/BigMissile")
            };
            mTowerLowerTextures = new Texture2D[]
            {
                ContentHolder.LoadTexture("Towers/towerDefense_tile180"),
                ContentHolder.LoadTexture("Towers/towerDefense_tile181")
            };
            mTowerUpperTextures = new Texture2D[]
            {
                ContentHolder.LoadTexture("Towers/towerDefense_tile249"),
                ContentHolder.LoadTexture("Towers/towerDefense_tile250"),
                ContentHolder.LoadTexture("Towers/SlowDown")
            };
        }

        public void Update(in GameTime gameTime, in List<Enemy> enemies)
        {
            Tower tower;
            for (int i = 0; i < mTowers.Count; i++)
            {
                tower = mTowers[i];
                tower.Update(gameTime);
                tower.UIButtons[(int)eUIButtonType.Upgrade].
                    CheckPlayerMoney(Money);

                if (tower.IsUIShowed)
                {
                    mSelectedTower = tower;
                }

                if (tower is SlowDownTower)
                {
                    SlowDownTower slowDownTower = (SlowDownTower)tower;
                    for (int k = 0; k < enemies.Count; k++)
                    {
                        if (slowDownTower.IsInRange(enemies[k].Center))
                        {
                            if (!slowDownTower.Targets.Contains(enemies[k]))
                            {
                                slowDownTower.Targets.Add(enemies[k]);
                            }
                        }
                    }
                }
                else
                {
                    if (tower.HasTarget == false)
                    {
                        tower.GetClosestEnemy(enemies);
                    }

                    if (tower.IsDamageApplied)
                    {
                        Score += tower.Damage;
                        tower.IsDamageApplied = false;
                    }
                }
            }

            if (!IsPreviewShowed && mSelectedTower != null)
            {
                if (mSelectedTower.IsUpgraded)
                {
                    mSelectedTower.IsUpgraded = false;
                    Money -= mSelectedTower.CostUpgrade;
                    mRow = (int)mSelectedTower.Position.Y / Shared.CellHeight;
                    mColumn = (int)mSelectedTower.Position.X / Shared.CellWidth;

                    switch (mSelectedTower)
                    {
                        case GreenTower gt:
                            mMap.Layout[mRow, mColumn] += 
                                (int)Math.Pow(10, mSelectedTower.Level) 
                                * (int)eTowerType.Green;
                            gt.LevelUp();
                            break;
                        case RedTower rt:
                            mMap.Layout[mRow, mColumn] += 
                                (int)Math.Pow(10, mSelectedTower.Level) 
                                * (int)eTowerType.Red;
                            rt.LevelUp();
                            break;
                        case SlowDownTower sdt:
                            mMap.Layout[mRow, mColumn] += 
                                (int)Math.Pow(10, mSelectedTower.Level) 
                                * (int)eTowerType.SlowDown;
                            sdt.LevelUp();
                            break;
                        default:
                            break;
                    }
                }
                else if (mSelectedTower.IsSold)
                {
                    mRow = (int)mSelectedTower.Position.Y / Shared.CellHeight;
                    mColumn = (int)mSelectedTower.Position.X / Shared.CellWidth;
                    mMap.Layout[mRow, mColumn] = -1;
                    Money += mSelectedTower.CostSell;
                    mTowers.Remove(mSelectedTower);
                    mSelectedTower = null;
                }
            }

            if (Cursor.State.LeftButton == ButtonState.Released
                && Cursor.OldState.LeftButton == ButtonState.Pressed)
            {
                if (IsPreviewShowed && isCellClear())
                {
                    if (Money >= mSelectedTower.Cost)
                    {
                        mTowers.Add(mSelectedTower);
                        Money -= mSelectedTower.Cost;
                        mMap.Layout[mRow, mColumn] = SelectedTowerTypeIndex;
                        mSelectedTower.CreateUIButtons();
                        mSelectedTower.IsAddedOntoMap = true;
                        CreateTower();
                    }
                }
            }
            else if (Cursor.State.RightButton == ButtonState.Released
                && Cursor.OldState.RightButton == ButtonState.Pressed)
            {
                if (IsPreviewShowed)
                {
                    mSelectedTower = null;
                    IsPreviewShowed = false;
                }
            }
        }

        public void Draw(in SpriteBatch spriteBatch)
        {
            for (int i = 0; i < mTowers.Count; i++)
            {
                mTowers[i].Draw(spriteBatch);
            }

            if (IsPreviewShowed)
            {
                DrawPreview(spriteBatch);
            }

            if (mSelectedTower != null)
            {
                if (mSelectedTower.IsAddedOntoMap)
                {
                    if (mSelectedTower.IsUIShowed)
                    {
                        mSelectedTower.DrawUI(spriteBatch);
                    }
                    else
                    {
                        mSelectedTower = null;
                    }
                }
            }
        }

        public void DrawPreview(in SpriteBatch spriteBatch)
        {
            mColumn = Cursor.X / Shared.CellWidth;
            mRow = Cursor.Y / Shared.CellHeight;

            mPreviewX = mColumn * Shared.CellWidth;
            mPreviewY = mRow * Shared.CellHeight;

            mSelectedTower.Position = new Vector2(mPreviewX, mPreviewY);
            mPreviewPosition = new Vector2(mPreviewX + Shared.CellWidth / 2,
                mPreviewY + Shared.CellHeight / 2);

            if (!isCellClear() || Money < mSelectedTower.Cost)
            {
                mPreviewTowerColor = Color.Red;
                mRangeColor = Color.Red;
            }
            else
            {
                mPreviewTowerColor = Color.White;
                mRangeColor = Color.DodgerBlue;
            }

            mSelectedTower.ShowRange(spriteBatch, mRangeColor);
            mSelectedTower.Draw(spriteBatch, mPreviewTowerColor);
        }

        public void CreateTower()
        {
            switch (SelectedTowerTypeIndex)
            {
                case (int)eTowerType.Green:
                    mSelectedTower = new GreenTower(
                        mBulletTextures[SelectedTowerTypeIndex],
                        mTowerUpperTextures[SelectedTowerTypeIndex],
                        mPreviewPosition,
                        mTowerLowerTextures[SelectedTowerTypeIndex]);
                    break;
                case (int)eTowerType.Red:
                    mSelectedTower = new RedTower(
                        mBulletTextures[SelectedTowerTypeIndex],
                        mTowerUpperTextures[SelectedTowerTypeIndex],
                        mPreviewPosition,
                        mTowerLowerTextures[SelectedTowerTypeIndex]);
                    break;
                case (int)eTowerType.SlowDown:
                    mSelectedTower = new SlowDownTower(
                        mTowerUpperTextures[SelectedTowerTypeIndex],
                        mPreviewPosition);
                    break;
                default:
                    break;
            }
        }

        private bool isCellClear()
        {
            if (Shared.StageRectangle.Contains(Cursor.X, Cursor.Y))
            {
                if (mMap.Layout[mRow, mColumn] == -1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}