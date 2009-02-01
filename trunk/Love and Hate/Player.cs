using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Love_and_Hate
{
    abstract public class Player : Sprite
    {
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
            float fPlayerBoundingRadius = Config.Instance.GetAsInt("PlayerBoundingRadius");

            if (fPlayerBoundingRadius == 0)
                fPlayerBoundingRadius = this.mSpriteTexture.Width / 4;

            this.mBounds =
                new BoundingSphere
                (
                    new Vector3(this.mPosition.X, this.mPosition.Y, 0),
                    fPlayerBoundingRadius
                );            

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            List<Enemy> destroy = new List<Enemy>();

            foreach (Enemy e in Program.Instance.mEnemies)
            {
                if (CheckForCollision(e))
                {                   
                    e.Destroy();
                    destroy.Add(e);
                }
            }

            foreach (Enemy e in destroy)
            {
                Program.Instance.mEnemies.Remove(e);
            }


            base.Update(gameTime);
        }

        public bool CheckForCollision(Sprite obj)
        {
            return this.Bounds.Intersects(obj.Bounds);
        }
    }
}
