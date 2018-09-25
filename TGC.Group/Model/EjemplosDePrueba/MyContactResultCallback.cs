using BulletSharp;
using BulletSharp.Math;
using System;
using System.Drawing;

namespace TGC.Group.Model
{
    internal class MyContactResultCallback : ContactResultCallback
    {
        int x = 0;
        private DynamicsWorld _world;
        protected CollisionDispatcher dispatcher;
        private RigidBody bodyCollision;

        public MyContactResultCallback(CollisionDispatcher dispatch,  DynamicsWorld world, RigidBody boxBody)
        {
            dispatcher = dispatch;
            _world = world;
            bodyCollision = boxBody;
    }

        public override float AddSingleResult(ManifoldPoint cp,
            CollisionObjectWrapper colObj0Wrap, int partId0, int index0,
            CollisionObjectWrapper colObj1Wrap, int partId1, int index1)
        {
            //Vector3 ptA = cp.PositionWorldOnA;
            //Vector3 ptB = cp.PositionWorldOnB;
            //Vector3 color = new Vector3(255, 0, 255);


            if (cp.Distance < 0.0f)
            {
                if (bodyCollision == colObj1Wrap.CollisionObject)
                {
                    //Vector3 ptA = cp.PositionWorldOnA;
                    //Vector3 ptB = cp.PositionWorldOnB;
                    //Vector3 normalOnB = cp.NormalWorldOnB;
                    //bodyCollision.ApplyForce(new Vector3(255, 255, 255), new Vector3(0, 0, 0));
                    //bodyCollision.ApplyImpulse(new Vector3(100, 0, 100), new Vector3(0, 200, 0));
                     //_world.DebugDrawer.DrawLine(ref ptA, ref ptB, ref color);
                    //Console.WriteLine("normalOnB: (x " + normalOnB.X + ", y " + normalOnB.Y + ", z " + normalOnB.Z + ")");
                    Console.WriteLine("" + colObj1Wrap.CollisionObject.ToString());//  "ptA.y: " + ptA.Y + "; ptB.y: " + ptB.Y);
                }

            }
            return 0;
        }

        //CollisionAlgorithm algorithm = dispatcher.FindAlgorithm(colObj0Wrap, colObj1Wrap, , DispatcherQueryType.ClosestPointAlgorithms);

        //var conf = new DefaultCollisionConfiguration();
        //var dispatcher = new CollisionDispatcher(conf);
        //dispatcher.FindAlgorithm()
        //DispatcherInfo info = new DispatcherInfo();
        //ManifoldResult result = new ManifoldResult(colObj0Wrap, colObj1Wrap);

        //algorithm.ProcessCollision(colObj0Wrap, colObj1Wrap, info, result);

        //if ( result.PersistentManifold.NumContacts > 0)
        //{ Console.WriteLine("tiempo: " + x); }

        //return result.PersistentManifold.NumContacts;
    }
    


}