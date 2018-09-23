﻿using BulletSharp;
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
    public class Bala : BulletObject
    {
        #region variables
        private TGCSphere esfera;
        private TgcMesh canion;
        #endregion

        public Bala(TgcMesh canion, GameLogic logica)
        {
            crearBody(canion.Position, canion.Rotation);
            logica.addBulletObject(this);
            callback = new CollisionCallbackPlanta(logica, this);
            this.canion = canion;
        }

        public void init(string textura)
        {
            var d3dDevice = D3DDevice.Instance.Device;

            #region configurarObjeto

            var texture = TgcTexture.createTexture(D3DDevice.Instance.Device, GameModel.mediaDir + "modelos\\Textures\\" + textura + ".jpg");
            esfera = new TGCSphere(1, texture.Clone(), TGCVector3.Empty);
            esfera.Scale = new TGCVector3(40.5f, 40.5f, 40.5f);
            esfera.Position = canion.Position;
            esfera.Rotation = canion.Rotation;
            esfera.updateValues();
          
            objetos.Add(esfera);
            
            #endregion
        }

        //public override void Update()
        //{
        //    //TGCVector3 movimientoY = new TGCVector3(0, velocidadY -= gravedad, 0);
        //    //TGCVector3 movimiento = DireccionXZ() * velocidadX + movimientoY;
        //    //TGCVector3 translacion = movimiento * VELOCIDAD * GameModel.time;
        //    //esfera.Move(translacion);
        //}

        ////devuelve la direccion en el plano XZ
        // public TGCVector3 DireccionXZ()
        //{
        //    return new TGCVector3(FastMath.Sin(esfera.Rotation.Y), 0, FastMath.Cos(esfera.Rotation.Y));
        //}

        public override void Render()
        {
            //if (body != null) //el body muere antes al collisionar y tira exception
            //{
                body.Translate(new Vector3(7, 0, 7));
                esfera.Transform = TGCMatrix.Scaling(10, 10, 10) * new TGCMatrix(body.InterpolationWorldTransform);
            //}
            esfera.Render();
        }

    }
}