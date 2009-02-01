using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Love_and_Hate
{
    public class LoadingScreen : Sprite
    {
        public LoadingScreen(Game game, ContentManager theContentManager)
            : base(game)
        {            
        }

        protected override void SetAssetName()
        {
            mAssetName = "loadingscreen_1280_01";
        }
    }
}
