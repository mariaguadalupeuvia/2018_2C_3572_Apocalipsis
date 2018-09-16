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
    public class Canion
    {
        #region variables
        List<Bala> disparos = new List<Bala>();
        private TgcMesh canion;
        float axisRotation = 0;
        float ayisRotation = 0;
        private const float AXIS_ROTATION_SPEED = 0.02f;
        protected Microsoft.DirectX.Direct3D.Effect efecto;
        GamePhysics physicWorld;
        #endregion

        public void Init(GamePhysics world)
        {
            var d3dDevice = D3DDevice.Instance.Device;

            #region configurarEfecto
            efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderPlanta.fx");
            #endregion

            #region configurarObjeto

            canion = new TgcSceneLoader().loadSceneFromFile(GameModel.mediaDir + "modelos\\CANIONERO-TgcScene.xml").Meshes[0];
            canion.Scale = new TGCVector3(40.5f, 40.5f, 40.5f);
            canion.Position = new TGCVector3(500f, 420f, 1500f);
            canion.RotateX(90);
            canion.RotateY(90);
            canion.Effect = efecto;
            canion.Technique = "RenderScene";

            #endregion

            physicWorld = world;
        }

        public void disparar()
        {
            Bala disparo = new Bala(canion, physicWorld);
          
            disparo.init();
            physicWorld.addBulletObject(disparo);
            disparos.Add(disparo);  
        }

        public void update(TgcD3dInput Input)
        {
            #region chequearInput
            if (Input.keyDown(Key.UpArrow))
            {
                axisRotation -= AXIS_ROTATION_SPEED * GameModel.time;
            }
            if (Input.keyDown(Key.DownArrow))
            {
                axisRotation += AXIS_ROTATION_SPEED * GameModel.time;
            }

            if (Input.keyDown(Key.RightArrow))
            {
                ayisRotation -= AXIS_ROTATION_SPEED * GameModel.time;
            }
            if (Input.keyDown(Key.LeftArrow))
            {
                ayisRotation += AXIS_ROTATION_SPEED * GameModel.time;
            }
            if (Input.keyUp(Key.P))
            {
                disparar();
            }
            #endregion

            canion.RotateX(axisRotation);
            canion.RotateY(ayisRotation);

            axisRotation = 0;
            ayisRotation = 0;
        }

        public void Render()
        {
            canion.Render();
            disparos.ForEach(d => d.render());
        }

        public void Dispose()
        {
            canion.Dispose();
            disparos.ForEach(d => d.dispose());
        }
     }
}
