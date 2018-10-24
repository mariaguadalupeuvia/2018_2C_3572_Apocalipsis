using Microsoft.DirectX.DirectInput;
using System;
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
    public class Menu : Estado
    {
        #region variables
        private Drawer2D drawer2D;
        CustomSprite seleccion = new CustomSprite();
        CustomSprite ayuda = new CustomSprite();
        CustomSprite play = new CustomSprite();
        bool seleccion1 = true;
        #endregion

        public void Init(TgcD3dInput input)
        {
            var d3dDevice = D3DDevice.Instance.Device;
            drawer2D = new Drawer2D();

            #region configurarSprites

            play.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\menu.png", D3DDevice.Instance.Device);
            var textureSize = play.Bitmap.Size;
            play.Position = new TGCVector2(FastMath.Max((D3DDevice.Instance.Width - textureSize.Width) * 0.5f, 0), FastMath.Max((D3DDevice.Instance.Height - textureSize.Height) * 0.5f, 0));// 200, 100);

            ayuda.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\seleccion1.png", D3DDevice.Instance.Device);
            textureSize = ayuda.Bitmap.Size;
            ayuda.Position = play.Position; 

            seleccion.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\seleccion2.png", D3DDevice.Instance.Device);
            textureSize = seleccion.Bitmap.Size;
            seleccion.Position = play.Position;
           
            #endregion
        }

        public void Render()
        {
            drawer2D.BeginDrawSprite();
            drawer2D.DrawSprite(play);
            if (seleccion1)
            {
                drawer2D.DrawSprite(ayuda);
            }
            else
            {
                drawer2D.DrawSprite(seleccion);
            }
            drawer2D.EndDrawSprite();
        }

        public void Dispose()
        {
            seleccion.Dispose();
            ayuda.Dispose();
            play.Dispose();
        }

        public void Update(TgcD3dInput Input)
        {
            #region chequearInput
            if (Input.keyUp(Key.LeftArrow))
            {
                seleccion1 = true;
            }
            if (Input.keyUp(Key.RightArrow))
            {
                seleccion1 = false;
            }
            if (Input.keyUp(Key.Return))
            {
                if (seleccion1)
                {
                    cambiarEstado(Input);
                }
                else
                {
                    cambiarAyuda(Input);
                } 
            }
            #endregion
        }

        public void cambiarEstado(TgcD3dInput Input)
        {
            Estado estado = new Play();
            estado.Init(Input);
            GameModel.estadoDelJuego = estado;
            GameModel.enPlay = true;
        }
        public void cambiarAyuda(TgcD3dInput Input)
        {
            Estado estado = new Ayuda(this);
            estado.Init(Input);
            GameModel.estadoDelJuego = estado;
        }
    }
}
