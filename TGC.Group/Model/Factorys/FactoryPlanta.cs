using BulletSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Group.Model.GameObjects;
using TGC.Group.Model.GameObjects.BulletObjects;
using TGC.Group.Model.GameObjects.BulletObjects.Plantas;

namespace TGC.Group.Model
{
    public static class FactoryPlanta
    {
        public static Planta crearCanion(TGCVector3 posicion, GameLogic logica, Plataforma plataforma)
        {
            Planta canion = new Canion(posicion, logica, plataforma);
            //posicion = new TGCVector3(posicion.X, posicion.Y - 74, posicion.Z);
            canion.body = FactoryBody.crearBodyPlanta(new TGCVector3(20, 15, 5), posicion);
           // Console.WriteLine("posicion canion: " + posicion);
            canion.callback = new CollisionCallbackFloor(logica, canion);
            logica.addBulletObject(canion);
            return canion;
        }
        public static Planta crearCongelador(TGCVector3 posicion, GameLogic logica, Plataforma plataforma)
        {
            Planta congelador = new Congelador(posicion, logica, plataforma);
            //posicion = new TGCVector3(posicion.X, posicion.Y - 74, posicion.Z);
            congelador.body = FactoryBody.crearBodyPlanta(new TGCVector3(20, 15, 5), posicion);
            //Console.WriteLine("posicion congelador: " + posicion);
            congelador.callback = new CollisionCallbackFloor(logica, congelador);
            logica.addBulletObject(congelador);
            return congelador;
        }
        public static Planta crearGirasol(TGCVector3 posicion, GameLogic logica, Plataforma plataforma)
        {
            Planta girasol = new Girasol(posicion, logica, plataforma);
            //posicion = new TGCVector3(posicion.X, posicion.Y - 74, posicion.Z);
            girasol.body = FactoryBody.crearBodyPlanta(new TGCVector3(20, 15, 1), posicion);
            //Console.WriteLine("posicion girasol: " + posicion);
            girasol.callback = new CollisionCallbackFloor(logica, girasol);
            logica.addBulletObject(girasol);
            return girasol;
        }
        public static Planta crearChile(TGCVector3 posicion, GameLogic logica, Plataforma plataforma)
        {
            Planta chile = new Chile(posicion, logica, plataforma);
            //posicion = new TGCVector3(posicion.X, posicion.Y - 74, posicion.Z);
            chile.body = FactoryBody.crearBodyPlanta(new TGCVector3(20, 15, 5), posicion);
            //Console.WriteLine("posicion chile: " + posicion);
            chile.callback = new CollisionCallbackFloor(logica, chile);
            logica.addBulletObject(chile);
            return chile;
        }
        public static Planta crearMina(TGCVector3 posicion, GameLogic logica, Plataforma plataforma)
        {
            Planta mina = new Mina(posicion, logica, plataforma);
            //posicion = new TGCVector3(posicion.X, posicion.Y - 74, posicion.Z);
            mina.body = FactoryBody.crearBodyPlanta(new TGCVector3(20, 15, 5), posicion);
            //Console.WriteLine("posicion mina: " + posicion);
            mina.callback = new CollisionCallbackFloor(logica, mina);
            logica.addBulletObject(mina);
            return mina;
        }
        public static Planta crearNuez(TGCVector3 posicion, GameLogic logica, Plataforma plataforma)
        {
            Planta nuez = new Nuez(posicion, logica, plataforma);
            //posicion = new TGCVector3(posicion.X, posicion.Y - 74, posicion.Z);
            nuez.body = FactoryBody.crearBodyPlanta(new TGCVector3(25, 15, 15), posicion);
            //Console.WriteLine("posicion nuez: " + posicion);
            nuez.callback = new CollisionCallbackFloor(logica, nuez);
            logica.addBulletObject(nuez);
            return nuez;
        }
        public static Planta crearSuperCanion(TGCVector3 posicion, GameLogic logica, Plataforma plataforma)
        {
            Planta superCanion = new SuperCanion(posicion, logica, plataforma);
            //posicion = new TGCVector3(posicion.X, posicion.Y - 74, posicion.Z);
            superCanion.body = FactoryBody.crearBodyPlanta(new TGCVector3(20, 15, 5), posicion);
            // Console.WriteLine("posicion canion: " + posicion);
            superCanion.callback = new CollisionCallbackFloor(logica, superCanion);
            logica.addBulletObject(superCanion);
            return superCanion;
        }
    }
}
