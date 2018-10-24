﻿using System;
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
        List<GameObject> gameObjects = new List<GameObject>() { new Skybox(), new Terreno(), new Escenario()};
        GameObject agua = new Agua(); //los objetos transparentes se renderean arriba de todo
        private Gui.Gui gui = new Gui.Gui();
        public GameLogic logica = new GameLogic();
        public ZombieRey zombieRey;
        public ParedFondo pared;
        #endregion

        public void Init(TgcD3dInput Input)
        {
            #region inicializarRendereables

            gameObjects.ForEach(g => g.Init());
            logica.Init(Input);
            agua.Init(); 
            gui.Init();

            zombieRey = new ZombieRey(new TGCVector3(100, 50,100/*700, 50, 6000*/), logica);
            logica.addBulletObject(zombieRey);
            pared = new ParedFondo(logica, this);
            #endregion
        }

        internal void gameOver()
        {
            cambiarEstado(new TgcD3dInput());
        }

        public void victoria()
        {
            gameObjects.Add(agua);
            Estado estado = new Victoria(gameObjects);
            estado.Init(new TgcD3dInput());
            GameModel.enPlay = false;
            GameModel.estadoDelJuego = estado;
        }
        public void cambiarEstado(TgcD3dInput Input)
        {
            gameObjects.Add(agua);
            Estado estado = new GameOver(gameObjects);
            estado.Init(Input);
            GameModel.enPlay = false;
            GameModel.estadoDelJuego = estado;
        }
        
        public void Render()
        {
            #region render
            gameObjects.ForEach(g => g.Render());
            logica.Render();
            agua.Render();
            gui.Render();
            pared.Render();
            #endregion
        }

        public void Dispose()
        {
            #region dispose
            gameObjects.ForEach(g => g.Dispose());
            agua.Dispose();
            logica.Dispose();
            gui.Dispose();
            #endregion
        }

        public void Update(TgcD3dInput Input)
        {
            #region update
            zombieRey.Update(Input);
            gameObjects.ForEach(g => g.Update());
            agua.Update();
            logica.Update(Input);
            #endregion

            if(GameLogic.cantidadZombiesMuertos > 25)
            {
                victoria();
            }
        }
    }
}