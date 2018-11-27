using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;

namespace TGC.Group.Model.GameObjects
{
    public class Helicoptero : GameObject
    {
        TgcMesh helicoptero;
        TgcMesh helice;
        bool volar = false;

        public override void Init()
        {
            var d3dDevice = D3DDevice.Instance.Device;
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");

            #region configurarObjetos
            helicoptero = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\heliX-TgcScene.xml").Meshes[0];
            helicoptero.Scale = new TGCVector3(10.5f, 10.5f, 10.5f);
            helicoptero.Position = new TGCVector3(5000f, 200f, 500f);
            helicoptero.RotateY(90);
            helicoptero.Effect = efecto;
            helicoptero.Technique = "RenderScene";

            objetos.Add(helicoptero);

            helice = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\heliceCero-TgcScene.xml").Meshes[0];
            helice.Scale = new TGCVector3(10.5f, 10.5f, 10.5f);
            helice.Position = new TGCVector3(5000f, 200f, 420f);
            helice.RotateY(90);
            helice.Effect = efecto;
            helice.Technique = "RenderScene";

            objetos.Add(helice);
            #endregion
        }

        public void despegar()
        {
            volar = true;
            GameSound.volar(); 
        }

        public void update(Core.Input.TgcD3dInput input)
        {
            var moveVector = TGCVector3.Empty;

            if (input.keyDown(Key.B))
            {
                moveVector += new TGCVector3(0, 7, 0);
                if(!volar)
                {
                    despegar();
                }
            }
            if (input.keyDown(Key.N))
            {
                moveVector += new TGCVector3(-17, 0, 0);
            }
            if (input.keyDown(Key.M))
            {
                moveVector += new TGCVector3(0, 0, -17);
            }

            helicoptero.Position = helicoptero.Position + moveVector;
            helice.Position = helicoptero.Position + moveVector;

            if (volar)
            {
                helice.RotateY(1);
            }
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
