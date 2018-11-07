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
using System.Timers;

namespace TGC.Group.Model
{
    public class LogicaSimplificada : GameLogic
    {
        #region variables
        private GamePhysics physicWorld = new GamePhysics(); // este va a tener los objetos colisionables
        Random random = new Random();
        int tipoZombie = 0;

        private  List<Zombie> zombies = new List<Zombie>();
        float[] posiciones = { -1200, -1000 -800, -500,-400, -200, 0, 200, 350, 700, 900, 1020, 1300, 1600 };//1600, 1600, 1600, 1600, 1600 };//

        static Timer time;
        static bool tiempoCumplido = false;
        #endregion

        private static int INTERVALO = 45000;

        public void Init(TgcD3dInput Input)
        {
            physicWorld.Init();

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
            INTERVALO -= 2;
        }

        public void Update(TgcD3dInput Input)
        {
            physicWorld.Update();
        }

        public void Render()
        {
            physicWorld.Render();
        }

        public void Dispose()
        {
            physicWorld.Dispose();
        }

        #region crearObjetos
        internal void addBulletObject(BulletObject objeto)
        {
            physicWorld.addBulletObject(objeto);
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
        #endregion

        #region gestionColisiones
        internal void desactivar(BulletObject bulletObject)
        {
            physicWorld.desactivados.Add(bulletObject);
        }

        internal bool esPlanta(RigidBody body, Zombie zombie)
        {
            return false;
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
        #endregion
    }
}