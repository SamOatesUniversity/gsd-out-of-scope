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
    static class Enemy_Container
    {
        private static List<Entity_Enemy> m_enemy = new List<Entity_Enemy>();
        

        public static void Init(World world, ContentManager content)
        {
            Random rand = new Random();

            for (int i = 0; i < GamePlay.enemy_count; i++)
            {
                Graphics.Entity.Model model = new Graphics.Entity.Model();
                model.Init(content.Load<Model>("enemy/enemy"));
                Entity_Enemy new_enemy = new Entity_Enemy(model);

                new_enemy.setSoundKill = content.Load<SoundEffect>("crit_death" + rand.Next(1, 5));
                new_enemy.position = GamePlay.get_enemy(i).Position;
                new_enemy.rotation = GamePlay.get_enemy(i).Rotation;
                new_enemy.waypoint = GamePlay.get_enemy(i).WayPoint;
                m_enemy.Add(new_enemy);
                world.AddEntity(new_enemy);
            }
        }

        public static List<Entity_Enemy> enemies()
        {
            return m_enemy;
        }

        public static void reset()
        {
            for (int i = 0; i < m_enemy.Count; i++)
            {
                m_enemy[i].position = GamePlay.get_enemy(i).Position;
                m_enemy[i].rotation = GamePlay.get_enemy(i).Rotation;
                m_enemy[i].waypoint = GamePlay.get_enemy(i).WayPoint;
            }           
        }
    }
}
