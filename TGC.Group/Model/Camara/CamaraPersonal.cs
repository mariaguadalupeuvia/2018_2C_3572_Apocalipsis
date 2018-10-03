using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using System.Drawing;
using System.Windows.Forms;
using TGC.Core.Camara;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Mathematica;

namespace TGC.Group.Model
{
    public class CamaraPersonal : TgcCamera
    {
        #region variables
        private readonly Point mouseCenter;
        private TGCMatrix cameraRotation;
        private TGCVector3 directionView;
        private float leftrightRot;
        private float updownRot;
        private TGCVector3 positionEye;
        private TgcD3dInput Input { get; }
        public float MovementSpeed { get; set; }
        public float RotationSpeed { get; set; }
        public float JumpSpeed { get; set; }
        #endregion

        public CamaraPersonal(TgcD3dInput input)
        {
            #region inicializarVariables
            this.Input = input;
            this.positionEye = TGCVector3.Empty;
            this.mouseCenter = new Point(D3DDevice.Instance.Device.Viewport.Width / 2, D3DDevice.Instance.Device.Viewport.Height / 2);
            this.RotationSpeed = 0.1f;
            this.MovementSpeed = 500f;
            this.JumpSpeed = 500f;
            this.directionView = new TGCVector3(0.8f, -0.8f, -1); //new TGCVector3(0, 0, -1); 
            this.leftrightRot = FastMath.PI_HALF;
            this.updownRot = -FastMath.PI / 10.0f;
            this.cameraRotation = TGCMatrix.RotationX(updownRot) * TGCMatrix.RotationY(leftrightRot);///*TGCMatrix.RotationX(updownRot) **/ TGCMatrix.RotationY(leftrightRot);
            #endregion
        }

        public CamaraPersonal(TGCVector3 positionEye, TgcD3dInput input) : this(input)
        {
            this.positionEye = positionEye;
        }

        private TGCVector3 actualizarMoveVector()
        {
            var moveVector = TGCVector3.Empty;

            //Forward
            if (Input.keyDown(Key.W))
            {
                moveVector += new TGCVector3(0, 0, -1) * MovementSpeed;
            }
            //Backward
            if (Input.keyDown(Key.S))
            {
                moveVector += new TGCVector3(0, 0, 1) * MovementSpeed;
            }
            //Strafe right
            if (Input.keyDown(Key.D))
            {
                moveVector += new TGCVector3(-1, 0, 0) * MovementSpeed;
            }
            //Strafe left
            if (Input.keyDown(Key.A))
            {
                moveVector += new TGCVector3(1, 0, 0) * MovementSpeed;
            }
            ////Jump
            //if (Input.keyDown(Key.Space))
            //{
            //    moveVector += TGCVector3.Up * JumpSpeed;
            //}
            ////Crouch
            //if (Input.keyDown(Key.LeftControl))
            //{
            //    moveVector += TGCVector3.Down * JumpSpeed;
            //}

            return moveVector;
        }

        public override void UpdateCamera(float elapsedTime)
        {
            var moveVector = actualizarMoveVector();

            #region actualizarTransformacion
            //Solo rotar si se esta aprentando el boton izq del mouse
            if (Input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                leftrightRot -= -Input.XposRelative * RotationSpeed;
                updownRot -= Input.YposRelative * RotationSpeed;
                cameraRotation = /*TGCMatrix.RotationX(updownRot)* */  TGCMatrix.RotationY(leftrightRot);
            }

            var cameraRotatedPositionEye = TGCVector3.TransformNormal(moveVector * elapsedTime, cameraRotation);
            positionEye += cameraRotatedPositionEye;

            //Calculamos el target de la camara, segun su direccion inicial y las rotaciones en screen space x,y.
            var cameraRotatedTarget = TGCVector3.TransformNormal(directionView, cameraRotation);
            var cameraFinalTarget = positionEye + cameraRotatedTarget;

            //Se calcula el nuevo vector de up producido por el movimiento del update.
            var cameraOriginalUpVector = DEFAULT_UP_VECTOR;
            var cameraRotatedUpVector = TGCVector3.TransformNormal(cameraOriginalUpVector, cameraRotation);

            #endregion

            base.SetCamera(positionEye, cameraFinalTarget, cameraRotatedUpVector);
        }
    }
}

