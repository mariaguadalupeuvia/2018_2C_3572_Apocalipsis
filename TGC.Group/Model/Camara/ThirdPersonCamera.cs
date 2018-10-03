using TGC.Core.Camara;
using TGC.Core.Mathematica;

namespace TGC.Group.Model.Camara
{
    public class ThirdPersonCamera : TgcCamera
    {
        #region variables
        private TGCVector3 position;
        public float OffsetHeight { get; set; }
        public float OffsetForward { get; set; }
        public TGCVector3 TargetDisplacement { get; set; }
        public float RotationY { get; set; }
        public TGCVector3 Target { get; set; }

        public TGCMatrix rotacion { get; set; }
        #endregion

        #region constructores
        public ThirdPersonCamera()
        {
            resetValues();
        }

        public ThirdPersonCamera(TGCVector3 target, float offsetHeight, float offsetForward) : this()
        {
            Target = target;
            OffsetHeight = offsetHeight;
            OffsetForward = offsetForward;
            RotationY = 0;
        }

        public ThirdPersonCamera(TGCVector3 target, TGCVector3 targetDisplacement, float offsetHeight, float offsetForward)
            : this()
        {
            Target = target;
            TargetDisplacement = targetDisplacement;
            OffsetHeight = offsetHeight;
            OffsetForward = offsetForward;
            RotationY = 0;
        }
        #endregion

        public override void UpdateCamera(float elapsedTime)
        {
            TGCVector3 targetCenter;
            CalculatePositionTarget(out position, out targetCenter);
            SetCamera(position, targetCenter);
        }

        #region auxiliares
        public void resetValues()
        {
            OffsetHeight = 20;
            OffsetForward = -120;
            RotationY = 0;
            TargetDisplacement = TGCVector3.Empty;
            Target = TGCVector3.Empty;
            position = TGCVector3.Empty;
        }

        public void setTargetOffsets(TGCVector3 target, float offsetHeight, float offsetForward)
        {
            Target = target;
            OffsetHeight = offsetHeight;
            OffsetForward = offsetForward;
        }

        public void CalculatePositionTarget(out TGCVector3 pos, out TGCVector3 targetCenter)
        {
            //alejarse, luego rotar y lueg ubicar camara en el centro deseado
            targetCenter = TGCVector3.Add(Target, TargetDisplacement);
            var m = TGCMatrix.Translation(0, OffsetHeight, OffsetForward) * rotacion/*TGCMatrix.RotationY(RotationY)*/ * TGCMatrix.Translation(targetCenter);

            //Extraer la posicion final de la matriz de transformacion
            pos = new TGCVector3(m.M41, m.M42, m.M43);
        }

        public void rotateY(float angle)
        {
            RotationY += angle;
        }
        #endregion
    }
}