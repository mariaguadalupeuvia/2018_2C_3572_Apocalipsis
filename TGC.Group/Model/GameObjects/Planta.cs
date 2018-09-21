using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using Microsoft.DirectX.DirectInput;
using TGC.Core.Input;
using TGC.Group.Model.GameObjects.BulletObjects;

namespace TGC.Group.Model.GameObjects
{
    public abstract class Planta
    {
        protected int nivelResistencia;
        protected int costoEnSoles;
        protected Microsoft.DirectX.Direct3D.Effect efecto;
        protected GamePhysics physicWorld;

        public void Init(GamePhysics physicWorld)
        {
            this.physicWorld = physicWorld;

            #region configurarEfecto
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            #endregion
        }

        public abstract void cambiarTecnicaShader(string tecnica);

        public abstract void Update(TgcD3dInput Input);
        public abstract void Render();
        public abstract void Dispose();
        public abstract int getCostoEnSoles();

    }
}