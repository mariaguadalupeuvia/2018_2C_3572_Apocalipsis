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

        public Chile(GamePhysics world, TGCVector3 posicion)
        {
            base.Init(world);

            #region configurarObjeto
            float factorEscalado = 16.0f;
            chile = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Chile-TgcScene.xml").Meshes[0];
            chile.Scale = new TGCVector3(factorEscalado, factorEscalado, factorEscalado);
            chile.Position =  new TGCVector3(posicion.X , posicion.Y - 40, posicion.Z + 20);
            chile.Effect = efecto;
            chile.Technique = "RenderScene";

            #endregion
        }

        public override void Dispose()
        {
            chile.Dispose();
        }

        public override void Render()
        {
            chile.Render();
        }

        public override void Update(TgcD3dInput Input)
        {
          
        }
        public override int getCostoEnSoles()
        {
            return 300;
        }
        public override void cambiarTecnicaShader(string tecnica)
        {
            chile.Technique = tecnica;
        }
    }
}
