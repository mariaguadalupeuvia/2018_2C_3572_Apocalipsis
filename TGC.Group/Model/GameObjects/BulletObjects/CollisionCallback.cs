using BulletSharp;
using BulletSharp.Math;
using System;
using System.Drawing;

namespace TGC.Group.Model.GameObjects.BulletObjects
{
    public class CollisionCallback : ContactResultCallback
    {
        private BulletObject bulletObject;

        public CollisionCallback(BulletObject objeto)
        {
            bulletObject = objeto;
        }

        public override float AddSingleResult(ManifoldPoint cp,
            CollisionObjectWrapper colObj0Wrap, int partId0, int index0,
            CollisionObjectWrapper colObj1Wrap, int partId1, int index1)
        {
            if (cp.Distance < 0.0f)
            {
                if (bulletObject.body == colObj1Wrap.CollisionObject)
                {
                    Console.WriteLine("Colisione");
                    //bulletObject.Dispose();
                }
            }
            return 0;
        }

    }


}
