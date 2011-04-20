using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Out_of_Scope
{
    abstract class Entity
    {
        protected Graphics.Entity.Model m_model;
        protected Vector3 m_position, m_rotation, m_scale;

        public virtual void update(Visualisation vis, GameTime time, GraphicsDevice graphics)
        {

        }

        public Graphics.Entity.Model model
        {
            get { return m_model; }
        }

        public Vector3 position
        {
            get { return m_position; }
            set { m_position = value; }
        }

        public Vector3 rotation
        {
            get { return m_rotation; }
            set { m_rotation = value; }
        }

        public Vector3 scale
        {
            get { return m_scale; }
            set { m_scale = value; }
        }
    }
}
