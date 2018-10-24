using BulletSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Group.Model.EstadosJuego;

namespace TGC.Group.Model.GameObjects.BulletObjects.CollisionCallbacks
{
    public class CollisionCallbackFinal : ContactResultCallback
    {
        GameLogic logica;
        Play play;

        public CollisionCallbackFinal(GameLogic logica, Play play)
        {
            this.logica = logica;
            this.play = play;
        }

        public override float AddSingleResult(ManifoldPoint cp,
            CollisionObjectWrapper colObj0Wrap, int partId0, int index0,
            CollisionObjectWrapper colObj1Wrap, int partId1, int index1)
        {
            if (cp.Distance < 0.0f)
            {
                if (logica.esApocalipsisZombie((RigidBody)colObj1Wrap.CollisionObject))// esApocalipsisZombie() tiene efecto cuando es true
                {
                    //Console.WriteLine("Un zombie llego al final de la isla");
                    play.gameOver(); 
                }
            }
            return 0;
        }

    }
}
