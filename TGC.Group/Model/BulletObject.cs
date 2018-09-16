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

namespace TGC.Group.Model
{
    public class BulletObject : GameObject
    {
        public RigidBody body { get; set; }
        public ContactResultCallback callback { get; set; }
        protected GamePhysics physicWorld;

        float radio = 10;
        float masa = 0.1f;

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
            callback = new CollisionCallback(this, physicWorld);

        }

        public void crearBody(TGCVector3 origen, TGCVector3 director)//este es para los disparos
        {
            var ballShape = new SphereShape(radio);
            var ballTransform = TGCMatrix.Identity;
            ballTransform.Origin = origen;

            //var ballTransform = TGCMatrix.RotationYawPitchRoll(MathUtil.SIMD_HALF_PI, MathUtil.SIMD_QUARTER_PI, MathUtil.SIMD_2_PI).ToBsMatrix;
            //ballTransform.Origin = new TGCVector3(0, 600, 0).ToBsVector;

            var ballMotionState = new DefaultMotionState(ballTransform.ToBsMatrix);
            //Podriamos no calcular la inercia para que no rote, pero es correcto que rote tambien.
            var ballLocalInertia = ballShape.CalculateLocalInertia(masa);
            var ballInfo = new RigidBodyConstructionInfo(masa, ballMotionState, ballShape, ballLocalInertia);
           
            body = new RigidBody(ballInfo);
            var dir = director.ToBsVector;// new TGCVector3(100, 100, 1).ToBsVector;
            dir.Normalize();
            body.LinearVelocity = dir * 75;
           // body.LinearFactor = TGCVector3.One.ToBsVector;
            body.ApplyImpulse(dir , new TGCVector3(0, 0, 0).ToBsVector);
            callback = new CollisionCallback(this, physicWorld);
        }

        public override void Dispose()
        {
            body.Dispose();
            base.Dispose();
        }

        public override void Update()
        {
            //Console.WriteLine("Update no implementado");
            //throw new NotImplementedException();
        }
    }
}
