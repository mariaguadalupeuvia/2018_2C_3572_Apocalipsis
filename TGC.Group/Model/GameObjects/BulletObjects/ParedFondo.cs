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
    public class ParedFondo : BulletObject, IPostProcess 
    {
        protected TgcMesh tag;
        public ParedFondo(GameLogic logica, Play play)
        {
            #region configurarEfecto
            Microsoft.DirectX.Direct3D.Effect efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            #endregion

            #region configurarObjeto
            tag = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Tag-TgcScene.xml").Meshes[0];
            tag.Scale = new TGCVector3(500, 500, 500);
            tag.Effect = efecto;
            tag.Technique = "RenderScene";
            tag.Position = new TGCVector3(100f, 340f, -3300f);
            tag.RotateZ(90.1f);

            objetos.Add(tag);

            body = FactoryBody.crearBodyPared(new TGCVector3(4000, 5, 1), new TGCVector3(0f, 215f, -3300f));
            callback = new CollisionCallbackFinal(logica, play);
            logica.addBulletObject(this);
            #endregion

            PostProcess.agregarPostProcessObject(this);
        }

        #region gestionarTecnicasShader
        public void cambiarTecnicaDefault()
        {
            tag.Technique = "RenderScene";
        }
        public void cambiarTecnicaPostProceso()
        {
            tag.Technique = "RenderScene";
        }
        #endregion

        public override void Update()
        {

        }

        public override void Render()
        {
            tag.Render();
        }
    }
}
