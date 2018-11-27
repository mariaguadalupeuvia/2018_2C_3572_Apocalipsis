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
using BulletSharp;
using TGC.Group.Model.GameObjects.BulletObjects;
using TGC.Group.Model.GameObjects.BulletObjects.CollisionCallbacks;
using Microsoft.DirectX;

namespace TGC.Group.Model
{
    public class Tablero : BulletObject, IPostProcess 
    {
        #region variables
        List<Plataforma> plataformas = new List<Plataforma>();
        private TGCVector3 collisionPoint;
        private TGCBox collisionPointMesh;
        private TgcPickingRay pickingRay;
        private bool selected;
        public Plataforma plataformaSeleccionada { get; set; }
        TgcMesh contenedor;
        public GameLogic logica;
        #endregion
        List<Pared> paredes = new List<Pared>();

        public Tablero(GameLogic logica)
        {
            this.logica = logica;
            //x = -850, -150, 550, 1250
            paredes.Add(new Pared(logica, -1550));
            paredes.Add(new Pared(logica, -850));
            paredes.Add(new Pared(logica, -150));
            paredes.Add(new Pared(logica, 550));
            paredes.Add(new Pared(logica, 1250));
            paredes.Add(new Pared(logica, 1950));
        }

        public void Init(TgcD3dInput Input)
        {
            crearTableroPicking(5,9);

            pickingRay = new TgcPickingRay(Input);
             
            //Crear caja para marcar en que lugar hubo colision
            collisionPointMesh = TGCBox.fromSize(new TGCVector3(3, 3, 3), Color.Red);
            collisionPointMesh.AutoTransform = true;
            selected = false;

            #region configurarEfecto
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            #endregion

            #region contenedor
            contenedor = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\contenedor-TgcScene.xml").Meshes[0];
            contenedor.Scale = new TGCVector3(45.5f, 45.5f, 45.5f);
            contenedor.Position = new TGCVector3(-2000f, 400f, -800f);
            contenedor.Effect = efecto;
            contenedor.Technique = "RenderSceneProgresivo";

            objetos.Add(contenedor);
            //glowObjects.Add(contenedor);
            
            #endregion

            //este body seria del tablero o isla principal
            body = FactoryBody.crearBodyIsla();
            callback = new CollisionCallbackIsla(logica);
            logica.addBulletObject(this);

            PostProcess.agregarPostProcessObject(this);
        }

        #region gestionarTecnicasShader
        public void cambiarTecnicaDefault()
        {
            contenedor.Technique = "RenderSceneProgresivo";
            plataformas.ForEach(p => p.Technique = "RenderScene");
        }
        public void cambiarTecnicaPostProceso()
        {
            contenedor.Technique = "RenderSceneProgresivo";
            plataformas.ForEach(p => p.Technique = "RenderScene");
        }
        #endregion

        public void Update(TgcD3dInput Input)
        {
            efecto.SetValue("_Time", GameModel.time);
            efecto.SetValue("alturaEnY", GameLogic.cantidadZombiesMuertos * 10);

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
            base.Render();
        }

        public void Dispose()
        {
            plataformas.ForEach(p => p.Dispose());
        }

        #region crearObjetos
        private void crearTableroPicking(int filas, int columnas)
        {
            int i, j;
            int x = -1200, y = 293, z = -3000;

            for (i = 0; i < filas; i++)
            {
                for (j = 0; j < columnas; j++)
                {
                    plataformas.Add(new Plataforma(new TGCVector3(x, y, z)));
                    z += 450;
                }
                x += 700;
                z = -3000;

            }
        }
        #endregion

        public void efectoSombra(TGCVector3 lightDir, TGCVector3 lightPos, TGCMatrix lightView, TGCMatrix projMatrix)
        {
            efecto.SetValue("g_vLightPos", new Vector4(lightPos.X, lightPos.Y, lightPos.Z, 1));
            efecto.SetValue("g_vLightDir", new Vector4(lightDir.X, lightDir.Y, lightDir.Z, 1));
            efecto.SetValue("g_mProjLight", projMatrix.ToMatrix());
            efecto.SetValue("g_mViewLightProj", (lightView * projMatrix).ToMatrix());
        }

        public void cambiarTecnicaShadow(Texture shadowTex)
        {
            contenedor.Technique = "RenderShadow";
            plataformas.ForEach(p => p.Technique = "RenderShadow");
            efecto.SetValue("g_txShadow", shadowTex);
        }
    }
}
