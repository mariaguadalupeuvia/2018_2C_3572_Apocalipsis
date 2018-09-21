using BulletSharp.Math;
using System;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;

namespace TGC.Group.Model.GameObjects
{
    public class Sol : BulletObject
    {
        private TGCSphere esfera;
        private TgcMesh canion;

        public Sol(TgcMesh canion, GamePhysics world)
        {
            physicWorld = world;
            crearBody(canion.Position, new TGCVector3(1, 2, 1));
            this.canion = canion;
        }

        public void init()
        {
            var d3dDevice = D3DDevice.Instance.Device;

            #region configurarObjeto

            var texture = TgcTexture.createTexture(D3DDevice.Instance.Device, GameModel.mediaDir + "modelos\\Textures\\mina.jpg");
            esfera = new TGCSphere(1, texture.Clone(), TGCVector3.Empty);
            esfera.Scale = new TGCVector3(60.5f, 60.5f, 60.5f);
            esfera.Position = canion.Position;
            esfera.Rotation = canion.Rotation;

            esfera.updateValues();

            objetos.Add(esfera);

            #endregion
        }


        public void render()
        {
            try //el body muere antes al collisionar y tira exception
            {
               // body.Translate(new Vector3(7, 0, 7));
                esfera.Transform = TGCMatrix.Scaling(20, 20, 20) * new TGCMatrix(body.InterpolationWorldTransform);
            }
            catch { }

            esfera.Render();
        }

        public void dispose()
        {
            esfera.Dispose();
        }
    }
}