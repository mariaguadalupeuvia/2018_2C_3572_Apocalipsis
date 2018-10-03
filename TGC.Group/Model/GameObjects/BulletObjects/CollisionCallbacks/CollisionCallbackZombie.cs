using BulletSharp;
using BulletSharp.Math;
using System;
using System.Drawing;

namespace TGC.Group.Model.GameObjects.BulletObjects
{
    public class CollisionCallbackZombie : ContactResultCallback //este es para zombies que quieren plantas para comer
    {
        private Zombie zombie;
        GameLogic logica;

        public CollisionCallbackZombie(GameLogic logica, Zombie objeto)
        {
            this.logica = logica;
            zombie = objeto;
        }

        public override float AddSingleResult(ManifoldPoint cp,
            CollisionObjectWrapper colObj0Wrap, int partId0, int index0,
            CollisionObjectWrapper colObj1Wrap, int partId1, int index1)
        {
            if (cp.Distance < 0.0f)
            {
                if (logica.floor() == colObj1Wrap.CollisionObject)
                {

                    if (zombie.enCaidaLibre())
                    { 
                        logica.desactivar(zombie);
                    }
                }
                else if (logica.esPlanta((RigidBody)colObj1Wrap.CollisionObject, zombie))// esPlanta() tiene efecto cuando es true, es decir que es dañada por el zombie que se detiene a comer
                {
                    //Console.WriteLine("Un zombie colisionó con una planta!!!");
                }
            }
            return 0;
        }
    }
}