using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Love_and_Hate
{
    public class Enemy : Sprite
    {
        private float mMaxSpeed = 0f;
        private float mChaseStrength = 1000;
        private float mAvoidStrength = 2000;
        private float mMinHelpless = 0.5f;
        Vector2 mVelocity = new Vector2();
        Vector2 mTarget = new Vector2();
        public int mLevel = 0;
        public AnimatedSprite mAttackSide;
        public AnimatedSprite mRunSide;
        public enum eEnemyState
        {
            ATTACK = 0,
            RUN
        }

        private eEnemyState mState = eEnemyState.ATTACK;

        public eEnemyState EnemyState
        {
            get { return mState; }
            set { mState = value; }
        }



        public Enemy(Game game, ContentManager theContentManager) : base(game)
        {
            
        }

        protected override void SetAssetName()
        {
            mAssetName = "dot_green";
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();


            // init scale
            Random random = new Random((int)DateTime.Now.Ticks);

            Player smallestPlayer = Program.Instance.GetSmallestPlayer();
            Enemy smallestEnemy = Program.Instance.GetSmallestEnemy();
            Player largestPlayer = Program.Instance.GetLargestPlayer();
            Enemy largestEnemy = Program.Instance.GetLargestEnemy();

            int helplessCount = GetHelplessEnemyCount();

            if (smallestEnemy == null)
            {
                mScale.X = mPixelScale * random.Next((int)(smallestPlayer.PixelWidth * 0.5f), (int)(largestPlayer.PixelWidth * 2.0f));
            }
            else if (helplessCount < Program.Instance.mEnemies.Count * mMinHelpless)
            {
                mScale.X = mPixelScale * random.Next(16, (int)smallestPlayer.PixelWidth);
            }
            else if (largestEnemy.PixelWidth < largestPlayer.PixelWidth)
            {
                mScale.X = mPixelScale * random.Next((int)largestPlayer.PixelWidth, (int)(largestPlayer.PixelWidth * 2.0f));
            }
            else
            {
                mScale.X = mPixelScale * random.Next((int)(smallestPlayer.PixelWidth * 0.5f), (int)(largestPlayer.PixelWidth * 2.0f));
            }

            mScale.Y = mScale.X;

            this.mBounds =
                new BoundingSphere
                (
                    new Vector3(this.mPosition.X, this.mPosition.Y, 0),
                    this.Radius
                );
            
            GetNewTarget();

            mMaxSpeed = 4000f / PixelWidth;

            // init position
            switch (random.Next(1, 5))
            {
                case 1: // left
                    {
                        mPositionX = -Radius;
                        mPositionY = random.Next(0, Config.Instance.GetAsInt("ScreenHeight"));
                        break;
                    }
                case 2: // top
                    {
                        mPositionX = random.Next(0, Config.Instance.GetAsInt("ScreenWidth"));
                        mPositionY = -Radius;
                        break;
                    }
                case 3: // right
                    {
                        mPositionX = Config.Instance.GetAsInt("ScreenWidth") + Radius;
                        mPositionY = random.Next(0, Config.Instance.GetAsInt("ScreenHeight"));
                        break;
                    }
                case 4: // bottom
                    {
                        mPositionX = random.Next(0, Config.Instance.GetAsInt("ScreenWidth"));
                        mPositionY = Config.Instance.GetAsInt("ScreenHeight") + Radius;
                        break;
                    }
            }
            int iPlayerFrameRate = Config.Instance.GetAsInt("PlayerFrameRate");

            mAttackSide = new AnimatedSprite(Game, new Vector2(), 0, mScale.X/2, 0, "\\enemy\\AttackSide\\enemyattack", 8, iPlayerFrameRate);
            mRunSide = new AnimatedSprite(Game, new Vector2(), 0, mScale.X/2, 0, "\\enemy\\RunSide\\enemyrunside", 8, iPlayerFrameRate);


        }

        public override void Draw(GameTime gameTime)
        {
            if (EnemyState == eEnemyState.RUN)
            {
                if (mVelocity.X > 10)
                    this.mRunSide.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                else if (mVelocity.X < -10)
                    this.mRunSide.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                else if (mVelocity.Y > 10)
                    this.mRunSide.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                else
                    this.mRunSide.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
            }
            else
            {
                if (mVelocity.X > 10)
                    this.mAttackSide.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                else if (mVelocity.X < -10)
                    this.mAttackSide.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                else if (mVelocity.Y > 10)
                    this.mAttackSide.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                else
                    this.mAttackSide.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
            }
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            float mls = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            this.mAttackSide.Update(gameTime);
            this.mRunSide.Update(gameTime);

            // move towards nearest prey
            Player prey = GetNearestPrey();

            if (prey != null)
            {
                Vector2 dir = prey.mPosition - mPosition;
                dir.Normalize();

                mVelocity = mVelocity + mls * (dir * mChaseStrength);
                EnemyState = eEnemyState.ATTACK;
            }
            else
            {
                // no prey, so take a leisurely walk
                Vector2 dir = mTarget - mPosition;

                if (dir.Length() > InterestRadius)
                {
                    dir.Normalize();

                    mVelocity = mVelocity + mls * (dir * mChaseStrength);
                }
                else
                {
                    GetNewTarget();
                }
                EnemyState = eEnemyState.RUN;
            }

            // move away from nearest predator
            Player predator = GetNearestPredator();

            if (predator != null)
            {
                Vector2 dir = mPosition - predator.mPosition;
                if (dir.Length() - Radius - predator.Radius < InterestRadius)
                {
                    dir.Normalize();

                    mVelocity = mVelocity + mls * (dir * mAvoidStrength);
                }
            }

            // move away from nearest enemy
            Enemy enemy = GetNearestEnemy();

            if (enemy != this)
            {               
                Vector2 dir = mPosition - enemy.mPosition;
                if (dir.Length() - Radius - enemy.Radius < InterestRadius)
                {
                    dir.Normalize();

                    mVelocity = mVelocity + mls * (dir * mAvoidStrength);
                }
            }

            // add drag
            Vector2 drag = new Vector2(-mVelocity.X, -mVelocity.Y);
            if (drag.Length() != 0)
            {
                drag.Normalize();
                mVelocity = mVelocity + mls * (drag * (mVelocity.Length() * 2));
            }

            if (mVelocity.Length() > mMaxSpeed)
            {
                mVelocity.Normalize();
                mVelocity = mVelocity * mMaxSpeed;
            }

            // set position
            mPositionX = mPosition.X + mls * mVelocity.X;
            mPositionY = mPosition.Y + mls * mVelocity.Y;


            base.Update(gameTime);
        }

        public Player GetNearestPrey()
        {
            Player closest = null;
            float closestDistance = 0f;

            foreach (Player player in Program.Instance.GamePlayers)
            {
                if (player.PixelWidth < PixelWidth)
                {
                    if (closestDistance == 0)
                    {
                        closest = player;
                        Vector2 distance = player.mPosition - mPosition;
                        closestDistance = distance.Length();
                    }
                    else
                    {
                        Vector2 distance = player.mPosition - mPosition;
                        if (distance.Length() < closestDistance)
                        {
                            closest = player;
                            closestDistance = distance.Length();
                        }
                    }
                }
            }

            return closest;
        }

        public Player GetNearestPredator()
        {
            Player closest = null;
            float closestDistance = 0f;

            foreach (Player player in Program.Instance.GamePlayers)
            {
                if (player.PixelWidth > PixelWidth)
                {
                    if (closestDistance == 0)
                    {
                        closest = player;
                        Vector2 distance = player.mPosition - mPosition;
                        closestDistance = distance.Length();
                    }
                    else
                    {
                        Vector2 distance = player.mPosition - mPosition;
                        if (distance.Length() < closestDistance)
                        {
                            closest = player;
                            closestDistance = distance.Length();
                        }
                    }
                }
            }

            return closest;
        }

        public Enemy GetNearestEnemy()
        {
            Enemy closest = this;
            float closestDistance = 0f;

            foreach (Enemy enemy in Program.Instance.mEnemies)
            {
                if (closestDistance == 0)
                {
                    closest = enemy;
                    Vector2 distance = enemy.mPosition - mPosition;
                    closestDistance = distance.Length();
                }
                else
                {
                    Vector2 distance = enemy.mPosition - mPosition;
                    if (distance.Length() - Radius - enemy.Radius < closestDistance)
                    {
                        closest = enemy;
                        closestDistance = distance.Length();
                    }
                }
            }

            return closest;
        }

        private int GetHelplessEnemyCount()
        {
            Player smallest = Program.Instance.GetSmallestPlayer();
            int count = 0;
            foreach (Enemy e in Program.Instance.mEnemies)
            {
                if (e.PixelWidth < smallest.PixelWidth)
                {
                    count++;
                }
            }
            return count;
        }

        private void GetNewTarget()
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            mTarget.X = random.Next(0, Config.Instance.GetAsInt("ScreenWidth"));
            mTarget.Y = random.Next(0, Config.Instance.GetAsInt("ScreenHeight"));
        }
    }
}
