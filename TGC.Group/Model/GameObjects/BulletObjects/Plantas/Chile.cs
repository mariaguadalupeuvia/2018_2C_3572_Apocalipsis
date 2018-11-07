using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model.GameObjects
{
    public class Chile : Planta
    {
        private TgcMesh chile;
        TgcMesh semiesfera;

        public Chile(TGCVector3 posicion, GameLogic logica, Plataforma plataforma)
        {
            base.Init(logica, plataforma);

            #region configurarObjeto
            float factorEscalado = 16.0f;
            chile = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Chile-TgcScene.xml").Meshes[0];
            chile.Scale = new TGCVector3(factorEscalado, factorEscalado, factorEscalado);
            chile.Position =  new TGCVector3(posicion.X , posicion.Y - 40, posicion.Z + 20);
            chile.Effect = efecto;
            chile.Technique = "Explosivo";
            #endregion

            //var d3dDevice = D3DDevice.Instance.Device;
            //Texture fuego = TextureLoader.FromFile(d3dDevice, GameModel.mediaDir + "modelos\\Textures\\fuego0.jpg");
            //efecto.SetValue("NormalMap", fuego);

            semiesfera = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Semiesfera-TgcScene.xml").Meshes[0];
            semiesfera.Scale = new TGCVector3(100.5f, 100.5f, 100.5f);
            semiesfera.Effect = efecto;
            semiesfera.Technique = "calado";
            semiesfera.Position = new TGCVector3(posicion.X, 260, posicion.Z);

            Explosivo disparo = new Explosivo(new TGCVector3(posicion.X, posicion.Y - 40, posicion.Z + 20), logica, this);
            PostProcess.agregarPostProcessObject(this);
        }

        #region gestionTecnicasShader
        public override void cambiarTecnicaDefault()
        {
            chile.Technique = "RenderScene";
        }
        public override void cambiarTecnicaPostProceso()
        {
            chile.Technique = "dark";
        }
        public override void cambiarTecnicaShader(string tecnica)
        {
            chile.Technique = tecnica;
        }
        #endregion

        public override void Render()
        {
            chile.Render();
            semiesfera.Render();
        }

        public override void Dispose()
        {
            
        }

        public override void Update(TgcD3dInput Input)
        {
            efecto.SetValue("_Time", GameModel.time);
            semiesfera.RotateY(GameModel.time);
        }

        public override int getCostoEnSoles()
        {
            return 300;
        }
    }
}
