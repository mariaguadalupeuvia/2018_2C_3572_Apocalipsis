using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;

namespace TGC.Group.Model.GameObjects
{
    public class Explosion
    {
        #region variables
        protected TgcMesh semiesfera;
        protected Effect efecto;

        float velocidadY = 100;
        float gravedad = 250;
        float movimientoY;
        float tiempo = 0;
        public bool activo = true;
        #endregion

        public virtual void Init(TGCVector3 posicion)
        { 
            #region configurarEfecto
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            #endregion

            #region configurarObjeto
            semiesfera = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\explosivoMape-TgcScene.xml").Meshes[0]; 
            semiesfera.Scale = new TGCVector3(70.5f, 70.5f, 70.5f);
            semiesfera.Effect = efecto;
            semiesfera.Technique = "Explosivo2";
            semiesfera.Position = new TGCVector3(posicion.X, 260, posicion.Z);
            #endregion

            GameSound.explotar();
        }

        public void Render()
        {
            tiempo += 0.003f;
            movimientoY = (velocidadY * tiempo - gravedad * tiempo * tiempo) / 10;

            efecto.SetValue("_Time", tiempo);
            efecto.SetValue("movimientoY", movimientoY);

            semiesfera.Render();
            if (tiempo > 2) activo = false; 
            
        }

        public void Dispose()
        {
            semiesfera.Dispose(); 
        }
    }
}
