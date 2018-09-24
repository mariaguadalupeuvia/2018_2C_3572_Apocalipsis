using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model.GameObjects.BulletObjects
{
    class BalaCongelante : Disparo
    {
        public BalaCongelante(TgcMesh planta, GameLogic logica)
        {
            crearBody(planta.Position, planta.Rotation);
            logica.addBulletObject(this);
            callback = new CollisionCallbackPlanta(logica, this);
        }

        public override void dañarZombie(Zombie zombie)
        {
            zombie.congelate();
        }
    }
}
