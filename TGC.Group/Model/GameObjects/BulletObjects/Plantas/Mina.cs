using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model.GameObjects
{
    public class Mina : Planta
    {
        #region variables
        private TgcMesh mina;
        #endregion

        public Mina(TGCVector3 posicion, GameLogic logica, Plataforma plataforma)
        {
            base.Init(logica, plataforma);

            #region configurarObjeto
            mina = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Mina-TgcScene.xml").Meshes[0];
            mina.Scale = new TGCVector3(35.5f, 35.5f, 35.5f);
            mina.Position = new TGCVector3(posicion.X , posicion.Y - 35, posicion.Z - 50);
            mina.Effect = efecto;
            mina.Technique = "RenderScene";
            #endregion

            Explosivo disparo = new Explosivo(new TGCVector3(posicion.X, posicion.Y - 40, posicion.Z + 20), logica, this);
            PostProcess.agregarPostProcessObject(this);
        }

        #region gestionTecnicasShader
        public override void cambiarTecnicaDefault()
        {
            mina.Technique = "RenderScene";
        }
        public override void cambiarTecnicaPostProceso()
        {
            mina.Technique = "dark";
        }
        public override void cambiarTecnicaShader(string tecnica)
        {
            mina.Technique = tecnica;
        }
        public override void cambiarTecnicaShadow(Texture shadowTex)
        {
            mina.Technique = "RenderShadow";
            efecto.SetValue("g_txShadow", shadowTex);
        }
        #endregion

        public override void Render()
        {
            mina.Render();
        }

        public override void Dispose()
        {
            logica.agregarExplosion(mina.Position);  
        }

        public override void Update(TgcD3dInput Input)
        {
        }

        public override int getCostoEnSoles()
        {
            return 150;
        }
    }
}
