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

namespace TGC.Group.Model.GameObjects.BulletObjects.Disparos
{
    public class SuperBala : Disparo
    {
        public SuperBala(TgcMesh planta, GameLogic logica, float angulo)
        {
            body = FactoryBody.crearBodyConImpulsoDoble(planta.Position, radio, masa, planta.Rotation, angulo);
            logica.addBulletObject(this);
            callback = new CollisionCallbackDisparo(logica, this);
        }

        public override void dañarZombie(Zombie zombie)
        {
            zombie.recibirDaño();
        }
    }
}