using BulletSharp.Math;
using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Camara;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Group.Model.Camara;

namespace TGC.Group.Model.GameObjects.BulletObjects.Zombies
{
    public class ZombieRey : Zombie
    {
        #region variables
        public static ThirdPersonCamera camaraInterna;
        protected TgcMesh corona;
        private const int MAXIMO_DAÑO_SOPORTADO = 100;
        public float MovementSpeed;
        public float RotationSpeed;
        private float leftrightRot;
        public static bool activo = false;
        #endregion

        public ZombieRey(TGCVector3 posicion, GameLogic logica) : base(posicion, logica)
        {
            #region configurarObjeto
            corona = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Sombrero-TgcScene.xml").Meshes[0];
            corona.Scale = new TGCVector3(40, 40, 40);
            corona.Position = posicion;
            corona.Effect = efecto;
            corona.Technique = "RenderSceneCongelada";
            zombie.Technique = "RenderSceneCongelada";
            zombie.Scale = new TGCVector3(65, 68, 65);
            objetos.Add(corona);
            #endregion

            MovementSpeed = 7;
            RotationSpeed = 0.1f;
            leftrightRot = FastMath.PI_HALF;
            camaraInterna = new ThirdPersonCamera(zombie.Position, new TGCVector3(0, zombie.Position.Y + 10 , 0), 50, 175);
            // camaraInterna.rotacion = TGCMatrix.RotationY(0);
            camaraInterna.rotacion = TGCMatrix.RotationY(leftrightRot);
        }
    
        public override void Render()
        {
           // zombie.Position = new TGCVector3(body.InterpolationWorldTransform.M41, body.InterpolationWorldTransform.M42, body.InterpolationWorldTransform.M43);
            //corona.Position = new TGCVector3(body.InterpolationWorldTransform.M41, body.InterpolationWorldTransform.M42 + 20, body.InterpolationWorldTransform.M43);
            zombie.Render();
            //corona.Render();
        }

        public void Update(TgcD3dInput Input)
        {
            if (!activo) return;

            #region manejarInput
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

            //Solo rotar si se esta aprentando el boton izq del mouse
            if (Input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                leftrightRot -= -Input.XposRelative * RotationSpeed;
                camaraInterna.rotacion = TGCMatrix.RotationY(leftrightRot);
            }
            #endregion

            zombie.Position = zombie.Position + moveVector;
            zombie.Position = new TGCVector3(zombie.Position.X, alturaEnPunto() + 30, zombie.Position.Z);
            //body.Translate(zombie.Position.ToBsVector);
            camaraInterna.Target = zombie.Position;
        }

        #region cosasPocoInteresantes
        private int alturaEnPunto()
        {
            int x = (int)(zombie.Position.X / 255f);
            int z = (int)(zombie.Position.Z / 255f);
            return (int)(Terreno.alturaEnPunto(x + 32, z + 32) * 1.7f);
        }

        public static void desactivarCamaraInterna()
        {
            activo = false;
        }

        public static TgcCamera activarCamaraInterna()
        {
            activo = true;
            return camaraInterna;
        }
        #endregion

        #region respuestaAAtaqueDePlanta
        public virtual void recibirDaño()
            {
                daño ++;
                if (daño == MAXIMO_DAÑO_SOPORTADO) //despues de X balazos quedas en caida libre, cuando tocas el piso vas a dispose
                {
                    morir();
                }
            }

        public override void congelate()
        {
            daño--; //cuando le disparan balas congelantes se fortalece
        }
        #endregion

        }
    }