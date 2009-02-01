using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Love_and_Hate
{
    public class Player : Sprite
    {
        private class PlayerMergeList
        {
            List<Player> players = new List<Player>();

            void Add(Player p)
            {
                players.Add(p);
            }

            bool Find(Player p)
            {
                if (players.Contains(p))
                    return true;

                return false;
            }
        }

        // Merging properties
        private static bool m_bCaptainOfMerge = false;
        private static bool m_bIsMerged       = false;

        private static Dictionary<PlayerIndex, PlayerMergeList> m_PlayerMerges = 
            new Dictionary<PlayerIndex, PlayerMergeList>();

        private PlayerIndex m_id;
        
        public PlayerIndex id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        public Player(Game game) : base(game)
        {
            Trace.WriteLine("You're trying to instantiate a 'Player' class!  This is an abstract class.");
        }

        public Player(Game game, PlayerIndex id) : base(game)
        {
            this.m_id = id;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            float fPlayerBoundingRadius = Config.Instance.GetAsInt("PlayerBoundingRadius");

            if (fPlayerBoundingRadius == 0)
                fPlayerBoundingRadius = this.mSpriteTexture.Width / 4;

            this.mBounds =
                new BoundingSphere
                (
                    new Vector3(this.mPosition.X, this.mPosition.Y, 0),
                    fPlayerBoundingRadius
                );
           
            mScale.X = mPixelScale * 32;
            mScale.Y = mScale.X;            
        }

        public override void Update(GameTime gameTime)
        {                        
            foreach (Enemy e in Program.Instance.mEnemies)
            {
                if (CheckForCollision(e))
                {                   
                    e.Destroy();
                }
            }

            foreach (Player p in Program.Instance.GamePlayers)
            {
                if (CheckForCollision(p))
                {
                    // Case 1 - I am not merged with any other players
                 
                        // Has a captain been selected?
                            // If yes then add me to the list of merged players for this list
                            // If no then make me the captain

                    // Case 2 - I am merged with one or more players
                        
                        // Another player is trying to join the collective
                        
                        // All players in the collective will need to have their merge
                        // buttons pressed as well as the new player wanting to join to add
                        // the new player
                            
                        // If all players have their merge buttons pressed
                            // If yes then add the new player to the merged player list   
                        

                        //if (IsMergeButtonPressed() && p.IsMergeButtonPressed())
                        //{
                        Trace.WriteLine("Wonder twin powers activate!");

                        //if (m_bIsMerged == false)
                        //{
                        //    m_bCaptainOfMerge = true;
                        //    m_MergedPlayers.Add(p);
                        //}
                        //else
                          //  return;

                        //p.IsMerged     = true;
                        //m_MergedPlayers = p;
                        //}
                }
            }

            GamePadState state = GamePad.GetState(id);

            // Special handling for Player One
            if (id == PlayerIndex.One)
            {
                //if (!state.IsConnected)
                //{
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
                        
                        base.Update(gameTime);
                        return;
                    }
                //}
            }

            state.ThumbSticks.Left.Normalize();

            this.mPositionX += state.ThumbSticks.Left.X * Config.Instance.GetAsInt("PlayerSpeed");
            this.mPositionY += state.ThumbSticks.Left.Y * Config.Instance.GetAsInt("PlayerSpeed") * -1;

            base.Update(gameTime);
        }

        public bool CheckForCollision(Sprite obj)
        {
            return this.Bounds.Intersects(obj.Bounds);
        }

        bool IsButtonPressed(ButtonState btn)
        {
            if (btn == ButtonState.Pressed)
                return true;

            return false;
        }

        public bool IsMergeButtonPressed()
        {
            String sMergeBtn = Config.Instance.GetAsString("PlayerMergeBtn");

            if (!String.IsNullOrEmpty(sMergeBtn))
            {
                if (sMergeBtn.Equals("a", StringComparison.InvariantCultureIgnoreCase))
                    return IsButtonPressed(GamePad.GetState(this.id).Buttons.A);

                else if (sMergeBtn.Equals("b", StringComparison.InvariantCultureIgnoreCase))
                    return IsButtonPressed(GamePad.GetState(this.id).Buttons.B);

                else if (sMergeBtn.Equals("x", StringComparison.InvariantCultureIgnoreCase))
                    return IsButtonPressed(GamePad.GetState(this.id).Buttons.X);

                else if (sMergeBtn.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                    return IsButtonPressed(GamePad.GetState(this.id).Buttons.Y);

                Trace.WriteLine("Player::IsMergeButtonPressed - Merge button exists but not a valid x-box controll button");
            }

            Trace.WriteLine("Player::IsMergeButtonPressed - No player merge button detailed");
            return false;
        }

        protected override void SetAssetName()
        {
            this.mAssetName = "dot_black";

            base.SetAssetName();
        }
    }
}
