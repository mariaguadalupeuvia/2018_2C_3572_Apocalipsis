using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Input;

namespace TGC.Group.Model.EstadosJuego
{
    public interface Estado
    {
        void Init(TgcD3dInput input);
        void Update(TgcD3dInput Input);
        void Render();
        void Dispose();
        void cambiarEstado(TgcD3dInput Input);
    }
}
