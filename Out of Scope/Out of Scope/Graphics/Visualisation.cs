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
    class Visualisation
    {
        private GraphicsDevice m_graphics_device;
        private SpriteBatch m_sprite_batch;

        private Color m_backdrop_colour;

        private List<Graphics.GfxEntity> m_entity = new List<Graphics.GfxEntity>();
        private List<Graphics.Entity.Sprite> m_sprite = new List<Graphics.Entity.Sprite>();

        public Visualisation()
        {
            m_backdrop_colour = Color.CornflowerBlue;
        }

        public void Init(GraphicsDevice device)
        {
            m_graphics_device = device;
            m_sprite_batch = new SpriteBatch(m_graphics_device);
        }

        public void AddEntity(Graphics.GfxEntity entity)
        {
            m_entity.Add(entity);
        }

        public void AddEntity(Graphics.Entity.Sprite entity)
        {
            m_sprite.Add(entity);
        }

        public void Draw()
        {
            m_graphics_device.Clear(m_backdrop_colour);

            foreach (Graphics.GfxEntity entity in m_entity)
            {
                if( entity.Visible )
                    entity.Draw(m_sprite_batch, m_graphics_device);
            }

            foreach (Graphics.Entity.Sprite sprite in m_sprite)
            {
                if (sprite.Visible)
                    sprite.Draw(m_sprite_batch, m_graphics_device);
            }
        }
    }
}
