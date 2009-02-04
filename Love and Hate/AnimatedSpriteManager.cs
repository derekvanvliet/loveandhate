using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Love_and_Hate
{
    public class AnimatedSpriteManager : DrawableGameComponent
    {
        static SpriteBatch mSB;
        List<AnimatedSpriteEx> mSprites = new List<AnimatedSpriteEx>();

        public AnimatedSpriteManager(Game game)
            : base(game)
        {

        }

        protected override void LoadContent()
        {
            if (mSB == null)
                mSB = new SpriteBatch(this.GraphicsDevice);

            base.LoadContent();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            if (mSB != null)
            {
                foreach (AnimatedSpriteEx s in mSprites)
                    s.Draw(mSB);
            }

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
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

            base.Update(gameTime);
        }

        public void Add(AnimatedSpriteEx sprite)
        {
            mSprites.Add(sprite);
        }
    }
}
