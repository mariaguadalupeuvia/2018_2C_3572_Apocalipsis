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
        List<GameObject> gameObjects = new List<GameObject>() { new Skybox(), new Terreno() , new Escenario()};
        GameObject agua = new Agua(); //los objetos transparentes se renderean arriba de todo

        private GamePhysics physicWorld = new GamePhysics(); // este va a tener solo objetos colisionables
        //private Bullet prueba = new Bullet();
        private Gui.Gui gui = new Gui.Gui();
      //  private Escenario escenario = new Escenario();
        public GameLogic logica = new GameLogic();

        public static float time = 0.0f;
        public static string mediaDir;
        public static string shadersDir;

        
        #endregion

        public override void Init()
        {
            var d3dDevice = D3DDevice.Instance.Device;

            #region variablesDeClase

            mediaDir = MediaDir;
            shadersDir = ShadersDir;

            #endregion

            #region frustum  //alejar far plane
            d3dDevice.Transform.Projection = Matrix.PerspectiveFovLH(Geometry.DegreeToRadian(45.0f),
                (float)d3dDevice.CreationParameters.FocusWindow.Width / d3dDevice.CreationParameters.FocusWindow.Height, 1f, 50000f);

            #endregion

            #region inicializarRendereables

            gameObjects.ForEach(g => g.Init());
            physicWorld.Init();
           // physicWorld.addBulletObject(new Caja(physicWorld));
            //prueba.Init();
            gui.Init();
         //   escenario.Init();
            agua.Init();
            logica.Init(physicWorld, Input);

            #endregion

            Camara = new CamaraPersonal(new TGCVector3(1500f, 450f, 1500f), Input);
        }

        public override void Update()
        {
            PreUpdate();

            #region update
            time += 0.003f;
            gameObjects.ForEach(g => g.Update());
            physicWorld.Update();
           // escenario.Update();
           // prueba.Update();
            agua.Update();
            logica.Update(Input);
            #endregion

            PostUpdate();
        }

        public override void Render()
        {
            PreRender();

            #region render
            gameObjects.ForEach(g => g.Render());
            physicWorld.Render();
           // escenario.Render();
            //prueba.Render();
            logica.Render();
            agua.Render();
            
            gui.Render();
            #endregion

            PostRender();
        }

        public override void Dispose()
        {
            #region dispose
            gameObjects.ForEach(g => g.Dispose());
            physicWorld.Dispose();
            //prueba.Dispose();
            agua.Dispose();
            logica.Dispose();
            gui.Dispose();
          //  escenario.Dispose();
            #endregion
        }

    }
}