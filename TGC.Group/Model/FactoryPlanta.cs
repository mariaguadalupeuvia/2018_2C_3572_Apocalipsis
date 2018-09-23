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
        public static Planta crearCanion(TGCVector3 posicion, GameLogic logica)
        {
            return new Canion(posicion, logica);
        }
        public static Planta crearGirasol(TGCVector3 posicion, GameLogic logica)
        {
            return new Girasol(posicion, logica);
        }
        public static Planta crearCongelador(TGCVector3 posicion, GameLogic logica)
        {
            return new Congelador(posicion, logica);
        }
        public static Planta crearChile(TGCVector3 posicion, GameLogic logica)
        {
            return new Chile(posicion, logica);
        }
        public static Planta crearMina(TGCVector3 posicion, GameLogic logica)
        {
            return new Mina(posicion, logica);
        }

    }
}
