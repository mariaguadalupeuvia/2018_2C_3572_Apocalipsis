using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Group.Model.Gui;

namespace TGC.Group.Model.EstadosJuego
{
    public class Ayuda : Estado
    {
        #region variables
        private List<CustomSprite> sprites = new List<CustomSprite>();
        private Drawer2D drawer2D;
        Estado menu;

        static int siguiente = 0;
        static bool cambiar = true;
        #endregion

        static Timer time;
        GameModel gameModel;

        public Ayuda(Estado menu, GameModel gm)
        {
            this.menu = menu;
            gameModel = gm;
        }

        public void cambiarEstado(TgcD3dInput Input)
        {
            Estado estado = menu;
            GameModel.estadoDelJuego = estado;
        }

        public void Init(TgcD3dInput input)
        {
            var d3dDevice = D3DDevice.Instance.Device;
            drawer2D = new Drawer2D();

            #region configurarSprites
            CustomSprite sprite = new CustomSprite();
            sprite.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\mata.png", D3DDevice.Instance.Device);
            var textureSize = sprite.Bitmap.Size;
            sprite.Position = new TGCVector2(FastMath.Max((D3DDevice.Instance.Width - textureSize.Width) * 0.5f, 0), FastMath.Max((D3DDevice.Instance.Height - textureSize.Height) * 0.5f, 0));
            sprites.Add(sprite);

            CustomSprite sprite2 = new CustomSprite();
            sprite2.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\evita.png", D3DDevice.Instance.Device);
            sprite2.Position = new TGCVector2(FastMath.Max((D3DDevice.Instance.Width - textureSize.Width) * 0.5f, 0), FastMath.Max((D3DDevice.Instance.Height - textureSize.Height) * 0.5f, 0));
            sprites.Add(sprite2);

            CustomSprite sprite3 = new CustomSprite();
            sprite3.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\crea.png", D3DDevice.Instance.Device);

            sprite3.Position = new TGCVector2(FastMath.Max((D3DDevice.Instance.Width - textureSize.Width) * 0.5f, 0), FastMath.Max((D3DDevice.Instance.Height - textureSize.Height) * 0.5f, 0));
            sprites.Add(sprite3);

            #endregion

            time = new Timer(500);
            time.Elapsed += OnTimedEvent;
            time.AutoReset = true;
            time.Enabled = true;
        }

        static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            cambiar = true;
        }

        public void Render()
        {
            drawer2D.BeginDrawSprite();
            drawer2D.DrawSprite(sprites[siguiente]);
            drawer2D.EndDrawSprite();
        }

        public void Dispose()
        {
            sprites.ForEach(s => s.Dispose());
            time.Dispose();
        }

        public void Update(TgcD3dInput Input)
        {
            #region chequearInput

            if (cambiar)
            {
                if (Input.keyUp(Key.LeftArrow))
                {
                    siguiente++;
                    if (siguiente > 2) siguiente = 0;
                    cambiar = false;
                }
                if (Input.keyUp(Key.RightArrow))
                {
                    siguiente--;
                    if (siguiente < 0) siguiente = 2;
                    cambiar = false;
                }
            }

            if (Input.keyUp(Key.Return))
            {
                  cambiarEstado(Input);
            }

            #endregion
        }
    }
}
