using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TGC.Core.Collision;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SkeletalAnimation;

namespace TGC.Group.Model.Gui
{
    public class Menu 
    {
        #region variables
        List<MenuItem> items = new List<MenuItem>();
        public MenuItem itemSeleccionado { get; set; }

        private TGCVector3 collisionPoint;
        private TGCBox collisionPointMesh;
        private TgcPickingRay pickingRay;
        private bool selected;
        #endregion

        public void Init(TgcD3dInput Input)
        {
            crearItems();
            pickingRay = new TgcPickingRay(Input);

            //Crear caja para marcar en que lugar hubo colision
            collisionPointMesh = TGCBox.fromSize(new TGCVector3(3, 3, 3), Color.Red);
            collisionPointMesh.AutoTransform = true;
            selected = false;
        }

        private void crearItems()
        {
            items.Add(new MenuItem(new TGCVector3(100, 600, 100), "Play"));
            items.Add(new MenuItem(new TGCVector3(100, 500, 100), "Ayuda"));
            items.Add(new MenuItem(new TGCVector3(100, 400, 100), "?"));
        }

        public void Update(TgcD3dInput Input)
        {
            #region chequeoDeColision

            //Si hacen clic con el mouse, ver si hay colision RayAABB
            if (Input.buttonPressed(TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                pickingRay.updateRay();

                //Testear Ray contra el AABB de todos los meshes
                foreach (var unItem in items)//.Where(p => !p.ocupado).ToList())
                {
                    var aabb = unItem.mesh.BoundingBox;
                  
                    //Ejecutar test, si devuelve true se carga el punto de colision collisionPoint
                    selected = TgcCollisionUtils.intersectRayAABB(pickingRay.Ray, aabb, out collisionPoint);
                    if (selected)
                    {
                        itemSeleccionado = unItem;
                        unItem.mesh.BoundingBox.setRenderColor(Color.LightBlue);
                        itemSeleccionado.manejarEvento();
                        break;
                    }
                }
            }
            #endregion
        }
        public void Render()
        {
            #region renderizado
            items.ForEach(i => i.Render());
            //Renderizar BoundingBox del mesh seleccionado
            if (selected)
            {
                //Render de AABB
                itemSeleccionado.mesh.BoundingBox.Render();
                
                //Dibujar caja que representa el punto de colision
                collisionPointMesh.Position = collisionPoint;
                collisionPointMesh.Render();
            }
            #endregion
        }

        public void Dispose()
        {
            items.ForEach(i => i.Dispose());
        }
    }
}

