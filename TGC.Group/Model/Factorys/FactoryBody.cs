using BulletSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.Textures;

namespace TGC.Group.Model.GameObjects.BulletObjects
{
    public static class FactoryBody
    {
        public static RigidBody crearBodyPlanta(TGCVector3 escala, TGCVector3 origen)
        {
            return crearBodyCubicoEstatico(escala, origen);
        }
        public static RigidBody crearBodyIsla(TGCVector3 escala, TGCVector3 origen)
        {
            return crearBodyCubicoEstatico(escala, origen);
        }
        public static RigidBody crearBodyZombie(TGCVector3 origen)//este lo usan los zombies
        {
            return crearBodyEsferico(origen, 200, 1);
        }
        public static RigidBody crearBodyExplosivo(TGCVector3 origen, float radio)//este lo usan las minas y los chiles
        {
            return crearBodyEsfericoEstatico(origen, radio);
        }

        #region crearBodysGenericos
        private static RigidBody crearBodyCubicoEstatico(TGCVector3 escala, TGCVector3 origen)
        {
            #region CAJA

            var boxShape = new BoxShape(escala.ToBsVector);
            var boxTransform = TGCMatrix.Identity;
            boxTransform.Origin = origen;
            DefaultMotionState boxMotionState = new DefaultMotionState(boxTransform.ToBsMatrix);
            var boxInfo = new RigidBodyConstructionInfo(0, boxMotionState, boxShape);
            return new RigidBody(boxInfo);

            #endregion
        }
        public static RigidBody crearBodyCubico(float masa, TGCVector3 escala, TGCVector3 origen)
        {
            #region CAJA

            //Se crea una caja de tamaño 20 con rotaciones y origien en 10,100,10 y 1kg de masa.
            var boxShape = new BoxShape(escala.ToBsVector);
            var boxTransform = TGCMatrix.RotationYawPitchRoll(MathUtil.SIMD_HALF_PI, MathUtil.SIMD_QUARTER_PI, MathUtil.SIMD_2_PI).ToBsMatrix;
            boxTransform.Origin = origen.ToBsVector; // new TGCVector3(0, 600, 0).ToBsVector;
            DefaultMotionState boxMotionState = new DefaultMotionState(boxTransform);
            //Es importante calcular la inercia caso contrario el objeto no rotara.
            var boxLocalInertia = boxShape.CalculateLocalInertia(masa);
            var boxInfo = new RigidBodyConstructionInfo(masa, boxMotionState, boxShape, boxLocalInertia);
            return new RigidBody(boxInfo);

            #endregion
        }
        public static RigidBody crearBodyEsfericoEstatico(TGCVector3 origen, float radio)
        {
            #region BOLA

            var ballShape = new SphereShape(radio);
            var ballTransform = TGCMatrix.Identity;
            ballTransform.Origin = origen;
            var ballMotionState = new DefaultMotionState(ballTransform.ToBsMatrix);
            var ballInfo = new RigidBodyConstructionInfo(0, ballMotionState, ballShape);
            return new RigidBody(ballInfo);

            #endregion
        }
        public static RigidBody crearBodyEsferico(TGCVector3 origen, float radio, float masa)
        {
            #region BOLA

            var ballShape = new SphereShape(radio);
            var ballTransform = TGCMatrix.Identity;
            ballTransform.Origin = origen;
            var ballMotionState = new DefaultMotionState(ballTransform.ToBsMatrix);
            var ballLocalInertia = ballShape.CalculateLocalInertia(masa);
            var ballInfo = new RigidBodyConstructionInfo(masa, ballMotionState, ballShape, ballLocalInertia);
            return new RigidBody(ballInfo);

            #endregion
        }

        public static RigidBody crearBodyConImpulso(TGCVector3 origen, float radio, float masa, TGCVector3 director)//este es para los disparos
        {
            RigidBody body = crearBodyEsferico(origen, radio, masa);
            // var dir = director.ToBsVector;
            //director.Normalize();
            TGCVector3 dir = new TGCVector3(0, 0, 90);
            //dir *= 50;
            //body.LinearVelocity = dir * 75;
            //body.LinearFactor = TGCVector3.One.ToBsVector;
            body.ApplyImpulse(dir.ToBsVector, new TGCVector3(0, 20, 0).ToBsVector);//new TGCVector3(0, 15, 0).ToBsVector, new TGCVector3(0, 20, 0).ToBsVector);
            return body;
        }

        #endregion
    }
}
