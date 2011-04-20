using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Out_of_Scope
{
    abstract class Sprite
    {
        protected Graphics.Entity.Sprite m_sprite;
        protected Vector2 m_position, m_scale;
        protected float m_rotation;

        public virtual void update(Visualisation vis, GameTime time, GraphicsDevice graphics)
        {

        }

        public Graphics.Entity.Sprite sprite
        {
            get { return m_sprite; }
        }
    }
}
