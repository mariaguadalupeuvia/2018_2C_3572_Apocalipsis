using Microsoft.DirectX.Direct3D;
using System;
using System.Drawing;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Core.Text;

namespace TGC.Group.Model.Gui
{
    public class MenuItem
    {
        #region variables
        public TgcMesh mesh { get; set; }
        protected Effect efecto;
        private TgcText2D texto = new TgcText2D();
        #endregion

        public MenuItem(TGCVector3 posicion, string caption)
        {
            var d3dDevice = D3DDevice.Instance.Device;

            #region configurarEfecto
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            #endregion

            #region configurarMesh
            mesh = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Tag-TgcScene.xml").Meshes[0];
            mesh.Scale = new TGCVector3(200.5f, 75.5f, 85.5f);
            mesh.Position = posicion;
            mesh.Effect = efecto;
            mesh.Technique = "RenderScene";
            mesh.BoundingBox.setRenderColor(Color.Red);
            mesh.AutoTransform = true;
            #endregion

            #region texto
            //Crear texto 2, especificando color, alineacion, posicion, tamano y fuente.
            texto.Text = caption;
            texto.Color = Color.ForestGreen;
            texto.Align = TgcText2D.TextAlign.LEFT;
            texto.Position = new Point((int)posicion.X + 300, (int)posicion.Y - 200);
            texto.Size = new Size(800, 20);
            texto.changeFont(new System.Drawing.Font("Console", 45, FontStyle.Bold | FontStyle.Italic));
            #endregion
        }

        public void Render()
        {
            mesh.Render();
            texto.render();
        }

        public void Dispose()
        {
            mesh.Dispose();
            texto.Dispose();
        }
        public void manejarEvento()
        {
            mesh.Technique = "RenderSceneCongelada";
        }
        public void desSeleccionar()
        {
            mesh.Technique = "RenderScene";
        }
    }
}