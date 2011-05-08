using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace Out_of_Scope
{
    class Player : Sprite
    {
        Graphics.Entity.Text m_debug_text;
        SoundEffect m_gun_fire;
        List<Graphics.Entity.Sprite> m_bullets = new List<Graphics.Entity.Sprite>();
        List<Graphics.Entity.Sprite> m_enemy = new List<Graphics.Entity.Sprite>();

        int m_bullet_count = 13, m_enemy_count;
        float m_last_shot = 0.0f;
        bool m_lost = false, m_has_won = false;

        public Player(Graphics.Entity.Sprite hud, Graphics.Entity.Text debug_text, SoundEffect gun_fire)
        {
            m_sprite = hud;
            m_position = Vector2.Zero;
            m_scale = Vector2.One;
            m_rotation = 0.0f;
            m_debug_text = debug_text;

            Camera.position = GamePlay.spawn_point;
            m_gun_fire = gun_fire;

        }

        public void LoadIcons(GraphicsDevice graphics, String bullet_icon, String enemy_icon, ContentManager content, Visualisation vis)
        {
            for (int i = 0; i < m_bullet_count; i++)
            {
                Graphics.Entity.Sprite new_bullet = new Graphics.Entity.Sprite();
                new_bullet.Init(content.Load<Texture2D>(bullet_icon));
                new_bullet.Visible = false;
                new_bullet.destination = new Rectangle(
                    (int)((((float)graphics.Viewport.Width / 1366.0f) * 175.0f) + (((float)graphics.Viewport.Width / 1366.0f) * 15.0f * i)), 
                    (int)(((float)graphics.Viewport.Height / 768.0f) * 714.0f), 
                    (int)(((float)graphics.Viewport.Width / 1366.0f) * 15.0f),
                    (int)(((float)graphics.Viewport.Height / 768.0f) * 47.0f));
                m_bullets.Add(new_bullet);
                vis.AddEntity(new_bullet);
            }

            m_enemy_count = Enemy_Container.enemies().Count;
            for (int i = 0; i < m_enemy_count; i++)
            {
                Graphics.Entity.Sprite new_enemy = new Graphics.Entity.Sprite();
                new_enemy.Init(content.Load<Texture2D>(enemy_icon));
                new_enemy.Visible = false;
                new_enemy.destination = new Rectangle(
                    (int)((((float)graphics.Viewport.Width / 1366.0f) * 171.0f) + (((float)graphics.Viewport.Width / 1366.0f) * 31.0f * i)),
                    (int)(((float)graphics.Viewport.Height / 768.0f) * 664.0f),
                    (int)(((float)graphics.Viewport.Width / 1366.0f) * 31.0f),
                    (int)(((float)graphics.Viewport.Height / 768.0f) * 34.0f));
                m_enemy.Add(new_enemy);
                vis.AddEntity(new_enemy);
            }
        }

        public override void update(Visualisation vis, GameTime time, GraphicsDevice graphics, GameState gamestate)
        {
            if (!m_lost)
            {
                switch (gamestate)
                {
                    case GameState.game:
#if DEBUG
                        if (Input.Down) Camera.Move(new Vector3(0, 0, -4));
                        if (Input.Up) Camera.Move(new Vector3(0, 0, 4));
                        if (Input.Left) Camera.Move(new Vector3(-4, 0, 0));
                        if (Input.Right) Camera.Move(new Vector3(4, 0, 0));
#endif
                        float offset = ((float)Math.Sin(time.TotalGameTime.TotalMilliseconds * 0.001f) * (0.0001f * (GamePlay.difficulty.GetHashCode() * 3)));
#if DEBUG
                        offset = 0.0f;
#endif
                        Camera.Turn(new Vector3((Input.Y * 0.01f) + offset, Input.X * 0.01f, 0.0f));

                        if (!m_has_won) Camera.fov = Camera.fov + (Input.Zoom * -0.017f); else Camera.fov = (float)Math.PI / 3.0f;

                        m_debug_text.text = "Camera Position : " + Camera.position.ToString() +
                            "\nCamera Rotation : " + Camera.rotation.ToString();

                        bool shot_fired = false;

                        if (Input.Fire && m_bullet_count > 0 &&
                             (float)time.TotalGameTime.TotalMilliseconds - m_last_shot > 2000.0f)
                        {
                            m_bullets[m_bullet_count - 1].Visible = false;
                            m_bullet_count--;
                            shot_fired = true;
                            m_last_shot = (float)time.TotalGameTime.TotalMilliseconds;
                            m_gun_fire.Play();
                        }

                        List<Entity_Enemy> list = Enemy_Container.enemies();
                        for (int i = 0; i < list.Count; i++)
                        {
                            float? shot_ray = Camera.CastBulletRay(list[i], graphics);
                            if (shot_ray != null)
                            {
                                if (shot_fired && m_bullet_count > 0)
                                {
                                    list[i].Kill();
                                    m_enemy[m_enemy_count - 1].Visible = false;
                                    m_enemy_count--;
                                }

                                m_debug_text.text += "\nEnemy Id : " + i
                                    + "\nPosition : " + list[i].position
                                    + "\nRotation : " + list[i].rotation;
                            }
                        }

                        if (m_enemy_count == 0) m_has_won = true;

                        break;

                    case GameState.menu:
                        offset = ((float)Math.Sin(time.TotalGameTime.TotalMilliseconds * 0.0005f) * 0.1f);
                        Camera.rotation = new Vector3(-0.49f + offset, 0.34f, 0.0f);
                        break;

                }
            }
        }

        public void show_hud(bool show)
        {
            m_sprite.Visible = show;

            if (GamePlay.difficulty != GameDifficulty.easy)
                m_bullet_count = GamePlay.difficulty == GameDifficulty.hard ? 7 : 10;

            for (int i = 0; i < m_bullet_count; i++)
                m_bullets[i].Visible = show;
            for (int i = 0; i < Enemy_Container.enemies().Count; i++)
                m_enemy[i].Visible = show;
        }

        public void Kill()
        {
            //m_lost = true;
            Camera.fov = (float)Math.PI / 3.0f;
            show_hud(false);
        }

        public bool hasWon()
        {
            return m_has_won;
        }

        public void reset()
        {
            m_has_won = false;
            m_lost = false;
            m_enemy_count = GamePlay.enemy_count;
            m_bullet_count = 13;
            m_last_shot = 0.0f;
            Enemy_Container.reset();
        }
    }
}
