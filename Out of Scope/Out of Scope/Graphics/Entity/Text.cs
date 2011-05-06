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
    class Text : Sprite
    {
        private SpriteFont m_font;
        private String m_text;
        private Vector2 m_position;
        private Color m_colour;

        public void Init(SpriteFont font)
        {
            m_font = font;
            m_text = "";
            m_position = Vector2.Zero;
            m_colour = Color.White;
        }

        public void Init(SpriteFont font, String text)
        {
            m_font = font;
            m_text = text;
            m_position = Vector2.Zero;
            m_colour = Color.White;
        }

        public void Init(SpriteFont font, String text, Vector2 position)
        {
            m_font = font;
            m_text = text;
            m_position = position;
            m_colour = Color.White;
        }

        public void Init(SpriteFont font, String text, Vector2 position, Color colour)
        {
            m_font = font;
            m_text = text;
            m_position = position;
            m_colour = colour;
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice device)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(m_font, m_text, m_position, m_colour);

            spriteBatch.End();

            device.BlendState = BlendState.Opaque;
            device.DepthStencilState = DepthStencilState.Default;
            device.SamplerStates[0] = SamplerState.LinearWrap;

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.CullClockwiseFace;
            device.RasterizerState = rs;
        }

        //public Vector2 position
        //{
        //    get { return m_position; }
        //    set { position = value; }
        //}

        public String text
        {
            get { return m_text; }
            set { m_text = value; }
        }

        //public Color colour
        //{
        //    get { return m_colour; }
        //    set { m_colour = value; }
        //}

        public SpriteFont font
        {
            get { return m_font; }
            set { m_font = value; }
        }

    }
}
