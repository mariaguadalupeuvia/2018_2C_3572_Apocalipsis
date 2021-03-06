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
using BulletSharp;
using Microsoft.DirectX;

namespace TGC.Group.Model.GameObjects
{
    public abstract class Planta : BulletObject, IPostProcess
    {
        #region variables
        protected int nivelResistencia = 1000;
        protected int costoEnSoles;
        protected Microsoft.DirectX.Direct3D.Effect efecto;
        protected GameLogic logica;
        protected Plataforma plataforma;
        #endregion

        public void Init(GameLogic logica, Plataforma plataformaSeleccionada)
        {
            this.logica = logica;
            plataforma = plataformaSeleccionada;

            #region configurarEfecto
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            #endregion
        }

        public abstract void Update(TgcD3dInput Input);
        public abstract void Render();
        public abstract void Dispose();
        public abstract int getCostoEnSoles();
        public abstract void cambiarTecnicaShader(string tecnica);

        internal void teComen(Zombie zombie)
        {
            nivelResistencia--;
            if (nivelResistencia == 0)
            {
                zombie.empezaACaminar();
                liberar();
            }
        }

        public void liberar()
        {
            logica.removePlanta(this);
            logica.desactivar(this);
            plataforma.ocupado = false;
        }

        public abstract void cambiarTecnicaDefault();
        public abstract void cambiarTecnicaPostProceso();
        public abstract void cambiarTecnicaShadow(Texture shadowTex);

        public void efectoSombra(TGCVector3 lightDir, TGCVector3 lightPos, TGCMatrix lightView, TGCMatrix projMatrix)
        {
            efecto.SetValue("g_vLightPos", new Vector4(lightPos.X, lightPos.Y, lightPos.Z, 1));
            efecto.SetValue("g_vLightDir", new Vector4(lightDir.X, lightDir.Y, lightDir.Z, 1));
            efecto.SetValue("g_mProjLight", projMatrix.ToMatrix());
            efecto.SetValue("g_mViewLightProj", (lightView * projMatrix).ToMatrix());
        }

    }
}