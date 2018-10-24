using BulletSharp.Math;
using System;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;
using TGC.Group.Model.GameObjects.BulletObjects;

namespace TGC.Group.Model.GameObjects
{
    public class Explosivo : Disparo //este es para una mina
    {
        Planta planta;
        GameLogic logica;

        public Explosivo(TGCVector3 posicion, GameLogic logica, Planta planta)
        {
            this.logica = logica;
            body = FactoryBody.crearBodyExplosivo(posicion, 10);
            this.logica.addBulletObject(this);
            callback = new CollisionCallbackDisparo(logica, this);
            this.planta = planta;
        }

        public override void dañarZombie(Zombie zombie)
        {
            zombie.morir();
            planta.liberar(); 
        }

        public override void Render()
        {
            
        }

        public override void Dispose()
        {
            planta.Dispose();
            base.Dispose();
        }
    }
}