using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Out_of_Scope
{
    class World
    {
        Visualisation m_vis;
        private List<Entity> m_entity = new List<Entity>();
        private List<Sprite> m_sprite = new List<Sprite>();

        public World(Visualisation visualisation)
        {
            m_vis = visualisation;
        }

        public void AddEntity(Entity entity)
        {
            m_vis.AddEntity(entity.model);
            m_entity.Add(entity);
        }

        public void AddEntity(Sprite entity)
        {
            m_vis.AddEntity(entity.sprite);
            m_sprite.Add(entity);
        }

        public void Update(GameTime time, GraphicsDevice graphics)
        {
            foreach (Sprite entity in m_sprite)
            {
                entity.update(m_vis, time, graphics);
            }
            foreach (Entity entity in m_entity)
            {
                entity.update(m_vis, time, graphics);
            }
        }
    }
}
