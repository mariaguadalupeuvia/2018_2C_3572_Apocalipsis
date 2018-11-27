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
using TGC.Core.Direct3D;
using TGC.Group.Model.GameObjects;

namespace TGC.Group.Model
{
    class Escenario : GameObject 
    {
        #region variables
        private List<TgcMesh> meshes = new List<TgcMesh>();
        private Octree octree = new Octree();
        TGCVector3 pmin = new TGCVector3(0, -100, 0);
        TGCVector3 pmax = new TGCVector3(0, -100, 0);
        //TgcMesh helice;
        #endregion

        TgcMesh tubo;
        CubeTexture cubemap;
        

        public void setEyePosition(float[] eyePosition)
        {
            efecto.SetValue("fvEyePosition", eyePosition);
        }

        public override void Init()
        {
            #region variables
            TgcMesh flecha;
            TgcMesh tumba;

            TgcScene roquedal;
            TgcScene roquedal2;
            TgcScene muelle1;
            #endregion

            var d3dDevice = D3DDevice.Instance.Device;
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            
            cubemap = TextureLoader.FromCubeFile(d3dDevice, GameModel.mediaDir + "texturas\\skybox\\cube2018.dds");

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

            flecha = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Flecha-TgcScene.xml").Meshes[0];
            flecha.Scale = new TGCVector3(320.5f, 320.5f, 320.5f);
            flecha.Position = new TGCVector3(0, 450f, 4800f);
            flecha.RotateY(150);
            flecha.RotateZ(150);
            flecha.Effect = efecto;
            flecha.Technique = "RenderScene";
            obtenerPminYPmax(flecha.BoundingBox.PMin, flecha.BoundingBox.PMax);
            meshes.Add(flecha);
            #endregion

            //efecto.SetValue("texCubeMap", cubemap);
            //tubo = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\TUBO_MARIO-TgcScene.xml").Meshes[0];
            //tubo.Scale = new TGCVector3(35.5f, 35.5f, 35.5f);
            //tubo.Position = new TGCVector3(1500f, 350f, 1500f);
            //tubo.Effect = efecto;
            //tubo.Technique = "cube";
            //obtenerPminYPmax(tubo.BoundingBox.PMin, tubo.BoundingBox.PMax);
            //meshes.Add(tubo);

            #region  crearOctree
            octree.create(meshes, new TgcBoundingAxisAlignBox(pmin, pmax));
            octree.createDebugOctreeMeshes();
            #endregion

        }

        public override void Update()
        {

        }

        public override void Render()
        {
           octree.render(GameModel.frustum, false);//el true o false es para renderear o no el tree
           
        }

        public override void Dispose()
        {
            meshes.ForEach(m => m.Dispose());
             
        }

        #region cosasPocoImportantes
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
        #endregion
    }
}
