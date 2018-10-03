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
            body = FactoryBody.crearBodyConImpulso(planta.Position, radio, masa, planta.Rotation);
            logica.addBulletObject(this);
            callback = new CollisionCallbackDisparo(logica, this);
        }

        public override void dañarZombie(Zombie zombie)
        {
            zombie.congelate();
        }
    }
}
