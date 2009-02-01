using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace Love_and_Hate
{
    public class AnimatedSprite 
    {
        private int framecount;
        private List<Texture2D> FrameTextures = new List<Texture2D>();
        private float TimePerFrame;
        private int Frame;
        private float TotalElapsed;
        private bool Paused;
        private string Asset;
        private int FrameCount;
        private int FramesPerSec;

        public float Rotation, Scale, Depth;
        public Vector2 Origin;
        public Game GameObj;

        SpriteBatch sb;

        public AnimatedSprite(Game game, Vector2 origin, float rotation, float scale, float depth, string asset, int frameCount, int framesPerSec)
        {
            this.GameObj = game;
            this.Origin = origin;
            this.Rotation = rotation;
            this.Scale = scale;
            this.Depth = depth;

            this.Asset        = asset;
            this.FrameCount   = frameCount;
            this.FramesPerSec = framesPerSec;

            //LoadContent();
            Load(game.Content, Asset, FrameCount, FramesPerSec);
        }

        //protected override void LoadContent()
        //{
            

        //    Load(Game.Content, Asset, FrameCount, FramesPerSec);

        //    base.LoadContent();
        //}

        public void Load(ContentManager content, string asset, int frameCount, int framesPerSec)
        {
            sb = new SpriteBatch(GameObj.GraphicsDevice);

            framecount = frameCount;

            for (int i = 0; i < frameCount; i++)
            {
                String sFullAssetName = asset + "_00";

                if ( i < 10 )
                {
                    sFullAssetName += "0";
                    sFullAssetName += i;
                }
                else
                    sFullAssetName += i;        

                sFullAssetName = GameObj.Content.RootDirectory + sFullAssetName;

                FrameTextures.Add(content.Load<Texture2D>(sFullAssetName));
            }
            
            //myTexture = content.Load<Texture2D>(asset);
            TimePerFrame = (float)1 / framesPerSec;
            Frame = 0;
            TotalElapsed = 0;
            Paused = false;
        }

        // class AnimatedTexture
        public void UpdateFrame(float elapsed)
        {
            if (Paused)
               return;

            TotalElapsed += elapsed;

            if (TotalElapsed > TimePerFrame)
            {
                Frame++;
                
                // Keep the Frame between 0 and the total frames, minus one.
                Frame = Frame % framecount;
                TotalElapsed -= TimePerFrame;
            }
        }

        public void Draw(GameTime gameTime, Vector2 pos, SpriteEffects effects)
        {
            if (sb != null)
            {
                // TODO: Add your drawing code here
                sb.Begin();

                DrawFrame(sb, pos, effects);

                sb.End();
            }

            //base.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            if (this.IsPaused)
                Play();

            UpdateFrame(gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
        }

        // class AnimatedTexture
        public void DrawFrame(SpriteBatch batch, Vector2 screenPos, SpriteEffects effects)
        {
            DrawFrame(batch, Frame, screenPos, effects);
        }

        public void DrawFrame(SpriteBatch batch, int frame, Vector2 screenPos, SpriteEffects effects)
        {
            int FrameWidth = FrameTextures[frame].Width;// / framecount;
            
            //Rectangle sourcerect = new Rectangle(FrameWidth * frame, 0, FrameWidth, FrameTextures[frame].Height);
            Rectangle sourcerect = new Rectangle(FrameWidth, 0, FrameWidth, FrameTextures[frame].Height);

            batch.Draw(FrameTextures[frame], screenPos, null, Color.White, 0, screenPos, this.Scale, effects, 0);//, Rotation, Origin, Scale, SpriteEffects.None, Depth);
            //batch.Draw(FrameTextures[frame], screenPos, sourcerect, Color.White, Rotation, Origin, Scale, SpriteEffects.None, Depth);
        }

        public bool IsPaused
        {
            get { return Paused; }
        }
        public void Reset()
        {
            Frame = 0;
            TotalElapsed = 0f;
        }
        public void Stop()
        {
            Pause();
            Reset();
        }
        public void Play()
        {
            Paused = false;
        }
        public void Pause()
        {
            Paused = true;
        }

    }
}
