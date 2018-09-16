using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using Microsoft.DirectX.DirectInput;
using TGC.Core.Input;
using TGC.Group.Model.GameObjects.BulletObjects;

namespace TGC.Group.Model.GameObjects
{
    public class Planta
    {
        private Canion canion;
        private TgcMesh planta;
        protected Microsoft.DirectX.Direct3D.Effect efecto;

        public void Init(GamePhysics physicWorld)
        {
            var d3dDevice = D3DDevice.Instance.Device;

            #region configurarEfecto
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            #endregion

            #region configurarObjetos

            canion = new Canion();
            canion.Init(physicWorld);

            planta = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\PLANTA-TgcScene.xml").Meshes[0];
            planta.Scale = new TGCVector3(35.5f, 35.5f, 35.5f);
            planta.Position = new TGCVector3(500f, 200f, 1500f);

            planta.Effect = efecto;
            planta.Technique = "RenderScene";

            #endregion
        }

        public void update(TgcD3dInput Input)
        {
            canion.update(Input);
        }

        public void Render()
        {
            planta.Render();
            canion.Render();
        }

        public void Dispose()
        {
            canion.Dispose();
            planta.Dispose();
        }
    }
}