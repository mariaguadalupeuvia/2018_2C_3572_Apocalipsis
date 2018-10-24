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

            #region manejarAltura
            
            int x = (int)(zombie.Position.X / 255f);
            int z = (int)(zombie.Position.Z / 255f);
            int y = alturaEnPunto(x, z);

            if (y != -1)
            {
                zombie.Position = zombie.Position + moveVector;
                zombie.Position = new TGCVector3(zombie.Position.X, y + 30, zombie.Position.Z);
                //body.Translate(zombie.Position.ToBsVector);
            }
            else
            {
                if (z < 0) z = 0;
                if (z > 63) z = 63;
                if (x < 0) x = 0;
                if (x > 63) x = 63;
                zombie.Position = new TGCVector3(x, zombie.Position.Y, z);
            }
            #endregion

            camaraInterna.Target = zombie.Position;
        }

        #region cosasPocoInteresantes
        private int alturaEnPunto(int x, int z)
        {
            //int x = (int)(zombie.Position.X / 255f);
            //int z = (int)(zombie.Position.Z / 255f);
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