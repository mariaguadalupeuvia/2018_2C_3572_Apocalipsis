using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Group.Model.GameObjects.BulletObjects.CollisionCallbacks;

namespace TGC.Group.Model.GameObjects.BulletObjects
{
    public class ParedFondo : BulletObject
    {
        protected TgcMesh tag;
        public ParedFondo(GameLogic logica)
        {
            #region configurarEfecto
            Microsoft.DirectX.Direct3D.Effect efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            #endregion

            #region configurarObjeto
            tag = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Tag-TgcScene.xml").Meshes[0];
            tag.Scale = new TGCVector3(1000.5f, 300.5f, 1000.5f);
            tag.Effect = efecto;
            tag.Technique = "RenderScene";
            tag.Position = new TGCVector3(0, 340f, 0);
            tag.RotateZ(3.1f);

            objetos.Add(tag);

            body = FactoryBody.crearBodyParedFinal(new TGCVector3(4800, 5, 1), new TGCVector3(1150, 300, 4060));

            callback = new CollisionCallbackFinal(logica);
            logica.addBulletObject(this);
            #endregion
        }

        public override void Update()
        {

        }

        public override void Render()
        {
            tag.Render();
        }
    }
}
