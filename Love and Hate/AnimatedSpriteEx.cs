using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Love_and_Hate
{
    public class AnimatedSpriteEx
    {
        enum eAnimationType
        {
            KILL,
            INFINITE
        }

        private eAnimationType mAnimType = eAnimationType.KILL;

        private int mFrameCount;
        private List<Texture2D> mFrameTextures = new List<Texture2D>();
        private float mTimePerFrame;
        private int mFrame;
        private float mTotalElapsed;

        private bool mPaused;

        private string mAsset;

        private Vector2 mPosition;

        public Vector2 Position
        {
            get { return mPosition; }
            set { mPosition = value; }
        }

        private Vector2 mRotation = new Vector2(0, 0);
        private Vector2 mScale = new Vector2(1, 1);
        private float mDepth;

        private Vector2 mOrigin;
        private Game mGame;

        bool mDispose = false;

        public bool Dispose
        {
            get { return mDispose; }
            set { mDispose = value; }
        }

        private SpriteEffects mEffects = SpriteEffects.None;

        public AnimatedSpriteEx(Game game)
        {
            this.mGame = game;
            //this.Origin = origin;
            //this.Rotation = rotation;
            //this.Scale = new Vector2(1, 1);
            //this.Depth = depth;

            //this.Asset        = asset;
            //this.FrameCount   = frameCount;
            //this.FramesPerSec = framesPerSec;

            //LoadContent();
            //Load(game.Content, asset, frameCount, framesPerSec);
        }

        public void Load(ContentManager content, Vector2 origin, string asset, int frameCount, int framesPerSec)
        {
            mFrameCount = frameCount;

            for (int i = 0; i < frameCount; i++)
            {
                String sFullAssetName = asset + "_00";

                if (i < 10)
                {
                    sFullAssetName += "0";
                    sFullAssetName += i;
                }
                else
                    sFullAssetName += i;

                sFullAssetName = mGame.Content.RootDirectory + sFullAssetName;

                mFrameTextures.Add(mGame.Content.Load<Texture2D>(sFullAssetName));
            }

            mTimePerFrame = (float)1 / framesPerSec;
            mFrame = 0;
            mTotalElapsed = 0;
            mPaused = false;
        }

        // class AnimatedTexture
        public void UpdateFrame(float elapsed)
        {
            if (mPaused)
                return;

            mTotalElapsed += elapsed;

            if (mTotalElapsed > mTimePerFrame)
            {
                mFrame++;

                // Keep the Frame between 0 and the total frames, minus one.
                mFrame = mFrame % mFrameCount;
                mTotalElapsed -= mTimePerFrame;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            DrawFrame(sb, mPosition, mEffects);
            sb.End();
        }

        public void Update(GameTime gameTime)
        {
            if (mAnimType == eAnimationType.KILL)
            {
                if (mFrame == mFrameCount - 1)
                {
                    mDispose = true;
                    //AudioManager.Instance.PlaySound("MonsterAttack");
                    return;
                }
            }

            UpdateFrame(gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
        }

        public void DrawFrame(SpriteBatch batch, Vector2 screenPos, SpriteEffects effects)
        {
            DrawFrame(batch, mFrame, screenPos, effects);
        }

        public void DrawFrame(SpriteBatch batch, int frame, Vector2 screenPos, SpriteEffects effects)
        {
            int FrameWidth = mFrameTextures[frame].Width;

            Rectangle sourcerect = new Rectangle(FrameWidth, 0, FrameWidth, mFrameTextures[frame].Height);

            batch.Draw(mFrameTextures[frame], screenPos, null, Color.White, 0, new Vector2(), this.mScale, effects, 0);
        }

        public bool IsPaused
        {
            get { return mPaused; }
        }

        public void Reset()
        {
            mFrame = 0;
            mTotalElapsed = 0f;
        }

        public void Stop()
        {
            Pause();
            Reset();
        }

        public void Play()
        {
            mPaused = false;
        }

        public void Pause()
        {
            mPaused = true;
        }
    }
}
