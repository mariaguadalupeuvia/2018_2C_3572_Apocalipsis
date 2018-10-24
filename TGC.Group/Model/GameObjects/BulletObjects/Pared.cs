using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Group.Model.EstadosJuego;
using TGC.Group.Model.GameObjects.BulletObjects.CollisionCallbacks;

namespace TGC.Group.Model.GameObjects.BulletObjects
{
    public class Pared : BulletObject
    {
        protected TgcMesh tag;
        float rotador = 0;

        public Pared(GameLogic logica, float x)
        {
            #region configurarEfecto
            Microsoft.DirectX.Direct3D.Effect efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            #endregion

            #region configurarObjeto
            tag = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Tag-TgcScene.xml").Meshes[0];
            tag.Scale = new TGCVector3(100, 100, 100);
            tag.Effect = efecto;
            tag.Technique = "RenderScene";
            tag.Position = new TGCVector3(x, 440f, -3300f);
            tag.RotateZ(90.1f);

            objetos.Add(tag);

            //x = -850, -150, 550, 1250
            body = FactoryBody.crearBodyPared(new TGCVector3(150, 50, 50), new TGCVector3(x, 215f, -3300));//new TGCVector3(320, 5, 3300), new TGCVector3(x, 215f, 0));
            callback = new CollisionCallbackWall(logica);
            logica.addBulletObject(this);
            #endregion
        }

        public override void Update()
        {
            rotador += 0.01f;
            tag.RotateZ(rotador);
        }

        public override void Render()
        {
            tag.Render();
        }
    }
}