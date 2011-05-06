using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Out_of_Scope
{
    class EnemyData
    {
        public Vector3 Position = new Vector3();
        public Vector3 Rotation = new Vector3();
        public List<Vector3> WayPoint = new List<Vector3>();

        public EnemyData(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
    
    static class GamePlay
    {
        private static List<Vector3> m_spawn_point = new List<Vector3>();
        private static List<List<EnemyData>> m_enemy_point = new List<List<EnemyData>>();
        private static int m_mode = 0;

        public static void Init()
        {
            m_spawn_point.Add(new Vector3(-45.74f, -31.86f, 65.0f));

            m_enemy_point.Add(new List<EnemyData>());
            m_enemy_point[0].Add(new EnemyData( new Vector3(-298.0f, -92.0f, 108.9f),   new Vector3() ));
            m_enemy_point[0].Add(new EnemyData( new Vector3(-66.0f, -90.0f, 182.1f),    new Vector3() ));
            m_enemy_point[0].Add(new EnemyData( new Vector3(-88.9f, -27.5f, -26.4f),    new Vector3() ));
            m_enemy_point[0].Add(new EnemyData( new Vector3(86.0f, -80.8f, 92.0f),      new Vector3() ));
            m_enemy_point[0].Add(new EnemyData( new Vector3(-155.5f, -94.1f, 455.3f),   new Vector3() ));
            m_enemy_point[0].Add(new EnemyData( new Vector3(99.5f, -59.3f, 20.2f),      new Vector3() ));

            List<Vector3> new_waypoint = new List<Vector3>();
            new_waypoint.Add(m_enemy_point[0][1].Position);
            new_waypoint.Add(new Vector3(-91.0f, -91.5f, 260.0f));
            m_enemy_point[0][1].WayPoint = new_waypoint;

            //m_spawn_point.Add(new Vector3(-302.21f, -69.70f, 147.64f));
            //m_enemy_point.Add(new List<Vector3>());
            //m_enemy_point[1].Add(new Vector3(0.0f, 0.0f, 0.0f));
        }

        public static EnemyData get_enemy(int id)
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
