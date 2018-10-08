using BulletSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model.GameObjects.BulletObjects.CollisionCallbacks
{
    public class CollisionCallbackFinal : ContactResultCallback
    {
        GameLogic logica;

        public CollisionCallbackFinal(GameLogic logica)
        {
            this.logica = logica;
        }

        public override float AddSingleResult(ManifoldPoint cp,
            CollisionObjectWrapper colObj0Wrap, int partId0, int index0,
            CollisionObjectWrapper colObj1Wrap, int partId1, int index1)
        {
            if (cp.Distance < 0.0f)
            {
                if (logica.esApocalipsisZombie((RigidBody)colObj1Wrap.CollisionObject))// esZombieEnIsla() tiene efecto cuando es true
                {
                    Console.WriteLine("Un zombie llego al final de la isla");
                }
            }
            return 0;
        }

    }
}
