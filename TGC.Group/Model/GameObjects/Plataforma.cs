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
using System.Drawing;

namespace TGC.Group.Model.GameObjects
{
    public class Plataforma : TgcMesh
    {
        public TgcMesh plataforma { get; set; }
        protected Microsoft.DirectX.Direct3D.Effect efecto;

        public Plataforma(TGCVector3 posicion)
        {
            var d3dDevice = D3DDevice.Instance.Device;

            #region configurarEfecto
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            #endregion

            #region configurarObjetos
            plataforma = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\PLATAFORMA-TgcScene.xml").Meshes[0];
            plataforma.Scale = new TGCVector3(35.5f, 15.5f, 35.5f);
            plataforma.Position = posicion;
            plataforma.Effect = efecto;
            plataforma.Technique = "RenderScene";
            plataforma.BoundingBox.setRenderColor(Color.Red);
            plataforma.AutoTransform = true;
            #endregion
        }

        public void Render()
        {
            plataforma.Render();
            plataforma.BoundingBox.Render();
        }

        public void Dispose()
        {
            plataforma.Dispose();
        }
    }
}