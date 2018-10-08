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

        /*
        private Menu menu = new Menu();

        List<GameObject> gameObjects = new List<GameObject>() { new Skybox(), new Terreno(), new Escenario()};
        GameObject agua = new Agua(); //los objetos transparentes se renderean arriba de todo
        //private Bullet prueba = new Bullet();
        private Gui.Gui gui = new Gui.Gui();
        public GameLogic logica = new GameLogic();
        public ZombieRey zombieRey;
        */
        TgcCamera camaraAerea;

        public static float time = 0.0f;
        public static string mediaDir;
        public static string shadersDir;
        public static TgcFrustum frustum;
       // private TgcText2D text1;
        #endregion

        public override void Init()
        {
            var d3dDevice = D3DDevice.Instance.Device;

           
            //text1 = new TgcText2D();

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
            //estadoDelJuego = new Play();
            estadoDelJuego.Init(Input);

            //estadoDelJuego = estadoDelJuego.cambiarEstado();
            //estadoDelJuego.Init(Input);
            /*
            #region inicializarRendereables

            gameObjects.ForEach(g => g.Init());
            //prueba.Init();
            logica.Init(Input);
            agua.Init(); 
            gui.Init();

            zombieRey = new ZombieRey(new TGCVector3(700, 50, 6000), logica);
            logica.addBulletObject(zombieRey);

            #endregion

            menu.Init(Input);
            */
            //camaraAerea = new CamaraPersonal(new TGCVector3(1214, 950, 2526), Input);
            camaraAerea = new CamaraPersonal(new TGCVector3(171, 453, 577), Input);
            Camara = camaraAerea;
        }

        public override void Update()
        {
            PreUpdate();
            /*
            #region manejarCamara
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
            #endregion
            */
            time += 0.003f;
            if (time > 500) time = 0;
            frustum = Frustum;

            estadoDelJuego.Update(Input);
            /*
            #region update
            zombieRey.Update(Input);
            gameObjects.ForEach(g => g.Update());
           // prueba.Update();
            agua.Update();
            logica.Update(Input);
            text1.Text = "camara: (" + Camara.Position.X + ", " + Camara.Position.Y + ", " + Camara.Position.Z + ")";
            #endregion

            menu.Update(Input);
            */
            PostUpdate();
        }

        public override void Render()
        {
            PreRender();
            estadoDelJuego.Render();
            /*
            #region render
            gameObjects.ForEach(g => g.Render());
            //prueba.Render();
            logica.Render();
            agua.Render();
            gui.Render();
            text1.render();
            #endregion
            menu.Render(); 
            */
            PostRender();
        }

        public override void Dispose()
        {
            estadoDelJuego.Dispose();
            /*
            #region dispose
            gameObjects.ForEach(g => g.Dispose());
            //prueba.Dispose();
            agua.Dispose();
            logica.Dispose();
            gui.Dispose();
            text1.Dispose();
            #endregion
            menu.Dispose();
            */
        }
    }
}