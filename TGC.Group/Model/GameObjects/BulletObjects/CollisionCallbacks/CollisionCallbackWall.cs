using BulletSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model.GameObjects.BulletObjects.CollisionCallbacks
{
    public class CollisionCallbackWall : ContactResultCallback // este es para disparos de plantas
    {
        private BulletObject bulletObject;//este seria un sol o algo que no daña
        GameLogic logica;

        public CollisionCallbackWall(GameLogic logica)
        {
            this.logica = logica;
        }

        public override float AddSingleResult(ManifoldPoint cp,
            CollisionObjectWrapper colObj0Wrap, int partId0, int index0,
            CollisionObjectWrapper colObj1Wrap, int partId1, int index1)
        {
            if (cp.Distance < 0.0f)
            {
                //if (logica.floor() == colObj1Wrap.CollisionObject)
                //{
                //    //Console.WriteLine("Un sol colisiono con floor!!!");
                //    //si choqué con el piso me despido de este mundo 
                //    logica.desactivar(bulletObject);
                //}
                logica.esZombie((RigidBody)colObj1Wrap.CollisionObject, true);// esZombie() tiene efecto colateral con esta firma, simplemente muere

            }
            return 0;
        }
    }
}