using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Out_of_Scope
{
    enum GameState
    {
        menu,
        game
    };

    public class Main : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        Visualisation m_vis;
        World m_world;
        MenuController m_menu;

        Entity_City m_city;
        Entity_SkyBox m_skybox;
        Player m_player;

        Graphics.Entity.Sprite m_black_screen;
        Graphics.Entity.Text m_ingame_text, m_clock_text;

        GameState m_game_state = GameState.menu;
        Song m_song;
        SoundEffect m_clock_tick;
        float m_clock_tick_timer;

        float m_game_start_time = 0.0f, m_game_win_time = 0.0f, m_game_lost_time = 0.0f;
        int m_win_time = -1;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            Input.Init(GraphicsDevice.Viewport);
            GamePlay.Init();
            m_vis = new Visualisation();
            m_world = new World(m_vis);
            m_menu = new MenuController();
                       
            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_song = Content.Load<Song>("FightForFreedoms");
            
            DebugHelp.Draw_BoundingBox.Init(Content.Load<Effect>("lines"), GraphicsDevice);

            m_vis.Init(GraphicsDevice);

            Enemy_Container.Init(m_world, Content);

            Graphics.Entity.Text debug_text = new Graphics.Entity.Text();
            debug_text.Init(Content.Load<SpriteFont>("default_font"));
            
            Graphics.Entity.Sprite hud = new Graphics.Entity.Sprite();
            hud.Init(Content.Load<Texture2D>("scope-hd"), GraphicsDevice.Viewport.Bounds);
            hud.Visible = false;
            m_player = new Player(hud, debug_text, Content.Load<SoundEffect>("Fire"));          
            m_world.AddEntity(m_player);
            m_player.LoadIcons(GraphicsDevice, "Bullet-Icon", "Enemy-Icon", Content, m_vis);

            List<Entity_Explosion> explosions_list = new List<Entity_Explosion>();

            for (int i = 0; i < 4; i++)
            {
                Graphics.Entity.Billboard explosion_model = new Graphics.Entity.Billboard();
                explosion_model.Init(Content.Load<Model>("Explosion"),
                                    Content.Load<Effect>("EffectBillboard"),
                                    Content.Load<Texture2D>("explosion_texture"));
                Entity_Explosion explosion = new Entity_Explosion(explosion_model);
                m_world.AddEntity(explosion);
                explosions_list.Add(explosion);
            }

            List<SoundEffect> explosion_sound = new List<SoundEffect>();

            for (int i = 0; i < 3; i++)
            {
                SoundEffect new_explosion_sound = Content.Load<SoundEffect>("explode_" + (i+1));
                explosion_sound.Add(new_explosion_sound);
            }

            BasicEffect city_effect = new BasicEffect(GraphicsDevice);
            city_effect.FogEnabled = true;
            city_effect.FogColor = new Vector3(0.6f, 0.6f, 0.6f);
            city_effect.FogStart = 0.0f;
            city_effect.FogEnd = 2000.0f;
            city_effect.SpecularPower = 10000000.0f;

            Graphics.Entity.Model city = new Graphics.Entity.Model();
            city.Init(Content.Load<Model>("city/city"));
            city.basiceffect = city_effect;
            m_city = new Entity_City(city, explosions_list, explosion_sound);
            m_world.AddEntity(m_city);

            Graphics.Entity.SkyBox skybox = new Graphics.Entity.SkyBox();
            skybox.Init(Content.Load<Model>("skybox/skysphere"), 
                        Content.Load<Effect>("skybox/skybox_fx"),
                        Content.Load<TextureCube>("skybox/skytexture"));
            m_skybox = new Entity_SkyBox(skybox);
            m_world.AddEntity(m_skybox);

            //Menu

            m_menu.Init(Content, m_vis, m_world, GraphicsDevice);

            //Fonts

            m_ingame_text = new Graphics.Entity.Text();
            m_ingame_text.Init(Content.Load<SpriteFont>("InGameFont"));
            m_ingame_text.text = "Shoot All The Terrorists Before They Set The Bombs Off...";
            m_ingame_text.Visible = false;
            m_ingame_text.position = new Vector2((GraphicsDevice.Viewport.Width * 0.5f) - 
                m_ingame_text.font.MeasureString(m_ingame_text.text).X * 0.5f,
                0.0f);
            m_vis.AddEntity(m_ingame_text);

            m_clock_text = new Graphics.Entity.Text();
            m_clock_text.Init(Content.Load<SpriteFont>("InGameFont"), "00:00:60");
            m_clock_text.Visible = false;
            m_clock_text.position = new Vector2(
                (((float)GraphicsDevice.Viewport.Width / 1366.0f) * 1024.0f), 
                (((float)GraphicsDevice.Viewport.Height / 768.0f) * 690.0f) );
            m_vis.AddEntity(m_clock_text);

            m_clock_tick = Content.Load<SoundEffect>("clockTick");

            m_black_screen = new Graphics.Entity.Sprite();
            m_black_screen.Init(Content.Load<Texture2D>("blackPixel"),
                new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height) );
            m_black_screen.Visible = false;
            m_vis.AddEntity(m_black_screen);

            //play audio
            MediaPlayer.Volume = 0.5f;
            MediaPlayer.Play(m_song);
            MediaPlayer.IsRepeating = true;

