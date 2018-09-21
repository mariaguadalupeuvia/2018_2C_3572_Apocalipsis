﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model.GameObjects
{
    public class Mina : Planta
    {
        private TgcMesh mina;

        public Mina(GamePhysics world, TGCVector3 posicion)
        {
            base.Init(world);

            #region configurarObjeto

            mina = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Mina-TgcScene.xml").Meshes[0];
            mina.Scale = new TGCVector3(35.5f, 35.5f, 35.5f);
            mina.Position = new TGCVector3(posicion.X , posicion.Y - 35, posicion.Z - 50);
            mina.Effect = efecto;
            mina.Technique = "RenderScene";

            #endregion
        }

        public override void Render()
        {
            mina.Render();
        }

        public override void Update(TgcD3dInput Input)
        {
          
        }
        public override void Dispose()
        {
            mina.Dispose();
        }
        public override int getCostoEnSoles()
        {
            return 200;
        }
        public override void cambiarTecnicaShader(string tecnica)
        {
            mina.Technique = tecnica;
        }
    }
}