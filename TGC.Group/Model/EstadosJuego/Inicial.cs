﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.Text;
using TGC.Group.Model.GameObjects.BulletObjects;
using TGC.Group.Model.Gui;

namespace TGC.Group.Model.EstadosJuego
{
    public class Inicial: Estado
    {
        #region variables
        private List<CustomSprite> sprites = new List<CustomSprite>();
        private Drawer2D drawer2D;
        CustomSprite barra2 = new CustomSprite();
        List<Zombie> zombies = new List<Zombie>();
        GameModel gameModel; 
        private float tiempo = 0;
        private PostProcess postProcess;
       
        public GameLogic logica = new LogicaSimplificada();

        public Inicial()
        {

        }
        public Inicial(PostProcess postProcess, GameModel gm)
        {
            this.postProcess = postProcess;
            this.gameModel = gm;
        }
        #endregion

        public void Init(TgcD3dInput input)
        {
            var d3dDevice = D3DDevice.Instance.Device;
            drawer2D = new Drawer2D();

            logica.Init(input);
            #region configurarSprites
            CustomSprite apocalipsis = new CustomSprite();
            apocalipsis.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\apocalipsisYa.png", D3DDevice.Instance.Device);
            var textureSize = apocalipsis.Bitmap.Size;
            apocalipsis.Position = new TGCVector2(FastMath.Max((D3DDevice.Instance.Width - textureSize.Width)* 0.5f, 0), FastMath.Max((D3DDevice.Instance.Height - textureSize.Height) * 0.5f, 0));
            sprites.Add(apocalipsis);
            
            barra2.Bitmap = new CustomBitmap(GameModel.mediaDir + "\\sprites\\barraAmarilla.png", D3DDevice.Instance.Device);
            barra2.Position = new TGCVector2(apocalipsis.Position.X, FastMath.Max(D3DDevice.Instance.Height * 0.879f, 0));
            barra2.Scaling = new TGCVector2(1, 1);

            sprites.Add(barra2);
            #endregion
        }

        public void Render()
        {
            logica.Render();
            drawer2D.BeginDrawSprite();
            sprites.ForEach(s => drawer2D.DrawSprite(s));
            drawer2D.EndDrawSprite();
        }

        public void Dispose()
        {
            sprites.ForEach(s => s.Dispose());
            logica.Dispose();
        }

        public void Update(TgcD3dInput Input)
        {
            if (tiempo < 65.5f)
            {
                tiempo = GameModel.time * 60;
                barra2.Scaling = new TGCVector2(tiempo, 1);
            }
            else
            {
                cambiarEstado(Input);
            }

            logica.Update(Input);
        }

        public void cambiarEstado(TgcD3dInput Input)
        {
            Estado estado = new Menu(postProcess, gameModel);
            estado.Init(Input);

            GameModel.estadoDelJuego = estado;
        }
    }
}
