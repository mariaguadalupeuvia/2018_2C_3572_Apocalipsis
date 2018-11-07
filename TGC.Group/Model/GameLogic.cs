﻿using Microsoft.DirectX.DirectInput;
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
using System.Timers;

namespace TGC.Group.Model
{
    public class GameLogic
    {
        #region variables
        public static int cantidadEnergia = 50;
        public static int cantidadZombiesMuertos = 0;
        private GamePhysics physicWorld = new GamePhysics(); // este va a tener los objetos colisionables
        private Tablero tablero;
        TGCVector3 posicionSeleccionada;
        int tiempoDeHorda = 0;
        Random random = new Random();
        int tipoZombie = 0;
        
        List<Planta> plantas = new List<Planta>();
        List<Planta> plantasIlegales = new List<Planta>();
        private static  List<Zombie> zombies = new List<Zombie>();
        float[] posiciones = { -1200, -500, 200, 900, 1600 };//1600, 1600, 1600, 1600, 1600 };//

        static Timer time;
        static bool tiempoCumplido = false;
        #endregion

        private const int MAXIMA_CANTIDAD_DE_ZOMBIES = 50;
        private static int INTERVALO = 45000;

        public void Init(TgcD3dInput Input)
        {
            physicWorld.Init();
            tablero = new Tablero(this);
            tablero.Init(Input);

            #region manejarTiempo
            time = new Timer(INTERVALO);
            time.Elapsed += OnTimedEvent;
            time.AutoReset = true;
            time.Enabled = true;
            #endregion
        }

        static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            tiempoCumplido = true;
            INTERVALO -=2;
        }

        public void Update(TgcD3dInput Input)
        {
            physicWorld.Update();

            #region manejarCreacionDeZombies
            if (zombies.Count < MAXIMA_CANTIDAD_DE_ZOMBIES)
            {
                if (tiempoCumplido)
                {
                    tiempoCumplido = false;
                    
                    crearZombies();
                    tiempoDeHorda++;
                    quitarPlantasIlegales();
                    if (tiempoDeHorda > 10)
                    {
                        crearHorda();
                        tiempoDeHorda = 0;
                    }
                }
            }
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
            if (Input.keyDown(Key.NumPad6))
            {
                addPlanta(FactoryPlanta.crearNuez(posicionSeleccionada, this, tablero.plataformaSeleccionada));
            }
            if (Input.keyDown(Key.NumPad7))
            {
                addPlanta(FactoryPlanta.crearSuperCanion(posicionSeleccionada, this, tablero.plataformaSeleccionada));
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
            int i;
            for (i = 0; i < random.Next(25); i++)
            {
                crearZombies();
            }
        }

        private void crearZombies()
        {
            Zombie zombie;
            switch (tipoZombie)
            {
                case 0:
                    zombie = new Zombie(new TGCVector3(posiciones[random.Next(5)], 900f, 5040f), this);
                    zombies.Add(zombie);
                    physicWorld.addBulletObject(zombie);
                    break;
                case 1:
                    zombie = new ZombieXD(new TGCVector3(posiciones[random.Next(5)], 900f, 5040f), this);
                    zombies.Add(zombie);
                    physicWorld.addBulletObject(zombie);
                    break;
                case 2:
                    zombie = new ZombieVip(new TGCVector3(posiciones[random.Next(5)], 900f, 5040f), this);
                    zombies.Add(zombie);
                    physicWorld.addBulletObject(zombie);
                    break;
            }

            tipoZombie++;
            if (tipoZombie > 2) tipoZombie = 0;
        }

        public List<Zombie> crearHordasZombies()
        {
            int i;
            float[] posiciones = { -1200, -1000 - 800, -500, -400, -200, 0, 200, 350, 700, 900, 1020, 1300, 1600 };
            for (i = 0; i < 100; i++)
            {
                Zombie zombie;
                switch (tipoZombie)
                {
                    case 0:
                        zombie = new Zombie(new TGCVector3(posiciones[random.Next(13)], 900f, 5040f), this);
                        zombies.Add(zombie);
                        physicWorld.addBulletObject(zombie);
                        break;
                    case 1:
                        zombie = new ZombieXD(new TGCVector3(posiciones[random.Next(13)], 900f, 5040f), this);
                        zombies.Add(zombie);
                        physicWorld.addBulletObject(zombie);
                        break;
                    case 2:
                        zombie = new ZombieVip(new TGCVector3(posiciones[random.Next(13)], 900f, 5040f), this);
                        zombies.Add(zombie);
                        physicWorld.addBulletObject(zombie);
                        break;
                }

                tipoZombie++;
                if (tipoZombie > 2) tipoZombie = 0;
            }
            return zombies;
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

        //aca llego cuando un zombie colisiona con un ?
        public Boolean esZombie(RigidBody body)
        {
            Zombie zombieGolpeado = zombies.Find(z => z.body == body);
            if (zombieGolpeado != null)
            {
               // Console.WriteLine("posicion del globo al chocar con pared: " + zombieGolpeado.globoPosicion());
            }
            return (zombieGolpeado != null);
        }

        //aca llego cuando un zombie colisiona con un ventilador
        public Boolean esZombie(RigidBody body, bool opcion)
        {
            Zombie zombieGolpeado = zombies.Find(z => z.body == body);
            if (zombieGolpeado != null)
            {
                zombieGolpeado.morir();
            }
            return (zombieGolpeado != null);
        }
        //aca llego cuando un zombie colisiona con la isla pricipal
        public bool esZombieEnIsla(RigidBody body)
        {
            Zombie zombieAterrizado = zombies.Find(z => z.body == body);
            if (zombieAterrizado != null)
            {
                zombieAterrizado.llegaste();
            }
            return zombieAterrizado != null;
        }

        internal bool esApocalipsisZombie(RigidBody body)
        {
            Zombie zombieFinal = zombies.Find(z => z.body == body);
            if (zombieFinal != null)
            {
                //Console.WriteLine("game over, pos final del zombie en x:" + zombieFinal.POSICION().X  + ", en z:" + zombieFinal.POSICION().Z);  
            }
            return zombieFinal != null;
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
