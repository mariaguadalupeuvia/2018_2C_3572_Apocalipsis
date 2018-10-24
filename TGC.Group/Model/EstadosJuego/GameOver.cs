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
    public class GameOver : Estado
    {
        #region variables
        private List<CustomSprite> sprites = new List<CustomSprite>();
        private Drawer2D drawer2D;
        List<GameObject> gameObjects;
        int contador = 0;
        #endregion

        public GameOver(List<GameObject> objetos)
        {
            gameObjects = objetos;
        }

        public void Init(TgcD3dInput input)
        {
            var d3dDevice = D3DDevice.Instance.Device;
            drawer2D = new Drawer2D();

            #region configurarSprites
            CustomSprite gameOver = new CustomSprite();
            gameOver.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\gameOver.png", D3DDevice.Instance.Device);
            var textureSize = gameOver.Bitmap.Size;
            gameOver.Position = new TGCVector2(FastMath.Max(D3DDevice.Instance.Width * 0.15f, 0), FastMath.Max(D3DDevice.Instance.Height * 0.25f, 0));
            sprites.Add(gameOver);
            #endregion
        }

        public void Render()
        {
            gameObjects.ForEach(g => g.Render());
            drawer2D.BeginDrawSprite();
            sprites.ForEach(s => drawer2D.DrawSprite(s));
            drawer2D.EndDrawSprite();
        }

        public void Dispose()
        {
            gameObjects.ForEach(g => g.Dispose());
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