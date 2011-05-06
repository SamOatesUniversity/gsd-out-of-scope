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
    public class Main : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        Visualisation m_vis;
        World m_world;

        Entity_City m_city;
        Entity_SkyBox m_skybox;
        Player m_player;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Input.Init(GraphicsDevice.Viewport);
            GamePlay.Init();
            m_vis = new Visualisation();
            m_world = new World(m_vis);

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
                       
            base.Initialize();
        }

        protected override void LoadContent()
        {
            DebugHelp.Draw_BoundingBox.Init(Content.Load<Effect>("lines"), GraphicsDevice);

            m_vis.Init(GraphicsDevice);

            Enemy_Container.Init(m_world, Content);

            Graphics.Entity.Text debug_text = new Graphics.Entity.Text();
            debug_text.Init(Content.Load<SpriteFont>("default_font"));
            
            Graphics.Entity.Sprite hud = new Graphics.Entity.Sprite();
            hud.Init(Content.Load<Texture2D>("scope-hd"), GraphicsDevice.Viewport.Bounds);
            m_player = new Player(hud, debug_text);
            m_world.AddEntity(m_player);

            BasicEffect city_effect = new BasicEffect(GraphicsDevice);
            city_effect.FogEnabled = true;
            city_effect.FogColor = new Vector3(0.6f, 0.6f, 0.6f);
            city_effect.FogStart = 0.0f;
            city_effect.FogEnd = 2000.0f;
            city_effect.SpecularPower = 10000000.0f;

            Graphics.Entity.Model city = new Graphics.Entity.Model();
            city.Init(Content.Load<Model>("city/city"));
            city.basiceffect = city_effect;
            m_city = new Entity_City(city);
            m_world.AddEntity(m_city);

            Graphics.Entity.SkyBox skybox = new Graphics.Entity.SkyBox();
            skybox.Init(Content.Load<Model>("skybox/skysphere"), 
                        Content.Load<Effect>("skybox/skybox_fx"),
                        Content.Load<TextureCube>("skybox/skytexture"));
            m_skybox = new Entity_SkyBox(skybox);
            m_world.AddEntity(m_skybox);

#if DEBUG
            m_vis.AddEntity(debug_text);
#endif
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if(Input.Quit) this.Exit();

            m_world.Update(gameTime, GraphicsDevice);

            if(this.IsActive)Input.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            m_vis.Draw();

            base.Draw(gameTime);
        }
    }
}
