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
        public static RigidBody crearBodyCubico(float masa)
        {
            #region CAJA

            //Se crea una caja de tamaño 20 con rotaciones y origien en 10,100,10 y 1kg de masa.
            var boxShape = new BoxShape(10, 10, 10);
            var boxTransform = TGCMatrix.RotationYawPitchRoll(MathUtil.SIMD_HALF_PI, MathUtil.SIMD_QUARTER_PI, MathUtil.SIMD_2_PI).ToBsMatrix;
            boxTransform.Origin = new TGCVector3(0, 600, 0).ToBsVector;
            DefaultMotionState boxMotionState = new DefaultMotionState(boxTransform);
            //Es importante calcular la inercia caso contrario el objeto no rotara.
            var boxLocalInertia = boxShape.CalculateLocalInertia(masa);
            var boxInfo = new RigidBodyConstructionInfo(masa, boxMotionState, boxShape, boxLocalInertia);
            return new RigidBody(boxInfo);

            #endregion
        }
    }
}
