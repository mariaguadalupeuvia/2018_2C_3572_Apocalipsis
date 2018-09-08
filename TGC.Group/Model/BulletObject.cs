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

namespace TGC.Group.Model
{
    public class BulletObject : GameObject
    {
        public RigidBody body { get; set; }
        public ContactResultCallback callback { get; set; }

        float radio = 10;
        TGCVector3 origen = new TGCVector3(0, 600, 0);
        float masa = 1f;

        public override void Init()
        {
        }

        public void crearBody()
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
           // body.ApplyImpulse(new TGCVector3(100, 200, 0).ToBsVector, new TGCVector3(0, 0, 0).ToBsVector);
            callback = new CollisionCallback(this);

        }

        public override void Dispose()
        {
            body.Dispose();
            base.Dispose();
        }

        public override void Update()
        {
            Console.WriteLine("Update no implementado");
            throw new NotImplementedException();
        }
    }
}
