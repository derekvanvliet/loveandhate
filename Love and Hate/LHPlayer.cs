using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Love_and_Hate
{
    public class LHPlayer : Player
    {
        const String PLAYER_MERGE_BTN = "PlayerMergeBtn";

        public LHPlayer(Game game, PlayerIndex id): base(game, id)
        {

        }

        bool IsButtonPressed(ButtonState btn)
        {
            if (btn == ButtonState.Pressed)
                return true;

            return false;
        }

        public bool IsMergeButtonPressed()
        {
            String sMergeBtn = Config.Instance.GetAsString(PLAYER_MERGE_BTN);

            if ( !String.IsNullOrEmpty(sMergeBtn) )
            {
                if (sMergeBtn.Equals("a", StringComparison.InvariantCultureIgnoreCase))
                    return IsButtonPressed(GamePad.GetState(this.id).Buttons.A);

                else if (sMergeBtn.Equals("b", StringComparison.InvariantCultureIgnoreCase))
                    return IsButtonPressed(GamePad.GetState(this.id).Buttons.B);

                else if (sMergeBtn.Equals("x", StringComparison.InvariantCultureIgnoreCase))
                    return IsButtonPressed(GamePad.GetState(this.id).Buttons.X);

                else if (sMergeBtn.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                    return IsButtonPressed(GamePad.GetState(this.id).Buttons.Y);

                Trace.WriteLine("LHPlayer::IsMergeButtonPressed - Merge button exists but not a valid x-box controll button");
            }

            Trace.WriteLine("LHPlayer::IsMergeButtonPressed - No player merge button detailed");
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            if (IsMergeButtonPressed())
            {
                Trace.WriteLine(String.Format("Player {0} is ready to merge!", this.id));
            }

            GamePadState state = GamePad.GetState(id);

            // Special handling for Player One
            if (id == PlayerIndex.One)
            {
                if (!state.IsConnected)
                {
                    if (Keyboard.GetState().GetPressedKeys().Length > 0)
                    {
                        int moveY = Keyboard.GetState().IsKeyDown(Keys.W) ? -1 : 0;
                        int moveX = Keyboard.GetState().IsKeyDown(Keys.D) ? 1 : 0;

                        if (moveX == 0)
                            moveX = Keyboard.GetState().IsKeyDown(Keys.A) ? -1 : 0;

                        if (moveY == 0)
                            moveY = Keyboard.GetState().IsKeyDown(Keys.S) ? 1 : 0;

                        this.mPositionX += moveX * Config.Instance.GetAsInt("PlayerSpeed");
                        this.mPositionY += moveY * Config.Instance.GetAsInt("PlayerSpeed");

                        return;
                    }
                }
            }
            
            state.ThumbSticks.Left.Normalize();

            this.mPositionX += state.ThumbSticks.Left.X * Config.Instance.GetAsInt("PlayerSpeed");
            this.mPositionY += state.ThumbSticks.Left.Y * Config.Instance.GetAsInt("PlayerSpeed") * -1;               
 
            base.Update(gameTime);
        }

        protected override void SetAssetName()
        {
            this.mAssetName = "dot_black";

            base.SetAssetName();
        }

        public override void Initialize()
        {
            mPositionX = Config.Instance.GetAsInt("ScreenWidth") * 0.5f;
            mPositionY = Config.Instance.GetAsInt("ScreenHeight") * 0.5f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            mScale.X = mPixelScale * 32;
            mScale.Y = mScale.X;

        }
    }
}
