using BulletSharp;
using BulletSharp.Math;
using System;
using System.Drawing;

namespace TGC.Group.Model.GameObjects.BulletObjects
{
    public class CollisionCallbackZombie : ContactResultCallback //este es para zombies que quieren plantas para comer
    {
        private BulletObject zombie;

        GameLogic logica;
        public CollisionCallbackZombie(GameLogic logica, BulletObject objeto)
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
                    //Console.WriteLine("Un zombie colisionó con floor!!!");
                    //si choqué con el piso me despido de este mundo 
                    logica.desactivar(zombie);
                }
                else if (logica.esPlanta((RigidBody)colObj1Wrap.CollisionObject))// esPlanta() tiene efecto cuando es true, es decir que es dañada por el zombie
                {
                    Console.WriteLine("Un zombie colisionó con una planta!!!");
                    logica.desactivar(zombie);
                }
            }
            return 0;
        }
    }
}