#if DEBUG
            m_vis.AddEntity(debug_text);
#endif
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (Input.Quit) this.Exit();

            if (m_game_state == GameState.menu) 
            {
                if (!m_menu.Update(ref m_game_state))
                {
                    m_ingame_text.Visible = true;
                    m_clock_text.Visible = true;
                    m_player.show_hud(true);
                    m_win_time = -1;
                    m_game_start_time = (float)gameTime.TotalGameTime.TotalMilliseconds;
                }
            }

            if (m_game_state == GameState.game)
            {
                //Update timer
                int total_time = 91;
                if (GamePlay.difficulty == GameDifficulty.medium) total_time = 61;
                if (GamePlay.difficulty == GameDifficulty.hard) total_time = 31;

                if (m_player.hasWon())
                {
                    if (m_win_time == -1)
                    {
                        if (m_game_win_time == 0.0f) m_game_win_time = (float)gameTime.TotalGameTime.TotalMilliseconds;
                        m_clock_text.Visible = false;
                        m_win_time = (int)(total_time - ((gameTime.TotalGameTime.TotalMilliseconds - m_game_start_time)) * 0.001f);
                        m_player.show_hud(false);
                        m_ingame_text.text = "You Saved the City! With\n" + "00:00:" + m_win_time + " Time Remaining!";
                        m_ingame_text.position = new Vector2((GraphicsDevice.Viewport.Width * 0.5f) -
                            m_ingame_text.font.MeasureString(m_ingame_text.text).X * 0.5f,
                            0.0f);
                        m_ingame_text.Visible = true;
                    }

                    if ((float)gameTime.TotalGameTime.TotalMilliseconds - m_game_win_time > 5000)
                    {
                        m_ingame_text.Visible = false;
                        m_ingame_text.text = "Shoot All The Terrorists Before They Set The Bombs Off...";
                        m_ingame_text.position = new Vector2((GraphicsDevice.Viewport.Width * 0.5f) -
                            m_ingame_text.font.MeasureString(m_ingame_text.text).X * 0.5f,
                            0.0f);
                        m_game_win_time = 0.0f;
                        m_game_state = GameState.menu;
                        m_player.reset();
                        m_menu.show();
                    }
                }
                else
                {
                    int time_left = (int)(total_time - ((gameTime.TotalGameTime.TotalMilliseconds - m_game_start_time)) * 0.001f);
                    time_left = time_left <= 0 ? 0 : time_left;
                    string time_left_s = time_left < 10 ? "0" + time_left.ToString() : time_left.ToString();
                    m_clock_text.text = "00:00:" + time_left_s;    

                    if (time_left <= 0)
                    {
                        if (m_game_lost_time == 0.0f) m_game_lost_time = (float)gameTime.TotalGameTime.TotalMilliseconds;
                        m_city.explode(gameTime);
                        m_player.Kill();

                        if ((float)gameTime.TotalGameTime.TotalMilliseconds - m_game_lost_time > 5000)
                        {
                            //black screen//
                            m_black_screen.Visible = true;
                            MediaPlayer.Stop();

                            if ((float)gameTime.TotalGameTime.TotalMilliseconds - m_game_lost_time > 10000)
                            {
                                //back to main menu//
                                m_black_screen.Visible = false;
                                m_clock_text.Visible = false;
                                m_game_lost_time = 0.0f;
                                MediaPlayer.Play(m_song);
                                m_game_state = GameState.menu;
                                m_player.reset();
                                m_city.reset();
                                m_menu.show();
                            }
                        }
                    }
                    else
                    {
                        if (time_left > 10)
                        {
                            if ((float)gameTime.TotalGameTime.TotalMilliseconds - m_clock_tick_timer > 1000)
                            {
                                m_clock_tick.Play();
                                m_clock_tick_timer = (float)gameTime.TotalGameTime.TotalMilliseconds;
                            }
                        }
                        else if (time_left > 3)
                        {
                            if ((float)gameTime.TotalGameTime.TotalMilliseconds - m_clock_tick_timer > 500)
                            {
                                m_clock_tick.Play();
                                m_clock_tick_timer = (float)gameTime.TotalGameTime.TotalMilliseconds;
                            }
                        }
                        else if (time_left > 1)
                        {
                            if ((float)gameTime.TotalGameTime.TotalMilliseconds - m_clock_tick_timer > 100)
                            {
                                m_clock_tick.Play();
                                m_clock_tick_timer = (float)gameTime.TotalGameTime.TotalMilliseconds;
                            }
                        }
                        else
                        {
                            m_clock_tick.Play();
                        }
                    }

                    if (m_game_state == GameState.game &&
                        (float)gameTime.TotalGameTime.TotalMilliseconds - m_game_start_time > 5000)
                    {
                        m_ingame_text.Visible = false;
                    }
                }
                
            }

            //Update world
            m_world.Update(gameTime, GraphicsDevice, m_game_state);

            //Update input
            if (this.IsActive && m_game_state == GameState.game) Input.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            m_vis.Draw();

            base.Draw(gameTime);
        }
    }
}
