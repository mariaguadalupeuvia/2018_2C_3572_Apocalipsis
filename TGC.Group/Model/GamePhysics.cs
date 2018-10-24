using BulletSharp;
using BulletSharp.Math;
using System;
using System.Collections.Generic;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.Textures;
using TGC.Group.Model.GameObjects.BulletObjects;

namespace TGC.Group.Model
{
    public class GamePhysics
    {
        #region variables
        protected DiscreteDynamicsWorld dynamicsWorld;
        protected CollisionDispatcher dispatcher;
        protected DefaultCollisionConfiguration collisionConfiguration;
        protected SequentialImpulseConstraintSolver constraintSolver;
        protected BroadphaseInterface overlappingPairCache;

        public List<BulletObject> bulletObjects = new List<BulletObject>();
        public List<BulletObject> desactivados = new List<BulletObject>();
       
      //  private TgcPlane floorMesh;
        public RigidBody floorBody;
        #endregion

        public void Init()
        {
            #region MUNDO //Creamos el mundo fisico por defecto.
            
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

            #region PISO 
            var d3dDevice = D3DDevice.Instance.Device;

            //El piso es un plano estatico se dice que si tiene masa 0 es estatico.
            var floorShape = new StaticPlaneShape(TGCVector3.Up.ToBsVector, 0);
            var floorMotionState = new DefaultMotionState();// Matrix.Translation(0f, 200f, -700f));//esto puede ir sin parametro y funciona
            var floorInfo = new RigidBodyConstructionInfo(0, floorMotionState, floorShape);
            floorBody = new RigidBody(floorInfo);
            dynamicsWorld.AddRigidBody(floorBody);

            //Cargamos objetos de render del framework.
            //var floorTexture = TgcTexture.createTexture(d3dDevice, GameModel.mediaDir + "texturas\\terrain\\TerrainTexture1.jpg");
            //floorMesh = new TgcPlane(new TGCVector3(0, 0, 0), new TGCVector3(400, 0f, 400), TgcPlane.Orientations.XZplane, floorTexture);
            #endregion
        }

        public void Update()
        {
            bulletObjects.ForEach(b => dynamicsWorld.ContactTest(b.body, b.callback));
            bulletObjects.ForEach(b => b.Update());
            removerDesactivados();//Al colisionar los disparos mueren, las plantan son comida y a los zombies los matan a tiros

            dynamicsWorld.StepSimulation(1/60f, 10);
        }

        public void Render()
        {
            bulletObjects.ForEach(b => b.Render());
           // floorMesh.Render();
        }

        public void Dispose()
        {
            #region dispose
            dynamicsWorld.Dispose();
            dispatcher.Dispose();
            collisionConfiguration.Dispose();
            constraintSolver.Dispose();
            overlappingPairCache.Dispose();

            bulletObjects.ForEach(b => b.Dispose());
            //floorMesh.Dispose();
            #endregion
        }

        #region agregarYQuitarObjetosDeLista
        public void addBulletObject(BulletObject objeto)
        {
            bulletObjects.Add(objeto);
            dynamicsWorld.AddRigidBody(objeto.body);
        }

        public void removeBulletObject(BulletObject objeto)
        {
            bulletObjects.Remove(objeto);
            dynamicsWorld.RemoveRigidBody(objeto.body);
            objeto.Dispose();
        }

        private void removerDesactivados()
        {
            desactivados.ForEach(d => removeBulletObject(d));
            desactivados.Clear();
        }
        #endregion
    }

}