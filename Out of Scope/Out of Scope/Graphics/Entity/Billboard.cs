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
    class Billboard : Model 
    {
        public void Init(Microsoft.Xna.Framework.Graphics.Model model, Effect effect, Texture2D texture)
        {
            m_model = model;
            m_effect = effect;
            m_texture = texture;
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice device)
        {
            //point plain at camera...
            float rot_offset = Vector3.Dot(Camera.position - m_position, Vector3.Forward) < 0 ? 0.0f : (float)Math.PI;

            m_rotation.Y = (float)Math.Atan((m_position.X - Camera.position.X)
                                            / (m_position.Z - Camera.position.Z)) + rot_offset;

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

                    m_effect.Parameters["Texture"].SetValue(m_texture);

                    m_effect.CurrentTechnique = m_effect.Techniques["Technique1"];
                    foreach (EffectPass pass in m_effect.CurrentTechnique.Passes)
                        m_model.Meshes[i].Draw();
                }
            }
        }        
    }
}
