using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.Shaders;
using TGC.Core.Terrain;

namespace TGC.Group.Model.GameObjects
{
    public class Agua : GameObject, IPostProcess 
    {
        TgcSimpleTerrain agua = new TgcSimpleTerrain();

        public override void Init()
        {
            var d3dDevice = D3DDevice.Instance.Device;

            #region configurarEfecto
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderAgua.fx");
            Texture alphaMap = TextureLoader.FromFile(d3dDevice, GameModel.mediaDir + "texturas\\terrain\\Heightmap3_1.jpg");
            efecto.SetValue("texAlphaMap", alphaMap);
            Texture normalMap = TextureLoader.FromFile(d3dDevice, GameModel.mediaDir + "texturas\\terrain\\bump.jpg");
            efecto.SetValue("NormalMap", normalMap);
            #endregion

            #region configurarObjeto
            agua.loadTexture(GameModel.mediaDir + "texturas\\terrain\\agua1.jpg");
            agua.loadHeightmap(GameModel.mediaDir + "texturas\\terrain\\negro.jpg", 255f, 1.5f, new TGCVector3(0, -95, 0));

            agua.Effect = efecto;
            agua.Technique = "RenderScene";
            tecnica = "RenderScene";
            objetos.Add(agua);
            #endregion

            PostProcess.agregarPostProcessObject(this);
        }

        #region gestionarTecnicasShader
        public void cambiarTecnicaDefault()
        {
            agua.Technique = tecnica;
        }
        public void cambiarTecnicaPostProceso()
        {
            agua.Technique = "dark";
        }
        public void cambiarTecnica(string tec)
        {
            tecnica = tec;
            agua.Technique = tecnica;
        }
        #endregion

        public override void Update()
        {
            efecto.SetValue("_Time", GameModel.time);
        }
        public void efectoSombra(TGCVector3 lightDir, TGCVector3 lightPos, TGCMatrix lightView, TGCMatrix projMatrix)
        {
            efecto.SetValue("g_vLightPos", new Vector4(lightPos.X, lightPos.Y, lightPos.Z, 1));
            efecto.SetValue("g_vLightDir", new Vector4(lightDir.X, lightDir.Y, lightDir.Z, 1));
            efecto.SetValue("g_mProjLight", projMatrix.ToMatrix());
            efecto.SetValue("g_mViewLightProj", (lightView * projMatrix).ToMatrix());
        }

        public void cambiarTecnicaShadow(Texture shadowTex)
        {
            agua.Technique = "RenderShadow";
            efecto.SetValue("g_txShadow", shadowTex);
        }
    }
}
