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
using TGC.Group.Model.GameObjects.BulletObjects.Zombies;
using BulletSharp.Math;

namespace TGC.Group.Model
{
    public class GameLogic
    {
        #region variables
        public static int cantidadEnergia = 50;
        public static int cantidadZombiesMuertos = 0;
        private GamePhysics physicWorld = new GamePhysics(); // este va a tener los objetos colisionables
        private Tablero tablero;

        internal bool esApocalipsisZombie(RigidBody collisionObject)
        {
            return false;
        }

        TGCVector3 posicionSeleccionada;
        int tiempoDeHorda = 0;

        List<Planta> plantas = new List<Planta>();
        List<Planta> plantasIlegales = new List<Planta>();
        private static  List<Zombie> zombies = new List<Zombie>();
        #endregion

        private const int MAXIMA_CANTIDAD_DE_ZOMBIES = 25;

        public void Init(TgcD3dInput Input)
        {
            physicWorld.Init();
            tablero = new Tablero(this);
            tablero.Init(Input);
            crearZombies();
        }

        public void Update(TgcD3dInput Input)
        {
            physicWorld.Update();

            #region manejarCreacionDeZombies

            //if (zombies.Count < MAXIMA_CANTIDAD_DE_ZOMBIES)
            //{
            //    if (new Random().Next(250) == 77)
            //    {
            //        crearZombies();
            //        tiempoDeHorda++;
            //        quitarPlantasIlegales();
            //        if (tiempoDeHorda > 10)
            //        {
            //            crearHorda();
            //            tiempoDeHorda = 0;
            //        }
            //    }
            //}
            #endregion

            plantas.ForEach(P => P.Update(Input));

            #region manejoTablero
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
            #endregion

            #region chequearInput
            if (Input.keyDown(Key.NumPad1))
            {
                addPlanta(FactoryPlanta.crearCanion(posicionSeleccionada, this, tablero.plataformaSeleccionada));
            }
            if (Input.keyDown(Key.NumPad2))
            {
                addPlanta(FactoryPlanta.crearGirasol(posicionSeleccionada, this, tablero.plataformaSeleccionada));
            }
            if (Input.keyDown(Key.NumPad3))
            {
                addPlanta(FactoryPlanta.crearCongelador(posicionSeleccionada, this, tablero.plataformaSeleccionada));
            }
            if (Input.keyDown(Key.NumPad4))
            {
                addPlanta(FactoryPlanta.crearMina(posicionSeleccionada, this, tablero.plataformaSeleccionada));
            }
            if (Input.keyDown(Key.NumPad5))
            {
                addPlanta(FactoryPlanta.crearChile(posicionSeleccionada, this, tablero.plataformaSeleccionada));
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
            plantas.ForEach(P => P.Dispose());//chequear que los explosivos generan error aca
            plantasIlegales.ForEach(P => P.Dispose());
            physicWorld.Dispose();
            tablero.Dispose();
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
            Zombie zombie;

            zombie = new Zombie(new TGCVector3(-1200, 900f, 5040f), this);
            zombies.Add(zombie);
            physicWorld.addBulletObject(zombie);

            zombie = new ZombieVip(new TGCVector3(-500, 900f, 5040f), this);
            zombies.Add(zombie);
            physicWorld.addBulletObject(zombie);

            zombie = new ZombieXD(new TGCVector3(200, 900f, 5040f), this);
            zombies.Add(zombie);
            physicWorld.addBulletObject(zombie);

            zombie = new ZombieVip(new TGCVector3(900, 900f, 5040f), this);
            zombies.Add(zombie);
            physicWorld.addBulletObject(zombie);

            zombie = new ZombieXD(new TGCVector3(1600, 900f, 5040f), this);
            zombies.Add(zombie);
            physicWorld.addBulletObject(zombie);

        }
        private void crearZombiesVIEJO()
        {
            Random random = new Random();
            int j = random.Next();
            Zombie zombie;

            switch (j % 3)
            {
                case 0:
                    zombie = new Zombie(new TGCVector3(400, 900f, 5040f), this);
                    zombies.Add(zombie);
                    physicWorld.addBulletObject(zombie);
                    break;
                case 1:
                    zombie = new ZombieVip(new TGCVector3(450, 910f, 5000f), this);
                    zombies.Add(zombie);
                    physicWorld.addBulletObject(zombie);
                    break;
                case 2:
                    zombie = new ZombieXD(new TGCVector3(550, 930f, 5100f), this);
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

        internal bool esPlanta(RigidBody body, Zombie zombie)
        {
            Planta plantaColisionada = plantas.Find(p => p.body == body);
            if (plantaColisionada != null)
            {
                zombie.empezaAComer();
                plantaColisionada.teComen(zombie); 
            }
            return plantaColisionada != null;
        }

        //aca llego cuando un zombie colisiona con un disparo
        public bool esZombie(RigidBody body, Disparo disparo)
        {
            Zombie zombieGolpeado = zombies.Find(z => z.body == body);
            if (zombieGolpeado != null)
            {
                zombieGolpeado.teGolpearon(disparo);
            }
            return zombieGolpeado != null;
        }

        //aca llego cuando un zombie colisiona con la isla pricipal
        public bool esZombieEnIsla(RigidBody body)
        {
            Zombie zombieAterrizado = zombies.Find(z => z.body == body);
            if (zombieAterrizado != null)
            {
                zombieAterrizado.llegaste();
               // Console.WriteLine("ZOMBIE " + zombieAterrizado.nombre);

            }
            return zombieAterrizado != null;
        }
        #endregion

        #region cosasPocoImportantes
        public RigidBody floor()
        {
            return physicWorld.floorBody;
        }

        public void removePlanta(Planta unaPlanta)
        {
            plantas.Remove(unaPlanta);
        }

        public void quitarPlantasIlegales()
        {
            plantasIlegales.ForEach(P => P.Dispose());
            plantasIlegales.Clear();
        }
        #endregion
    }
}
