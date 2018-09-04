using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX.Direct3D;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public abstract class GameObject : IRenderObject
    {
        protected Effect efecto;
        protected IRenderObject objeto;

        bool IRenderObject.AlphaBlendEnable { get => false; set => objeto.AlphaBlendEnable = value; }

        public abstract void Init();
        public abstract void Update();

        public virtual void Render()
        {
            objeto.Render();
        }

        public virtual void Dispose()
        {
            objeto.Dispose();
        }
    }
}
