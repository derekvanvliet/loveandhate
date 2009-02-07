using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

public enum GameState
{
    Init,
    Title,
    Game,
    GameOver
}

namespace Love_and_Hate
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        private GameState mGameState;
        public List<Enemy> mEnemies;
        public int mEnemiesKilled = 0;
        public int mMaxEnemies = 0;
        public Background mBackground;
        public LoadingScreen mLoadingScreen;
        public int mGameStarted;
        public int mTimeLimit = Config.Instance.GetAsInt("TimeLimit") * 1000;
        public float mTimer;
        public Text mTime;
        public int mCurTime;
        public RankFirstNum rnum1;
        public RankSecondNum rnum2;
        public RankThirdNum rnum3;
        public RankP1 rp1;
        public RankP2 rp2;
        public RankP3 rp3;
        public RankP4 rp4;
        public PressA pressA;
        private bool bRestart;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = Config.Instance.GetAsInt("ScreenWidth");
            graphics.PreferredBackBufferHeight = Config.Instance.GetAsInt("ScreenHeight");
            graphics.IsFullScreen = false;

            mEnemies = new List<Enemy>();

            mMaxEnemies = Config.Instance.GetAsInt("MaxEnemies");

            m_GamePlayers = new List<Player>();

            bRestart = false;

            Content.RootDirectory = "Content";
        }

        private List<Player> m_GamePlayers;

        public List<Player> GamePlayers
        {
            get { return m_GamePlayers; }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {  
 			// TODO: Add your initialization logic here
            mGameState = GameState.Init;

           
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            //spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here          
            AudioManager.Instance.Load(this.Content, "\\audio\\Love and Hate.xgs");
            AnimatedSpriteManager.Instance.LoadContent(this.GraphicsDevice);            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        bool bIsMusicPlaying = false;

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

     		// TODO: Add your update logic here
            switch(mGameState)
            {
                case GameState.Init:
                    this.InitUpdate(gameTime);
                    break;
                case GameState.Title:
                    this.TitleUpdate(gameTime);
                    break;
                case GameState.Game:                                       
                    this.GameUpdate(gameTime);

                    if (!bIsMusicPlaying)
                    {
                        AudioManager.Instance.PlaySound("BackgroundMusic");
                        bIsMusicPlaying = true;
                    }

                    break;
                case GameState.GameOver:
                    this.GameOverUpdate(gameTime);
                    break;
            }

            base.Update(gameTime);

            AudioManager.Instance.Update();
            AnimatedSpriteManager.Instance.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        protected void InitUpdate(GameTime gameTime)
        {
            mLoadingScreen = new LoadingScreen(this, this.Content);
            mGameState = GameState.Title;
        }

        protected void TitleUpdate(GameTime gameTime)
        {
            bool bStartGame = false;

            // if start game
            if (GamePad.GetCapabilities(PlayerIndex.One).IsConnected)
            {
                if (IsButtonPressed(GamePad.GetState(PlayerIndex.One).Buttons.A))
                    bStartGame = true;
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    bStartGame = true;
            }

            if (bStartGame)
            {
                mLoadingScreen.Destroy();
                
                mBackground = new Background(this, this.Content);

                // Player one will always be added regardless of whether a controller is connected or not
                //
                if (GamePad.GetCapabilities(PlayerIndex.One).IsConnected)
                    m_GamePlayers.Add(new Player(this, PlayerIndex.One, false));

                if (GamePad.GetCapabilities(PlayerIndex.Two).IsConnected)
                    m_GamePlayers.Add(new Player(this, PlayerIndex.Two, false));

                if (GamePad.GetCapabilities(PlayerIndex.Three).IsConnected)
                    m_GamePlayers.Add(new Player(this, PlayerIndex.Three, false));

                if (GamePad.GetCapabilities(PlayerIndex.Four).IsConnected)
                    m_GamePlayers.Add(new Player(this, PlayerIndex.Four, false));

                if (m_GamePlayers.Count < 1)
                    m_GamePlayers.Add(new Player(this, PlayerIndex.One, true));                            

                if (m_GamePlayers.Count == 1)
                    m_GamePlayers.Add(new Player(this, PlayerIndex.Two, true));

                mGameState = GameState.Game;

                mGameStarted = gameTime.TotalGameTime.Milliseconds;
                mTimer = 0;

                mTime = new Text(this, "Courier");
                UpdateTimeText();
            }
        }

        protected void UpdateTimeText()
        {
            int newTime = (mTimeLimit - (int)mTimer) / 1000;
            if (mCurTime != newTime)
            {
                mCurTime = newTime;

                mTime.Print(640, 0, String.Format("{0}", newTime));
            }
        }

        protected void GameUpdate(GameTime gameTime)
        {
            mTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (mTimer < mTimeLimit)
            {
                UpdateTimeText();

                if (mEnemies.Count < mMaxEnemies)
                {
                    mEnemies.Add(new Enemy(this, this.Content));
                }
            }
            else
            {
                // game over
                int width = Config.Instance.GetAsInt("ScreenWidth");
                int height = Config.Instance.GetAsInt("ScreenHeight");

                rnum1 = new RankFirstNum(this, this.Content);
                rnum2 = new RankSecondNum(this, this.Content);
                rnum3 = new RankThirdNum(this, this.Content);
                pressA = new PressA(this, this.Content);

                rnum1.mPosition = new Vector2(width / 2 - 64, height / 2 - 64 - 32);
                rnum2.mPosition = rnum1.mPosition + new Vector2(0, 128);
                rnum3.mPosition = rnum2.mPosition + new Vector2(0, 128);
                pressA.mPosition = new Vector2(width - 128, 128);

                List<Player> lp = new List<Player>();
                List<Player> sorted = new List<Player>();
                List<Sprite> positions = new List<Sprite>();

                positions.Add(rnum1);
                positions.Add(rnum2);
                positions.Add(rnum3);

                foreach (Player p in GamePlayers)
                {
                    lp.Add(p);
                }

                while (lp.Count > 0)
                {
                    Player largest = null;

                    foreach (Player p in lp)
                    {
                        if (largest == null)
                        {
                            largest = p;
                        }
                        else if (p.PixelWidth > largest.PixelWidth)
                        {
                            largest = p;
                        }
                    }

                    sorted.Add(largest);
                    lp.Remove(largest);
                }
                
                for (int i = 0; i < sorted.Count; i++)
                {
                    Player p = sorted[i];
                    switch (p.id)
                    {
                        case PlayerIndex.One:
                            {
                                rp1 = new RankP1(this, this.Content);
                                rp1.mPositionX = positions[i].mPositionX + 128;
                                rp1.mPositionY = positions[i].mPositionY;
                                break;
                            }
                        case PlayerIndex.Two:
                            {
                                rp2 = new RankP2(this, this.Content);
                                rp2.mPositionX = positions[i].mPositionX + 128;
                                rp2.mPositionY = positions[i].mPositionY;
                                break;
                            }
                        case PlayerIndex.Three:
                            {
                                rp3 = new RankP3(this, this.Content);
                                rp3.mPositionX = positions[i].mPositionX + 128;
                                rp3.mPositionY = positions[i].mPositionY;
                                break;
                            }
                        case PlayerIndex.Four:
                            {
                                rp4 = new RankP4(this, this.Content);
                                rp4.mPositionX = positions[i].mPositionX + 128;
                                rp4.mPositionY = positions[i].mPositionY;
                                break;
                            }
                    }
                    if (i == 2)
                    {
                        break;
                    }
                }

                mGameState = GameState.GameOver;
            }
        }

        protected void GameOverUpdate(GameTime gameTime)
        {
            if (!bRestart)
            {
                if (GamePad.GetCapabilities(PlayerIndex.One).IsConnected)
                {
                    if (IsButtonPressed(GamePad.GetState(PlayerIndex.One).Buttons.A))
                        bRestart = true;
                }
                else
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.A))
                        bRestart = true;
                }
            }
            else
            {
                if (GamePad.GetCapabilities(PlayerIndex.One).IsConnected)
                {
                    if (!IsButtonPressed(GamePad.GetState(PlayerIndex.One).Buttons.A))
                        Restart();
                }
                else
                {
                    if (!Keyboard.GetState().IsKeyDown(Keys.A))
                        Restart();
                }
            }
        }

        public void Restart()
        {
            bRestart = false;
            mBackground.Destroy();

            foreach (Player p in m_GamePlayers)
            {
                p.Destroy();
            }

            m_GamePlayers = new List<Player>();

            foreach (Enemy e in mEnemies)
            {
                e.Destroy();
            }

            mEnemies = new List<Enemy>();

            mMaxEnemies = Config.Instance.GetAsInt("MaxEnemies");


            if(rnum1 != null)
                rnum1.Destroy();
            if(rnum2 != null)
                rnum2.Destroy();
            if(rnum3 != null)
                rnum3.Destroy();
            if(rp1 != null)
                rp1.Destroy();
            if(rp2 != null)
                rp2.Destroy();
            if(rp3 != null)
                rp3.Destroy();
            if(rp4 != null)
                rp4.Destroy();

            mTime.Dispose();

            mGameState = GameState.Init;
        }

        public Enemy GetSmallestEnemy()
        {
            Enemy smallest = null;

            foreach (Enemy e in mEnemies)
            {
                if (smallest == null)
                {
                    smallest = e;
                }
                else if (e.PixelWidth < smallest.PixelWidth)
                {
                    smallest = e;
                }
            }

            return smallest;
        }

        public Player GetSmallestPlayer()
        {
            Player smallest = null;

            foreach (Player p in GamePlayers)
            {
                if (smallest == null)
                {
                    smallest = p;
                }
                else if (p.PixelWidth < smallest.PixelWidth)
                {
                    smallest = p;
                }
            }

            return smallest;
        }
        public Enemy GetLargestEnemy()
        {
            Enemy largest = null;

            foreach (Enemy e in mEnemies)
            {
                if (largest == null)
                {
                    largest = e;
                }
                else if (e.PixelWidth > largest.PixelWidth)
                {
                    largest = e;
                }
            }

            return largest;
        }

        public Player GetLargestPlayer()
        {
            Player largest = null;

            foreach (Player p in GamePlayers)
            {
                if (largest == null)
                {
                    largest = p;
                }
                else if (p.PixelWidth > largest.PixelWidth)
                {
                    largest = p;
                }
            }

            return largest;
        }
        bool IsButtonPressed(ButtonState btn)
        {
            if (btn == ButtonState.Pressed)
                return true;

            return false;
        }

    }
}
