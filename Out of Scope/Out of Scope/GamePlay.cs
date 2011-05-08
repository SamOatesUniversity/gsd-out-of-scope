using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Out_of_Scope
{
    enum GameDifficulty
    {
        easy = 1,
        medium = 2,
        hard = 3
    };


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
        private static Vector3 m_spawn_point;
        private static List<EnemyData> m_enemy_point = new List<EnemyData>();
        private static GameDifficulty m_difficulty = GameDifficulty.easy;

        public static void Init()
        {
            m_spawn_point = new Vector3(-45.74f, -31.86f, 65.0f);

            m_enemy_point.Add(new EnemyData( new Vector3(-289.0f, -91.4f, 108.0f),   new Vector3() ));
            m_enemy_point.Add(new EnemyData( new Vector3(-66.0f, -90.0f, 182.1f),    new Vector3() ));
            m_enemy_point.Add(new EnemyData( new Vector3(-88.9f, -27.5f, -26.4f),    new Vector3() ));
            m_enemy_point.Add(new EnemyData( new Vector3(86.0f, -80.8f, 92.0f),      new Vector3() ));
            m_enemy_point.Add(new EnemyData( new Vector3(-155.5f, -94.1f, 455.3f),   new Vector3() ));
            m_enemy_point.Add(new EnemyData( new Vector3(99.5f, -59.3f, 20.2f),      new Vector3() ));

            List<Vector3> id_0_way_point = new List<Vector3>();
            id_0_way_point.Add(m_enemy_point[0].Position);
            id_0_way_point.Add(new Vector3(-289.0f, -92.0f, 106.0f));
            id_0_way_point.Add(new Vector3(-289.0f, -92.0f, 91.0f));
            id_0_way_point.Add(new Vector3(-289.0f, -91.4f, 88.0f));
            id_0_way_point.Add(new Vector3(-215.0f, -90.0f, 79.0f));
            id_0_way_point.Add(new Vector3(-215.0f, -90.6f, 81.0f));
            id_0_way_point.Add(new Vector3(-215.0f, -90.6f, 97.0f));
            id_0_way_point.Add(new Vector3(-215.0f, -90.0f, 99.0f));
            m_enemy_point[0].WayPoint = id_0_way_point;

            List<Vector3> id_1_way_point = new List<Vector3>();
            id_1_way_point.Add(m_enemy_point[1].Position);
            id_1_way_point.Add(new Vector3(-91.0f, -91.5f, 260.0f));
            m_enemy_point[1].WayPoint = id_1_way_point;

            List<Vector3> id_3_way_point = new List<Vector3>();
            id_3_way_point.Add(m_enemy_point[3].Position);
            id_3_way_point.Add(new Vector3(168.0f, -73.0f, 89.0f));
            m_enemy_point[3].WayPoint = id_3_way_point;

            List<Vector3> id_4_way_point = new List<Vector3>();
            id_4_way_point.Add(m_enemy_point[4].Position);
            id_4_way_point.Add(new Vector3(-150.0f, -93.0f, 434.0f));
            id_4_way_point.Add(new Vector3(-142.0f, -93.0f, 416.0f));
            id_4_way_point.Add(new Vector3(-111.0f, -92.5f, 355.0f));
            id_4_way_point.Add(new Vector3(-142.0f, -93.0f, 416.0f));
            id_4_way_point.Add(new Vector3(-150.0f, -93.0f, 434.0f));
            m_enemy_point[4].WayPoint = id_4_way_point;
        }

        public static EnemyData get_enemy(int id)
        {
            return m_enemy_point[id];
        }

        public static GameDifficulty difficulty
        {
            get { return m_difficulty; }
            set { m_difficulty = value; }
        }

        public static Vector3 spawn_point
        {
            get { return m_spawn_point; }
        }

        public static int enemy_count
        {
            get { return m_enemy_point.Count; }
        }
    }
}
