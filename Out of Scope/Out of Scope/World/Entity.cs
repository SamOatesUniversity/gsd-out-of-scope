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

        public virtual void update(Visualisation vis, GameTime time, GraphicsDevice graphics, GameState gamestate)
        {
            m_model.position = m_position;
            m_model.rotation = m_rotation;
            m_model.scale = m_scale;
            m_model.view = Camera.LookAt();
            m_model.projection = Camera.Projection(graphics);
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
