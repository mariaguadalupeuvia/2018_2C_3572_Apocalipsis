using Microsoft.DirectX.Direct3D;
using System.Collections.Generic;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using BulletSharp;
using TGC.Core.Shaders;

namespace TGC.Group.Model
{
    class Escenario : IRenderObject // aca voy a ir poniendo todos los objetos estaticos
    {
        private List<TgcMesh> gameObjects = new List<TgcMesh>();

        private TgcScene girasoles;
        private TgcScene roquedal;
        private TgcScene muelle1;

        private TgcMesh tubo { get; set; }

        protected Effect efecto;

        public bool AlphaBlendEnable { get; set; }

        public void Init()
        {
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");



            var loader = new TgcSceneLoader();
            girasoles = loader.loadSceneFromFile(GameModel.mediaDir + "modelos\\GIRASOLES_TUBO-TgcScene.xml");
            foreach (TgcMesh mesh in girasoles.Meshes)
            {
                mesh.Scale =new TGCVector3(25.5f, 25.5f, 25.5f);
                mesh.Position = new TGCVector3(mesh.Position.X, 260f, mesh.Position.Z);
                mesh.Effect = efecto;
                mesh.Technique = "RenderScene";
            }

            roquedal = loader.loadSceneFromFile(GameModel.mediaDir + "modelos\\ROQUEDAL-TgcScene.xml");
            foreach (TgcMesh mesh in roquedal.Meshes)
            {
                mesh.Scale = new TGCVector3(90.5f, 90.5f, 90.5f);
                mesh.Position = new TGCVector3(0, 10f, mesh.Position.Z + 5500f);
                mesh.Effect = efecto;
                mesh.Technique = "RenderScene";
            }

            muelle1 = loader.loadSceneFromFile(GameModel.mediaDir + "modelos\\MUELLE-TgcScene.xml");
            foreach (TgcMesh mesh in muelle1.Meshes)
            {
                mesh.Scale = new TGCVector3(25.5f, 25.5f, 25.5f);
                mesh.Position = new TGCVector3(0, 260f, mesh.Position.Z + 500f);
                mesh.Effect = efecto;
                mesh.Technique = "RenderScene";
            }

            tubo = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\TUBO_MARIO-TgcScene.xml").Meshes[0];
            tubo.Scale = new TGCVector3(35.5f, 35.5f, 35.5f);
            tubo.Position = new TGCVector3(500f, 200f, 1500f);
            tubo.Effect = efecto;
            tubo.Technique = "RenderScene";

            gameObjects.Add(tubo);
        }

        public void Render()
        {
            foreach (TgcMesh mesh in gameObjects)
            {
                mesh.Render();
            }
            muelle1.RenderAll();
            girasoles.RenderAll();
            roquedal.RenderAll();
        }

        public void Dispose()
        {
            foreach (TgcMesh mesh in gameObjects)
            {
                mesh.Dispose();
            }
            muelle1.DisposeAll();
            girasoles.DisposeAll();
            roquedal.DisposeAll();
        }
    }
}
