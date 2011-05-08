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
    class Entity_Explosion : Entity
    {
        public Entity_Explosion(Graphics.Entity.Model model)
        {
            m_model = model;
            m_position = new Vector3();
            m_scale = new Vector3(4.0f, 4.0f, 4.0f);
            m_rotation = new Vector3(0.0f, (float)Math.PI, 0.0f);
            m_model.Visible = false;
        }

        public override void update(Visualisation vis, GameTime time, GraphicsDevice graphics, GameState gamestate)
        {
            //point plain at camera...
            Random rand = new Random();
            m_rotation.Z += rand.Next(100) * 0.01f;

            m_scale = new Vector3(rand.Next(1000) * 0.01f, rand.Next(1000) * 0.01f, 1.0f);

            base.update(vis, time, graphics, gamestate);
        }
    }
}
