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

        public void crearBody(TGCVector3 origen)//este lo usan los zombies
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
            body.LinearVelocity = dir * 75;
            //body.LinearFactor = TGCVector3.One.ToBsVector;
            body.ApplyImpulse(dir , new TGCVector3(0, 0, 0).ToBsVector);  
        }

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
