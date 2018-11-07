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

namespace TGC.Group.Model.GameObjects.BulletObjects.Plantas
{
    public class Nuez : Planta
    {
        #region variables
        private TgcMesh nuez;
        #endregion

        public Nuez(TGCVector3 posicion, GameLogic logica, Plataforma plataforma)
        {
            base.Init(logica, plataforma);

            #region configurarObjeto

            float factorEscalado = 160.0f;

            nuez = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\nuez-TgcScene.xml").Meshes[0];
            nuez.Scale = new TGCVector3(factorEscalado * 0.5f, factorEscalado * 0.5f, factorEscalado * 0.5f);
            nuez.Position = new TGCVector3(posicion.X , posicion.Y + 60, posicion.Z + 5);
            nuez.RotateZ(1.5f);
            nuez.RotateY(90);
            nuez.Effect = efecto;
            nuez.Technique = "RenderScene";

            #endregion

            nivelResistencia = 10000;
            PostProcess.agregarPostProcessObject(this);
        }

        #region gestionTecnicasShader
        public override void cambiarTecnicaDefault()
        {
            nuez.Technique = "RenderScene";
        }
        public override void cambiarTecnicaPostProceso()
        {
            nuez.Technique = "dark";
        }
        public override void cambiarTecnicaShader(string tecnica)
        {
            nuez.Technique = tecnica;
        }
        #endregion

        public override void Update(TgcD3dInput Input)
        {
        }

        public override void Render()
        {
            nuez.Render();
        }

        public override void Dispose()
        {
            nuez.Dispose();
        }

        public void disparar()
        {
        }

        public override int getCostoEnSoles()
        {
            return 250;
        }
    }
}