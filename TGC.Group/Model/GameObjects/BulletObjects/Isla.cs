using BulletSharp;
using BulletSharp.Math;
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
using TGC.Group.Model.GameObjects.BulletObjects.CollisionCallbacks;

namespace TGC.Group.Model.GameObjects.BulletObjects
{
    public class Isla : BulletObject//esta me parece que no se usa nunca
    {
        #region variables
        protected TgcMesh isla;

        #endregion

        public Isla(GameLogic logica)
        {
            //#region configurarEfecto
            //Microsoft.DirectX.Direct3D.Effect efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            //#endregion

            //#region configurarObjeto
            //isla = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Isla-TgcScene.xml").Meshes[0];
            //isla.Scale = new TGCVector3(1000.5f, 300.5f, 1000.5f);
            //isla.Effect = efecto;
            //isla.Technique = "RenderScene";
            //isla.Position = new TGCVector3(0, 340f, 0);
            //isla.RotateZ(3.1f);

            //objetos.Add(isla);

            //body = FactoryBody.crearBodyIsla();
           
            //callback = new CollisionCallbackIsla(logica);
            //logica.addBulletObject(this);
            //#endregion
        }

        public override void Update()
        {

        }

        public override void Render()
        {
            isla.Render();
        }

    }
}