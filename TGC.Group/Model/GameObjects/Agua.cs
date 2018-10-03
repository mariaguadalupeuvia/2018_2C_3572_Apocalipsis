﻿using Microsoft.DirectX.Direct3D;
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
    public class Agua : GameObject
    {
        TgcSimpleTerrain agua = new TgcSimpleTerrain();

        public override void Init()
        {
            var d3dDevice = D3DDevice.Instance.Device;

            #region configurarEfecto
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderAgua.fx");
            Texture alphaMap = TextureLoader.FromFile(d3dDevice, GameModel.mediaDir + "texturas\\terrain\\Heightmap3_1.jpg");
            efecto.SetValue("texAlphaMap", alphaMap);
            #endregion

            #region configurarObjeto
            agua.loadTexture(GameModel.mediaDir + "texturas\\terrain\\agua1.jpg");
            agua.loadHeightmap(GameModel.mediaDir + "texturas\\terrain\\Heightmap3.jpg", 255f,  1.5f, new TGCVector3(0, -95, 0));
            agua.Effect = efecto;
            agua.Technique = "RenderScene";

            objetos.Add(agua);
            #endregion
        }

        public override void Update()
        {
            efecto.SetValue("_Time", GameModel.time);
        }
    }
}
