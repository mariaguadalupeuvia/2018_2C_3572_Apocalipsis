using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace TGC.Group.Model
{
    public interface IPostProcess
    {
        void cambiarTecnicaShadow(Microsoft.DirectX.Direct3D.Texture shadowTex);
        void cambiarTecnicaDefault();
        void cambiarTecnicaPostProceso();
        void Render();
        void efectoSombra(TGCVector3 lightDir, TGCVector3 lightPos, TGCMatrix lightView, TGCMatrix projMatrix);
    }
}
