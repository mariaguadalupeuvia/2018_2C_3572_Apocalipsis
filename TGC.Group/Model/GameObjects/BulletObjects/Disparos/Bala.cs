using BulletSharp;
using BulletSharp.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Core.Textures;

namespace TGC.Group.Model.GameObjects.BulletObjects
{
    public class Bala : Disparo
    {
        public Bala(TgcMesh planta, GameLogic logica)
        {
            crearBody(planta.Position, planta.Rotation);
            logica.addBulletObject(this);
            callback = new CollisionCallbackDisparo(logica, this);
        }

        public override void dañarZombie(Zombie zombie)
        {
            zombie.recibirDaño();
        }
    }
}