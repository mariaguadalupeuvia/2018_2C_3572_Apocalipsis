using BulletSharp;
using BulletSharp.Math;
using System;
using System.Drawing;

namespace TGC.Group.Model.GameObjects.BulletObjects
{
    public class CollisionCallback : ContactResultCallback // este es para disparos de plantas
    {
        private BulletObject bulletObject;//este seria una bala o disparo
                                          
        GameLogic logica;
        public CollisionCallback(GameLogic logica,BulletObject objeto)
        {
            this.logica = logica;
            bulletObject = objeto;
        }

        public override float AddSingleResult(ManifoldPoint cp,
            CollisionObjectWrapper colObj0Wrap, int partId0, int index0,
            CollisionObjectWrapper colObj1Wrap, int partId1, int index1)
        {
            if (cp.Distance < 0.0f)
            {
                if (logica.floor() == colObj1Wrap.CollisionObject)
                {
                    Console.WriteLine("Colisione con floor!!!");
                    //si choqué con el piso me despido de este mundo 
                     logica.desactivar(bulletObject);
                }
                //else if (logica.esZombie((RigidBody)colObj1Wrap.CollisionObject) )// esZombie() tiene efecto cuando es true
                //{
                //    Console.WriteLine("Colisione con un zombie!!!");
                //}
            }
            return 0;
        }
    }
}
