using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace Love_and_Hate
{
    public class Player : Sprite
    {
        public enum ePlayerState
        {
            WALK = 0,
            IDLE,
            RUN
        }

        private ePlayerState mState = ePlayerState.IDLE;

        public ePlayerState PlayerState 
        {
            get { return mState;  }
            set { mState = value; }
        }

        public int mLevel = 1;
        private float mMaxSpeed = 0f;
        Vector2 mVelocity = new Vector2();

        private class PlayerMergeList
        {
            public List<Player> players = new List<Player>();

            public void Add(Player p)
            {
                players.Add(p);
            }

            public bool Find(Player p)
            {
                if (players.Contains(p))
                    return true;

                return false;
            }

            public bool AreAllPlayersReadyToMerge()
            {
                foreach (Player p in players)
                {
                    if (!p.IsMergeButtonPressed())
                        return false;
                }

                return true;
            }

            public bool IsAnyoneTryingToBreakTheMerge()
            {
                foreach (Player p in players)
                {
                    if ( p.IsMergeBreakButtonPressed() )
                        return true;
                }

                return false;
            }
        }

        float moveX = 0;
        float moveY = 0;

        // Merging properties
        private bool m_bIsMerged = false;

        public bool IsMerged
        {
            get { return m_bIsMerged; }
            set { m_bIsMerged = value; }
        }

        // Index on the dictionary is the merge captain
        private static Dictionary<PlayerIndex, PlayerMergeList> m_PlayerMerges = 
            new Dictionary<PlayerIndex, PlayerMergeList>();

        private static bool IsThisPlayerCaptain(Player p)
        {
            if (m_PlayerMerges.ContainsKey(p.id))
                return true;

            return false;
        }

        private static void RemoveMergeList(PlayerIndex captain)
        {
            if ( m_PlayerMerges.ContainsKey(captain) )
            {
                foreach ( Player p in m_PlayerMerges[captain].players )
                {
                    p.IsMerged = false;
                }

                m_PlayerMerges[captain].players.Clear();
                m_PlayerMerges.Remove(captain);
            }
        }

        AnimatedSprite m_idleFrontAnim;
        AnimatedSprite m_runAnim;        

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

			Reset(32);

            int iPlayerFrameRate = Config.Instance.GetAsInt("PlayerFrameRate");

            switch (id)
            {
                case PlayerIndex.One:
                    {
                        this.m_idleFrontAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player01\\idle\\p1_idle", 8, iPlayerFrameRate);
                        this.m_runAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player01\\run\\p1_run", 8, iPlayerFrameRate);
                        break;
                    }
                case PlayerIndex.Two:
                    {
                        this.m_idleFrontAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player\\IdleFront\\p2_idle", 8, iPlayerFrameRate);
                        this.m_runAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player\\RunLeft\\p2_run", 8, iPlayerFrameRate);
                        break;
                    }
                case PlayerIndex.Three:
                    {
                        this.m_idleFrontAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player\\IdleFront\\p3_idle", 8, iPlayerFrameRate);
                        this.m_runAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player\\RunLeft\\p3_run", 8, iPlayerFrameRate);
                        break;
                    }
                case PlayerIndex.Four:
                    {
                        this.m_idleFrontAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player\\IdleFront\\p4_idle", 8, iPlayerFrameRate);
                        this.m_runAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player\\RunLeft\\p4_run", 8, iPlayerFrameRate);
                        break;
                    }
            }

        }

        public override void Draw(GameTime gameTime)
        {
            switch (this.PlayerState)
            {
                case ePlayerState.IDLE:
                    this.m_idleFrontAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                    break;

                case ePlayerState.RUN:
                    {

                        if (mVelocity.X > 10)
                            this.m_runAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                        else if (mVelocity.X < -10)
                            this.m_runAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                        else if (mVelocity.Y > 10)
                            this.m_runAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                        else if (mVelocity.Y < -10)
                            this.m_runAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                        else
                            this.m_idleFrontAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);

                        break;
                    }
            }                

            base.Draw(gameTime);           
        }

        public void SetState()
        {
            if (IsStopped())
                this.PlayerState = ePlayerState.IDLE;
            else
                this.PlayerState = ePlayerState.RUN;
        }

        public override void Update(GameTime gameTime)
        {
            SetState();

            
            switch(this.PlayerState)
            {
                case ePlayerState.IDLE:
                    this.m_idleFrontAnim.Update(gameTime);
                    break;

                case ePlayerState.RUN:
                    this.m_runAnim.Update(gameTime);
                    break;
            }
            
            // Merging code

            float mls = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            if ( this.IsMerged  )
            {
                if (!Player.IsThisPlayerCaptain(this))
                {
                    base.Update(gameTime);
                    return;
                }
                else
                {
                    if (m_PlayerMerges.ContainsKey(this.id))
                    {
                        // Check if someone is trying to break the merge which
                        // anyone in the merge group is allowed to do
                        if (m_PlayerMerges[this.id].IsAnyoneTryingToBreakTheMerge() || IsMergeBreakButtonPressed())
                        {
                            // If yes then this captain and its merge need to be removed from our 
                            // merge list
                            Player.RemoveMergeList(this.id);

                            this.IsMerged = false;
                        }
                    }
                }
            }

            List<Enemy> destroy = new List<Enemy>();

            foreach (Enemy e in Program.Instance.mEnemies)
            {

                if (CheckForCollision(e))
                {
                    if (e.PixelWidth < this.PixelWidth)
                    {
                        e.Destroy();
                        destroy.Add(e);

                        Vector2 pos = mPosition;

                        PixelWidth += 5;
                        PixelHeight += 5;


                        //mScale.X *= 1.1f;
                        //mScale.Y = mScale.X;
                        mPosition = pos;
                        mBounds.Radius = Radius;
                        mBounds.Center.X = mPositionX;
                        mBounds.Center.Y = mPositionY;

                        this.m_idleFrontAnim.Scale = mScale.X;
                        this.m_runAnim.Scale = mScale.X;

                        Program.Instance.mEnemiesKilled++;
                        if (Program.Instance.mEnemiesKilled % 3 == 0)
                        {
                            if (Program.Instance.mMaxEnemies < 100)
                                Program.Instance.mMaxEnemies++;
                        }
                    }
                    else
                    {
                        e.Destroy();
                        destroy.Add(e);

                        Reset(32);
                    }
                }
            }

            foreach (Enemy e in destroy)
            {
                Program.Instance.mEnemies.Remove(e);
            }

            foreach (Player p in Program.Instance.GamePlayers)
            {
                // Don't bother checking if this player has collided with itself
                //
                if (p.id == this.id)
                    continue;

                if (CheckForCollision(p))
                {
                    // Case 1 - I am not merged with any other players
                    if (!IsMerged)
                    {
                        //if (IsMergeButtonPressed() && p.IsMergeButtonPressed())
                        //{
                            m_bIsMerged = true;

                            // Has a captain been selected?
                            // If yes then add me to the list of merged players for this list
                            if (m_PlayerMerges.ContainsKey(this.id))
                            {
                                m_PlayerMerges[this.id].Add(p);
                            }
                            // If no then make me the captain
                            else
                            {
                                m_PlayerMerges[this.id] = new PlayerMergeList();
                                
                                p.IsMerged = true;
                                m_PlayerMerges[this.id].Add(p);
                                
                            }
                        //}
                    }

                    // Case 2 - I am merged with one or more players
                    else
                    {
                        // Another player is trying to join the collective.  Since all parties involved in the
                        // collision will send a collision event only the captain should check if members
                        // of its list have merge keys pressed.  To avoid doing too much processing.
                        //
                       if (Player.IsThisPlayerCaptain(this))
                       {    
                           // Checked if this player is already merged in with this captain.  If so
                           // then don't bother checking if we should merge it.
                           //
                           if (!p.IsMerged)
                           {
                               //bool bAllPlayersReadyToMerge = false;
                               //foreach (Player pMerged in Player.m_PlayerMerges[this.id].players)
                               //{
                                   // All players in the collective will need to have their merge
                                   // buttons pressed as well as the new player wanting to join to add
                                   // the new player

                                   // If all players have their merge buttons pressed
                                   // If yes then add the new player to the merged player list   
                                   //if (!pMerged.IsMergeButtonPressed())
                                   //{
                                     //  bAllPlayersReadyToMerge = false;
                                       //break;
                                   //}
                               //}

                               // Check if all the players who are already merged and the new merged player have their keys
                               // pressed to merge.  If they do then merge them!
                               //
                               if (Player.m_PlayerMerges[this.id].AreAllPlayersReadyToMerge() && p.IsMergeButtonPressed())
                                   Player.m_PlayerMerges[this.id].Add(p);
                           }
                       }
                    }
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
                        this.moveY = Keyboard.GetState().IsKeyDown(Keys.W) ? -1 : 0;
                        this.moveX = Keyboard.GetState().IsKeyDown(Keys.D) ? 1 : 0;

                        if (moveX == 0)
                            moveX = Keyboard.GetState().IsKeyDown(Keys.A) ? -1 : 0;

                        if (moveY == 0)
                            moveY = Keyboard.GetState().IsKeyDown(Keys.S) ? 1 : 0;

                        mVelocity.X += mls * (moveX * 1000);
                        mVelocity.Y += mls * (moveY * 1000);

                        Vector2 drag = new Vector2(-mVelocity.X, -mVelocity.Y);
                        if (drag.Length() != 0)
                        {
                            drag.Normalize();
                            mVelocity = mVelocity + mls * (drag * (mVelocity.Length()*2));
                        }

                        if (mVelocity.Length() > mMaxSpeed)
                        {
                            mVelocity.Normalize();
                            mVelocity = mVelocity * mMaxSpeed;
                        }

                        // set position
                        mPositionX = mPosition.X + mls * mVelocity.X;
                        mPositionY = mPosition.Y + mls * mVelocity.Y;

                        if (mPositionX + Radius > Config.Instance.GetAsInt("ScreenWidth"))
                            mPositionX = Config.Instance.GetAsInt("ScreenWidth") - Radius;
                        if (mPositionX - Radius < 0)
                            mPositionX = Radius;
                        if (mPositionY + Radius > Config.Instance.GetAsInt("ScreenHeight"))
                            mPositionY = Config.Instance.GetAsInt("ScreenHeight") - Radius;
                        if (mPositionY - Radius < 0)
                            mPositionY = Radius;

                        base.Update(gameTime);
                        return;
                    }
                //}
            }

            this.moveX = state.ThumbSticks.Left.X;
            this.moveY = state.ThumbSticks.Left.Y;

            state.ThumbSticks.Left.Normalize();

            this.mVelocity.X += mls * (state.ThumbSticks.Left.X*1000);
            this.mVelocity.Y += mls * (-state.ThumbSticks.Left.Y*1000);

            Vector2 drag2 = new Vector2(-mVelocity.X, -mVelocity.Y);
            if (drag2.Length() != 0)
            {
                drag2.Normalize();
                mVelocity = mVelocity + mls * (drag2 * (mVelocity.Length() * 2));
            }

            if (mVelocity.Length() > mMaxSpeed)
            {
                mVelocity.Normalize();
                mVelocity = mVelocity * mMaxSpeed;
            }

            // set position
            mPositionX = mPosition.X + mls * mVelocity.X;
            mPositionY = mPosition.Y + mls * mVelocity.Y;

            if (mPositionX + Radius > Config.Instance.GetAsInt("ScreenWidth"))
                mPositionX = Config.Instance.GetAsInt("ScreenWidth") - Radius;
            if (mPositionX - Radius < 0)
                mPositionX = Radius;
            if (mPositionY + Radius > Config.Instance.GetAsInt("ScreenHeight"))
                mPositionY = Config.Instance.GetAsInt("ScreenHeight") - Radius;
            if (mPositionY - Radius < 0)
                mPositionY = Radius;

            base.Update(gameTime);
        }

        public bool IsStopped()
        {
            if (mVelocity.Length() < 25)
                return true;

            return false;
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

        public bool IsMergeBreakButtonPressed()
        {
            String sMergeBtn = Config.Instance.GetAsString("PlayerBreakMergeBtn");

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

 		public override void Initialize()
        {
            ResetPosition();
            base.Initialize();
        }
	
 		public void Damage()
        {
            mLevel = 1;
        }

        public void Reset(int newPixelWidth)
        {
            mScale.X = mPixelScale * newPixelWidth;
            mScale.Y = mScale.X;
            if (m_idleFrontAnim != null)
            {
                this.m_idleFrontAnim.Scale = mScale.X;
                this.m_runAnim.Scale = mScale.X;
            }
            mMaxSpeed = 5000f / PixelWidth;

            float fPlayerBoundingRadius = this.Radius;

            if (this.mBounds == null)
            {
                this.mBounds =
                    new BoundingSphere
                    (
                        new Vector3(this.mPosition.X, this.mPosition.Y, 0),
                        fPlayerBoundingRadius
                    );
            }
            else
            {
                mBounds.Center.X = mPositionX;
                mBounds.Center.Y = mPositionY;

                mBounds.Radius = Radius;
            }
        }

        public void ResetPosition()
        {
            int width = Config.Instance.GetAsInt("ScreenWidth");
            int height = Config.Instance.GetAsInt("ScreenHeight");

            switch (id)
            {
                case PlayerIndex.One:
                    {
                        mPosition = new Vector2(width * 0.5f * 0.5f, height * 0.5f * 0.5f);
                        break;
                    }
                case PlayerIndex.Two:
                    {
                        mPosition = new Vector2(width * 0.5f + width * 0.5f * 0.5f, height * 0.5f * 0.5f);
                        break;
                    }
                case PlayerIndex.Three:
                    {
                        mPosition = new Vector2(width * 0.5f * 0.5f, height * 0.5f + height * 0.5f * 0.5f);
                        break;
                    }
                case PlayerIndex.Four:
                    {
                        mPosition = new Vector2(width * 0.5f + width * 0.5f * 0.5f, height * 0.5f + height * 0.5f * 0.5f);
                        break;
                    }
            }
        }
    }
}
