using BulletSharp;
using BulletSharp.Math;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
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
    public class Zombie : BulletObject, IPostProcess
    {
        #region variables
        protected TgcMesh zombie;
        protected TgcMesh globo;
        protected float caidaFactor = 0;
        protected float velocidad = -7;
        protected float subir = 350;
        protected float daño = 0;
        public string nombre;
        #endregion
     
        private const int MAXIMO_DAÑO_SOPORTADO = 25;

        public Zombie(TGCVector3 posicion, GameLogic logica)
        {
            body = FactoryBody.crearBodyZombie(new TGCVector3(posicion.X, posicion.Y + 350, posicion.Z)); 
            callback = new CollisionCallbackZombie(logica, this);

            #region configurarEfecto
            var d3dDevice = D3DDevice.Instance.Device;
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            //Texture bumpMap = TextureLoader.FromFile(d3dDevice, GameModel.mediaDir + "texturas\\terrain\\NormalMapNieve2.jpg");
            //efecto.SetValue("NormalMap", bumpMap);
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

            PostProcess.agregarPostProcessObject(this);
        }

        #region gestionarTecnicasShader
        public void cambiarTecnicaDefault()
        {
            globo.Technique = "RenderZombie";
            zombie.Technique = "RenderScene"; 
        }
        public void cambiarTecnicaPostProceso()
        {
            globo.Technique = "RenderZombie";
            zombie.Technique = "dark";
        }
        #endregion

        public override void Update()
        {
           body.Translate(new Vector3(0, caidaFactor, -1 + velocidad));
        }
        
        private int altura()
        {
            #region manejarAltura
            
            int x = (int)(zombie.Position.X / 255f);
            int z = (int)(zombie.Position.Z / 255f);
            int y = alturaEnPunto(x, z);

            if (y != -1)
            {
                return y;
            }
            else
            {
                if (z < 0) z = 0;
                if (z > 63) z = 63;
                if (x < 0) x = 0;
                if (x > 63) x = 63;
                return (int)zombie.Position.Y;
            }
            #endregion
        }

        private int alturaEnPunto(int x, int z)
        {
            return (int)(Terreno.alturaEnPunto(x + 32, z + 32) * 1.7f);
        }
        public override void Render()
        {
            if ((body.InterpolationWorldTransform.M42 < 400) && (caidaFactor != -10))
            {
                subir+= 0.8f;
                int y = altura();
                zombie.Position = new TGCVector3(body.InterpolationWorldTransform.M41/*zombie.Position.X*/, y/*430*/, body.InterpolationWorldTransform.M43);
                globo.Position = new TGCVector3(body.InterpolationWorldTransform.M41, body.InterpolationWorldTransform.M42 + subir /*+ 500*/, body.InterpolationWorldTransform.M43);

            }
            else
            {
                zombie.Position = new TGCVector3(zombie.Position.X, body.InterpolationWorldTransform.M42, body.InterpolationWorldTransform.M43);
                globo.Position = new TGCVector3(body.InterpolationWorldTransform.M41, body.InterpolationWorldTransform.M42 + 150, body.InterpolationWorldTransform.M43);
            }
            //zombie.Transform = new TGCMatrix(body.InterpolationWorldTransform);
            //globo.Transform = new TGCMatrix(body.InterpolationWorldTransform);

            //zombie.Position = new TGCVector3(body.InterpolationWorldTransform.M41, body.InterpolationWorldTransform.M42, body.InterpolationWorldTransform.M43);
            //globo.Position = new TGCVector3(body.InterpolationWorldTransform.M41, body.InterpolationWorldTransform.M42 + 150, body.InterpolationWorldTransform.M43);
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

            GameSound.hablar(); 
        }
        public virtual bool enCaidaLibre()
        {
            return (caidaFactor == -10);
        }

        public void morir()
        {
            caidaFactor = -10;
            GameLogic.cantidadZombiesMuertos++;

            //SACAR LOS MUERTOS DE LA LISTA DE ZOMBIES DEL LOGIC
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

        public void empezaAComer()
        {
            velocidad = 0;
            GameSound.hablar();
        }
        public void empezaACaminar()
        {
            velocidad = -1;
            body.ApplyImpulse(new TGCVector3(0, 0, velocidad).ToBsVector, new TGCVector3(0, 0, 0).ToBsVector);
        }

        public void llegaste()
        {
            velocidad = -1;
        }
        #endregion

        #region cosasPocoImportantes
        internal void avanza()
        {
            velocidad = -3;
        }
        public TGCVector3 POSICION()
        {
            return zombie.Position;
        }
        public TGCVector3 globoPosicion()
        {
            return globo.Position;
        }
        #endregion
    }
}