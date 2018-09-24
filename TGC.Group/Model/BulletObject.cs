using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulletSharp;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.Textures;
using TGC.Group.Model.GameObjects.BulletObjects;
using Microsoft.DirectX.Direct3D;
using BulletSharp.Math;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class BulletObject : GameObject
    {
        #region variables
        public RigidBody body { get; set; }
        public ContactResultCallback callback { get; set; }

        float radio = 10;
        float masa = 0.1f;
        #endregion

        public override void Init()
        {
        }

        #region crearBodys
        public void crearBodyZombie(TGCVector3 origen)//este lo usan los zombies
        {
            radio = 200;
            crearBody(origen);
        }

        public void crearBodyExplosivo(TGCVector3 origen, float radio)//este lo usan las minas y los chiles
        {
            this.radio = radio;
            crearBodyEstatico(origen);
        }

        public void crearBodyEstatico(TGCVector3 origen)
        {
            var ballShape = new SphereShape(radio);
            var ballTransform = TGCMatrix.Identity;
            ballTransform.Origin = origen;

            var ballMotionState = new DefaultMotionState(ballTransform.ToBsMatrix);
            var ballInfo = new RigidBodyConstructionInfo(0, ballMotionState, ballShape);

            body = new RigidBody(ballInfo);
        }


        public void crearBody(TGCVector3 origen)
        {
            var ballShape = new SphereShape(radio);
            var ballTransform = TGCMatrix.Identity;
            ballTransform.Origin = origen;

            var ballMotionState = new DefaultMotionState(ballTransform.ToBsMatrix);
            var ballLocalInertia = ballShape.CalculateLocalInertia(masa);
            var ballInfo = new RigidBodyConstructionInfo(masa, ballMotionState, ballShape, ballLocalInertia);

            body = new RigidBody(ballInfo);
        }

        public void crearBody(TGCVector3 origen, TGCVector3 director)//este es para los disparos
        {
            crearBody(origen);

            var dir = director.ToBsVector;
            dir.Normalize();
            //body.LinearVelocity = dir * 75;
            //body.LinearFactor = TGCVector3.One.ToBsVector;
            body.ApplyImpulse(new TGCVector3(0, 15, 0).ToBsVector, new TGCVector3(0, 20, 0).ToBsVector);  
        }
        #endregion

        public override void Dispose()
        {
            body.Dispose();
            base.Dispose();
        }

        public override void Update()
        {
           // Console.WriteLine("Update no implementado");
        }
 
    }
}
