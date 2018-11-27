using BulletSharp;
using BulletSharp.Math;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Group.Model.GameObjects.BulletObjects;

namespace TGC.Group.Model
{
    public class Bomba : BulletObject, IPostProcess
    {
        #region variables
        private TgcMesh bomba;
        GameLogic logica; 

        bool explotando = false;
        float velocidadY = 80;
        float gravedad = 250;
        float movimientoY;
        float tiempo = 0;
        string tecnicaDefault = "RenderScene";
        #endregion

        public Bomba(TGCVector3 posicion, GameLogic logica)
        {
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            this.logica = logica;

            #region configurarObjeto

            bomba = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\BOMBA-TgcScene.xml").Meshes[0];
            bomba.Scale = new TGCVector3(40.5f, 80.5f, 40.5f);
            bomba.Effect = efecto;
            bomba.Technique = tecnicaDefault;
            bomba.Position = posicion;
            objetos.Add(bomba);

            body = FactoryBody.crearBodyZombie(new TGCVector3(posicion.X, posicion.Y + 350, posicion.Z));
            callback = new CollisionCallbackFloor(logica, this);
            logica.addBulletObject(this);

            #endregion

            PostProcess.agregarPostProcessObject(this);
        }

        public override void Update()
        {
            if ((bomba.Position.Y < 400 ) && (!explotando))
            {
                explotando = true;
                tecnicaDefault = "Explosivo3";
                tiempo = 0;
            }

            tiempo += 0.09f;
            movimientoY = (velocidadY * tiempo - gravedad * tiempo * tiempo) / 10;

            efecto.SetValue("_Time", tiempo);
            efecto.SetValue("movimientoY", movimientoY);
        }

        public override void Render()
        {
            body.Translate(new Vector3(0, -35, 0));
            bomba.Position = new TGCVector3(bomba.Position.X, body.InterpolationWorldTransform.M42, bomba.Position.Z);
            bomba.Render();
        }

        public override void Dispose()
        {
            logica.agregarExplosionChile(bomba.Position);
        }

        #region gestionarTecnicasShader
        public void cambiarTecnicaDefault()
        {
            bomba.Technique = tecnicaDefault;
        }
        public void cambiarTecnicaPostProceso()
        {
            bomba.Technique = "RenderScene";
        }
        public void efectoSombra(TGCVector3 lightDir, TGCVector3 lightPos, TGCMatrix lightView, TGCMatrix projMatrix)
        {
            efecto.SetValue("g_vLightPos", new Microsoft.DirectX.Vector4(lightPos.X, lightPos.Y, lightPos.Z, 1));
            efecto.SetValue("g_vLightDir", new Microsoft.DirectX.Vector4(lightDir.X, lightDir.Y, lightDir.Z, 1));
            efecto.SetValue("g_mProjLight", projMatrix.ToMatrix());
            efecto.SetValue("g_mViewLightProj", (lightView * projMatrix).ToMatrix());
        }

        public void cambiarTecnicaShadow(Texture shadowTex)
        {
            bomba.Technique = "RenderShadow";
            efecto.SetValue("g_txShadow", shadowTex);
        }
        #endregion
    }
}
