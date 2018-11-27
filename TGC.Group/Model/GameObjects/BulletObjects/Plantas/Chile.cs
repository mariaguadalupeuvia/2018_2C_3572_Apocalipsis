using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model.GameObjects
{
    public class Chile : Planta
    {
        private TgcMesh chile;
        TgcMesh haloFuego;

        public Chile(TGCVector3 posicion, GameLogic logica, Plataforma plataforma)
        {
            base.Init(logica, plataforma);
           
            #region configurarObjeto
            float factorEscalado = 16.0f;
            chile = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Chile-TgcScene.xml").Meshes[0];
            chile.Scale = new TGCVector3(factorEscalado, factorEscalado, factorEscalado);
            chile.Position =  new TGCVector3(posicion.X , posicion.Y - 40, posicion.Z + 20);
            chile.Effect = efecto;
            chile.Technique = "Explosivo";

            haloFuego = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\semiesfera-TgcScene.xml").Meshes[0];
            haloFuego.Scale = new TGCVector3(100.5f, 100.5f, 100.5f);
            haloFuego.Effect = efecto;
            haloFuego.Technique = "calado";
            haloFuego.Position = new TGCVector3(posicion.X, 260, posicion.Z);
            #endregion

            Explosivo disparo = new Explosivo(new TGCVector3(posicion.X, posicion.Y - 40, posicion.Z + 20), logica, this);
            PostProcess.agregarPostProcessObject(this);
        }

        #region gestionTecnicasShader
        public override void cambiarTecnicaDefault()
        {
            chile.Technique = "RenderScene";
        }
        public override void cambiarTecnicaPostProceso()
        {
            chile.Technique = "dark";
        }
        public override void cambiarTecnicaShader(string tecnica)
        {
            chile.Technique = tecnica;
        }
        public override void cambiarTecnicaShadow(Texture shadowTex)
        {
            chile.Technique = "RenderShadow";
            efecto.SetValue("g_txShadow", shadowTex);
        }
        #endregion

        public override void Render()
        {
            chile.Render();
            haloFuego.Render();
        }

        public override void Dispose()
        {
            logica.agregarExplosionChile(chile.Position);
        }

        public override void Update(TgcD3dInput Input)
        {
            efecto.SetValue("_Time", GameModel.time);
            haloFuego.RotateY(GameModel.time);
        }

        public override int getCostoEnSoles()
        {
            return 300;
        }
    }
}
