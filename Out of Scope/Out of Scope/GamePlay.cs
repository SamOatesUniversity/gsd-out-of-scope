using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Out_of_Scope
{
    static class GamePlay
    {
        private static List<Vector3> m_spawn_point = new List<Vector3>();
        private static List<List<Vector3>> m_enemy_point = new List<List<Vector3>>();
        private static int m_mode = 0;

        public static void Init()
        {
            m_spawn_point.Add(new Vector3(-45.74f, -31.86f, 65.0f));
            m_enemy_point.Add(new List<Vector3>());
            m_enemy_point[0].Add(new Vector3(-298.0f, -92.0f, 108.9f));
            m_enemy_point[0].Add(new Vector3(-66.0f, -90.0f, 182.1f));
            m_enemy_point[0].Add(new Vector3(-88.9f, -27.5f, -26.4f));
            m_enemy_point[0].Add(new Vector3(86.0f, -80.8f, 92.0f));
            m_enemy_point[0].Add(new Vector3(-155.5f, -94.1f, 455.3f));
            m_enemy_point[0].Add(new Vector3(99.5f, -59.3f, 20.2f));


            m_spawn_point.Add(new Vector3(-302.21f, -69.70f, 147.64f));
            m_enemy_point.Add(new List<Vector3>());
            m_enemy_point[1].Add(new Vector3(0.0f, 0.0f, 0.0f));


            //Random rand = new Random();
            //m_mode = rand.Next(0, m_spawn_point.Count);
        }

        public static Vector3 enemy_position( int id )
        {
            return m_enemy_point[m_mode][id];
        }

        public static Vector3 spawn_point
        {
            get { return m_spawn_point[m_mode]; }
        }

        public static int enemy_count
        {
            get { return m_enemy_point[m_mode].Count; }
        }
    }
}
