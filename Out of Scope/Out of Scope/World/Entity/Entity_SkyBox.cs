﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Out_of_Scope
{
    class Entity_SkyBox : Entity
    {
        public Entity_SkyBox(Graphics.Entity.Model model)
        {
            m_model = model;
            m_position = new Vector3(0.0f, 0.0f, 0.0f);
            m_scale = new Vector3(10.0f, 10.0f, 10.0f);
            m_rotation = Vector3.Zero;
        }

        public override void update(Visualisation vis, GameTime time, GraphicsDevice graphics, GameState gamestate)
        {
            base.update(vis, time, graphics, gamestate);
        }
    }
}
