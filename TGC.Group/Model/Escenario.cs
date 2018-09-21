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
    class Escenario : GameObject // aca voy a ir poniendo todos los objetos estaticos (y algunos otros como para probar)
    {
        #region variables
        private List<TgcMesh> meshes = new List<TgcMesh>();
        private List<TgcScene> scenes = new List<TgcScene>();
        #endregion

        public override void Init()
        {
            #region variables
            TgcMesh tubo;
            TgcMesh flecha;

            TgcMesh contenedor;
            TgcMesh helicoptero;
   
            TgcScene roquedal;
            TgcScene roquedal2;
            TgcScene muelle1;
            #endregion

            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            var loader = new TgcSceneLoader();

            #region muelles
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

            #region otros
            tubo = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\TUBO_MARIO-TgcScene.xml").Meshes[0];
            tubo.Scale = new TGCVector3(35.5f, 35.5f, 35.5f);
            tubo.Position = new TGCVector3(1500f, 150f, 1500f);
            tubo.Effect = efecto;
            tubo.Technique = "RenderScene";

            meshes.Add(tubo);

            flecha = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Flecha-TgcScene.xml").Meshes[0];
            flecha.Scale = new TGCVector3(320.5f, 320.5f, 320.5f);
            flecha.Position = new TGCVector3(0, 450f, 4800f);
            flecha.RotateY(150);
            flecha.RotateZ(150);
            flecha.Effect = efecto;
            flecha.Technique = "RenderScene";

            meshes.Add(flecha);



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
        }

        public override void Update()
        {
            efecto.SetValue("_Time", GameModel.time);
        }

        public override void Render()
        {
            meshes.ForEach(m => m.Render());
            scenes.ForEach(s => s.RenderAll());
        }

        public override void Dispose()
        {
            meshes.ForEach(m => m.Dispose());
            scenes.ForEach(s => s.DisposeAll());
        }

    }
}
