using Microsoft.DirectX.Direct3D;
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

namespace TGC.Group.Model.GameObjects
{
    public class Girasol : Planta
    {
        #region variables
        List<Sol> soles = new List<Sol>();
        private TgcMesh girasol;
        private TgcMesh tallo;
        private int espera = 0;
        #endregion

        public Girasol(GamePhysics world, TGCVector3 posicion)
        {
            base.Init(world);

            #region configurarObjeto

            float factorEscalado = 30.0f;
            girasol = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Girasol-TgcScene.xml").Meshes[0];
            girasol.Scale = new TGCVector3(factorEscalado, factorEscalado, factorEscalado);
            girasol.Position = new TGCVector3(posicion.X - 8 , posicion.Y + 35, posicion.Z - 10);
            girasol.RotateX(60);
            girasol.RotateY(90);
            girasol.RotateZ(90);
            girasol.Effect = efecto;
            girasol.Technique = "RenderScene";

            tallo = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\PlantaFinal-TgcScene.xml").Meshes[0];
            tallo.Scale = new TGCVector3(factorEscalado * 0.5f, factorEscalado * 0.5f, factorEscalado * 0.5f);
            tallo.Position = new TGCVector3(posicion.X  , posicion.Y - 50, posicion.Z );// new TGCVector3(500f, 200f, 1500f);
            tallo.Effect = efecto;
            tallo.Technique = "RenderScene";
            #endregion

            physicWorld = world;
        }

        public void disparar()
        {
            Sol disparo = new Sol( girasol, physicWorld);
            disparo.init();
            GameLogic.cantidadEnergia += 50;
            physicWorld.addBulletObject(disparo);
            soles.Add(disparo);
        }

        public override void Update(TgcD3dInput Input)
        {
            espera++;
            if (espera == 900)
            {
                disparar();
                espera = 0;
            }
        }

        public override void Render()
        {
            girasol.Render();
            tallo.Render();
            soles.ForEach(d => d.render());
        }

        public override void Dispose()
        {
            girasol.Dispose();
            tallo.Dispose();
            soles.ForEach(d => d.dispose());
        }

        public override int getCostoEnSoles()
        {
            return 50;
        }

        public override void cambiarTecnicaShader(string tecnica)
        {
            girasol.Technique = tecnica;
            tallo.Technique = tecnica;
        }
    }
    }