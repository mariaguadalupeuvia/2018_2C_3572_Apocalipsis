using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Core.Terrain;
using TGC.Core.Textures;

namespace TGC.Group.Model.GameObjects
{
    public class SkyboxShader : IRenderObject 
    {
        #region variables
        public Effect efecto;
        public string tecnica;
        private float time = 0.1f;
        private TGCVector3 center;
        public float SkyEpsilon { get; set; }
        public TGCVector3 Size { get; set; }
        public TGCVector3 Center
        {
            get { return center; }
            set
            {
                center = value;
                Traslation = TGCMatrix.Translation(center);
            }
        }
        public Color Color { get; set; }
        private TGCMatrix Traslation { get; set; }
        public TgcMesh[] Faces { get; }
        public string[] FaceTextures { get; }
        public bool AlphaBlendEnable
        {
            get { return Faces[0].AlphaBlendEnable; }
            set
            {
                foreach (var face in Faces)
                {
                    face.AlphaBlendEnable = value;
                }
            }
        }
        #endregion

        public enum SkyFaces
        {
            Up = 0,
            Down = 1,
            Front = 2,
            Back = 3,
            Right = 4,
            Left = 5
        }

        public SkyboxShader()
        {
            Faces = new TgcMesh[6];
            FaceTextures = new string[6];
            SkyEpsilon = 25f;
            Color = Color.White;
            Center = TGCVector3.Empty;
            Size = new TGCVector3(1000, 1000, 1000);
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderCielo.fx");
        }

        public void Update()
        {
            //efecto.SetValue("_Time", GameModel.time);
        }

        public void Render()
        {
            foreach (var face in Faces)
            {
                face.Transform = TGCMatrix.Identity * Traslation;
                face.Render();
            }
        }

        public void Dispose()
        {
            foreach (var face in Faces)
            {
                face.Dispose();
            }
        }

        /// <summary>
        ///     Configurar la textura de una cara del SkyBox.
        ///     Para aplicar los cambios se debe llamar InitSkyBox.
        /// </summary>
        /// <param name="face">Cara del SkyBox</param>
        /// <param name="texturePath">Path de la textura</param>
        public void setFaceTexture(SkyFaces face, string texturePath)
        {
            FaceTextures[(int)face] = texturePath;
        }

        /// <summary>
        ///     Tomar los valores configurados y crear el SkyBox. Solo invocar en tiempo de INIT!!!
        /// </summary>
        public void Init()
        {
            //Crear cada cara
            for (var i = 0; i < Faces.Length; i++)
            {
                //Crear mesh de D3D
                var m = new Mesh(2, 4, MeshFlags.Managed, TgcSceneLoader.DiffuseMapVertexElements, D3DDevice.Instance.Device);
                var skyFace = (SkyFaces)i;

                // Cargo los vértices
                using (var vb = m.VertexBuffer)
                {
                    var data = vb.Lock(0, 0, LockFlags.None);
                    var colorRgb = Color.ToArgb();
                    cargarVertices(skyFace, data, colorRgb);
                    vb.Unlock();
                }

                // Cargo los índices
                using (var ib = m.IndexBuffer)
                {
                    var ibArray = new short[6];
                    cargarIndices(ibArray);
                    ib.SetData(ibArray, 0, LockFlags.None);
                }

                //Crear TgcMesh
                var faceName = Enum.GetName(typeof(SkyFaces), skyFace);
                var faceMesh = new TgcMesh(m, "SkyBox-" + faceName, TgcMesh.MeshRenderType.DIFFUSE_MAP);
                faceMesh.Materials = new[] { D3DDevice.DEFAULT_MATERIAL };
                faceMesh.createBoundingBox();
                faceMesh.Enabled = true;
                faceMesh.AutoTransform = false;

                //textura
                var texture = TgcTexture.createTexture(D3DDevice.Instance.Device, FaceTextures[i]);
                faceMesh.DiffuseMaps = new[] { texture };

                faceMesh.Effect = efecto;
                faceMesh.Technique = "RenderScene";// "apocalipsis";

                Faces[i] = faceMesh;
            }
        }

