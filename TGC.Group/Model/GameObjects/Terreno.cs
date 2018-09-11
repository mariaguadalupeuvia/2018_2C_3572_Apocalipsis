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
        TgcSimpleTerrain terrain = new TgcSimpleTerrain();

        public override void Init()
        {
            var d3dDevice = D3DDevice.Instance.Device;

            #region configurarEfecto
            Texture bumpMap = TextureLoader.FromFile(d3dDevice, GameModel.mediaDir + "texturas\\terrain\\NormalMapCristales.jpg");
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderTierra.fx");
            efecto.SetValue("NormalMap", bumpMap);
            #endregion

            #region configurarObjeto
            terrain.loadTexture(GameModel.mediaDir + "texturas\\terrain\\TerrainTexture1.jpg");
            terrain.loadHeightmap(GameModel.mediaDir + "texturas\\terrain\\Heightmap3.jpg", 255f, 1.6f, new TGCVector3(0, -100, 0));
            terrain.Effect = efecto;
            terrain.Technique = "RenderScene";

            objeto = terrain;
            #endregion
        }

        public override void Update()
        {
           
        }
    }
}
