using BulletSharp;
using BulletSharp.Math;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Core.Terrain;
using TGC.Core.Textures;

namespace TGC.Group.Model.GameObjects.BulletObjects
{
    public class Zombie : BulletObject  
    {
        #region variables
        protected TgcMesh zombie;
        protected TgcMesh globo;
        protected float caidaFactor = 0;
        protected float velocidad = -6;
        protected float daño = 0;
        private const int MAXIMO_DAÑO_SOPORTADO = 25;
        #endregion

        public Zombie(TGCVector3 posicion, GameLogic logica)
        {
            crearBodyZombie(new TGCVector3(posicion.X, posicion.Y + 150, posicion.Z)); //(posicion);
            callback = new CollisionCallbackZombie(logica, this);
            
            #region configurarEfecto
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            #endregion

            #region configurarObjeto
            zombie = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Zombie7-TgcScene.xml").Meshes[0];
            zombie.Scale = new TGCVector3(55.5f, 55.5f, 55.5f);
            zombie.Position = posicion;
            zombie.Effect = efecto;
            zombie.Technique = "RenderScene";

            objetos.Add(zombie);
           
            globo = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\GLOBO-TgcScene.xml").Meshes[0];
            globo.Scale = new TGCVector3(40.5f, 40.5f, 40.5f);
            globo.Position = posicion;
            globo.Effect = efecto;
            globo.Technique = "RenderZombie";

            objetos.Add(globo);
            #endregion
        }

        public override void Update()
        {
            body.Translate(new Vector3(0, caidaFactor, velocidad));
            efecto.SetValue("_Time", GameModel.time);
        }

        public override void Render()
        {
            zombie.Position = new TGCVector3(body.InterpolationWorldTransform.M41, body.InterpolationWorldTransform.M42, body.InterpolationWorldTransform.M43);
            globo.Position = new TGCVector3(body.InterpolationWorldTransform.M41, body.InterpolationWorldTransform.M42 + 150, body.InterpolationWorldTransform.M43);

            //zombie.Transform = new TGCMatrix(body.InterpolationWorldTransform);
            //globo.Transform = new TGCMatrix(body.InterpolationWorldTransform);
            base.Render();
        }

        #region respuestaAAtaqueDePlanta
        public virtual void recibirDaño()
        {
            daño+= 1;
            efecto.SetValue("colorVida", daño * 0.1f); //cambia el color del globo

            if(daño == MAXIMO_DAÑO_SOPORTADO) //despues de 25 balazos quedas en caida libre, cuando tocas el piso vas a dispose
            {
                morir();
            }
        }

        public void morir()
        {
            caidaFactor = -10;
            GameLogic.cantidadZombiesMuertos++;
            Console.WriteLine("zombies fritos:" + GameLogic.cantidadZombiesMuertos);
        }

        public void teGolpearon(Disparo disparo)
        {
            disparo.dañarZombie(this);
        }

        public virtual void congelate()
        {
            globo.Technique = "RenderSceneCongelada";
            zombie.Technique = "RenderSceneCongelada";
            velocidad = -1;
        }
        #endregion

    }
}