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
        public List<Enemy> mEnemies = new List<Enemy>();
        public int mEnemiesKilled = 0;
        public int mMaxEnemies = Config.Instance.GetAsInt("MaxEnemies");
        public Background mBackground;
        public LoadingScreen mLoadingScreen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = Config.Instance.GetAsInt("ScreenWidth");
            graphics.PreferredBackBufferHeight = Config.Instance.GetAsInt("ScreenHeight");
            //graphics.IsFullScreen = true;

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
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

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

            // Have two players collided?
            // If yes
              // Are the players' merge buttons pressed?
                // If yes then merge

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
                    break;
                case GameState.GameOver:
                    this.GameOverUpdate(gameTime);
                    break;
            }


            base.Update(gameTime);
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
            // if start game

            if (GamePad.GetCapabilities(PlayerIndex.One).IsConnected)
            {
                if (IsButtonPressed(GamePad.GetState(PlayerIndex.One).Buttons.A))
                {
                    mLoadingScreen.Destroy();
                    mBackground = new Background(this, this.Content);
                    m_GamePlayers = new List<Player>();

                    // Player one will always be added regardless of whether a controller is connected or not
                    //
                    m_GamePlayers.Add(new Player(this, PlayerIndex.One));

                    if (GamePad.GetCapabilities(PlayerIndex.Two).IsConnected)
                        m_GamePlayers.Add(new Player(this, PlayerIndex.Two));

                    if (GamePad.GetCapabilities(PlayerIndex.Three).IsConnected)
                        m_GamePlayers.Add(new Player(this, PlayerIndex.Three));

                    if (GamePad.GetCapabilities(PlayerIndex.Four).IsConnected)
                        m_GamePlayers.Add(new Player(this, PlayerIndex.Four));


                    mGameState = GameState.Game;
                }
            }
        }

        protected void GameUpdate(GameTime gameTime)
        {
            if (mEnemies.Count < mMaxEnemies)
            {
                mEnemies.Add(new Enemy(this, this.Content));
            }
        }

        protected void GameOverUpdate(GameTime gameTime)
        {
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
