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
    class Entity_City : Entity
    {
        private List<Entity_Explosion> m_explosion;
        private bool m_dead = false;
        List<SoundEffect> m_explosion_sound;
        float m_explosion_start_sound = 0.0f;
        float m_explosion_start = 0.0f;

        public Entity_City(Graphics.Entity.Model model, List<Entity_Explosion> explosion, List<SoundEffect> explosion_sound)
        {
            m_model = model;
            m_position = new Vector3(0.0f, -100.0f, 0.0f);
            m_scale = Vector3.One;
            m_rotation = Vector3.Zero;

            m_explosion = explosion;

            m_explosion_sound = explosion_sound;

            m_explosion[0].position = Camera.position + new Vector3(0.0f, 0.0f, 300.0f);
            m_explosion[1].position = Camera.position + new Vector3(300.0f, 0.0f, 0.0f);
            m_explosion[2].position = Camera.position + new Vector3(0.0f, 0.0f, -300.0f);
            m_explosion[3].position = Camera.position + new Vector3(-300.0f, 0.0f, 0.0f);

            m_model.boundingbox_enabled = false;
        }

        public void explode(GameTime time)
        {
            if (!m_dead)
            {
                m_dead = true;
                m_explosion_start = (float)time.TotalGameTime.TotalMilliseconds;
                foreach (Entity_Explosion exp in m_explosion)
                {
                    exp.model.Visible = true;
                }
            }
            Random rand = new Random();
            if ((float)time.TotalGameTime.TotalMilliseconds - m_explosion_start_sound > rand.Next(1000, 3000) &&
                (float)time.TotalGameTime.TotalMilliseconds - m_explosion_start < 5000)
            {
                m_explosion_start_sound = (float)time.TotalGameTime.TotalMilliseconds;
                m_explosion_sound[rand.Next(0, 3)].Play();
            }
        }

        public void reset()
        {
            m_explosion[0].position = Camera.position + new Vector3(0.0f, 0.0f, 300.0f);
            m_explosion[0].model.Visible = false;
            m_explosion[1].position = Camera.position + new Vector3(300.0f, 0.0f, 0.0f);
            m_explosion[1].model.Visible = false;
            m_explosion[2].position = Camera.position + new Vector3(0.0f, 0.0f, -300.0f);
            m_explosion[2].model.Visible = false;
            m_explosion[3].position = Camera.position + new Vector3(-300.0f, 0.0f, 0.0f);
            m_explosion[3].model.Visible = false;
            m_dead = false;
        }

        public override void update(Visualisation vis, GameTime time, GraphicsDevice graphics, GameState gamestate)
        {
            if (m_dead)
            {
                foreach (Entity_Explosion exp in m_explosion)
                {
                    Vector3 move_distance = Camera.position - exp.position;
                    move_distance.Normalize();
                    exp.position += move_distance;
                }
            }

            base.update(vis, time, graphics, gamestate);
        }
    }
}
