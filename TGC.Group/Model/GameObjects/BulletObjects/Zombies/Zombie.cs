﻿using BulletSharp;
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
    public class Zombie : BulletObject  
    {
        #region variables
        protected TgcMesh zombie;
        protected TgcMesh globo;
        protected float caidaFactor = 0;
        protected float velocidad = -7;
        protected float daño = 0;
        public string nombre;
        #endregion
        protected TgcBoundingAxisAlignBox boundingBox;

        private const int MAXIMO_DAÑO_SOPORTADO = 25;
        public TGCVector3 POSICION()
        {
            return zombie.Position;  
        }
        public Zombie(TGCVector3 posicion, GameLogic logica)
        {
            body = FactoryBody.crearBodyZombie(new TGCVector3(posicion.X, posicion.Y + 350, posicion.Z)); //(posicion);
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

            //Vector3 pmin, pmax;
            //body.GetAabb(out pmin, out pmax);
            //TGCVector3 tgcpmin = new TGCVector3(pmin.X, pmin.Y, pmin.Z);
            //TGCVector3 tgcpmax = new TGCVector3(pmax.X, pmax.Y, pmax.Z);
            //boundingBox = new TgcBoundingAxisAlignBox(tgcpmin, tgcpmax);
            //boundingBox.setRenderColor(Color.Red);
            ////Console.WriteLine("ZOMBIE PMIN " + zombie.BoundingBox.PMin + ", PMAX " + zombie.BoundingBox.PMax);

            //zombie.BoundingBox = boundingBox;
            
            //Console.WriteLine("BODY PMIN " + pmin + ", PMAX " + pmax);
            // zombie.updateBoundingBox();
        }

        public override void Update()
        {
            body.Translate(new Vector3(0, caidaFactor, velocidad));
           // zombie.updateBoundingBox();
        }

        public override void Render()
        {
            zombie.Position = new TGCVector3(body.InterpolationWorldTransform.M41, body.InterpolationWorldTransform.M42, body.InterpolationWorldTransform.M43);
            globo.Position = new TGCVector3(body.InterpolationWorldTransform.M41, body.InterpolationWorldTransform.M42 + 150, body.InterpolationWorldTransform.M43);

            //zombie.Transform = new TGCMatrix(body.InterpolationWorldTransform);
            //globo.Transform = new TGCMatrix(body.InterpolationWorldTransform);
            base.Render();

           // zombie.BoundingBox.Render();
            //boundingBox.Render();
            
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
        public virtual bool enCaidaLibre()
        {
            return (caidaFactor == -10);
        }

        public void morir()
        {
            caidaFactor = -10;
            GameLogic.cantidadZombiesMuertos++;
            //SACAR LOS MUERTOS DE LA LISTA DE ZOMBIES DEL LOGIC
            //Console.WriteLine("zombies muertos:" + GameLogic.cantidadZombiesMuertos);
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
        }
        public void empezaACaminar()
        {
            velocidad = -1;
        }

        public void llegaste()
        {
            velocidad = -1;
            //aca el zombie tiene que dejar el globo y empezar a caminar
          //  body.ApplyImpulse(new TGCVector3(0, 150, 0).ToBsVector, new TGCVector3(0, 20, 0).ToBsVector);
        }
        #endregion

    }
}