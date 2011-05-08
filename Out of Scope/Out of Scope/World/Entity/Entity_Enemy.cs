using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Out_of_Scope
{
    enum enemyState
    {
        Standing,
        Walking,
        Dead
    };

    class Entity_Enemy : Entity
    {
        private enemyState m_state = enemyState.Standing;
        private List<Vector3> m_way_points;
        private Vector3 m_move_distance;
        private int m_active_way_point;
        private float m_rot_offset;
        float m_destination_angle;

        SoundEffect m_die_sound;

        public Entity_Enemy(Graphics.Entity.Model model)
        {
            m_model = model;
            m_position = new Vector3(0.0f, 0.0f, 0.0f);
            m_scale = new Vector3(0.05f, 0.05f, 0.05f);
            m_rotation = Vector3.Zero;
            m_active_way_point = 1;
            m_rot_offset = 0.0f;
        }

        public override void update(Visualisation vis, GameTime time, GraphicsDevice graphics, GameState gamestate)
        {
            switch (m_state)
            {
                case enemyState.Standing :

                    break;
                case enemyState.Walking:
                    if (m_way_points.Count > 1)
                    {
                        m_position += (m_move_distance * (0.08f * GamePlay.difficulty.GetHashCode()) );

                        float length = (m_way_points[m_active_way_point] - m_position).Length();

                        m_rotation.Y = m_destination_angle;

                        if (length <= 1.0f)
                        {
                            m_active_way_point++;
                            if (m_active_way_point >= m_way_points.Count)
                                m_active_way_point = 0;

                            m_move_distance = m_way_points[m_active_way_point] - m_position;
                            m_move_distance.Normalize();

                            m_rot_offset = Vector3.Dot(m_move_distance, Vector3.Forward) < 0 ? 0.0f : (float)Math.PI;

                            m_destination_angle = (float)Math.Atan((m_position.X - m_way_points[m_active_way_point].X)
                                                        / (m_position.Z - m_way_points[m_active_way_point].Z)) + m_rot_offset;

                        }
                    }
                    else
                    {
                        m_state = enemyState.Standing;
                    }
                    break;
                case enemyState.Dead:
                    if (m_rotation.X >= -MathHelper.PiOver2)
                    {
                        m_rotation.X -= 0.05f;
                        m_position.Y += 0.01f;
                    }
                    break;
            }

            base.update(vis, time, graphics, gamestate);
        }

        public void Kill()
        {
            m_state = enemyState.Dead;
            float distance = (Camera.position - m_position).Length();
            distance = ( 250.0f / distance );
            distance = distance > 1.0f ? 1.0f : distance;
            m_die_sound.Play(distance * 0.5f, 0.0f, 0.0f);
        }

        public SoundEffect setSoundKill
        {
            set { m_die_sound = value; }
        }

        public List<Vector3> waypoint
        {
            set 
            { 
                m_way_points = value;
                if (m_way_points.Count > 0)
                {
                    m_active_way_point = 1;
                    m_move_distance = m_way_points[m_active_way_point] - m_position;
                    m_move_distance.Normalize();

                    m_rot_offset = Vector3.Dot(m_move_distance, Vector3.Forward) < 0 ? 0.0f : (float)Math.PI;

                    m_destination_angle = (float)Math.Atan((m_position.X - m_way_points[m_active_way_point].X)
                                                / (m_position.Z - m_way_points[m_active_way_point].Z)) + m_rot_offset;

                    m_state = enemyState.Walking;
                }
                else
                {
                    m_state = enemyState.Standing;
                }
            }
        }
    }
}
