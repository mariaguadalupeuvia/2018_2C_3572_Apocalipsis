using BulletSharp;
using BulletSharp.Math;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;

namespace TGC.Group.Model
{
    public class Apocalipsis : BulletObject
    {
        TgcMesh bomba;
        //TgcMesh semiesfera;
        Effect efecto;

        public Apocalipsis(TGCVector3 posicion)
        {
            //physicWorld = world;
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");

            bomba = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\BOMBA-TgcScene.xml").Meshes[0];
            bomba.Scale = new TGCVector3(40.5f, 80.5f, 40.5f);
            bomba.Effect = efecto;
            bomba.Technique = "RenderScene";
            bomba.Position = posicion;
            objetos.Add(bomba);

            //semiesfera = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Semiesfera-TgcScene.xml").Meshes[0];
            //semiesfera.Scale = new TGCVector3(200.5f, 200.5f, 200.5f);
            //semiesfera.Effect = efecto;
            //semiesfera.Technique = "Explosivo";
            //semiesfera.Position = new TGCVector3(posicion.X, 220, posicion.Z); ;
          
            //crearBody(posicion);
        }

        public void Update(TgcD3dInput Input)
        {
            efecto.SetValue("_Time", GameModel.time);
        }

        public override void Render()
        {
            body.Translate(new Vector3(0, -35, 0));
            bomba.Position = new TGCVector3(bomba.Position.X, body.InterpolationWorldTransform.M42, bomba.Position.Z);
            bomba.Render();
            //semiesfera.Render();
        }
    }
}
