using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Out_of_Scope
{
    class Player : Sprite
    {
        Graphics.Entity.Text m_debug_text;

        public Player(Graphics.Entity.Sprite hud, Graphics.Entity.Text debug_text)
        {
            m_sprite = hud;
            m_position = Vector2.Zero;
            m_scale = Vector2.One;
            m_rotation = 0.0f;
            m_debug_text = debug_text;

            Random rand = new Random();

            Camera.position = GamePlay.spawn_point;

        }

        public override void update(Visualisation vis, GameTime time, GraphicsDevice graphics)
        {
#if DEBUG
            if (Input.Down) Camera.Move(new Vector3(0, 0, -4));
            if (Input.Up) Camera.Move(new Vector3(0, 0, 4));
            if (Input.Left) Camera.Move(new Vector3(-4, 0, 0));
            if (Input.Right) Camera.Move(new Vector3(4, 0, 0));
#endif
            float offset = ( (float)Math.Sin( time.TotalGameTime.TotalMilliseconds * 0.001f ) * 0.001f );
#if DEBUG
            offset = 0.0f;
#endif
            Camera.Turn(new Vector3((Input.Y * 0.01f) + offset, Input.X * 0.01f, 0.0f));

            Camera.fov = Camera.fov + ( Input.Zoom * -0.017f );

            m_debug_text.text = "Camera Position : " + Camera.position.ToString() + 
                "\nCamera Rotation : " + Camera.rotation.ToString();
            
            List<Entity_Enemy> list = Enemy_Container.enemies();
            for (int i = 0; i < list.Count; i++)
            {
                if (Camera.CastBulletRay(list[i], graphics) != null)
                {
                    if (Input.Fire)
                    {
                        list[i].Kill();
                    }

                    m_debug_text.text += "\nEnemy Id : " + i 
                        + "\nPosition : " + list[i].position
                        + "\nRotation : " + list[i].rotation;
                }
            }
            

        }
    }
}
