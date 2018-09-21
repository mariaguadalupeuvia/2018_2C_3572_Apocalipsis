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

namespace TGC.Group.Model.GameObjects.BulletObjects
{
    class Zombie : BulletObject  
    {
        TgcMesh zombie;
        TgcMesh globo;
        float posicionZ = 3200;

        public Zombie(TGCVector3 posicion, GamePhysics world)
        {
            physicWorld = world;
            crearBody(posicion);// new TGCVector3(500f, 200f, 1500f));

            #region configurarEfecto
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            #endregion

            #region configurarObjeto
            zombie = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Zombie7-TgcScene.xml").Meshes[0];
            zombie.Scale = new TGCVector3(55.5f, 55.5f, 55.5f);
            zombie.Position = posicion;//new TGCVector3(800f, 200f, 1200f);
            zombie.Effect = efecto;
            zombie.Technique = "RenderScene";

            objetos.Add(zombie);
           
            globo = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\GLOBO-TgcScene.xml").Meshes[0];
            globo.Scale = new TGCVector3(60.5f, 60.5f, 60.5f);
            globo.Position = new TGCVector3(posicion.X, posicion.Y + 200, posicion.Z);
            globo.Effect = efecto;
            globo.Technique = "RenderScene";

            objetos.Add(globo);
            #endregion
        }

        public override void Update()
        {
             body.Translate(new Vector3(0, 0, -10));
        }

        public override void Render()
        {
            zombie.Position = new TGCVector3(body.InterpolationWorldTransform.M41, body.InterpolationWorldTransform.M42, body.InterpolationWorldTransform.M43);
            globo.Position = new TGCVector3( body.InterpolationWorldTransform.M41, body.InterpolationWorldTransform.M42 + 200, body.InterpolationWorldTransform.M43);

            //zombie.Transform = new TGCMatrix(body.InterpolationWorldTransform);
            //globo.Transform = new TGCMatrix(body.InterpolationWorldTransform);
            base.Render();
          //  Console.WriteLine("zombie t: " + body.InterpolationWorldTransform);
        }
    }
}