
using BulletSharp;
using System;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.Textures;

namespace TGC.Group.Model
{
    public class Bullet //esta clase la tengo unicamente para probar cosas. no la voy a usar
    {
        private TgcPlane floorMesh;
        private TGCBox boxMesh;
        private TGCSphere sphereMesh;

        //Rigid Bodies:
        private RigidBody floorBody;
        private RigidBody boxBody;
        private RigidBody ballBody;

        protected DiscreteDynamicsWorld dynamicsWorld;
        protected CollisionDispatcher dispatcher;
        protected DefaultCollisionConfiguration collisionConfiguration;
        protected SequentialImpulseConstraintSolver constraintSolver;
        protected BroadphaseInterface overlappingPairCache;

        ContactResultCallback callback;

        public void Init()
        {
            #region MUNDO_FISICO //Creamos el mundo fisico por defecto.
            collisionConfiguration = new DefaultCollisionConfiguration();
            dispatcher = new CollisionDispatcher(collisionConfiguration);
           
            GImpactCollisionAlgorithm.RegisterAlgorithm(dispatcher);
            constraintSolver = new SequentialImpulseConstraintSolver();
            overlappingPairCache = new DbvtBroadphase(); 
            dynamicsWorld = new DiscreteDynamicsWorld(dispatcher, overlappingPairCache, constraintSolver, collisionConfiguration)
            {
                Gravity = new TGCVector3(0, -20f, 0).ToBsVector
            };
            #endregion

            var texture = TgcTexture.createTexture(D3DDevice.Instance.Device, GameModel.mediaDir + "texturas\\terrain\\NormalMapMar.png");
            //Creamos shapes y bodies.

            #region PISO 
            //El piso es un plano estatico se dice que si tiene masa 0 es estatico.
            var floorShape = new StaticPlaneShape(TGCVector3.Up.ToBsVector, 0);
            var floorMotionState = new DefaultMotionState();
            var floorInfo = new RigidBodyConstructionInfo(0, floorMotionState, floorShape);
            floorBody = new RigidBody(floorInfo);
            dynamicsWorld.AddRigidBody(floorBody);

            //Cargamos objetos de render del framework.
            var floorTexture = TgcTexture.createTexture(D3DDevice.Instance.Device, GameModel.mediaDir + "modelos\\Textures\\Canionero.jpg");
            floorMesh = new TgcPlane(new TGCVector3(0, 500, 0), new TGCVector3(400, 0f, 400), TgcPlane.Orientations.XZplane, floorTexture);
            #endregion

            #region CAJA
            //Se crea una caja de tamaño 20 con rotaciones y origien en 10,100,10 y 1kg de masa.
            var boxShape = new BoxShape(10, 10, 10);
            var boxTransform = TGCMatrix.RotationYawPitchRoll(MathUtil.SIMD_HALF_PI, MathUtil.SIMD_QUARTER_PI, MathUtil.SIMD_2_PI).ToBsMatrix;
            boxTransform.Origin = new TGCVector3(0, 600, 0).ToBsVector;
            DefaultMotionState boxMotionState = new DefaultMotionState(boxTransform);
            //Es importante calcular la inercia caso contrario el objeto no rotara.
            var boxLocalInertia = boxShape.CalculateLocalInertia(1f);
            var boxInfo = new RigidBodyConstructionInfo(1f, boxMotionState, boxShape, boxLocalInertia);
            boxBody = new RigidBody(boxInfo);
            dynamicsWorld.AddRigidBody(boxBody);

            //Es importante crear todos los mesh con centro en el 0,0,0 y que este coincida con el centro de masa definido caso contrario rotaria de otra forma diferente a la dada por el motor de fisica.
            boxMesh = TGCBox.fromSize(new TGCVector3(20, 20, 20), texture);

            #endregion

            #region BOLA
            //Se crea una esfera de tamaño 1 para escalarla luego (en render)
            //Crea una bola de radio 10 origen 50 de 1 kg.
            var ballShape = new SphereShape(10);
            var ballTransform = TGCMatrix.Identity;
            ballTransform.Origin = new TGCVector3(0, 200, 0);
            var ballMotionState = new DefaultMotionState(ballTransform.ToBsMatrix);
            //Podriamos no calcular la inercia para que no rote, pero es correcto que rote tambien.
            var ballLocalInertia = ballShape.CalculateLocalInertia(1f);
            var ballInfo = new RigidBodyConstructionInfo(1, ballMotionState, ballShape, ballLocalInertia);
            ballBody = new RigidBody(ballInfo);
            dynamicsWorld.AddRigidBody(ballBody);

            sphereMesh = new TGCSphere(1, texture.Clone(), TGCVector3.Empty);
            //Tgc no crea el vertex buffer hasta invocar a update values.
            sphereMesh.updateValues();

            #endregion

            callback = new MyContactResultCallback(dispatcher, dynamicsWorld, floorBody);// boxBody);
        }

        public void Update()
        {
            dynamicsWorld.ContactTest(ballBody, callback);
            dynamicsWorld.StepSimulation(1 / 60f, 10);
        }

        public void Render()
        {
            //Obtenemos la matrix de directx con la transformacion que corresponde a la caja.
            boxMesh.Transform = new TGCMatrix(boxBody.InterpolationWorldTransform);
            boxMesh.Render();

            //Obtenemos la transformacion de la pelota, en este caso se ve como se puede escalar esa transformacion.
            sphereMesh.Transform = TGCMatrix.Scaling(10, 10, 10) * new TGCMatrix(ballBody.InterpolationWorldTransform);
            sphereMesh.Render();

            floorMesh.Render();
        }

        public void Dispose()
        {
            dynamicsWorld.Dispose();
            dispatcher.Dispose();
            collisionConfiguration.Dispose();
            constraintSolver.Dispose();
            overlappingPairCache.Dispose();
            boxBody.Dispose();
            ballBody.Dispose();
            floorBody.Dispose();

            boxMesh.Dispose();
            sphereMesh.Dispose();

            floorMesh.Dispose();
        }
    }
}