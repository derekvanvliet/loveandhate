using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Love_and_Hate
{
    public class Sprite : DrawableGameComponent
    {
        //The current position of the Sprite
        private Vector2 mPos = new Vector2();
        public Vector2 mPosition
        {
            get
            {
                Vector2 n = new Vector2(PixelWidth * 0.5f, PixelHeight * 0.5f);
                return mPos + n;
            }

            set
            {
                Vector2 n = new Vector2(PixelWidth * 0.5f, PixelHeight * 0.5f);
                mPos = value - n;
            }
        }
        public float mPositionX
        {
            get
            {
                return mPos.X + PixelWidth * 0.5f;
            }

            set
            {
                mPos.X = value - PixelWidth * 0.5f;
            }
        }
        public float mPositionY
        {
            get
            {
                return mPos.Y + PixelHeight * 0.5f;
            }

            set
            {
                mPos.Y = value - PixelHeight * 0.5f;
            }
        }
        public float mRotation = 0f;
        public Vector2 mScale = new Vector2(1,1);
        private SpriteBatch mSpriteBatch;
        public string mAssetName;

        protected BoundingBox mBounds;
        private Texture2D mBboxTex;

        public float mPixelScale = 0f;


      
        //The texture object used when drawing the sprite
        protected Texture2D mSpriteTexture;

        public float PixelWidth
        {
            get
            {
                if (mSpriteTexture != null)
                    return mSpriteTexture.Width * mScale.X;

                return 0f;
            }
            set
            {
                if (mSpriteTexture != null)
                    mScale.X = mPixelScale * value;
            }
        }
        public float PixelHeight
        {
            get 
            {
                if(mSpriteTexture != null)
                    return mSpriteTexture.Height * mScale.Y;

                return 0f;
            }
            set
            {
                if (mSpriteTexture != null)
                    mScale.Y = mPixelScale * value;
            }
        }
        public float Radius
        {
            get { return PixelWidth * 0.5f; }
        }
        public float InterestRadius
        {
            get { return Radius * 2; }
        }

        public Sprite(Game game)
            : base(game)
        {
            this.SetAssetName();

            // TODO - Figure out why this causes sound to stop
            //this.mDebugText = new Text(game, "Courier");

            Game.Components.Add(this);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        //Load the texture for the sprite using the Content Pipeline
        protected override void LoadContent()
        {
            mSpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            mSpriteTexture = Game.Content.Load<Texture2D>(mAssetName);
            mPixelScale = 1.0f / mSpriteTexture.Width;

            mBboxTex = Game.Content.Load<Texture2D>("bbox");

            base.LoadContent();
        }

        //Draw the sprite to the screen
        //public override void Draw(GameTime gameTime)
        //{
        //    if ( this.GetType().ToString().Equals("Enemy") )
        //        return;

        //    mSpriteBatch.Begin();

        //    mSpriteBatch.Draw(mSpriteTexture, mPos, null, Color.White, mRotation, Vector2.Zero, mScale, SpriteEffects.None, 0f);
        //    mSpriteBatch.End();

        //    base.Draw(gameTime);
        //}


        public virtual void DrawSprite(GameTime gameTime)
        {
            mSpriteBatch.Begin();

            if (Config.Instance.DebugMode())
            {
                float bboxWidth = mBounds.Max.X - mBounds.Min.X;
                float bboxHeight = mBounds.Max.Y - mBounds.Min.Y;

                mSpriteBatch.Draw
                (
                    mBboxTex,
                    new Rectangle((int)mBounds.Min.X, (int)mBounds.Min.Y, (int)bboxWidth, (int)bboxHeight),
                    new Color(255, 255, 255)
                );
            }

            mSpriteBatch.Draw(mSpriteTexture, mPos, null, Color.White, mRotation, Vector2.Zero, mScale, SpriteEffects.None, 0f);
            mSpriteBatch.End();

            //base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            this.SetBboxPos(new Vector2(this.mPosition.X, this.mPosition.Y));

            base.Update(gameTime);
        }

        protected virtual void SetAssetName()
        {
        }

        public BoundingBox Bounds
        {
            get { return this.mBounds; }
        }

        public void SetBboxPos(Vector2 pos)
        {
            float x = pos.X - (this.PixelWidth / 2);
            float y = pos.Y - (this.PixelHeight / 2);

            float width  = this.PixelWidth  * 0.80f;
            float height = this.PixelHeight * 0.90f;

            this.mBounds.Min = new Vector3(x + (this.PixelWidth * 0.2f), y + (this.PixelHeight * 0.3f), -1);
            this.mBounds.Max = new Vector3(x + width, y + height, -1);
        }

        public void Destroy()
        {
            Game.Components.Remove(this);
        }
    }
}
