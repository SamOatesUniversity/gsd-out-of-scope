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
    static class Enemy_Container
    {
        private static List<Entity_Enemy> m_enemy = new List<Entity_Enemy>();

        public static void Init(World world, ContentManager content)
        {
            for (int i = 0; i < GamePlay.enemy_count; i++)
            {
                Graphics.Entity.Model model = new Graphics.Entity.Model();
                model.Init(content.Load<Model>("enemy/enemy"));
                Entity_Enemy new_enemy = new Entity_Enemy(model);
                new_enemy.position = GamePlay.enemy_position(i);
                m_enemy.Add(new_enemy);
                world.AddEntity(new_enemy);
            }
        }

        public static List<Entity_Enemy> enemies()
        {
            return m_enemy;
        }
    }
}
