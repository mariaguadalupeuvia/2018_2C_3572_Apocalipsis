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
    public class Terreno : GameObject, IPostProcess
    {
        public static TgcSimpleTerrain terrain = new TgcSimpleTerrain();

        public override void Init()
        {
            var d3dDevice = D3DDevice.Instance.Device;

            #region configurarEfecto
            Texture alphaMap = TextureLoader.FromFile(d3dDevice, GameModel.mediaDir + "texturas\\terrain\\Heightmap3_1.jpg");
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderTierra.fx");
            efecto.SetValue("alphaMap", alphaMap);

            Texture mainText = TextureLoader.FromFile(d3dDevice, GameModel.mediaDir + "texturas\\terrain\\terreno.jpg");
            efecto.SetValue("mainText", mainText);
            #endregion

            #region configurarObjeto
            terrain.loadTexture(GameModel.mediaDir + "texturas\\terrain\\TerrainTexture1.jpg");
            terrain.loadHeightmap(GameModel.mediaDir + "texturas\\terrain\\Heightmap3.jpg", 255f, 1.6f, new TGCVector3(0, -100, 0));
            terrain.Effect = efecto;
            terrain.Technique = "RenderScene";
            tecnica = "RenderScene";

            objetos.Add(terrain);
            #endregion

            PostProcess.agregarPostProcessObject(this);
        }

        #region gestionarTecnicasShader
        public void cambiarTecnicaDefault()
        {
            terrain.Technique = tecnica;
        }
        public void cambiarTecnicaPostProceso()
        {
            terrain.Technique = "dark";
        }
        public void cambiarTecnica(string tec)
        {
            tecnica = tec;
            terrain.Technique = tecnica;
        }
        #endregion

        #region cosasPocoImportantes
        public override void Update()
        {
        }

        public static int alturaEnPunto(int x, int z)
        {
            if ((z < 0) || (x < 0) || (z > 63) || (x > 63))
            {
                return -1;
            }
            return terrain.HeightmapData[x,z];
        }
        #endregion

        public void efectoSombra(TGCVector3 lightDir, TGCVector3 lightPos, TGCMatrix lightView, TGCMatrix projMatrix)
        {
            efecto.SetValue("g_vLightPos", new Vector4(lightPos.X, lightPos.Y, lightPos.Z, 1));
            efecto.SetValue("g_vLightDir", new Vector4(lightDir.X, lightDir.Y, lightDir.Z, 1));
            efecto.SetValue("g_mProjLight", projMatrix.ToMatrix());
            efecto.SetValue("g_mViewLightProj", (lightView * projMatrix).ToMatrix());
        }

        public void cambiarTecnicaShadow(Texture shadowTex)
        {
            terrain.Technique = "RenderShadow";
            efecto.SetValue("g_txShadow", shadowTex);
        }
    }
}
