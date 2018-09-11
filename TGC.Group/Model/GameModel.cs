
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

namespace TGC.Group.Model
{
    /// <summary>
    ///     Ejemplo para implementar el TP.
    ///     Inicialmente puede ser renombrado o copiado para hacer m�s ejemplos chicos, en el caso de copiar para que se
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

        List<GameObject> gameObjects = new List<GameObject>() { new Skybox(), new Terreno() };
        GameObject agua = new Agua(); //los objetos transparentes se renderean arriba de todo

        private PhysicsGame physicWorld = new PhysicsGame(); // este va a tener solo objetos colisionables
        //private Bullet prueba = new Bullet();
        private Gui.Gui gui = new Gui.Gui();
        private Escenario escenario = new Escenario();

        public static float time = 0.0f;
        public static string mediaDir;
        public static string shadersDir;

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
          //  physicWorld.addBulletObject(new Vida());
          //  physicWorld.addBulletObject(new Zombie());
            // prueba.Init();
            gui.Init();
            escenario.Init();
            agua.Init();
            #endregion

            Camara = new CamaraPersonal(new TGCVector3(1500f, 450f, 1500f), Input);
        }
    
        public override void Update()
        {
            PreUpdate();
            time += 0.003f;

            gameObjects.ForEach(g => g.Update());
            physicWorld.Update();
            //prueba.Update();
            agua.Update();

            PostUpdate();
        }

        public override void Render()
        {
            PreRender();
            
            gameObjects.ForEach(g => g.Render());
            escenario.Render();
            physicWorld.Render();
            // prueba.Render();
            agua.Render();
            gui.Render();
            PostRender();
        }

        public override void Dispose()
        {
            gameObjects.ForEach(g => g.Dispose());
            physicWorld.Dispose();
            //prueba.Dispose();
            agua.Dispose();
            gui.Dispose();
            escenario.Dispose();
        }

    }
}