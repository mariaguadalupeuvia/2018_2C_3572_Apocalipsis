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
    public class Vida : BulletObject //para probar las colisiones con el motor de fisica
    {
         TgcMesh vida;
        //TGCSphere vida;  //si uso sphere funciona, con mesh no

        public Vida( )
        {
             crearBody(new TGCVector3(1300f, 360f, 1500f));

            #region configurarEfecto
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            #endregion

            #region configurarObjeto

            vida = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\vida-TgcScene.xml").Meshes[0];
            vida.Scale = new TGCVector3(20.5f, 10.5f, 20.5f);
            vida.Effect = efecto;
            vida.Technique = "RenderScene";
            vida.RotateY(90);
            //vida.Transform = new TGCMatrix(body.InterpolationWorldTransform);
            //vida.UpdateMeshTransform();

            //var texture = TgcTexture.createTexture(D3DDevice.Instance.Device, GameModel.mediaDir + "texturas\\terrain\\NormalMapMar.png");
            //vida = new TGCSphere(1, texture.Clone(), TGCVector3.Empty);
            ////Tgc no crea el vertex buffer hasta invocar a update values.
            //vida.updateValues();
           
            objetos.Add(vida);
            #endregion
        }

        public override void Init()
        {
        }

        public override void Update()
        {
             vida.Transform =TGCMatrix.Scaling(10, 10, 10) * new TGCMatrix(body.InterpolationWorldTransform);
          //  vida.Transform = TGCMatrix.Scaling(10, 10, 10) * new TGCMatrix(body.InterpolationWorldTransform);
             vida.UpdateMeshTransform();
           // vida.updateValues();
        }

        public override void Render()
        {
            vida.Render();
        }
    }
}
