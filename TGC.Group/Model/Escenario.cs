using Microsoft.DirectX.Direct3D;
using System.Collections.Generic;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using BulletSharp;
using TGC.Core.Shaders;
using TGC.Core.Input;
using Microsoft.DirectX.DirectInput;

namespace TGC.Group.Model
{
    class Escenario : IRenderObject // aca voy a ir poniendo todos los objetos estaticos (y algunos otros como para probar)
    {
        private List<TgcMesh> meshes = new List<TgcMesh>();
        private List<TgcScene> scenes = new List<TgcScene>();

        protected Microsoft.DirectX.Direct3D.Effect efecto;
        public bool AlphaBlendEnable { get; set; }

        public void Init()
        {
            #region variables
            TgcMesh zombie;
            TgcMesh zombie1;
            TgcMesh zombie2;
            TgcMesh zombie3;
            TgcMesh tubo;
            TgcMesh contenedor;
            TgcMesh helicoptero;
            TgcMesh canion;
            TgcMesh planta;

            TgcScene girasoles;
            TgcScene roquedal;
            TgcScene roquedal2;
            TgcScene muelle1;
            #endregion

            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            var loader = new TgcSceneLoader();

            #region muelles

                girasoles = loader.loadSceneFromFile(GameModel.mediaDir + "modelos\\GIRASOLES_TUBO-TgcScene.xml");
                foreach (TgcMesh mesh in girasoles.Meshes)
                {
                    mesh.Scale =new TGCVector3(25.5f, 25.5f, 25.5f);
                    mesh.Position = new TGCVector3(mesh.Position.X, 260f, mesh.Position.Z);
                    mesh.Effect = efecto;
                    mesh.Technique = "RenderScene";
                }
            scenes.Add(girasoles);

                muelle1 = loader.loadSceneFromFile(GameModel.mediaDir + "modelos\\muelleGrande-TgcScene.xml");
                foreach (TgcMesh mesh in muelle1.Meshes)
                {
                    mesh.Scale = new TGCVector3(25.5f, 25.5f, 25.5f);
                    mesh.Position = new TGCVector3(0, 260f, mesh.Position.Z + 500f);
                    mesh.Effect = efecto;
                    mesh.Technique = "RenderScene";
                }
            scenes.Add(muelle1);
            #endregion

            #region piedras

            roquedal = loader.loadSceneFromFile(GameModel.mediaDir + "modelos\\ROQUEDAL-TgcScene.xml");
                foreach (TgcMesh mesh in roquedal.Meshes)
                {
                    mesh.Scale = new TGCVector3(90.5f, 90.5f, 90.5f);
                    mesh.Position = new TGCVector3(0, 10f, mesh.Position.Z + 5500f);
                    mesh.Effect = efecto;
                    mesh.Technique = "RenderScene";
                }
            scenes.Add(roquedal);
            roquedal2 = loader.loadSceneFromFile(GameModel.mediaDir + "modelos\\ROQUEDAL-TgcScene.xml");
                foreach (TgcMesh mesh in roquedal2.Meshes)
                {
                    mesh.Scale = new TGCVector3(90.5f, 90.5f, 90.5f);
                    mesh.Position = new TGCVector3(mesh.Position.X + 5500f, 10f, mesh.Position.Z + 5500f);
                    mesh.RotateY(90);
                    mesh.Effect = efecto;
                    mesh.Technique = "RenderScene";
                }
            scenes.Add(roquedal2);
            #endregion

            #region zombies

            zombie = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\ZombieZero-TgcScene.xml").Meshes[0];
                zombie.Scale = new TGCVector3(55.5f, 55.5f, 55.5f);
                zombie.Position = new TGCVector3(500f, 250f, 1800f);
                zombie.Effect = efecto;
                zombie.Technique = "RenderScene";
            meshes.Add(zombie);

                zombie1 = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\ZombieZero-TgcScene.xml").Meshes[0];
                zombie1.Scale = new TGCVector3(55.5f, 55.5f, 55.5f);
                zombie1.Position = new TGCVector3(800f, 250f,2000f);
                zombie1.Effect = efecto;
                zombie1.Technique = "RenderScene";
            meshes.Add(zombie1);

                zombie2 = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\ZombieZero-TgcScene.xml").Meshes[0];
                zombie2.Scale = new TGCVector3(55.5f, 55.5f, 55.5f);
                zombie2.Position = new TGCVector3(600f, 250f, 1400f);
                zombie2.Effect = efecto;
                zombie2.Technique = "RenderScene";
            meshes.Add(zombie2);

                zombie3 = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\ZombieZero-TgcScene.xml").Meshes[0];
                zombie3.Scale = new TGCVector3(55.5f, 55.5f, 55.5f);
                zombie3.Position = new TGCVector3(900f, 250f, 1300f);
                zombie3.Effect = efecto;
                zombie3.Technique = "RenderScene";
            meshes.Add(zombie3);

            #endregion

            #region otros

                tubo = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\TUBO_MARIO-TgcScene.xml").Meshes[0];
                tubo.Scale = new TGCVector3(35.5f, 35.5f, 35.5f);
                tubo.Position = new TGCVector3(1500f, 150f, 1500f);
                tubo.Effect = efecto;
                tubo.Technique = "RenderScene";

            meshes.Add(tubo);

            contenedor = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\contenedor-TgcScene.xml").Meshes[0];
            contenedor.Scale = new TGCVector3(45.5f, 45.5f, 45.5f);
            contenedor.Position = new TGCVector3(850f, 400f, 100f);
            contenedor.Effect = efecto;
            contenedor.Technique = "RenderSceneBlend";

            meshes.Add(contenedor);

            helicoptero = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\HelicopteroMilitar3-TgcScene.xml").Meshes[0];
                helicoptero.Scale = new TGCVector3(25.5f, 25.5f, 25.5f);
                helicoptero.Position = new TGCVector3(5200f, 200f, 500f);
                helicoptero.RotateY(90);
                helicoptero.Effect = efecto;
                helicoptero.Technique = "RenderScene";

            meshes.Add(helicoptero);

            #endregion

            #region planta

                //canion = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\CANIONERO-TgcScene.xml").Meshes[0];
                //canion.Scale = new TGCVector3(40.5f, 40.5f, 40.5f);
                //canion.Position = new TGCVector3(500f, 420f, 1500f);
                //canion.RotateX(90);
                //canion.RotateY(90);
                //canion.Effect = efecto;
                //canion.Technique = "RenderScene";
                //gameObjects.Add(canion);

                //planta = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\PLANTA-TgcScene.xml").Meshes[0];
                //planta.Scale = new TGCVector3(35.5f, 35.5f, 35.5f);
                //planta.Position = new TGCVector3(500f, 200f, 1500f);
           
                //planta.Effect = efecto;
                //planta.Technique = "RenderScene";

                //gameObjects.Add(planta);

            #endregion
        }
        public void Update()
        {
            efecto.SetValue("_Time", GameModel.time);
        }
        public void Render()
        {
            meshes.ForEach(m => m.Render());
            scenes.ForEach(s => s.RenderAll());
        }

        public void Dispose()
        {
            meshes.ForEach(m => m.Dispose());
            scenes.ForEach(s => s.DisposeAll());
        }
    }
}
