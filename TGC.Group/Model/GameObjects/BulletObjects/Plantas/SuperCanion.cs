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
using TGC.Group.Model.GameObjects.BulletObjects.Disparos;

namespace TGC.Group.Model.GameObjects.BulletObjects.Plantas
{
    public class SuperCanion : Planta
    {
        #region variables
        private TgcMesh canion1;
        private TgcMesh canion2;
        private TgcMesh tallo;
        private int espera = 0;
        #endregion

        public SuperCanion(TGCVector3 posicion, GameLogic logica, Plataforma plataforma)
        {
            base.Init(logica, plataforma);

            #region configurarObjeto
            float factorEscalado = 20.0f;
            canion1 = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\CANIONERO-TgcScene.xml").Meshes[0];
            canion1.Scale = new TGCVector3(factorEscalado, factorEscalado, factorEscalado);
            canion1.Position = new TGCVector3(posicion.X - 21, posicion.Y + 40, posicion.Z + 15);
            canion1.RotateX(14);
            canion1.RotateY(18);
            canion1.Effect = efecto;
            canion1.Technique = "RenderScene";

            canion2 = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\CANIONERO-TgcScene.xml").Meshes[0];
            canion2.Scale = new TGCVector3(factorEscalado, factorEscalado, factorEscalado);
            canion2.Position = new TGCVector3(posicion.X + 21, posicion.Y + 40, posicion.Z + 15);
            canion2.RotateX(-14);
            canion2.RotateY(48);
            canion2.Effect = efecto;
            canion2.Technique = "RenderScene";


            tallo = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\SuperCanion-TgcScene.xml").Meshes[0];
            tallo.Scale = new TGCVector3(factorEscalado * 0.75f, factorEscalado * 0.75f, factorEscalado * 0.75f);
            tallo.Position = new TGCVector3(posicion.X, posicion.Y - 50, posicion.Z);
            tallo.RotateY(4);
            tallo.Effect = efecto;
            tallo.Technique = "RenderScene";

            #endregion
        }

        public override void Update(TgcD3dInput Input)
        {
            espera++;
            if (espera == 100)
            {
                disparar();
                espera = 0;
            }
        }

        public override void Render()
        {
            canion1.Render();
            canion2.Render();
            tallo.Render();
        }

        public override void Dispose()
        {
            canion1.Dispose();
            canion2.Dispose();
            tallo.Dispose();
        }

        public override void cambiarTecnicaShader(string tecnica)
        {
            canion1.Technique = tecnica;
            canion2.Technique = tecnica;
            tallo.Technique = tecnica;
        }

        public void disparar()
        {
            SuperBala disparo = new SuperBala(canion1, logica, -45);
            disparo.init("Canionero", canion1); //el parametro es el nombre de la textura para el mesh 
            SuperBala disparo2 = new SuperBala(canion2, logica, 45);
            disparo2.init("Canionero", canion2); //el parametro es el nombre de la textura para el mesh                                 
        }

        public override int getCostoEnSoles()
        {
            return 150;
        }
    }
}
