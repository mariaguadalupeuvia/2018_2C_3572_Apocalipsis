using BulletSharp.Math;
using System;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;
using TGC.Group.Model.GameObjects.BulletObjects;

namespace TGC.Group.Model.GameObjects
{
    public class Sol : BulletObject
    {
        #region variables
        private TGCSphere esfera;
        private TgcMesh girasol;
        #endregion

        public Sol(TgcMesh girasol, GameLogic logica)
        {
            crearBody(girasol.Position, new TGCVector3(1, 2, 1));
            logica.addBulletObject(this);
            callback = new CollisionCallbackFloor(logica, this);
            
            this.girasol = girasol;
            GameLogic.cantidadEnergia += 50;
        }

        public void init()
        {
            var d3dDevice = D3DDevice.Instance.Device;

            #region configurarObjeto

            var texture = TgcTexture.createTexture(D3DDevice.Instance.Device, GameModel.mediaDir + "modelos\\Textures\\mina.jpg");
            esfera = new TGCSphere(1, texture.Clone(), TGCVector3.Empty);
            esfera.Scale = new TGCVector3(60.5f, 60.5f, 60.5f);
            esfera.Position = girasol.Position;
            esfera.Rotation = girasol.Rotation;
            esfera.updateValues();

            objetos.Add(esfera);

            #endregion
        }

        public override void Render()
        {
            //el body muere antes al colisionar y tira exception
            esfera.Transform = TGCMatrix.Scaling(15, 15, 15) * new TGCMatrix(body.InterpolationWorldTransform);
            esfera.Render();
        }

    }
}