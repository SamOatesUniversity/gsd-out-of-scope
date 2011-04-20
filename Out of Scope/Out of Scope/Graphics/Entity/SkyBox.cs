using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Out_of_Scope.Graphics.Entity
{
    class SkyBox : Model
    {
        private TextureCube m_cubemap;

        public void Init(Microsoft.Xna.Framework.Graphics.Model model, Effect effect, TextureCube texture)
        {
            m_model = model;
            m_effect = effect;
            m_cubemap = texture;

            m_scale = new Vector3(2000.0f, 2000.0f, 2000.0f);
            m_position = new Vector3(0.0f, -500.0f, 0.0f);
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice device)
        {
            device.BlendState = BlendState.Opaque;
            device.DepthStencilState = DepthStencilState.Default;
            device.SamplerStates[0] = SamplerState.LinearWrap;

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.CullClockwiseFace;
            device.RasterizerState = rs;

            Matrix[] transforms = new Matrix[m_model.Bones.Count];
            m_model.CopyAbsoluteBoneTransformsTo(transforms);

            Matrix World = Matrix.Identity *
                            Matrix.CreateScale(m_scale) *
                            Matrix.CreateFromYawPitchRoll(m_rotation.Y, m_rotation.X, m_rotation.Z) *
                            Matrix.CreateTranslation(m_position);

            for (int i = 0; i < m_model.Meshes.Count; i++)
            {
                foreach (ModelMeshPart part in m_model.Meshes[i].MeshParts)
                {
                    Matrix world_view_projection = (transforms[m_model.Meshes[i].ParentBone.Index] * World) *
                                                    m_view_xform * m_projection_xform;

                    part.Effect = m_effect;
                    m_effect.Parameters["world_view_projection_xform"].SetValue(world_view_projection);
                    m_effect.Parameters["world_xform"].SetValue(transforms[m_model.Meshes[i].ParentBone.Index] * World);
                    m_effect.Parameters["view_inverse_xform"].SetValue(Matrix.Invert(m_view_xform));

                    m_effect.Parameters["cubeMapTX"].SetValue(m_cubemap);

                    m_effect.CurrentTechnique = m_effect.Techniques["SkySphere"];
                    foreach (EffectPass pass in m_effect.CurrentTechnique.Passes)
                        m_model.Meshes[i].Draw();
                }
            }
        }

        public new TextureCube texture
        {
            get { return m_cubemap; }
            set { m_cubemap = value; }
        }
    }
}
