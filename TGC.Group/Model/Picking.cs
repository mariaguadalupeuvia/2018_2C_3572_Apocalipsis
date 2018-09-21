using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Collision;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Group.Model.GameObjects;
using Microsoft.DirectX.Direct3D;
using TGC.Core.Shaders;
using TGC.Core.Direct3D;

namespace TGC.Group.Model
{
    public class Picking
    {
        #region variables
        List<Plataforma> pickingObjects = new List<Plataforma>();
        private TGCVector3 collisionPoint;
        private TGCBox collisionPointMesh;
        private TgcPickingRay pickingRay;
        private bool selected;
        private TgcMesh selectedMesh;
        TgcMesh isla;
        #endregion

        private void crearTableroPicking(int filas, int columnas)
        {
            int i, j;
            int x = 100, y = 400, z = 1500;

            for ( i = 0; i< filas; i++)
            {
                for ( j = 0; j < columnas; j++)
                {
                    pickingObjects.Add(new Plataforma(new TGCVector3(x, y, z)));
                    z += 100;
                }
                x += 100;
                z = 1500;
            }
        }

        public void Init(TgcD3dInput Input)
        {
            crearTableroPicking(5,9);
            crearIsla();
            pickingRay = new TgcPickingRay(Input);

            //Crear caja para marcar en que lugar hubo colision
            collisionPointMesh = TGCBox.fromSize(new TGCVector3(3, 3, 3), Color.Red);
            collisionPointMesh.AutoTransform = true;
            selected = false;
        }

        private void crearIsla()
        {
            #region configurarEfecto
            //var d3dDevice = D3DDevice.Instance.Device;
            //Texture bumpMap = TextureLoader.FromFile(d3dDevice, GameModel.mediaDir + "texturas\\terrain\\NormalMapMar.png");
            Effect efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            //efecto.SetValue("NormalMap", bumpMap);
            #endregion
            
            isla = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Isla-TgcScene.xml").Meshes[0];
            isla.Scale = new TGCVector3(500.5f, 300.5f, 800.5f);
            isla.Effect = efecto;
            isla.Technique = "RenderScene";
            isla.Position = new TGCVector3(400, 340f, 1800f);
            isla.RotateZ(3.1f);
        }

        public TGCVector3 Update(TgcD3dInput Input)
        {
            #region chequeoDeColision

            //Si hacen clic con el mouse, ver si hay colision RayAABB
            if (Input.buttonPressed(TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                pickingRay.updateRay();

                //Testear Ray contra el AABB de todos los meshes
                foreach (var piker in pickingObjects)
                {
                    var aabb = piker.plataforma.BoundingBox;

                    //Ejecutar test, si devuelve true se carga el punto de colision collisionPoint
                    selected = TgcCollisionUtils.intersectRayAABB(pickingRay.Ray, aabb, out collisionPoint);
                    if (selected)
                    {
                        selectedMesh = piker.plataforma;
                        piker.plataforma.BoundingBox.setRenderColor(Color.LightBlue);
                        break;
                    }
                }
            }
            #endregion

            if (selectedMesh != null )
            {
                return selectedMesh.Position;
            }
            else
            {
                return new TGCVector3(100, 320, 500);
            }
            
        }

        public void Render()
        {
            #region renderizado
            pickingObjects.ForEach(p => p.Render());

            //Renderizar BoundingBox del mesh seleccionado
            if (selected)
            {
                //Render de AABB
                selectedMesh.BoundingBox.Render();

                //Dibujar caja que representa el punto de colision
                collisionPointMesh.Position = collisionPoint;
                collisionPointMesh.Render();
            }
            #endregion

            isla.Render();
        }

        public void Dispose()
        {
            pickingObjects.ForEach(p => p.Dispose());
            isla.Dispose();
        }
    }
}
