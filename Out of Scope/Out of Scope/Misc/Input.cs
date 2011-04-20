using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Out_of_Scope
{
    static class Input
    {
        enum InputType {
            keyboard,
            controller
        };

        private static InputType m_input_type;
        private static Vector3 m_old_mouse_position;
        private static bool m_mouse_locked, m_tab_down;
        private static Viewport m_viewport;

        public static void Init(Viewport viewport)
        {
            Update();
            m_old_mouse_position = Vector3.Zero;
            m_mouse_locked = true;
            m_tab_down = false;
            m_viewport = viewport;
            Mouse.SetPosition((int)(m_viewport.Width * 0.5f), (int)(m_viewport.Height * 0.5f));
        }

        public static void Update()
        {
            m_input_type = GamePad.GetState(PlayerIndex.One).IsConnected ? InputType.controller : InputType.keyboard;
            if (m_input_type == InputType.keyboard)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Tab) && !m_tab_down)
                {
                    m_tab_down = true;
                    m_mouse_locked = !m_mouse_locked;
                    if (m_mouse_locked) Mouse.SetPosition((int)(m_viewport.Width * 0.5f), (int)(m_viewport.Height * 0.5f));
                }
                else if (Keyboard.GetState().IsKeyUp(Keys.Tab))
                {
                    m_tab_down = false;
                }

                if (!m_mouse_locked &&
                    Mouse.GetState().LeftButton == ButtonState.Pressed &&
                    Mouse.GetState().X >= 0 && Mouse.GetState().X <= m_viewport.Width &&
                    Mouse.GetState().Y >= 0 && Mouse.GetState().X <= m_viewport.Height)
                {
                    m_mouse_locked = true;
                    Mouse.SetPosition((int)(m_viewport.Width * 0.5f), (int)(m_viewport.Height * 0.5f));
                }
            }
        }

        public static bool Quit
        {
            get
            {
                switch (m_input_type)
                {
                    case InputType.keyboard:
                        return Keyboard.GetState().IsKeyDown(Keys.Escape);
                    case InputType.controller:
                        return GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back);
                }
                return false;
            }
        }

        public static bool Down
        {
            get
            {
                switch (m_input_type)
                {
                    case InputType.keyboard:
                        return Keyboard.GetState().IsKeyDown(Keys.Down);
                    case InputType.controller:
                        return GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed;
                }
                return false;
            }
        }

        public static bool Up
        {
            get
            {
                switch (m_input_type)
                {
                    case InputType.keyboard:
                        return Keyboard.GetState().IsKeyDown(Keys.Up);
                    case InputType.controller:
                        return GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed;
                }
                return false;
            }
        }

        public static bool Left
        {
            get
            {
                switch (m_input_type)
                {
                    case InputType.keyboard:
                        return Keyboard.GetState().IsKeyDown(Keys.Left);
                    case InputType.controller:
                        return GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed;
                }
                return false;
            }
        }

        public static bool Right
        {
            get
            {
                switch (m_input_type)
                {
                    case InputType.keyboard:
                        return Keyboard.GetState().IsKeyDown(Keys.Right);
                    case InputType.controller:
                        return GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed;
                }
                return false;
            }
        }

        public static float X
        {
            get
            {
                switch (m_input_type)
                {
                    case InputType.keyboard:
                        if (m_mouse_locked)
                        {
                            float current_mouse_x = Mouse.GetState().X - (m_viewport.Width * 0.5f);
                            Mouse.SetPosition((int)(m_viewport.Width * 0.5f), Mouse.GetState().Y);
                            return current_mouse_x * 0.1f;
                        }
                        break;
                    case InputType.controller:
                        return GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X;
                }
                return 0.0f;
            }
        }

        public static float Y
        {
            get
            {
                switch (m_input_type)
                {
                    case InputType.keyboard:
                        if (m_mouse_locked)
                        {
                            float current_mouse_y = Mouse.GetState().Y - (m_viewport.Height * 0.5f);
                            Mouse.SetPosition(Mouse.GetState().X, (int)(m_viewport.Height * 0.5f));
                            return current_mouse_y * 0.1f;
                        }
                        break;
                    case InputType.controller:
                        return -GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y;
                }
                return 0.0f;
            }
        }

        public static bool Fire
        {
            get
            {
                switch (m_input_type)
                {
                    case InputType.keyboard:
                        return Mouse.GetState().LeftButton == ButtonState.Pressed;
                    case InputType.controller:
                        return GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.0f;
                }
                return false;
            }
        }

        public static float Zoom
        {
            get
            {
                switch (m_input_type)
                {
                    case InputType.keyboard:
                        float current_mouse_z = Mouse.GetState().ScrollWheelValue;
                        float change_in_mouse_z = current_mouse_z - m_old_mouse_position.Z;
                        m_old_mouse_position.Z = current_mouse_z;
                        return change_in_mouse_z * 0.01f;
                    case InputType.controller:
                        return GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y;
                }
                return 0.0f;
            }
        }
    }
}
