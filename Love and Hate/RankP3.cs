using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Love_and_Hate
{
    public class RankP3 : Sprite
    {
        public RankP3(Game game, ContentManager theContentManager)
            : base(game)
        {
        }

        protected override void SetAssetName()
        {
            mAssetName = "p3_idle_0000";
        }
    }
}
