using Microsoft.DirectX.Direct3D;
using System.Collections.Generic;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using BulletSharp;
using TGC.Core.Shaders;
using TGC.Core.Input;
using Microsoft.DirectX.DirectInput;
using TGC.Group.Model.Optimizacion;
using TGC.Core.BoundingVolumes;

namespace TGC.Group.Model
{
    class Escenario : GameObject //no esta funcionando el octree
    {
        #region variables
        private List<TgcMesh> meshes = new List<TgcMesh>();
        private Octree octree = new Octree();
        TGCVector3 pmin = new TGCVector3(0, -100, 0);
        TGCVector3 pmax = new TGCVector3(0, -100, 0);
        #endregion

        public override void Init()
        {
            #region variables
            //TgcMesh tubo;
            TgcMesh flecha;
            //TgcMesh isla;
            TgcMesh tumba;

            TgcMesh helicoptero;
   
            TgcScene roquedal;
            TgcScene roquedal2;
            TgcScene muelle1;
            #endregion

            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            var loader = new TgcSceneLoader();

            #region muelles
            //muelle1 = loader.loadSceneFromFile(GameModel.mediaDir + "modelos\\muelleGrande-TgcScene.xml");
            //foreach (TgcMesh mesh in muelle1.Meshes)
            //{
            //    mesh.Scale = new TGCVector3(25.5f, 25.5f, 25.5f);
            //    mesh.Position = new TGCVector3(0, 260f, mesh.Position.Z + 500f);
            //    mesh.Effect = efecto;
            //    mesh.Technique = "RenderScene";
            //    obtenerPminYPmax(mesh.BoundingBox.PMin, mesh.BoundingBox.PMax);
            //}
            //meshes.AddRange(muelle1.Meshes);
            #endregion

            #region piedras
            roquedal = loader.loadSceneFromFile(GameModel.mediaDir + "modelos\\ROQUEDAL-TgcScene.xml");
            foreach (TgcMesh mesh in roquedal.Meshes)
            {
                mesh.Scale = new TGCVector3(90.5f, 90.5f, 90.5f);
                mesh.Position = new TGCVector3(mesh.Position.X - 1000f, 10f, mesh.Position.Z + 5500f);
                mesh.Effect = efecto;
                mesh.Technique = "RenderScene";
                obtenerPminYPmax(mesh.BoundingBox.PMin, mesh.BoundingBox.PMax);
            }
            meshes.AddRange(roquedal.Meshes);

            roquedal2 = loader.loadSceneFromFile(GameModel.mediaDir + "modelos\\ROQUEDAL-TgcScene.xml");
            foreach (TgcMesh mesh in roquedal2.Meshes)
            {
                mesh.Scale = new TGCVector3(90.5f, 90.5f, 90.5f);
                mesh.Position = new TGCVector3(mesh.Position.X + 5500f, 10f, mesh.Position.Z + 5500f);
                mesh.RotateY(90);
                mesh.Effect = efecto;
                mesh.Technique = "RenderScene";
                obtenerPminYPmax(mesh.BoundingBox.PMin, mesh.BoundingBox.PMax);
            }
            meshes.AddRange(roquedal2.Meshes);
            #endregion

            #region otros
            //isla = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Isla-TgcScene.xml").Meshes[0];
            //isla.Scale = new TGCVector3(300.5f, 300.5f, 300.5f);
            //isla.Effect = efecto;
            //isla.Technique = "RenderScene";
            //isla.Position = new TGCVector3(400, 200f, 6500f);
            //isla.RotateZ(3);
            //obtenerPminYPmax(isla.BoundingBox.PMin, isla.BoundingBox.PMax);
            //meshes.Add(isla);

            tumba = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Tumbas-TgcScene.xml").Meshes[0];
            tumba.Scale = new TGCVector3(52.5f, 52.5f, 52.5f);
            tumba.Effect = efecto;
            tumba.Technique = "RenderScene";
            tumba.Position = new TGCVector3(1900, 280f, 6500f);

            obtenerPminYPmax(tumba.BoundingBox.PMin, tumba.BoundingBox.PMax);
            meshes.Add(tumba);

            tumba = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Tumbas-TgcScene.xml").Meshes[0];
            tumba.Scale = new TGCVector3(52.5f, 54, 52.5f);
            tumba.Effect = efecto;
            tumba.Technique = "RenderScene";
            tumba.Position = new TGCVector3(2000, 290f, 6405f);
            obtenerPminYPmax(tumba.BoundingBox.PMin, tumba.BoundingBox.PMax);
            meshes.Add(tumba);

            tumba = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Tumbas-TgcScene.xml").Meshes[0];
            tumba.Scale = new TGCVector3(50.5f, 52.5f, 50.5f);
            tumba.Effect = efecto;
            tumba.Technique = "RenderScene";
            tumba.Position = new TGCVector3(1840,320f, 6700f);
            tumba.RotateY(1);
            obtenerPminYPmax(tumba.BoundingBox.PMin, tumba.BoundingBox.PMax);
            meshes.Add(tumba);

            tumba = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Tumbas-TgcScene.xml").Meshes[0];
            tumba.Scale = new TGCVector3(57.5f, 52.5f,57.5f);
            tumba.Effect = efecto;
            tumba.Technique = "RenderScene";
            tumba.Position = new TGCVector3(2600, 320f, 6910f);
            tumba.RotateY(2);
            obtenerPminYPmax(tumba.BoundingBox.PMin, tumba.BoundingBox.PMax);
            meshes.Add(tumba);

            tumba = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Tumbas-TgcScene.xml").Meshes[0];
            tumba.Scale = new TGCVector3(52.5f, 52.5f, 52.5f);
            tumba.Effect = efecto;
            tumba.Technique = "RenderScene";
            tumba.Position = new TGCVector3(2920, 340f, 6200f);
            tumba.RotateY(1);
            obtenerPminYPmax(tumba.BoundingBox.PMin, tumba.BoundingBox.PMax);
            meshes.Add(tumba);

            //tubo = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\TUBO_MARIO-TgcScene.xml").Meshes[0];
            //tubo.Scale = new TGCVector3(35.5f, 35.5f, 35.5f);
            //tubo.Position = new TGCVector3(1500f, 150f, 1500f);
            //tubo.Effect = efecto;
            //tubo.Technique = "RenderScene";
            //obtenerPminYPmax(tubo.BoundingBox.PMin, tubo.BoundingBox.PMax);
            //meshes.Add(tubo);

            flecha = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Flecha-TgcScene.xml").Meshes[0];
            flecha.Scale = new TGCVector3(320.5f, 320.5f, 320.5f);
            flecha.Position = new TGCVector3(0, 450f, 4800f);
            flecha.RotateY(150);
            flecha.RotateZ(150);
            flecha.Effect = efecto;
            flecha.Technique = "RenderScene";
            obtenerPminYPmax(flecha.BoundingBox.PMin, flecha.BoundingBox.PMax);
            meshes.Add(flecha);

            helicoptero = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\HelicopteroMilitar3-TgcScene.xml").Meshes[0];
            helicoptero.Scale = new TGCVector3(10.5f, 10.5f, 10.5f);
            helicoptero.Position = new TGCVector3(5200f, 200f, 500f);
            helicoptero.RotateY(90);
            helicoptero.Effect = efecto;
            helicoptero.Technique = "RenderScene";
            obtenerPminYPmax(helicoptero.BoundingBox.PMin, helicoptero.BoundingBox.PMax);
            meshes.Add(helicoptero);

            #endregion

            //Crear Octree
            octree.create(meshes, new TgcBoundingAxisAlignBox(pmin, pmax));
            octree.createDebugOctreeMeshes();
        }

        public override void Update()
        {
            //efecto.SetValue("_Time", GameModel.time);
            //efecto.SetValue("alturaEnY", GameLogic.cantidadZombiesMuertos * 10);
        }

        public override void Render()
        {
           octree.render(GameModel.frustum, false);//el true o false es para renderear o no el tree
        }

        public override void Dispose()
        {
            meshes.ForEach(m => m.Dispose());
        }

        private void obtenerPminYPmax(TGCVector3 meshPmin, TGCVector3 meshPmax)
        {
            if ((pmin.X < meshPmin.X) && (pmin.Y < meshPmin.Y) && (pmin.Z < meshPmin.Z))
            {
                pmin = meshPmin;
            }
            if ((pmax.X > meshPmax.X) && (pmax.Y > meshPmax.Y) && (pmax.Z > meshPmax.Z))
            {
                pmax = meshPmax;
            }
        }
    }
}
