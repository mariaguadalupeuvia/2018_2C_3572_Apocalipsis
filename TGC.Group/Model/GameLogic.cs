using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Group.Model.GameObjects;
using TGC.Group.Model.GameObjects.BulletObjects;
using BulletSharp;

namespace TGC.Group.Model
{
    public class GameLogic
    {
        #region variables
        public static int cantidadEnergia = 50;
        public static int cantidadZombiesMuertos = 0;
        private GamePhysics physicWorld = new GamePhysics(); // este va a tener los objetos colisionables

        List<Planta> plantas = new List<Planta>();
        List<Planta> plantasIlegales = new List<Planta>();
        private static  List<Zombie> zombies = new List<Zombie>();

        private Tablero tablero = new Tablero();
        TGCVector3 posicionSeleccionada;
        int tiempoDeHorda = 0;
        #endregion

        public void Init(TgcD3dInput Input)
        {
            physicWorld.Init();
            tablero.Init(Input);
        }

        public void Update(TgcD3dInput Input)
        {
            physicWorld.Update();

            if (new Random().Next(250) == 77)
            {
                crearZombies();
                tiempoDeHorda++;
                quitarPlantasIlegales();
                if (tiempoDeHorda > 10)
                {
                    crearHorda();
                    tiempoDeHorda = 0;
                }
            }

            //if (new Random().Next(300) == 125)
            //{
            //    bombardear();
            //}

            plantas.ForEach(P => P.Update(Input));
            tablero.Update(Input);

            if (tablero.plataformaSeleccionada == null)
            {
                return;
            }
            if (tablero.plataformaSeleccionada.ocupado)
            {
                return;
            }
            posicionSeleccionada = tablero.plataformaSeleccionada.mesh.Position;

            #region chequearInput
            if (Input.keyDown(Key.NumPad1))
            {
                addPlanta(FactoryPlanta.crearCanion(posicionSeleccionada, this));
            }
            if (Input.keyDown(Key.NumPad2))
            {
                addPlanta(FactoryPlanta.crearGirasol(posicionSeleccionada, this));
            }
            if (Input.keyDown(Key.NumPad3))
            {
                addPlanta(FactoryPlanta.crearCongelador(posicionSeleccionada, this));
            }
            if (Input.keyDown(Key.NumPad4))
            {
                addPlanta(FactoryPlanta.crearMina(posicionSeleccionada, this));
            }
            if (Input.keyDown(Key.NumPad5))
            {
                addPlanta(FactoryPlanta.crearChile(posicionSeleccionada, this));
            }

            #endregion
        }

        public void Render()
        {
            physicWorld.Render();
            tablero.Render();
            plantas.ForEach(P => P.Render());
            plantasIlegales.ForEach(P => P.Render());
        }

        public void Dispose()
        {
            plantas.ForEach(P => P.Dispose());
            plantasIlegales.ForEach(P => P.Dispose());
            physicWorld.Dispose();
            tablero.Dispose();
        }

        public void quitarPlantasIlegales()
        {
            plantasIlegales.ForEach(P => P.Dispose());
            plantasIlegales.Clear();
        }

        #region crearObjetos
        internal void addBulletObject(BulletObject objeto)
        {
            physicWorld.addBulletObject(objeto);
        }

        private void addPlanta(Planta unaPlanta)
        {
            if (unaPlanta.getCostoEnSoles() > cantidadEnergia)
            {
                //hacer esto bien, si es transparente no deberia atacar
                unaPlanta.cambiarTecnicaShader("Transparente");
                plantasIlegales.Add(unaPlanta);
            }
            else
            {
                cantidadEnergia -= unaPlanta.getCostoEnSoles();
                tablero.plataformaSeleccionada.ocupado = true;
                plantas.Add(unaPlanta);
            }
        }

        private void bombardear()//ver donde esta fallando
        {
            //physicWorld.addBulletObject(new Apocalipsis(new TGCVector3(0, 5200f, 0)));
            //physicWorld.addBulletObject(new Apocalipsis(new TGCVector3(100, 5300f, 50)));
            //physicWorld.addBulletObject(new Apocalipsis(new TGCVector3(20, 5600f, 200)));
            //physicWorld.addBulletObject(new Apocalipsis(new TGCVector3(5, 5800f, 20)));
            //physicWorld.addBulletObject(new Apocalipsis(new TGCVector3(10, 6200f, 300)));
            //physicWorld.addBulletObject(new Apocalipsis(new TGCVector3(600, 5000f, 100)));
        }
        private void crearHorda()
        {
            Random random = new Random();
            int i;
            for (i = 0; i < random.Next(25); i++)
            {
                crearZombies();
            }
        }

        private void crearZombies()
        {
            Random random = new Random();
            int j = random.Next();
            Zombie zombie;

            switch (j % 3)
                {
                case 0:
                    zombie = new Zombie(new TGCVector3(400, 500f, 5040f), this);
                    zombies.Add(zombie);
                    physicWorld.addBulletObject(zombie);
                    break;
                case 1:
                    zombie = new ZombieVip(new TGCVector3(450, 510f, 5000f), this);
                    zombies.Add(zombie);
                    physicWorld.addBulletObject(zombie);
                    break;
                case 2:
                    zombie = new ZombieXD(new TGCVector3(550, 530f, 5100f), this);
                    zombies.Add(zombie);
                    physicWorld.addBulletObject(zombie);
                    break;
                }
        }
        #endregion

        #region gestionColisiones
        internal void desactivar(BulletObject bulletObject)
        {
            physicWorld.desactivados.Add(bulletObject);
        }

        internal bool esPlanta(RigidBody body)//por ahora no funciona porque planta no usa body
        {
            //Planta plantaColisionada = plantas.Find(p => p.body == body);
            //if (plantaColisionada != null)
            //{
            //    plantaColisionada.teComen();
            //}
            //return plantaColisionada != null;
            return false;
        }

        public bool esZombie(RigidBody body, Disparo disparo)
        {
            Zombie zombieGolpeado = zombies.Find(z => z.body == body);
            if (zombieGolpeado != null)
            {
                zombieGolpeado.teGolpearon(disparo);
            }
            return zombieGolpeado != null;
        }
        #endregion

        public RigidBody floor()
        {
            return physicWorld.floorBody;
        }

        public void removePlanta(Planta unaPlanta)
        {
            plantas.Remove(unaPlanta);
        }
    }
}
