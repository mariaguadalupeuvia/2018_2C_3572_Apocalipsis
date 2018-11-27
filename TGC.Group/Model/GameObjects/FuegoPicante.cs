using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;

namespace TGC.Group.Model.GameObjects
{
    public class FuegoPicante: Explosion
    { 
        public override void Init(TGCVector3 posicion)
        {
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");

            #region configurarObjetos
            semiesfera = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\icosfera2-TgcScene.xml").Meshes[0];
            semiesfera.Scale = new TGCVector3(100.5f, 100.5f, 100.5f);
            semiesfera.Effect = efecto;
            semiesfera.Technique = "Explosivo2";
            semiesfera.Position = new TGCVector3(posicion);
            #endregion

            GameSound.explotar();
        }
       
    }
}
