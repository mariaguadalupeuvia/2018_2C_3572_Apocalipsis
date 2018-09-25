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
    public class Chile : Planta
    {
        private TgcMesh chile;

        public Chile(TGCVector3 posicion, GameLogic logica)
        {
            base.Init(logica);

            #region configurarObjeto
            float factorEscalado = 16.0f;
            chile = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Chile-TgcScene.xml").Meshes[0];
            chile.Scale = new TGCVector3(factorEscalado, factorEscalado, factorEscalado);
            chile.Position =  new TGCVector3(posicion.X , posicion.Y - 40, posicion.Z + 20);
            chile.Effect = efecto;
            chile.Technique = "Explosivo";
            #endregion
            Explosivo disparo = new Explosivo(chile, logica, this);
        }

        public override void Dispose()
        {
            logica.removePlanta(this);
        }

        public override void Render()
        {
            chile.Render();
        }

        public override void Update(TgcD3dInput Input)
        {
            efecto.SetValue("_Time", GameModel.time);
        }

        public override void cambiarTecnicaShader(string tecnica)
        {
            chile.Technique = tecnica;
        }

        public override int getCostoEnSoles()
        {
            return 300;
        }
    }
}
