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
using System.Timers;

namespace TGC.Group.Model.GameObjects
{
    public class Girasol : Planta
    {
        #region variables
        private TgcMesh girasol;
        private TgcMesh tallo;
        //private int espera = 0;
        static Timer time;
        static bool tiempoCumplido = false;
        #endregion

        private const int INTERVALO = 10000;

        public Girasol(TGCVector3 posicion, GameLogic logica, Plataforma plataforma)
        {
            base.Init(logica, plataforma);

            #region configurarObjeto

            float factorEscalado = 30.0f;
            girasol = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Girasol-TgcScene.xml").Meshes[0];
            girasol.Scale = new TGCVector3(factorEscalado, factorEscalado, factorEscalado);
            girasol.Position = new TGCVector3(posicion.X - 8 , posicion.Y + 35, posicion.Z - 10);
            girasol.RotateX(60);
            girasol.RotateY(90);
            girasol.RotateZ(90);
            girasol.Effect = efecto;
            girasol.Technique = "RenderScene";

            tallo = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\PlantaFinal-TgcScene.xml").Meshes[0];
            tallo.Scale = new TGCVector3(factorEscalado * 0.5f, factorEscalado * 0.5f, factorEscalado * 0.5f);
            tallo.Position = new TGCVector3(posicion.X  , posicion.Y - 50, posicion.Z );// new TGCVector3(500f, 200f, 1500f);
            tallo.Effect = efecto;
            tallo.Technique = "RenderScene";

            #endregion

            #region manejarTiempo
            time = new Timer(INTERVALO);
            time.Elapsed += OnTimedEvent;
            time.AutoReset = true;
            time.Enabled = true;
            #endregion

            PostProcess.agregarPostProcessObject(this);
        }

        #region gestionTecnicasShader
        public override void cambiarTecnicaDefault()
        {
            girasol.Technique = "RenderScene";
            tallo.Technique = "RenderScene";
        }
        public override void cambiarTecnicaPostProceso()
        {
            girasol.Technique = "dark";
            tallo.Technique = "dark";
        }
        public override void cambiarTecnicaShader(string tecnica)
        {
            girasol.Technique = tecnica;
            tallo.Technique = tecnica;
        }
        public override void cambiarTecnicaShadow(Texture shadowTex)
        {
            girasol.Technique = "RenderShadow";
            tallo.Technique = "RenderShadow";
            efecto.SetValue("g_txShadow", shadowTex);
        }
        #endregion

        static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            tiempoCumplido = true;
        }
        public override void Update(TgcD3dInput Input)
        {
            if (tiempoCumplido)
            {
                tiempoCumplido = false;
                disparar();
            }
        }

        public override void Render()
        {
            girasol.Render();
            tallo.Render();
        }

        public override void Dispose()
        {
            girasol.Dispose();
            tallo.Dispose();
        }

        public void disparar()
        {
            Sol disparo = new Sol(girasol, logica);
            disparo.init("mina", girasol);
            disparo.agrandarMesh();
            GameLogic.cantidadEnergia += 50;
        }

        public override int getCostoEnSoles()
        {
            return 50;
        }
    }
}