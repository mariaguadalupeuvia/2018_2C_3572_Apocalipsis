using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Group.Model.GameObjects;

namespace TGC.Group.Model
{
    public static class FactoryPlanta
    {
        public static Planta crearCanion(GamePhysics world, TGCVector3 posicion)
        {
            return new Canion(world, posicion);
        }
        public static Planta crearGirasol(GamePhysics world, TGCVector3 posicion)
        {
            return new Girasol(world, posicion);
        }
        public static Planta crearCongelador(GamePhysics world, TGCVector3 posicion)
        {
            return new Congelador(world, posicion);
        }
        public static Planta crearChile(GamePhysics world, TGCVector3 posicion)
        {
            return new Chile(world, posicion);
        }
        public static Planta crearMina(GamePhysics world, TGCVector3 posicion)
        {
            return new Mina(world, posicion);
        }

    }
}
