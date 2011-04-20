using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DebugHelp
{
    static class Draw_BoundingBox
    {
        private static VertexPositionColor[] m_vpc = new VertexPositionColor[2];
        private static Effect m_effect;
        private static GraphicsDevice m_graphicsDevice;

        public static void Init(Effect effect, GraphicsDevice GraphicsDevice)
        {
            m_effect = effect;
            m_graphicsDevice = GraphicsDevice;
        }

        private static void DrawLine(Vector3 a, Vector3 b, Color c, Matrix world_view_projection)
        {
            m_vpc[0].Position = a;
            m_vpc[1].Position = b;
            m_vpc[0].Color = c;
            m_vpc[1].Color = c;

            m_effect.Parameters["WorldViewProjection"].SetValue(world_view_projection);
            m_effect.CurrentTechnique = m_effect.Techniques["Technique1"];
            foreach (EffectPass pass in m_effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                m_graphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, m_vpc, 0, 1);
            }
        }

        public static void Draw(BoundingBox bb, Matrix view_projection)
        {
#if DEBUG
            DrawLine(bb.Min, new Vector3(bb.Min.X, bb.Max.Y, bb.Min.Z), Color.Red, view_projection);
            DrawLine(new Vector3(bb.Min.X, bb.Max.Y, bb.Min.Z), new Vector3(bb.Max.X, bb.Max.Y, bb.Min.Z), Color.Red, view_projection);
            DrawLine(new Vector3(bb.Max.X, bb.Max.Y, bb.Min.Z), new Vector3(bb.Max.X, bb.Min.Y, bb.Min.Z), Color.Red, view_projection);
            DrawLine(new Vector3(bb.Max.X, bb.Min.Y, bb.Min.Z), bb.Min, Color.Red, view_projection);
            DrawLine(bb.Max, new Vector3(bb.Min.X, bb.Max.Y, bb.Max.Z), Color.Red, view_projection);
            DrawLine(new Vector3(bb.Min.X, bb.Max.Y, bb.Max.Z), new Vector3(bb.Min.X, bb.Min.Y, bb.Max.Z), Color.Red, view_projection);
            DrawLine(new Vector3(bb.Min.X, bb.Min.Y, bb.Max.Z), new Vector3(bb.Max.X, bb.Min.Y, bb.Max.Z), Color.Red, view_projection);
            DrawLine(new Vector3(bb.Max.X, bb.Min.Y, bb.Max.Z), bb.Max, Color.Red, view_projection);
            DrawLine(bb.Min, new Vector3(bb.Min.X, bb.Min.Y, bb.Max.Z), Color.Red, view_projection);
            DrawLine(new Vector3(bb.Min.X, bb.Max.Y, bb.Max.Z), new Vector3(bb.Min.X, bb.Max.Y, bb.Min.Z), Color.Red, view_projection);
            DrawLine(new Vector3(bb.Max.X, bb.Max.Y, bb.Min.Z), bb.Max, Color.Red, view_projection);
            DrawLine(new Vector3(bb.Max.X, bb.Min.Y, bb.Max.Z), new Vector3(bb.Max.X, bb.Min.Y, bb.Min.Z), Color.Red, view_projection);
#endif
        }
    }
}