using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.Shaders;
using TGC.Core.Terrain;

namespace TGC.Group.Model.GameObjects
{
    public class Skybox : GameObject
    {
        SkyboxShader skybox = new SkyboxShader();

        public override void Init()
        {
            var d3dDevice = D3DDevice.Instance.Device;

            //#region configurarEfecto
            // //por ahora no uso para nada el efecto pero ya va a servir
            //efecto = TgcShaders.loadEffect(GameModel.shadersDir + "shaderCielo.fx");
            //#endregion

            #region configurarObjeto

            //skybox.efecto = efecto;
            //skybox.tecnica = "RenderScene";
            skybox.Center = TGCVector3.Empty;

            skybox.Size = new TGCVector3(50000, 10000, 50000);

            var texturesPath = GameModel.mediaDir + "texturas\\skybox\\";

            //Configurar las texturas para cada una de las 6 caras
            skybox.setFaceTexture(SkyboxShader.SkyFaces.Up, texturesPath + "up1.jpg");
            skybox.setFaceTexture(SkyboxShader.SkyFaces.Down, texturesPath + "down1.jpg");
            skybox.setFaceTexture(SkyboxShader.SkyFaces.Left, texturesPath + "left1.jpg");
            skybox.setFaceTexture(SkyboxShader.SkyFaces.Right, texturesPath + "rigth1.jpg");
            skybox.setFaceTexture(SkyboxShader.SkyFaces.Front, texturesPath + "front1.jpg");
            skybox.setFaceTexture(SkyboxShader.SkyFaces.Back, texturesPath + "back1.jpg");
            skybox.SkyEpsilon = 25f;

            skybox.Init();
            objetos.Add(skybox);
            #endregion
        }

        public override void Update()
        {
            //efecto.SetValue("_Time", GameModel.time);
        }

        public void cambiarTechnique(string technique)
        {
            skybox.cambiarTechnique(technique);
        }
    }
}
