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
        CustomSprite seleccion1 = new CustomSprite();
        CustomSprite seleccion2 = new CustomSprite();
        CustomSprite seleccion3 = new CustomSprite();
        CustomSprite play = new CustomSprite();
        int seleccion = 0;
        GameModel gameModel;

        private PostProcess postProcess;

        public Menu(PostProcess postProcess, GameModel gm)
        {
            this.postProcess = postProcess;
            gameModel = gm;
        }
        #endregion

        public void Init(TgcD3dInput input)
        {
            var d3dDevice = D3DDevice.Instance.Device;
            drawer2D = new Drawer2D();

            #region configurarSprites

            play.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\menuCopy.png", D3DDevice.Instance.Device);
            var textureSize = play.Bitmap.Size;
            play.Position = new TGCVector2(FastMath.Max((D3DDevice.Instance.Width - textureSize.Width) * 0.5f, 0), FastMath.Max((D3DDevice.Instance.Height - textureSize.Height) * 0.5f, 0));// 200, 100);

            seleccion1.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\seleccion1.png", D3DDevice.Instance.Device);
            textureSize = seleccion1.Bitmap.Size;
            seleccion1.Position = play.Position; 

            seleccion2.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\seleccion2.png", D3DDevice.Instance.Device);
            textureSize = seleccion2.Bitmap.Size;
            seleccion2.Position = play.Position;

            seleccion3.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\seleccion3.png", D3DDevice.Instance.Device);
            textureSize = seleccion3.Bitmap.Size;
            seleccion3.Position = play.Position;
            #endregion
        }

        public void Render()
        {
            drawer2D.BeginDrawSprite();
            drawer2D.DrawSprite(play);
            if (seleccion == 0)
            {
                drawer2D.DrawSprite(seleccion1);
            }
            else if (seleccion == 1)
            {
                drawer2D.DrawSprite(seleccion2);
            }
            else
            {
                drawer2D.DrawSprite(seleccion3);
            }
            drawer2D.EndDrawSprite();
        }

        public void Dispose()
        {
            seleccion1.Dispose();
            seleccion2.Dispose();
            seleccion3.Dispose();
            play.Dispose();
        }

        public void Update(TgcD3dInput Input)
        {
            #region chequearInput
            if (Input.keyUp(Key.LeftArrow))
            {
                seleccion --;
                if (seleccion < 0) seleccion = 2;
            }
            if (Input.keyUp(Key.RightArrow))
            {
                seleccion++;
                if (seleccion > 2) seleccion = 0;
            }
            if (Input.keyUp(Key.Return))
            {
                if (seleccion == 0)
                {
                    cambiarEstado(Input);
                }
                else if (seleccion == 1)
                {
                    cambiarOpciones(Input);
                }
                else
                {
                    cambiarAyuda(Input);
                }
            }
            #endregion
        }

        #region manejarEstados
        public void cambiarEstado(TgcD3dInput Input)
        {
            Estado estado = new Play(postProcess, gameModel);
            estado.Init(Input);
            GameModel.estadoDelJuego = estado;
            GameModel.enPlay = true;
        }
        public void cambiarAyuda(TgcD3dInput Input)
        {
            Estado estado = new Ayuda(this, gameModel);
            estado.Init(Input);
            GameModel.estadoDelJuego = estado;
        }
        public void cambiarOpciones(TgcD3dInput Input)
        {
            Estado estado = new Opciones(this, gameModel);
            estado.Init(Input);
            GameModel.estadoDelJuego = estado;
        }
        #endregion
    }
}
