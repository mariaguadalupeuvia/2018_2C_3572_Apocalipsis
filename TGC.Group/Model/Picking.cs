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

namespace TGC.Group.Model
{
    public class Picking
    {
        #region variables
        List<Plataforma> pickingObjects;
        private TGCVector3 collisionPoint;
        private TGCBox collisionPointMesh;
        private TgcPickingRay pickingRay;
        private bool selected;
        private TgcMesh selectedMesh;
        #endregion

        public void Init(TgcD3dInput Input)
        {
            pickingObjects = new List<Plataforma>() { new Plataforma(new TGCVector3(800f, 320f, 550f)),
                                                      new Plataforma(new TGCVector3(700f, 320f, 550f)),
                                                      new Plataforma(new TGCVector3(600f, 320f, 550f)),
                                                      new Plataforma(new TGCVector3(500f, 320f, 550f))};
            pickingRay = new TgcPickingRay(Input);

            //Crear caja para marcar en que lugar hubo colision
            collisionPointMesh = TGCBox.fromSize(new TGCVector3(3, 3, 3), Color.Red);
            collisionPointMesh.AutoTransform = true;
            selected = false;
        }

        public void Update()
        {
            
        }

        public void Render(TgcD3dInput Input)
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
        }

        public void Dispose()
        {
            pickingObjects.ForEach(p => p.Dispose());
        }
    }
}
