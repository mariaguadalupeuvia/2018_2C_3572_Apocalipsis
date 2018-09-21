﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX.Direct3D;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public abstract class GameObject 
    {
        protected Effect efecto;
        protected List<IRenderObject> objetos = new List<IRenderObject>();

        public abstract void Init();
        public abstract void Update();

        public virtual void Render()
        {
            objetos.ForEach(m => m.Render());
        }

        public virtual void Dispose()
        {
            objetos.ForEach(m => m.Dispose());
        }
    }
}
