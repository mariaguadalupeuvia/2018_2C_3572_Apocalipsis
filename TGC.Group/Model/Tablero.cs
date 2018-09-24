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
    public class Tablero
    {
        #region variables
        List<Plataforma> plataformas = new List<Plataforma>();
        private TGCVector3 collisionPoint;
        private TGCBox collisionPointMesh;
        private TgcPickingRay pickingRay;
        private bool selected;
        public Plataforma plataformaSeleccionada { get; set; }
        TgcMesh apoyo;
        #endregion

        private void crearTableroPicking(int filas, int columnas)
        {
            int i, j;
            int x = 100, y = 400, z = 1500;

            for ( i = 0; i< filas; i++)
            {
                for ( j = 0; j < columnas; j++)
                {
                    plataformas.Add(new Plataforma(new TGCVector3(x, y, z)));
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
            Effect efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            #endregion

            #region configurarObjeto
            apoyo = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Isla-TgcScene.xml").Meshes[0];
            apoyo.Scale = new TGCVector3(500.5f, 300.5f, 800.5f);
            apoyo.Effect = efecto;
            apoyo.Technique = "RenderScene";
            apoyo.Position = new TGCVector3(400, 340f, 1900f);
            apoyo.RotateZ(3.1f);
            #endregion
        }

        public void Update(TgcD3dInput Input)
        {
            #region chequeoDeColision

            //Si hacen clic con el mouse, ver si hay colision RayAABB
            if (Input.buttonPressed(TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                pickingRay.updateRay();

                //Testear Ray contra el AABB de todos los meshes
                foreach (var unaPlataforma in plataformas)//.Where(p => !p.ocupado).ToList())
                {
                    var aabb = unaPlataforma.mesh.BoundingBox;

                    //Ejecutar test, si devuelve true se carga el punto de colision collisionPoint
                    selected = TgcCollisionUtils.intersectRayAABB(pickingRay.Ray, aabb, out collisionPoint);
                    if (selected)
                    {
                        plataformaSeleccionada = unaPlataforma;
                        unaPlataforma.mesh.BoundingBox.setRenderColor(Color.LightBlue);
                        break;
                    }
                }
            }
            #endregion
        }

        public void Render()
        {
            #region renderizado
            plataformas.ForEach(p => p.Render());

            //Renderizar BoundingBox del mesh seleccionado
            if (selected)
            {
                //Render de AABB
                plataformaSeleccionada.mesh.BoundingBox.Render();

                //Dibujar caja que representa el punto de colision
                collisionPointMesh.Position = collisionPoint;
                collisionPointMesh.Render();
            }
            #endregion

            apoyo.Render();
        }

        public void Dispose()
        {
            plataformas.ForEach(p => p.Dispose());
            apoyo.Dispose();
        }
    }
}
