using BulletSharp;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Core.Terrain;
using TGC.Core.Textures;

namespace TGC.Group.Model.GameObjects.BulletObjects
{
    public class Caja : BulletObject //por ahora esta para pruebas
    {
        //TgcMesh caja;
        private TGCBox boxMesh;
        private TgcMesh planta;

        public Caja(GameLogic logica)//GamePhysics world)
        {
            crearBody(new TGCVector3(1300f, 360f, 1500f));
            var d3dDevice = D3DDevice.Instance.Device;

         //   physicWorld = world;

            #region configurarEfecto
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            #endregion

            #region configurarObjeto

            planta = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\PLANTA-TgcScene.xml").Meshes[0];
            planta.Scale = new TGCVector3(35.5f, 35.5f, 35.5f);
            planta.Position = new TGCVector3(0, 600f, 0);

            planta.Effect = efecto;
            planta.Technique = "RenderScene";
            objetos.Add(planta);
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
            body = new RigidBody(boxInfo);
           
            var texture = TgcTexture.createTexture(D3DDevice.Instance.Device, GameModel.mediaDir + "texturas\\terrain\\NormalMapMar.png");
            //Es importante crear todos los mesh con centro en el 0,0,0 y que este coincida con el centro de masa definido caso contrario rotaria de otra forma diferente a la dada por el motor de fisica.
            boxMesh = TGCBox.fromSize(new TGCVector3(50, 50, 50), texture);

            #endregion
            callback = new CollisionCallback(logica, this);

            objetos.Add(boxMesh);
        }

        public override void Init()
        {
        }

        public override void Update()
        {
        }

        public override void Render()
        {
            //Obtenemos la matrix de directx con la transformacion que corresponde a la caja.
            //boxMesh.Transform = new TGCMatrix(body.InterpolationWorldTransform) ;
            //boxMesh.Render();
            
           // Console.WriteLine("matrix t: " + body.InterpolationWorldTransform);

            planta.Position = new TGCVector3(0, body.InterpolationWorldTransform.M42, 0);
            //planta.Transform = new TGCMatrix(body.InterpolationWorldTransform);
            //planta.UpdateMeshTransform();
            planta.Render();
        }
    }
}
