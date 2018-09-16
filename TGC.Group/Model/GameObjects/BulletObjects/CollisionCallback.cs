using BulletSharp;
using BulletSharp.Math;
using System;
using System.Drawing;

namespace TGC.Group.Model.GameObjects.BulletObjects
{
    public class CollisionCallback : ContactResultCallback
    {
        private BulletObject bulletObject;//este seria una bala o disparo
        GamePhysics physicsWorld;

        public CollisionCallback(BulletObject objeto, GamePhysics world)
        {
            bulletObject = objeto;
            physicsWorld = world;
        }

        public override float AddSingleResult(ManifoldPoint cp,
            CollisionObjectWrapper colObj0Wrap, int partId0, int index0,
            CollisionObjectWrapper colObj1Wrap, int partId1, int index1)
        {
            if (cp.Distance < 0.0f)
            {
                if (physicsWorld.floorBody == colObj1Wrap.CollisionObject)
                {
                    Console.WriteLine("Colisione con floor!!!");
                    //si choqué con el piso me despido de este mundo 
                    physicsWorld.desactivados.Add(bulletObject);
                }
            }

            return 0;
        }

    }


}
