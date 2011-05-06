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
        private List<Vector3> m_way_points;
        private Vector3 m_move_distance;
        private int m_active_way_point;

        public Entity_Enemy(Graphics.Entity.Model model)
        {
            m_model = model;
            m_position = new Vector3(0.0f, 0.0f, 0.0f);
            m_scale = new Vector3(0.05f, 0.05f, 0.05f);
            m_rotation = Vector3.Zero;
            m_active_way_point = 1;
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
            else
            {
                if (m_way_points.Count > 1)
                {
                    m_position += (m_move_distance * 0.001f);

                    if ((m_way_points[m_active_way_point] - m_position).Length() <= 1.0f)
                    {
                        m_active_way_point++;
                        if (m_active_way_point >= m_way_points.Count)
                            m_active_way_point = 0;

                        m_move_distance = m_way_points[m_active_way_point] - m_position;
                    }
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

        public List<Vector3> waypoint
        {
            set 
            { 
                m_way_points = value;
                if( m_way_points.Count > 0 )
                    m_move_distance = m_way_points[m_active_way_point] - m_position;
            }
        }
    }
}
