using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Out_of_Scope
{
    static class Camera
    {
        private static Vector3 m_pos, m_rot, m_Up, m_Look, m_Right;
        private static float m_fov = MathHelper.Pi / 3.0f, m_viewRange = 5000.0f;

        public static Vector3 rotation
        {
            set { m_rot = value; }
            get { return m_rot; }
        }

        public static Vector3 position
        {
            set { m_pos = value; }
            get { return m_pos; }
        }

        public static float fov
        {
            set 
            {
                float new_fov = value;
                if (new_fov >= 0.01f && new_fov <= MathHelper.Pi / 3.0f)
                    m_fov = new_fov; 
            }
            get { return m_fov; }
        }

        public static void Move(Vector3 amount)
        {
            m_pos += m_Look * -amount.Z;
            m_pos += m_Right * amount.X;
            m_pos += m_Up * amount.Y;
        }

        public static void Turn(Vector3 amount)
        {
            m_rot += amount;
        }

        public static Matrix LookAt()
        {
            m_Up = Vector3.Up;
            m_Look = Vector3.Forward;
            m_Right = Vector3.Right;

            Matrix yawMatrix = Matrix.CreateFromAxisAngle(m_Up, m_rot.Y);
            m_Look = Vector3.Transform(m_Look, yawMatrix);
            m_Right = Vector3.Transform(m_Right, yawMatrix);

            Matrix pitchMatrix = Matrix.CreateFromAxisAngle(m_Right, m_rot.X);
            m_Look = Vector3.Transform(m_Look, pitchMatrix);
            m_Up = Vector3.Transform(m_Up, pitchMatrix);

            Matrix rollMatrix = Matrix.CreateFromAxisAngle(m_Look, m_rot.Z);
            m_Right = Vector3.Transform(m_Right, rollMatrix);
            m_Up = Vector3.Transform(m_Up, rollMatrix);

            Matrix viewMatrix = Matrix.Identity;

            viewMatrix.M11 = m_Right.X; viewMatrix.M12 = m_Up.X; viewMatrix.M13 = m_Look.X;
            viewMatrix.M21 = m_Right.Y; viewMatrix.M22 = m_Up.Y; viewMatrix.M23 = m_Look.Y;
            viewMatrix.M31 = m_Right.Z; viewMatrix.M32 = m_Up.Z; viewMatrix.M33 = m_Look.Z;

            viewMatrix.M41 = -Vector3.Dot(m_pos, m_Right);
            viewMatrix.M42 = -Vector3.Dot(m_pos, m_Up);
            viewMatrix.M43 = -Vector3.Dot(m_pos, m_Look);

            return viewMatrix;
        }

        public static Matrix Projection(GraphicsDevice graphics)
        {
            return Matrix.CreatePerspectiveFieldOfView(
                m_fov,
                graphics.Viewport.AspectRatio,
                0.1f,
                m_viewRange);
        }

        public static float? CastBulletRay(Entity targetEntity, GraphicsDevice GraphicsDevice)
        {
            Vector3 near = new Vector3(GraphicsDevice.Viewport.Width * 0.5f, GraphicsDevice.Viewport.Height * 0.5f, 0.0f);
            Vector3 far = new Vector3(GraphicsDevice.Viewport.Width * 0.5f, GraphicsDevice.Viewport.Height * 0.5f, 1.0f);

            near = GraphicsDevice.Viewport.Unproject(near, Projection(GraphicsDevice), LookAt(), Matrix.Identity);
            far = GraphicsDevice.Viewport.Unproject(far, Projection(GraphicsDevice), LookAt(), Matrix.Identity);

            Ray ray = new Ray(near, Vector3.Normalize(far - near));
            return ray.Intersects(targetEntity.model.boundingbox);
        }
    }
}
