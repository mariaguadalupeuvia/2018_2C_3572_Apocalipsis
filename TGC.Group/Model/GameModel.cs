using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Mathematica;
using TGC.Core.Shaders;
using TGC.Core.Terrain;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TGC.Core.SceneLoader;
using TGC.Core.Camara;
using TGC.Group.Model.GameObjects;
using TGC.Group.Model.Gui;
using TGC.Group.Model.GameObjects.BulletObjects;
using System.Collections.Generic;
using System;
using Microsoft.DirectX.DirectInput;
using TGC.Core.BoundingVolumes;
using TGC.Core.Text;
using TGC.Group.Model.GameObjects.BulletObjects.Zombies;
using TGC.Group.Model.EstadosJuego;
using TGC.Core;

namespace TGC.Group.Model
{
    /// <summary>
    ///     Ejemplo para implementar el TP.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar el modelo que instancia GameForm <see cref="Form.GameForm.InitGraphics()" />
    ///     line 97.
    /// </summary>
     
    public class GameModel : TgcExample
    {
        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        /// <param name="mediaDir">Ruta donde esta la carpeta con los assets</param>
        /// <param name="shadersDir">Ruta donde esta la carpeta con los shaders</param>
        
        public GameModel(string mediaDir, string shadersDir) : base(mediaDir, shadersDir)
        {
            Category = Game.Default.Category;
            Name = Game.Default.Name;
            Description = Game.Default.Description;
        }

        #region variables
        public static Estado estadoDelJuego;
        public static bool enPlay = false;
        TgcCamera camaraAerea;

        public static float time = 0.0f;
        public static string mediaDir;
        public static string shadersDir;
        public static TgcFrustum frustum;
        #endregion

        HighResolutionTimer timer= new HighResolutionTimer();

        public override void Init()
        {
            #region devices
            var d3dDevice = D3DDevice.Instance.Device;
            var deviceSound = DirectSound.DsDevice;
            #endregion

            #region variablesDeClase
            frustum = Frustum;
            mediaDir = MediaDir;
            shadersDir = ShadersDir;
            #endregion

            #region frustum  //alejar far plane
            d3dDevice.Transform.Projection = Matrix.PerspectiveFovLH(Geometry.DegreeToRadian(45.0f),
                (float)d3dDevice.CreationParameters.FocusWindow.Width / d3dDevice.CreationParameters.FocusWindow.Height, 1f, 50000f);
            #endregion

            estadoDelJuego = new Inicial();
            estadoDelJuego.Init(Input);
            GameSound sonido = new GameSound(deviceSound);

            camaraAerea = new CamaraPersonal(new TGCVector3(1214, 1050, 2526), Input);
           // camaraAerea = new CamaraPersonal(new TGCVector3(171, 453, 577), Input);
            Camara = camaraAerea;
        }
        
        public override void Update()
        {
            PreUpdate();
            // timer.FramesPerSecond;

            time +=0.0003f;//0.003f;

            if (time > 500) time = 0;
            frustum = Frustum;

            estadoDelJuego.Update(Input);

            #region manejarCamara
            if (enPlay)
            {
                //3ra persona
                if (Input.keyDown(Key.C))
                {
                    Camara = ZombieRey.activarCamaraInterna();
                }
                //aerea
                if (Input.keyDown(Key.V))
                {
                    ZombieRey.desactivarCamaraInterna();
                    Camara = camaraAerea;
                }
            }
            #endregion

            PostUpdate();
        }

        public override void Render()
        {
            PreRender();
            estadoDelJuego.Render();
            PostRender();
        }

        public override void Dispose()
        {
            estadoDelJuego.Dispose();
        }
    }
}