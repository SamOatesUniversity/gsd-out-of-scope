using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Out_of_Scope
{
    class Entity_City : Entity
    {
        public Entity_City(Graphics.Entity.Model model)
        {
            m_model = model;
            m_position = new Vector3(0.0f, -100.0f, 0.0f);
            m_scale = Vector3.One;
            m_rotation = Vector3.Zero;

            m_model.boundingbox_enabled = false;
        }

        public override void update(Visualisation vis, GameTime time, GraphicsDevice graphics)
        {
            m_model.position = m_position;
            m_model.rotation = m_rotation;
            m_model.scale = m_scale;
            m_model.view = Camera.LookAt();
            m_model.projection = Camera.Projection(graphics);
        }
    }
}
