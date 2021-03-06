﻿using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using Microsoft.DirectX.DirectInput;
using TGC.Core.Input;
using TGC.Group.Model.GameObjects.BulletObjects;
using System.Drawing;
using Microsoft.DirectX;

namespace TGC.Group.Model.GameObjects
{
    public class Plataforma : TgcMesh, IPostProcess 
    {
        #region variables
        public TgcMesh mesh { get; set; }
        public bool ocupado { get; set; }
        protected Microsoft.DirectX.Direct3D.Effect efecto;
        #endregion

        public Plataforma(TGCVector3 posicion)
        {
            var d3dDevice = D3DDevice.Instance.Device;

            #region configurarEfecto
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            #endregion

            #region configurarMesh
            mesh = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\PLATAFORMA-TgcScene.xml").Meshes[0];
            mesh.Scale = new TGCVector3(35.5f, 15.5f, 35.5f);
            mesh.Position = posicion;
            mesh.Effect = efecto;
            mesh.Technique = "RenderScene";
            mesh.BoundingBox.setRenderColor(Color.Red);
            mesh.AutoTransform = true;
            #endregion

            ocupado = false;
            PostProcess.agregarPostProcessObject(this);
        }

        #region gestionTecnicasShader
        public void cambiarTecnicaDefault()
        {
            mesh.Technique = "RenderScene";
        }
        public void cambiarTecnicaPostProceso()
        {
            mesh.Technique = "RenderScene";
        }
        #endregion

        public void Render()
        {
            mesh.Render();
           // mesh.BoundingBox.Render();
        }

        public void Dispose()
        {
            mesh.Dispose();
        }

        public void renderGlow()
        {
            mesh.Render();
        }

        public void efectoSombra(TGCVector3 lightDir, TGCVector3 lightPos, TGCMatrix lightView, TGCMatrix projMatrix)
        {
            efecto.SetValue("g_vLightPos", new Vector4(lightPos.X, lightPos.Y, lightPos.Z, 1));
            efecto.SetValue("g_vLightDir", new Vector4(lightDir.X, lightDir.Y, lightDir.Z, 1));
            efecto.SetValue("g_mProjLight", projMatrix.ToMatrix());
            efecto.SetValue("g_mViewLightProj", (lightView * projMatrix).ToMatrix());
        }

        public void cambiarTecnicaShadow(Texture shadowTex)
        {
            mesh.Technique = "RenderShadow";
            efecto.SetValue("g_txShadow", shadowTex);
        }
    }
}