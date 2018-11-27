using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Group.Model.GameObjects.BulletObjects;

namespace TGC.Group.Model.GameObjects
{
    public class Congelador : Planta
    {
        #region variables
        private TgcMesh congelador;
        private TgcMesh tallo;
        private int espera = 0;
        //float axisRotation = 0;
        //float ayisRotation = 0;
        //private const float AXIS_ROTATION_SPEED = 0.02f;
        #endregion

        public Congelador(TGCVector3 posicion, GameLogic logica, Plataforma plataforma)
        {
            base.Init(logica, plataforma);

            #region configurarObjeto
            float factorEscalado = 20.0f;
            congelador = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\CANIONERO-TgcScene.xml").Meshes[0];
            congelador.Scale = new TGCVector3(factorEscalado, factorEscalado, factorEscalado);
            congelador.Position = new TGCVector3(posicion.X, posicion.Y + 40, posicion.Z);
            congelador.RotateX(90);
            congelador.Effect = efecto;
            congelador.Technique = "RenderSceneCongelada";

            tallo = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\PlantaFinal-TgcScene.xml").Meshes[0];
            tallo.Scale = new TGCVector3(factorEscalado * 0.75f, factorEscalado * 0.75f, factorEscalado * 0.75f);
            tallo.Position = new TGCVector3(posicion.X, posicion.Y - 50, posicion.Z);
            tallo.Effect = efecto;
            tallo.Technique = "RenderScene";
            #endregion

            PostProcess.agregarPostProcessObject(this);
        }

        #region gestionTecnicasShader
        public override void cambiarTecnicaDefault()
        {
            congelador.Technique = "RenderSceneCongelada";
            tallo.Technique = "RenderScene";
        }
        public override void cambiarTecnicaPostProceso()
        {
            congelador.Technique = "dark";
            tallo.Technique = "dark";
        }
        public override void cambiarTecnicaShader(string tecnica)
        {
            congelador.Technique = tecnica;
            tallo.Technique = tecnica;
        }
        public override void cambiarTecnicaShadow(Texture shadowTex)
        {
            congelador.Technique = "RenderShadow";
            tallo.Technique = "RenderShadow";
            efecto.SetValue("g_txShadow", shadowTex);
        }
        #endregion

        public override void Render()
        {
            congelador.Render();
            tallo.Render();
        }

        public override void Update(TgcD3dInput Input)
        {
            #region chequearInput
            //if (Input.keyDown(Key.UpArrow))
            //{
            //    axisRotation -= AXIS_ROTATION_SPEED * GameModel.time;
            //}
            //if (Input.keyDown(Key.DownArrow))
            //{
            //    axisRotation += AXIS_ROTATION_SPEED * GameModel.time;
            //}

            //if (Input.keyDown(Key.RightArrow))
            //{
            //    ayisRotation -= AXIS_ROTATION_SPEED * GameModel.time;
            //}
            //if (Input.keyDown(Key.LeftArrow))
            //{
            //    ayisRotation += AXIS_ROTATION_SPEED * GameModel.time;
            //}
            //if (Input.keyUp(Key.P))
            //{
            //    disparar();
            //}
            #endregion

            espera++;
            if (espera == 300)
            {
                disparar();
                espera = 0;
            }
        }

        public override void Dispose()
        {
            congelador.Dispose();
            tallo.Dispose();
        }

        public void disparar()
        {
            BalaCongelante disparo = new BalaCongelante(congelador, logica);
            disparo.init("tuboSol", congelador); //el parametro es el nombre de la textura del mesh
        }

        public override int getCostoEnSoles()
        {
            return 300;
        }
    }
}
