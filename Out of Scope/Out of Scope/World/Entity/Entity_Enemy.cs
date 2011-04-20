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
    class Entity_Enemy : Entity
    {
        private bool m_is_dead = false;

        public Entity_Enemy(Graphics.Entity.Model model)
        {
            m_model = model;
            m_position = new Vector3(0.0f, 0.0f, 0.0f);
            m_scale = new Vector3(0.05f, 0.05f, 0.05f);
            m_rotation = Vector3.Zero;
        }

        public override void update(Visualisation vis, GameTime time, GraphicsDevice graphics)
        {
            if (m_is_dead)
            {
                if (m_rotation.X >= -MathHelper.PiOver2)
                {
                    m_rotation.X -= 0.05f;
                    m_position.Y += 0.01f;
                }
            }
            m_model.position = m_position;
            m_model.rotation = m_rotation;
            m_model.scale = m_scale;
            m_model.view = Camera.LookAt();
            m_model.projection = Camera.Projection(graphics);
        }

        public void Kill()
        {
            m_is_dead = true;
        }
    }
}
