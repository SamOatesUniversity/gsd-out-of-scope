using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Out_of_Scope
{
    class MenuController
    {
        private Graphics.Entity.Sprite m_title_screen, m_cursor;

        private Graphics.Entity.Text[] m_menu_text;

        public MenuController()
        {

        }

        public void Init(ContentManager content, Visualisation vis, World world, GraphicsDevice graphics)
        {
            m_title_screen = new Graphics.Entity.Sprite();
            m_title_screen.Init(content.Load<Texture2D>("Menu"), graphics.Viewport.Bounds);
            vis.AddEntity(m_title_screen);

            m_menu_text = new Graphics.Entity.Text[3];

            for (int i = 0; i < 3; i++)
            {
                m_menu_text[i] = new Graphics.Entity.Text();
                m_menu_text[i].Init(content.Load<SpriteFont>("InGameFont"));
                vis.AddEntity(m_menu_text[i]);
            }


            m_menu_text[0].text = "Easy";
            m_menu_text[0].position = new Vector2(
                (((float)graphics.Viewport.Width / 1366.0f) * 641.0f),
                (((float)graphics.Viewport.Height / 768.0f) * 366.0f));

            m_menu_text[1].text = "Medium";
            m_menu_text[1].position = new Vector2(
                (((float)graphics.Viewport.Width / 1366.0f) * 619.0f),
                (((float)graphics.Viewport.Height / 768.0f) * 430.0f));

            m_menu_text[2].text = "Hard";
            m_menu_text[2].position = new Vector2(
                (((float)graphics.Viewport.Width / 1366.0f) * 637.0f),
                (((float)graphics.Viewport.Height / 768.0f) * 490.0f));

            int height = (int)m_menu_text[2].font.MeasureString("Hard").Y;
            m_cursor = new Graphics.Entity.Sprite();
            m_cursor.Init(content.Load<Texture2D>("cursor"), new Rectangle( 0, 0, height, height ) );
            vis.AddEntity(m_cursor);

        }

        public void show()
        {
            m_cursor.Visible = true;
            m_menu_text[0].Visible = true;
            m_menu_text[1].Visible = true;
            m_menu_text[2].Visible = true;
            m_title_screen.Visible = true;
        }

        public bool Update(ref GameState game_state)
        {
            int x = (int)Input.ActualX;
            int y = (int)Input.ActualY;

            m_cursor.position = new Vector2(
                x,
                y);

            x += 20;
            y += 20;

            for (int i = 0; i < 3; i++)
            {
                m_menu_text[i].colour = Color.White;

                if ((x > m_menu_text[i].position.X) &&
                    (x < m_menu_text[i].position.X + m_menu_text[i].font.MeasureString(m_menu_text[i].text).X) &&
                    (y > m_menu_text[i].position.Y) &&
                    (y < m_menu_text[i].position.Y + m_menu_text[i].font.MeasureString(m_menu_text[i].text).Y))
                {
                    m_menu_text[i].colour = Color.Red;
                    if (Input.Fire)
                    {
                        m_title_screen.Visible = false;
                        m_menu_text[0].Visible = false;
                        m_menu_text[1].Visible = false;
                        m_menu_text[2].Visible = false;

                        switch( i )
                        {
                            case 0:
                                GamePlay.difficulty = GameDifficulty.easy;
                                break;
                            case 1:
                                GamePlay.difficulty = GameDifficulty.medium;
                                break;
                            case 2:
                                GamePlay.difficulty = GameDifficulty.hard;
                                break;
                        }
                        

                        m_cursor.Visible = false;
                        game_state = GameState.game;
                        return false;
                    }
                }
            }

            return true;
        }

    }
}
