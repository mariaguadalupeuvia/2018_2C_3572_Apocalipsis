using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using Microsoft.DirectX.DirectInput;
using TGC.Core.Input;
using TGC.Group.Model.GameObjects.BulletObjects;

namespace TGC.Group.Model.GameObjects
{
    public class Canion : Planta
    {
        #region variables
        private TgcMesh canion;
        private TgcMesh tallo;
        float axisRotation = 0;
        float ayisRotation = 0;
        private const float AXIS_ROTATION_SPEED = 0.02f;
        #endregion

        public Canion(TGCVector3 posicion, GameLogic logica)
        {
            base.Init(logica);

            #region configurarObjeto
            float factorEscalado = 20.0f;
            canion = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\CANIONERO-TgcScene.xml").Meshes[0];
            canion.Scale = new TGCVector3(factorEscalado, factorEscalado, factorEscalado);
            canion.Position = new TGCVector3(posicion.X, posicion.Y + 40, posicion.Z);
            canion.RotateX(90);
            canion.RotateY(90);
            canion.Effect = efecto;
            canion.Technique = "RenderScene";

            tallo = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\PlantaFinal-TgcScene.xml").Meshes[0];
            tallo.Scale = new TGCVector3(factorEscalado * 0.75f, factorEscalado * 0.75f, factorEscalado * 0.75f);
            tallo.Position = new TGCVector3(posicion.X, posicion.Y - 50, posicion.Z);
            tallo.Effect = efecto;
            tallo.Technique = "RenderScene";

            #endregion
        }

        public override void Update(TgcD3dInput Input)
        {
            #region chequearInput
            if (Input.keyDown(Key.UpArrow))
            {
                axisRotation -= AXIS_ROTATION_SPEED * GameModel.time;
            }
            if (Input.keyDown(Key.DownArrow))
            {
                axisRotation += AXIS_ROTATION_SPEED * GameModel.time;
            }

            if (Input.keyDown(Key.RightArrow))
            {
                ayisRotation -= AXIS_ROTATION_SPEED * GameModel.time;
            }
            if (Input.keyDown(Key.LeftArrow))
            {
                ayisRotation += AXIS_ROTATION_SPEED * GameModel.time;
            }
            if (Input.keyUp(Key.P))
            {
                disparar();
            }
            #endregion

            canion.RotateX(axisRotation);
            canion.RotateY(ayisRotation);

            axisRotation = 0;
            ayisRotation = 0;
        }

        public override void Render()
        {
            canion.Render();
            tallo.Render();
        }

        public override void Dispose()
        {
            canion.Dispose();
            tallo.Dispose();
        }

        public override void cambiarTecnicaShader(string tecnica)
        {
            canion.Technique = tecnica;
            tallo.Technique = tecnica;
        }

        public void disparar()
        {
            Bala disparo = new Bala(canion, logica);
            disparo.init("Canionero", canion); //el parametro es el nombre de la textura para el mesh 
        }

        public override int getCostoEnSoles()
        {
            return 100;
        }
    }
}
