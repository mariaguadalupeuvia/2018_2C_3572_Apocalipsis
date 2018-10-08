using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Input;

namespace TGC.Group.Model.EstadosJuego
{
    public class Menu : Estado
    {
        public void cambiarEstado(TgcD3dInput Input)
        {
            Estado estado = new Play();
            estado.Init(Input);
            GameModel.estadoDelJuego = estado;
        }

        public void Dispose()
        {  
        }

        public void Init(TgcD3dInput input)
        { 
        }

        public void Render()
        { 
        }

        public void Update(TgcD3dInput Input)
        {
            cambiarEstado(Input);
        }
    }
}
