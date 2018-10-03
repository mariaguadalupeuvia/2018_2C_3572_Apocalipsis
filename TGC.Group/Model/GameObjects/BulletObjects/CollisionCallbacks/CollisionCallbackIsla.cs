using BulletSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model.GameObjects.BulletObjects.CollisionCallbacks
    {
        public class CollisionCallbackIsla : ContactResultCallback // este es para la isla principal
        {
            GameLogic logica;

            public CollisionCallbackIsla(GameLogic logica)
            {
                this.logica = logica;
            }

            public override float AddSingleResult(ManifoldPoint cp,
                CollisionObjectWrapper colObj0Wrap, int partId0, int index0,
                CollisionObjectWrapper colObj1Wrap, int partId1, int index1)
            {
                if (cp.Distance < 0.0f)
                {
                    if (logica.esZombieEnIsla((RigidBody)colObj1Wrap.CollisionObject))// esZombieEnIsla() tiene efecto cuando es true
                {
                        //Console.WriteLine("Colisione con la isla!!!, tengo que hacer algo aca");
                    }
                }
                return 0;
            }
        }
    }