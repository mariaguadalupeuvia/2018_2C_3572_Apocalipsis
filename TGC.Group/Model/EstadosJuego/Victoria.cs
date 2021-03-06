﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.Text;
using TGC.Group.Model.Gui;

namespace TGC.Group.Model.EstadosJuego
{
    public class Victoria : Estado
    {
        #region variables
        private List<CustomSprite> sprites = new List<CustomSprite>();
        private Drawer2D drawer2D;
        int contador = 0;
        #endregion

        public void Init(TgcD3dInput input)
        {
            var d3dDevice = D3DDevice.Instance.Device;
            drawer2D = new Drawer2D();

            #region configurarSprites
            CustomSprite victoria = new CustomSprite();
            victoria.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\victoria.png", D3DDevice.Instance.Device);
            var textureSize = victoria.Bitmap.Size;
            victoria.Position = new TGCVector2(FastMath.Max(D3DDevice.Instance.Width * 0.15f, 0), FastMath.Max(D3DDevice.Instance.Height * 0.25f, 0));
            sprites.Add(victoria);
            #endregion
        }

        public void Render()
        {
            drawer2D.BeginDrawSprite();
            sprites.ForEach(s => drawer2D.DrawSprite(s));
            drawer2D.EndDrawSprite();
        }

        public void Dispose()
        {
            sprites.ForEach(s => s.Dispose());
        }

        public void Update(TgcD3dInput Input)
        {
            contador++;
            if (contador == 5000)
            {
                cambiarEstado(Input);
            }
        }

        public void cambiarEstado(TgcD3dInput Input)
        {
            GameModel.time = 0;
            Estado estado = new Inicial();
            estado.Init(Input);

            GameModel.estadoDelJuego = estado;
        }

    }
}