using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Love_and_Hate
{
    public class AnimatedSpriteManager //: DrawableGameComponent
    {
        static AnimatedSpriteManager mInstance = new AnimatedSpriteManager();
        static SpriteBatch mSB;
        List<AnimatedSpriteEx> mSprites = new List<AnimatedSpriteEx>();

        static public AnimatedSpriteManager Instance
        {
            get
            {
                return mInstance;
            }
        }

        public AnimatedSpriteManager()
            //: base(game)
        {
            
        }

        public void LoadContent(GraphicsDevice gd)
        {
            if (mSB == null)
                mSB = new SpriteBatch(gd);

            //base.LoadContent();
        }

        public void Initialize()
        {
            //base.Initialize();
        }

        public void Draw(GameTime gameTime)
        {
            if (mSB != null)
            {
                foreach (AnimatedSpriteEx s in mSprites)
                    s.Draw(mSB);
            }

            //base.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            List<AnimatedSpriteEx> ItemsToRemove = new List<AnimatedSpriteEx>();

            foreach (AnimatedSpriteEx s in mSprites)
            {
                if (s.Dispose)
                    ItemsToRemove.Add(s);

                s.Update(gameTime);
            }

            // Cleanup objects which are flagged for removal
            foreach (AnimatedSpriteEx s in ItemsToRemove)
                mSprites.Remove(s);

            //base.Update(gameTime);
        }

        public void Add(AnimatedSpriteEx sprite)
        {
            mSprites.Add(sprite);
        }
    }
}
