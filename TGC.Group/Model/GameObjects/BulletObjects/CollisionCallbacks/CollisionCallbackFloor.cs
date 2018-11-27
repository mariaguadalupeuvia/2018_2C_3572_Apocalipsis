using BulletSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model.GameObjects.BulletObjects
{
    public class CollisionCallbackFloor : ContactResultCallback // este es para disparos de plantas
    {
        private BulletObject bulletObject;//este seria un sol o algo que no daña
        GameLogic logica;

        public CollisionCallbackFloor(GameLogic logica, BulletObject objeto)
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
                if (logica.esZombie((RigidBody)colObj1Wrap.CollisionObject))// esZombie() no tiene efecto colateral con esta firma
                {
                    //Console.WriteLine("Un zombie colisiono con planta!!!");
                }
                else if (logica.floor() == colObj1Wrap.CollisionObject)
                {
                    //si choqué con el piso me despido de este mundo 
                    logica.desactivar(bulletObject);
                    
                }
            }
            return 0;
        }
    }
}
