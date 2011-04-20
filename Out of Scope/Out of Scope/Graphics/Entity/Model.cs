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
    class Model : GfxEntity
    {
        protected Microsoft.Xna.Framework.Graphics.Model m_model;
        protected Vector3 m_scale, m_rotation, m_position;
        protected Matrix m_view_xform, m_projection_xform;
        protected Effect m_effect = null;
        protected BasicEffect m_basic_effect = null;
        protected Texture2D m_texture;
        protected BoundingBox m_boundingbox = new BoundingBox();
        protected bool m_boundingbox_enabled = true;

        public void Init(Microsoft.Xna.Framework.Graphics.Model model)
        {
            m_model = model;
            m_position = Vector3.Zero;
            m_scale = Vector3.One;
            m_rotation = Vector3.Zero;
        }

        public void Init(Microsoft.Xna.Framework.Graphics.Model model, Vector3 position)
        {
            m_model = model;
            m_position = position;
            m_scale = Vector3.One;
            m_rotation = Vector3.Zero;
        }

        public void Init(Microsoft.Xna.Framework.Graphics.Model model, Vector3 position, Vector3 rotation)
        {
            m_model = model;
            m_position = position;
            m_scale = Vector3.One;
            m_rotation = rotation;
        }

        public void Init(Microsoft.Xna.Framework.Graphics.Model model, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            m_model = model;
            m_position = position;
            m_scale = scale;
            m_rotation = rotation;
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice device)
        {
            if (m_basic_effect == null)
            {
                m_basic_effect = new BasicEffect(device);
                m_basic_effect.FogEnabled = true;
                m_basic_effect.FogColor = new Vector3(0.6f, 0.6f, 0.6f);
                m_basic_effect.FogStart = 0.0f;
                m_basic_effect.FogEnd = 2000.0f;
                m_basic_effect.SpecularPower = 10000000.0f;
            }

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

            foreach (ModelMesh mesh in m_model.Meshes)
            {
                for (int i = 0; i < mesh.Effects.Count; i++)
                {
                    BasicEffect effect = (BasicEffect)mesh.Effects[i];
                    effect.EnableDefaultLighting();
                    effect.AmbientLightColor = m_basic_effect.AmbientLightColor;
                    effect.DiffuseColor = m_basic_effect.DiffuseColor;
                    effect.EmissiveColor = m_basic_effect.EmissiveColor;
                    effect.FogColor = m_basic_effect.FogColor;
                    effect.FogEnabled = m_basic_effect.FogEnabled;
                    effect.FogEnd = m_basic_effect.FogEnd;
                    effect.FogStart = m_basic_effect.FogStart;
                    effect.PreferPerPixelLighting = m_basic_effect.PreferPerPixelLighting;
                    effect.SpecularColor = m_basic_effect.SpecularColor;
                    effect.SpecularPower = m_basic_effect.SpecularPower;

                    effect.Projection = m_projection_xform;
                    effect.View = m_view_xform;
                    effect.World = transforms[mesh.ParentBone.Index] * World;
                }
                mesh.Draw();
            }

            if (m_boundingbox_enabled)
            {
                m_boundingbox = CalculateBoundingBox(transforms, World);
                DebugHelp.Draw_BoundingBox.Draw(m_boundingbox, Matrix.Identity * m_view_xform * m_projection_xform);
            }
        }

        protected BoundingBox CalculateBoundingBox(Matrix[] transforms, Matrix world)
        {

            // Create variables to hold min and max xyz values for the model. Initialise them to extremes
            Vector3 modelMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            Vector3 modelMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

            foreach (ModelMesh mesh in m_model.Meshes)
            {
                //Create variables to hold min and max xyz values for the mesh. Initialise them to extremes
                Vector3 meshMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
                Vector3 meshMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);


                // There may be multiple parts in a mesh (different materials etc.) so loop through each
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    // The stride is how big, in bytes, one vertex is in the vertex buffer
                    // We have to use this as we do not know the make up of the vertex
                    int stride = part.VertexBuffer.VertexDeclaration.VertexStride;

                    byte[] vertexData = new byte[stride * part.NumVertices];
                    part.VertexBuffer.GetData(vertexData);

                    // Find minimum and maximum xyz values for this mesh part
                    // We know the position will always be the first 3 float values of the vertex data
                    Vector3 vertPosition = new Vector3();
                    for (int ndx = 0; ndx < vertexData.Length; ndx += stride)
                    {
                        vertPosition.X = BitConverter.ToSingle(vertexData, ndx);
                        vertPosition.Y = BitConverter.ToSingle(vertexData, ndx + sizeof(float));
                        vertPosition.Z = BitConverter.ToSingle(vertexData, ndx + sizeof(float) * 2);

                        // update our running values from this vertex
                        meshMin = Vector3.Min(meshMin, vertPosition);
                        meshMax = Vector3.Max(meshMax, vertPosition);
                    }
                }

                // transform by mesh bone transforms
                meshMin = Vector3.Transform(meshMin, transforms[mesh.ParentBone.Index] * world);
                meshMax = Vector3.Transform(meshMax, transforms[mesh.ParentBone.Index] * world);

                // Expand model extents by the ones from this mesh
                modelMin = Vector3.Min(modelMin, meshMin);
                modelMax = Vector3.Max(modelMax, meshMax);
            }


            // Create and return the model bounding box
            return new BoundingBox(modelMin, modelMax);

        }

        public bool boundingbox_enabled
        {
            get { return m_boundingbox_enabled; }
            set { m_boundingbox_enabled = value; }
        }

        public BoundingBox boundingbox
        {
            get { return m_boundingbox; }
        }

        public Vector3 position
        {
            get { return m_position; }
            set { m_position = value; }
        }

        public Vector3 rotation
        {
            get { return m_rotation; }
            set { m_rotation = value; }
        }

        public Vector3 scale
        {
            get { return m_scale; }
            set { m_scale = value; }
        }

        public Effect effect
        {
            get { return m_effect; }
            set { m_effect = value; }
        }

        public Texture2D texture
        {
            get { return m_texture; }
            set { m_texture = value; }
        }

        public Matrix projection
        {
            set { m_projection_xform = value; }
        }

        public Matrix view
        {
            set { m_view_xform = value; }
        }

        public BasicEffect basiceffect
        {
            set { m_basic_effect = value; }
        }
    }
}
