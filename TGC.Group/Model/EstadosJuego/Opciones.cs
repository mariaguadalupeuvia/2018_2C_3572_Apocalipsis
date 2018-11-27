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
    public class Opciones : Estado
    {
        #region variables
        private List<CustomSprite> escenas = new List<CustomSprite>();
        private List<CustomSprite> musicas = new List<CustomSprite>();
        CustomSprite opciones = new CustomSprite();

        private Drawer2D drawer2D;
        Estado menu;
        GameModel gameModel;

        bool musicaSel = true;
        bool tipoEscena = true;
        int escenaSel = 0;
        static bool cambiar = true;
        static Timer time;
        #endregion

        public Opciones(Estado menu, GameModel gm)
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
           
            opciones.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\opciones.png", D3DDevice.Instance.Device);
            var textureSize = opciones.Bitmap.Size;
            opciones.Position = new TGCVector2(FastMath.Max((D3DDevice.Instance.Width - textureSize.Width) * 0.5f, 0), FastMath.Max((D3DDevice.Instance.Height - textureSize.Height) * 0.5f, 0));

            CustomSprite normal = new CustomSprite();
            normal.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\0_normal.png", D3DDevice.Instance.Device);
            normal.Position = opciones.Position;
            escenas.Add(normal);
            CustomSprite picante = new CustomSprite();
            picante.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\0_picante.png", D3DDevice.Instance.Device);
            picante.Position = opciones.Position;
            escenas.Add(picante);
            CustomSprite helada = new CustomSprite();
            helada.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\0_helado.png", D3DDevice.Instance.Device);
            helada.Position = opciones.Position;
            escenas.Add(helada);
            CustomSprite glow = new CustomSprite();
            glow.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\0_noche.png", D3DDevice.Instance.Device);
            glow.Position = opciones.Position;
            escenas.Add(glow);

            CustomSprite god = new CustomSprite();
            god.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\0_god.png", D3DDevice.Instance.Device);
            god.Position = opciones.Position;
            escenas.Add(god);

            CustomSprite on1 = new CustomSprite();
            on1.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\1_on.png", D3DDevice.Instance.Device);
            on1.Position = opciones.Position;
            musicas.Add(on1);
            CustomSprite off1 = new CustomSprite();
            off1.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\1_off.png", D3DDevice.Instance.Device);
            off1.Position = opciones.Position;
            musicas.Add(off1);

            #endregion

            #region inicializarTiempo
            time = new Timer(500);
            time.Elapsed += OnTimedEvent;
            time.AutoReset = true;
            time.Enabled = true;
            #endregion
        }

        static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            cambiar = true;
        }

        public void Render()
        {
            int i = 1;
            if (musicaSel) i = 0;
            
            drawer2D.BeginDrawSprite();
            drawer2D.DrawSprite(opciones);
            drawer2D.DrawSprite(escenas[escenaSel]);
            drawer2D.DrawSprite(musicas[i]);
            drawer2D.EndDrawSprite();
        }

        public void Dispose()
        {
            escenas.ForEach(s => s.Dispose());
            musicas.ForEach(s => s.Dispose());
            time.Dispose();
            opciones.Dispose();
        }

        public void Update(TgcD3dInput Input)
        {
            #region chequearInput

            if (cambiar)
            {
                if (Input.keyUp(Key.UpArrow))
                {
                    if (tipoEscena)
                    {
                        escenaSel--;
                        if (escenaSel < 0) escenaSel = 4;
                    }
                    else
                    {
                        musicaSel = !musicaSel;
                    }

                    cambiar = false;
                }
                if (Input.keyUp(Key.DownArrow))
                {
                    if (tipoEscena)
                    {
                        escenaSel++;
                        if (escenaSel > 4) escenaSel = 0;

                    }
                    else
                    {
                        musicaSel = !musicaSel;
                    }
                    cambiar = false;
                }
                if (Input.keyUp(Key.LeftArrow))
                {
                    tipoEscena = true;
                    cambiar = false;
                }
                if (Input.keyUp(Key.RightArrow))
                {
                    tipoEscena = false;
                    cambiar = false;
                }
            }

            if (Input.keyUp(Key.Return))
            {
                cambiarEstado(Input);
            }

            #endregion

            #region manejarInput
            switch(escenaSel)
            {
                case 0:
                    gameModel.normal();
                    break;
                case 1:
                    gameModel.picante();
                    break;
                case 2:
                    gameModel.congelar();
                    break;
                case 3:
                    gameModel.glow();
                    break;
                case 4:
                    gameModel.god();
                    break;
            }
            gameModel.musica(musicaSel);  
            #endregion
        }
    }
}
