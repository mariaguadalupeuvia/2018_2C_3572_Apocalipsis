using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model.Gui
{
    class Gui : IRenderObject
    {
        private List<CustomSprite> sprites = new List<CustomSprite>();
        private Drawer2D drawer2D;

        public bool AlphaBlendEnable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
    }
}
