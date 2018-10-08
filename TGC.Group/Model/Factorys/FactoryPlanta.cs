using BulletSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Group.Model.GameObjects;
using TGC.Group.Model.GameObjects.BulletObjects;

namespace TGC.Group.Model
{
    public static class FactoryPlanta
    {
        public static Planta crearCanion(TGCVector3 posicion, GameLogic logica, Plataforma plataforma)
        {
            Planta canion = new Canion(posicion, logica, plataforma);
            canion.body = FactoryBody.crearBodyPlanta(new TGCVector3(5,5,5), posicion);
            canion.callback = new CollisionCallbackFloor(logica, canion);
            logica.addBulletObject(canion);
            return canion;
        }
        public static Planta crearCongelador(TGCVector3 posicion, GameLogic logica, Plataforma plataforma)
        {
            Planta congelador = new Congelador(posicion, logica, plataforma);
            congelador.body = FactoryBody.crearBodyPlanta(new TGCVector3(5, 5, 5), posicion);
            congelador.callback = new CollisionCallbackFloor(logica, congelador);
            logica.addBulletObject(congelador);
            return congelador;
        }
        public static Planta crearGirasol(TGCVector3 posicion, GameLogic logica, Plataforma plataforma)
        {
            Planta girasol = new Girasol(posicion, logica, plataforma);
            girasol.body = FactoryBody.crearBodyPlanta(new TGCVector3(1, 2, 1), posicion);
            girasol.callback = new CollisionCallbackFloor(logica, girasol);
            logica.addBulletObject(girasol);
            return girasol;
        }
        public static Planta crearChile(TGCVector3 posicion, GameLogic logica, Plataforma plataforma)
        {
            Planta chile = new Chile(posicion, logica, plataforma);
            chile.body = FactoryBody.crearBodyPlanta(new TGCVector3(5, 5, 5), posicion);
            chile.callback = new CollisionCallbackFloor(logica, chile);
            logica.addBulletObject(chile);
            return chile;
        }
        public static Planta crearMina(TGCVector3 posicion, GameLogic logica, Plataforma plataforma)
        {
            Planta congelador = new Mina(posicion, logica, plataforma);
            congelador.body = FactoryBody.crearBodyPlanta(new TGCVector3(5, 5, 5), posicion);
            congelador.callback = new CollisionCallbackFloor(logica, congelador);
            logica.addBulletObject(congelador);
            return congelador;
        }
    }
}
