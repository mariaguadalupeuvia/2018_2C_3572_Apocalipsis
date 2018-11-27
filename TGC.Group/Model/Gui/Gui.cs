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
        private TgcText2D soles = new TgcText2D();
        private TgcText2D zombies = new TgcText2D();
        CustomSprite GOD = new CustomSprite();
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

            CustomSprite plantas = new CustomSprite();
            plantas.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\PLANTAS.png", D3DDevice.Instance.Device);
            textureSize = plantas.Bitmap.Size;
            plantas.Position = new TGCVector2(0, FastMath.Max(D3DDevice.Instance.Height - textureSize.Height , 0));
            sprites.Add(plantas);

            CustomSprite girasol = new CustomSprite();
            girasol.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\girasol.png", D3DDevice.Instance.Device);
            girasol.Position = new TGCVector2(30, 30);
            sprites.Add(girasol);

            CustomSprite zombie = new CustomSprite();
            zombie.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\zombie.png", D3DDevice.Instance.Device);
            zombie.Position = new TGCVector2(30, 100);
            sprites.Add(zombie);

            GOD.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\GOD.png", D3DDevice.Instance.Device);
            GOD.Position = new TGCVector2(FastMath.Max(D3DDevice.Instance.Width - 60, 0), 0);
            #endregion

            #region texto
            //Crear texto 2, especificando color, alineacion, posicion, tamano y fuente.
            soles = new TgcText2D();
            soles.Text = "" + GameLogic.cantidadEnergia;
            soles.Color = Color.GreenYellow;
            soles.Align = TgcText2D.TextAlign.LEFT;
            soles.Position = new Point(100, 40);
            soles.Size = new Size(100, 10);
            soles.changeFont(new Font("Console", 25, FontStyle.Bold | FontStyle.Italic));


            zombies = new TgcText2D();
            zombies.Text = "" + GameLogic.cantidadZombiesMuertos;
            zombies.Color = Color.LawnGreen;
            zombies.Align = TgcText2D.TextAlign.LEFT;
            zombies.Position = new Point(100, 110);
            zombies.Size = new Size(100, 10);
            zombies.changeFont(new Font("Console", 25, FontStyle.Bold | FontStyle.Italic));
            #endregion
        }

        public void Render()
        {
            drawer2D.BeginDrawSprite();
            sprites.ForEach(s => drawer2D.DrawSprite(s));
            if (GameModel.modoGod) drawer2D.DrawSprite(GOD);
            drawer2D.EndDrawSprite();
            soles.Text = "" + GameLogic.cantidadEnergia;
            soles.render();
            zombies.Text = "" + GameLogic.cantidadZombiesMuertos;
            zombies.render();
        }

        public void Dispose()
        {
            sprites.ForEach(s => s.Dispose());
            soles.Dispose();
            zombies.Dispose();
        }
    }
}
