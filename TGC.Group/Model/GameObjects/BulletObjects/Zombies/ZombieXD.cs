using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model.GameObjects.BulletObjects
    {
        public class ZombieXD : Zombie
        {
            protected TgcMesh cono;
            int bajar = 0;
            private const int MAXIMO_DAÑO_SOPORTADO = 30;

            public ZombieXD(TGCVector3 posicion, GameLogic logica) : base(posicion, logica)
            {
                #region configurarObjeto
                cono = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Cono-TgcScene.xml").Meshes[0];
                cono.Scale = new TGCVector3(40.5f, 40.5f, 40.5f);
                cono.Position = posicion;
                cono.RotateX(10); 
                cono.Effect = efecto;
                cono.Technique = "RenderScene";

                objetos.Add(cono);
            //glowObjects.Add(cono);
           
            #endregion
        }

            public override void Render()
            {
            if (body.InterpolationWorldTransform.M42 < 400)
            {
                bajar ++;
                cono.Position = new TGCVector3(cono.Position.X, 520  - (bajar * 0.5f), body.InterpolationWorldTransform.M43 + 70 + bajar);

            }
            else
            {
                cono.Position = new TGCVector3(cono.Position.X, body.InterpolationWorldTransform.M42 + 120, body.InterpolationWorldTransform.M43 + 70);
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
                cono.Technique = "RenderSceneCongelada";
                base.congelate();
            }
            #endregion
        }
    }