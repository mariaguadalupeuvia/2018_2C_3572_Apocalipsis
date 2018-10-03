using BulletSharp;
using BulletSharp.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Core.Textures;

namespace TGC.Group.Model.GameObjects.BulletObjects
{
    public abstract class Disparo : BulletObject
    {
        #region variables
        protected TGCSphere esfera;
        #endregion

        public void init(string textura, TgcMesh planta)
        {
            var d3dDevice = D3DDevice.Instance.Device;

            #region configurarObjeto

            var texture = TgcTexture.createTexture(D3DDevice.Instance.Device, GameModel.mediaDir + "modelos\\Textures\\" + textura + ".jpg");
            esfera = new TGCSphere(1, texture.Clone(), TGCVector3.Empty);
            esfera.Scale = new TGCVector3(30.5f, 30.5f, 30.5f);
            esfera.Position = planta.Position;
            esfera.Rotation = planta.Rotation;
            esfera.updateValues();

            objetos.Add(esfera);

            #endregion
        }

        public override void Render()
        {
            //body.Translate(new Vector3(0, 0, 17));
            esfera.Transform = TGCMatrix.Scaling(10, 10, 10) * new TGCMatrix(body.InterpolationWorldTransform);
            esfera.Render();
        }

        public abstract void dañarZombie(Zombie zombie);

    }
}