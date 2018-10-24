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
    public class Terreno : GameObject
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
            terrain.Technique = "RenderScene"; //"apocalipsis";// 
            
            objetos.Add(terrain);
            #endregion
        }

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
    }
}
