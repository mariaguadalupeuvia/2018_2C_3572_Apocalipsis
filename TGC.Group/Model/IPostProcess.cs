﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model
{
    public interface IPostProcess
    {
         void cambiarTecnicaDefault();
         void cambiarTecnicaPostProceso();
         void Render();
    }
}
