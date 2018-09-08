using BulletSharp;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Core.Terrain;
using TGC.Core.Textures;

namespace TGC.Group.Model.GameObjects.BulletObjects
{
    class Zombie : BulletObject  //por ahora esta para pruebas
    {
        TgcMesh zombie;
        //TGCSphere zombie;

        public Zombie()
        {
            crearBody();

            #region configurarEfecto
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            #endregion

            #region configurarObjeto

            zombie = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\zombie-TgcScene.xml").Meshes[0];
            zombie.Scale = new TGCVector3(10.5f, 10.5f, 10.5f);
            zombie.Effect = efecto;
            zombie.Technique = "RenderScene";
            zombie.RotateY(90);
            //zombie.Transform = new TGCMatrix(body.InterpolationWorldTransform);
            //zombie.UpdateMeshTransform();

            //var d3dDevice = D3DDevice.Instance.Device;
            //Texture texture = TextureLoader.FromFile(d3dDevice, GameModel.mediaDir + "modelos\\Textures\\vida.jpg");
            //efecto.SetValue("texDiffuseMap", texture);

            //zombie = new TGCSphere(1, texture.Clone(), TGCVector3.Empty);
            ////Tgc no crea el vertex buffer hasta invocar a update values.
            //zombie.updateValues();
            objeto = zombie;
            #endregion
        }

        public override void Init()
        {
        }

        public override void Update()
        {

           // zombie.Transform = TGCMatrix.Scaling(10, 10, 10) * new TGCMatrix(body.InterpolationWorldTransform);
           // //  zombie.Transform = TGCMatrix.Scaling(10, 10, 10) * new TGCMatrix(body.InterpolationWorldTransform);
           // zombie.UpdateMeshTransform();
           // // zombie.updateValues();
           //// Console.WriteLine("posicion en y:" + zombie.Transform.ToString());// zombie.Position.Y);
        }

        public override void Render()
        {
            zombie.Render();
        }
    }
}