        #region cargarVertices
        private void cargarVertices(SkyFaces face, GraphicsStream data, int color)
        {
            switch (face)
            {
                case SkyFaces.Up:
                    cargarVerticesUp(data, color);
                    break;

                case SkyFaces.Down:
                    cargarVerticesDown(data, color);
                    break;

                case SkyFaces.Front:
                    cargarVerticesFront(data, color);
                    break;

                case SkyFaces.Back:
                    cargarVerticesBack(data, color);
                    break;

                case SkyFaces.Right:
                    cargarVerticesRight(data, color);
                    break;

                case SkyFaces.Left:
                    cargarVerticesLeft(data, color);
                    break;
            }
        }
        private void cargarVerticesUp(GraphicsStream data, int color)
        {
            TgcSceneLoader.DiffuseMapVertex v;
            var n = TGCVector3.Up;

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X - Size.X / 2 - SkyEpsilon,
                Center.Y + Size.Y / 2,
                Center.Z - Size.Z / 2 - SkyEpsilon
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 1;
            v.Tv = 0;
            data.Write(v);

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X - Size.X / 2 - SkyEpsilon,
                Center.Y + Size.Y / 2,
                Center.Z + Size.Z / 2 + SkyEpsilon
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 0;
            v.Tv = 0;
            data.Write(v);

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X + Size.X / 2 + SkyEpsilon,
                Center.Y + Size.Y / 2,
                Center.Z + Size.Z / 2 + SkyEpsilon
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 0;
            v.Tv = 1;
            data.Write(v);

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X + Size.X / 2 + SkyEpsilon,
                Center.Y + Size.Y / 2,
                Center.Z - Size.Z / 2 - SkyEpsilon
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 1;
            v.Tv = 1;
            data.Write(v);
        }
        private void cargarVerticesDown(GraphicsStream data, int color)
        {
            TgcSceneLoader.DiffuseMapVertex v;
            var n = TGCVector3.Down;

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X - Size.X / 2 - SkyEpsilon,
                Center.Y - Size.Y / 2,
                Center.Z + Size.Z / 2 + SkyEpsilon
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 0;
            v.Tv = 1;
            data.Write(v);

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X - Size.X / 2 - SkyEpsilon,
                Center.Y - Size.Y / 2,
                Center.Z - Size.Z / 2 - SkyEpsilon
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 1;
            v.Tv = 1;
            data.Write(v);

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X + Size.X / 2 + SkyEpsilon,
                Center.Y - Size.Y / 2,
                Center.Z - Size.Z / 2 - SkyEpsilon
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 1;
            v.Tv = 0;
            data.Write(v);

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X + Size.X / 2 + SkyEpsilon,
                Center.Y - Size.Y / 2,
                Center.Z + Size.Z / 2 + SkyEpsilon
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 0;
            v.Tv = 0;
            data.Write(v);
        }
        private void cargarVerticesFront(GraphicsStream data, int color)
        {
            TgcSceneLoader.DiffuseMapVertex v;
            var n = TGCVector3.Down;

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X - Size.X / 2 - SkyEpsilon,
                Center.Y + Size.Y / 2 + SkyEpsilon,
                Center.Z + Size.Z / 2
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 0;
            v.Tv = 0;
            data.Write(v);

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X - Size.X / 2 - SkyEpsilon,
                Center.Y - Size.Y / 2 - SkyEpsilon,
                Center.Z + Size.Z / 2
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 0;
            v.Tv = 1;
            data.Write(v);

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X + Size.X / 2 + SkyEpsilon,
                Center.Y - Size.Y / 2 - SkyEpsilon,
                Center.Z + Size.Z / 2
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 1;
            v.Tv = 1;
            data.Write(v);

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X + Size.X / 2 + SkyEpsilon,
                Center.Y + Size.Y / 2 + SkyEpsilon,
                Center.Z + Size.Z / 2
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 1;
            v.Tv = 0;
            data.Write(v);
        }
        private void cargarVerticesBack(GraphicsStream data, int color)
        {
            TgcSceneLoader.DiffuseMapVertex v;
            var n = TGCVector3.Down;

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X + Size.X / 2 + SkyEpsilon,
                Center.Y + Size.Y / 2 + SkyEpsilon,
                Center.Z - Size.Z / 2
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 0;
            v.Tv = 0;
            data.Write(v);

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X + Size.X / 2 + SkyEpsilon,
                Center.Y - Size.Y / 2 - SkyEpsilon,
                Center.Z - Size.Z / 2
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 0;
            v.Tv = 1;
            data.Write(v);

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X - Size.X / 2 - SkyEpsilon,
                Center.Y - Size.Y / 2 - SkyEpsilon,
                Center.Z - Size.Z / 2
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 1;
            v.Tv = 1;
            data.Write(v);

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X - Size.X / 2 - SkyEpsilon,
                Center.Y + Size.Y / 2 + SkyEpsilon,
                Center.Z - Size.Z / 2
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 1;
            v.Tv = 0;
            data.Write(v);
        }
        private void cargarVerticesRight(GraphicsStream data, int color)
        {
            TgcSceneLoader.DiffuseMapVertex v;
            var n = TGCVector3.Down;

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X + Size.X / 2,
                Center.Y + Size.Y / 2 + SkyEpsilon,
                Center.Z + Size.Z / 2 + SkyEpsilon
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 0;
            v.Tv = 0;
            data.Write(v);

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X + Size.X / 2,
                Center.Y - Size.Y / 2 - SkyEpsilon,
                Center.Z + Size.Z / 2 + SkyEpsilon
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 0;
            v.Tv = 1;
            data.Write(v);

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X + Size.X / 2,
                Center.Y - Size.Y / 2 - SkyEpsilon,
                Center.Z - Size.Z / 2 - SkyEpsilon
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 1;
            v.Tv = 1;
            data.Write(v);

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X + Size.X / 2,
                Center.Y + Size.Y / 2 + SkyEpsilon,
                Center.Z - Size.Z / 2 - SkyEpsilon
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 1;
            v.Tv = 0;
            data.Write(v);
        }
        private void cargarVerticesLeft(GraphicsStream data, int color)
        {
            TgcSceneLoader.DiffuseMapVertex v;
            var n = TGCVector3.Down;

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X - Size.X / 2,
                Center.Y + Size.Y / 2 + SkyEpsilon,
                Center.Z - Size.Z / 2 - SkyEpsilon
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 0;
            v.Tv = 0;
            data.Write(v);

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X - Size.X / 2,
                Center.Y - Size.Y / 2 - SkyEpsilon,
                Center.Z - Size.Z / 2 - SkyEpsilon
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 0;
            v.Tv = 1;
            data.Write(v);

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X - Size.X / 2,
                Center.Y - Size.Y / 2 - SkyEpsilon,
                Center.Z + Size.Z / 2 + SkyEpsilon
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 1;
            v.Tv = 1;
            data.Write(v);

            v = new TgcSceneLoader.DiffuseMapVertex();
            v.Position = new TGCVector3(
                Center.X - Size.X / 2,
                Center.Y + Size.Y / 2 + SkyEpsilon,
                Center.Z + Size.Z / 2 + SkyEpsilon
                );
            v.Normal = n;
            v.Color = color;
            v.Tu = 1;
            v.Tv = 0;
            data.Write(v);
        }
        #endregion

        /// <summary>
        ///     Generar array de indices
        /// </summary>
        private void cargarIndices(short[] ibArray)
        {
            var i = 0;
            ibArray[i++] = 0;
            ibArray[i++] = 1;
            ibArray[i++] = 2;
            ibArray[i++] = 0;
            ibArray[i++] = 2;
            ibArray[i++] = 3;
        }

        public void cambiarTechnique(string technique)
        {
            efecto.Technique = technique;
            for (int i = 0; i < 6; i++)
            {
                Faces[i].Technique = technique;
            }
        }
    }
}