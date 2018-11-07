using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Group.Model.GameObjects;
using TGC.Group.Model.GameObjects.BulletObjects;

namespace TGC.Group.Model.EstadosJuego
{
    public class Glowsy : Estado
    {
        List<GameObject> gameObjects = new List<GameObject>() { new Skybox(), new Terreno() };
        protected TgcMesh zombie;
        protected TgcMesh globo;
        protected Effect efecto;
        PostProcess postProcess;

        public Glowsy(PostProcess postProcess)
        {
           // postProcess.estado = this; 
            this.postProcess = postProcess;
        }

        public void cambiarEstado(TgcD3dInput Input)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            gameObjects.ForEach(g => g.Dispose());
            zombie.Dispose();
            globo.Dispose();  
        }

        public void Init(TgcD3dInput input)
        {
            gameObjects.ForEach(g => g.Init());

            #region configurarEfecto
            var d3dDevice = D3DDevice.Instance.Device;
           
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            #endregion

            #region configurarObjeto
            zombie = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\Zombie7-TgcScene.xml").Meshes[0];
            zombie.Scale = new TGCVector3(55.5f, 55.5f, 55.5f);
            zombie.Position = new TGCVector3(0, 360, 0);
            zombie.Effect = efecto;
            zombie.Technique = "RenderScene";

            globo = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\GLOBO-TgcScene.xml").Meshes[0];
            globo.Scale = new TGCVector3(40.5f, 40.5f, 40.5f);
            globo.Position = new TGCVector3(0, 760, 0); ;
            globo.Effect = efecto;
            globo.Technique = "RenderZombie";

            #endregion

        }

        public void Render()
        {
            postProcess.Render();
        }

        public void renderPostProcess()
        {
            gameObjects.ForEach(g => g.Render());
            
            zombie.Render();
            globo.Render();
        }

        public void Update(TgcD3dInput Input)
        {
            gameObjects.ForEach(g => g.Update()); 
        }

        public void RenderGlow()
        {
            zombie.Render();
            globo.Render();
        }
    }
}
