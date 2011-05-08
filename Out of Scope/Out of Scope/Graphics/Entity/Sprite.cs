using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Out_of_Scope.Graphics.Entity
{
    class Sprite : Graphics.GfxEntity
    {
        private Texture2D m_texture;
        private Rectangle m_dest;
        private Color m_colour;

        public void Init(Texture2D texture)
        {
            m_texture = texture;
            m_dest = new Rectangle( 0, 0, texture.Width, texture.Height );
            m_colour = Color.White;
        }

        public void Init(Texture2D texture, Vector2 position)
        {
            m_texture = texture;
            m_dest = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            m_colour = Color.White;
        }

        public void Init(Texture2D texture, Rectangle dest)
        {
            m_texture = texture;
            m_dest = dest;
            m_colour = Color.White;
        }

        public void Init( Texture2D texture, Rectangle dest, Color colour )
        {
            m_texture = texture;
            m_dest = dest;
            m_colour = colour;
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice device)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(m_texture, m_dest, m_colour);

            spriteBatch.End();
        }

        public Vector2 position
        {
            get { return new Vector2( m_dest.X, m_dest.Y ); }
            set 
            { 
                m_dest.X = (int)value.X; 
                m_dest.Y = (int)value.Y; 
            }
        }

        public Rectangle destination
        {
            get { return m_dest; }
            set { m_dest = value; }
        }

        public Color colour
        {
            get { return m_colour; }
            set { m_colour = value; }
        }
    }
}
