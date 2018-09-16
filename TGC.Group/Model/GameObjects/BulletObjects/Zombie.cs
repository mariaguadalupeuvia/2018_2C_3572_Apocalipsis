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
    class Zombie : BulletObject  
    {
        TgcMesh zombie;
        TgcMesh globo;

        public Zombie()
        {
           crearBody(new TGCVector3(500f, 200f, 1500f));
        }

        public override void Init()
        {
            #region configurarEfecto
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            #endregion

            #region configurarObjeto

            zombie = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\ZombieZero-TgcScene.xml").Meshes[0];
            zombie.Scale = new TGCVector3(100.5f, 100.5f, 100.5f);
            zombie.Effect = efecto;
            zombie.Technique = "RenderScene";
            zombie.RotateY(90);

            objetos.Add(zombie);
            #endregion

            globo = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\GLOBO-TgcScene.xml").Meshes[0];
            globo.Scale = new TGCVector3(100.5f, 100.5f, 100.5f);
            globo.Effect = efecto;
            globo.Technique = "RenderScene";
            objetos.Add(globo);
        }

        public override void Update()
        {

        }

    }
}