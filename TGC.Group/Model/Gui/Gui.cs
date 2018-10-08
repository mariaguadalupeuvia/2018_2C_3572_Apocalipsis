using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Text;

namespace TGC.Group.Model.Gui
{
    class Gui
    {
        #region variables
        private List<CustomSprite> sprites = new List<CustomSprite>();
        private Drawer2D drawer2D;
        private TgcText2D texto = new TgcText2D();
        #endregion

        public void Init()
        {
            var d3dDevice = D3DDevice.Instance.Device;
            drawer2D = new Drawer2D();

            #region configurarSprites

            CustomSprite sprite = new CustomSprite();
            sprite.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\emoji.png", D3DDevice.Instance.Device);
            var textureSize = sprite.Bitmap.Size;
            sprite.Position = new TGCVector2(FastMath.Max(D3DDevice.Instance.Width  - textureSize.Width / 0.75f, 0), 0);
            sprites.Add(sprite);

            CustomSprite ayuda = new CustomSprite();
            ayuda.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\ayuda2.png", D3DDevice.Instance.Device);
            textureSize = ayuda.Bitmap.Size;
            ayuda.Position = new TGCVector2(FastMath.Max(D3DDevice.Instance.Width - textureSize.Width, 0), FastMath.Max(D3DDevice.Instance.Height - textureSize.Height, 0));
            sprites.Add(ayuda);

            CustomSprite plantas = new CustomSprite();
            plantas.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\PLANTAS.png", D3DDevice.Instance.Device);
            textureSize = plantas.Bitmap.Size;
            plantas.Position = new TGCVector2(0, FastMath.Max(D3DDevice.Instance.Height - textureSize.Height , 0));
            sprites.Add(plantas);

            #endregion

            #region texto
            //Crear texto 2, especificando color, alineacion, posicion, tamano y fuente.
            texto = new TgcText2D();
            texto.Text = "Soles " + GameLogic.cantidadEnergia;
            texto.Color = Color.BlueViolet;
            texto.Align = TgcText2D.TextAlign.LEFT;
            texto.Position = new Point(100, 10);
            texto.Size = new Size(100, 10);
            texto.changeFont(new Font("Console", 25, FontStyle.Bold | FontStyle.Italic));
            #endregion
        }

        public void Render()
        {
            drawer2D.BeginDrawSprite();
            sprites.ForEach(s => drawer2D.DrawSprite(s));
            drawer2D.EndDrawSprite();
            texto.Text = "Soles " + GameLogic.cantidadEnergia;
            texto.render();
        }

        public void Dispose()
        {
            sprites.ForEach(s => s.Dispose());
            texto.Dispose();
        }
    }
}
