using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Love_and_Hate
{
    public class Text : DrawableGameComponent
    {
        SpriteFont m_font;
        String     m_msg;
        Vector2    m_pos = new Vector2();
        bool m_addedToComponents = false;
        SpriteBatch sb;
        
        public Text(Game game, String spriteFontFileName) : base(game)
        {
            try
            {
                sb = new SpriteBatch(Game.GraphicsDevice);
                m_font = Game.Content.Load<SpriteFont>(String.Format("fonts/{0}", spriteFontFileName));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(String.Format("Text::Text - Error loading font '{0}'", spriteFontFileName));
            }
        }

        public void Print(float posX, float posY, String msg)
        {
            m_pos.X = posX;
            m_pos.Y = posY;
            m_msg = msg;

            if (!m_addedToComponents)
            {
                Game.Components.Add(this);
                m_addedToComponents = true;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            try
            {
                sb.Begin();
                sb.DrawString(m_font, m_msg, m_pos, Color.White);
                sb.End();

                base.Draw(gameTime);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }
    }
}