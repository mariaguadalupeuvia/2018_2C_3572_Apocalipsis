using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Group.Model.GameObjects;
using TGC.Group.Model.GameObjects.BulletObjects;
using TGC.Group.Model.GameObjects.BulletObjects.Zombies;

namespace TGC.Group.Model.EstadosJuego
{
    public class Play : Estado
    {
        #region variables
        private Gui.Gui gui = new Gui.Gui();
        public GameLogic logica = new GameLogic();
        public ZombieRey zombieRey;
        public ParedFondo pared;
        private PostProcess postProcess;
        GameModel gameModel;
        #endregion


        public Play(PostProcess postProcess, GameModel gm)
        {
            this.postProcess = postProcess;
            postProcess.estado = this;
            gameModel = gm;
        }

        public void Init(TgcD3dInput Input)
        {
            #region inicializarRendereables

            logica.Init(Input);
            gui.Init();

            zombieRey = new ZombieRey(new TGCVector3(100, 50, 100/*700, 50, 6000*/), logica);
            logica.addBulletObject(zombieRey);
            pared = new ParedFondo(logica, this);
            #endregion

            if (gameModel.postProcesar)
            {
                gameModel.postProcessActivo = true;
            }
        }

        public void gameOver()
        {
            Estado estado = new GameOver();
            estado.Init(new TgcD3dInput());
            GameModel.enPlay = false;
            GameModel.estadoDelJuego = estado;
        }

        public void victoria()
        {
            Estado estado = new Victoria();
            estado.Init(new TgcD3dInput());
            GameModel.enPlay = false;
            GameModel.estadoDelJuego = estado;
        }

        public void cambiarEstado(TgcD3dInput Input)
        {

        }

        public void Render()
        {
            postProcess.Render();
        }
        public void renderPostProcess()
        {
            logica.Render();
            gui.Render();
            pared.Render();
        }

        public void Dispose()
        {
            logica.Dispose();
            gui.Dispose();
        }

        public void Update(TgcD3dInput Input)
        {
            zombieRey.Update(Input);
            logica.Update(Input);

            if(GameLogic.cantidadZombiesMuertos > 25)
            {
                victoria();
            }
        }

    }
}
