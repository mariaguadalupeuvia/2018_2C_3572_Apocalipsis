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

        public Explosivo(TgcMesh mesh, GameLogic logica, Planta planta)
        {
            this.logica = logica;
            crearBodyExplosivo(mesh.Position, 20);
            this.logica.addBulletObject(this);
            callback = new CollisionCallbackDisparo(logica, this);
            this.planta = planta;
        }

        public override void dañarZombie(Zombie zombie)
        {
            zombie.morir();
            logica.desactivar(planta);
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