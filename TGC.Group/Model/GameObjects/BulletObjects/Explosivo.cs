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
        public Explosivo(TgcMesh mesh, GameLogic logica, Planta planta)
        {
            crearBodyExplosivo(mesh.Position, 20);
            logica.addBulletObject(this);
            callback = new CollisionCallbackPlanta(logica, this);
            this.planta = planta;
        }

        public override void dañarZombie(Zombie zombie)
        {
            zombie.morir();
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