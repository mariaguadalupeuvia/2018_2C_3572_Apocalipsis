using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model.GameObjects.BulletObjects
{
    public class ZombieVip : Zombie
    {
        protected TgcMesh sombrero;
        private const int MAXIMO_DAÑO_SOPORTADO = 50;

        public ZombieVip(TGCVector3 posicion, GameLogic logica) : base(posicion, logica)
        {
            #region configurarObjeto
            sombrero = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Sombrero-TgcScene.xml").Meshes[0];
            sombrero.Scale = new TGCVector3(140.5f, 140.5f, 140.5f);
            sombrero.Position = posicion;
            sombrero.Effect = efecto;
            sombrero.Technique = "RenderScene";

            objetos.Add(sombrero);
            #endregion
        }

        public override void Render()
        {
            if (body.InterpolationWorldTransform.M42 < 400)
            {
                sombrero.Position = new TGCVector3(body.InterpolationWorldTransform.M41, body.InterpolationWorldTransform.M42 + 360 + (subir * 2), body.InterpolationWorldTransform.M43);
            }
            else
            {
                sombrero.Position = new TGCVector3(body.InterpolationWorldTransform.M41, body.InterpolationWorldTransform.M42 + 360, body.InterpolationWorldTransform.M43);
            }

           
            base.Render();
        }

        #region respuestaAAtaqueDePlanta
        public virtual void recibirDaño()
        {
            daño += 1;
            efecto.SetValue("colorVida", daño * 0.1f); //cambia el color del globo

            if (daño == MAXIMO_DAÑO_SOPORTADO) //despues de X balazos quedas en caida libre, cuando tocas el piso vas a dispose
            {
                morir();
            }
        }

        public override void congelate()
        {
            sombrero.Technique = "RenderSceneCongelada";
            base.congelate();
        }
        #endregion
    }
}