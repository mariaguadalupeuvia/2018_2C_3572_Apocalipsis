﻿using BulletSharp;
using BulletSharp.Math;
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
using TGC.Core.Sound;
using TGC.Core.Example;
using TGC.Core.Textures;
using Microsoft.DirectX.Direct3D;

namespace TGC.Group.Model.GameObjects.BulletObjects
{
    public abstract class Disparo : BulletObject, IPostProcess
    {
        #region variables
        protected TGCSphere esfera;
        #endregion

        public void init(string textura, TgcMesh planta)
        {
            var d3dDevice = D3DDevice.Instance.Device;

            #region configurarObjeto

            var texture = TgcTexture.createTexture(D3DDevice.Instance.Device, GameModel.mediaDir + "modelos\\Textures\\" + textura + ".jpg");
            esfera = new TGCSphere(1, texture.Clone(), TGCVector3.Empty);
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            esfera.Effect = efecto;
            esfera.Technique = "RenderScene";
            esfera.Scale = new TGCVector3(30.5f, 30.5f, 30.5f);
            esfera.Position = planta.Position;
            esfera.Rotation = planta.Rotation;
            esfera.updateValues();

            objetos.Add(esfera);
            #endregion

            GameSound.disparar();
            PostProcess.agregarPostProcessObject(this);
        }

        #region gestionarTecnicasShader
        public void cambiarTecnicaDefault()
        {
            esfera.Technique = "RenderScene";
        }
        public void cambiarTecnicaPostProceso()
        {
            esfera.Technique = "RenderScene";
        }
        #endregion

        public override void Render()
        {
            //body.Translate(new Vector3(0, 0, 17));
            esfera.Transform = TGCMatrix.Scaling(10, 10, 10) * new TGCMatrix(body.InterpolationWorldTransform);
            esfera.Render();
        }

        public abstract void dañarZombie(Zombie zombie);

        public void efectoSombra(TGCVector3 lightDir, TGCVector3 lightPos, TGCMatrix lightView, TGCMatrix projMatrix)
        {
            efecto.SetValue("g_vLightPos", new Microsoft.DirectX.Vector4(lightPos.X, lightPos.Y, lightPos.Z, 1));
            efecto.SetValue("g_vLightDir", new Microsoft.DirectX.Vector4(lightDir.X, lightDir.Y, lightDir.Z, 1));
            efecto.SetValue("g_mProjLight", projMatrix.ToMatrix());
            efecto.SetValue("g_mViewLightProj", (lightView * projMatrix).ToMatrix());
        }

        public void cambiarTecnicaShadow(Texture shadowTex)
        {
            esfera.Technique = "RenderShadow";
            efecto.SetValue("g_txShadow", shadowTex);
        }
    }
}