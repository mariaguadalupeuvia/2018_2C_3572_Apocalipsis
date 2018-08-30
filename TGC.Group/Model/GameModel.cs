
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.Shaders;
using TGC.Core.Terrain;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

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

        TgcSimpleTerrain terrain;
        TgcSkyBox skybox;

        public override void Init()
        {
            var d3dDevice = D3DDevice.Instance.Device;

            //agrandar frustum far plane
            d3dDevice.Transform.Projection = Matrix.PerspectiveFovLH(Geometry.DegreeToRadian(45.0f),
                (float)d3dDevice.CreationParameters.FocusWindow.Width / d3dDevice.CreationParameters.FocusWindow.Height, 1f, 50000f);

            //creo el cielo
            skybox = new TgcSkyBox();
            configurarSkybox();

            //creo el terreno
            terrain = new TgcSimpleTerrain();
            configurarTerrain();

            //creo la camara
            Camara = new CamaraPersonal(new TGCVector3(1500f, 450f, 1500f), Input);
        }

        public override void Update()
        {
            PreUpdate();
            
            PostUpdate();
        }

        public override void Render()
        {
            PreRender();

            skybox.Render();
            terrain.Render();
            
            PostRender();
        }

        public override void Dispose()
        {
            skybox.Dispose();
            terrain.Dispose();
        }

        public void configurarSkybox()
        {
            skybox.Center = TGCVector3.Empty;
            skybox.Size = new TGCVector3(50000, 10000, 50000);

            var texturesPath = MediaDir + "texturas\\skybox\\";

            //Configurar las texturas para cada una de las 6 caras
            skybox.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "up1.jpg");
            skybox.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "down1.jpg");
            skybox.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "left1.jpg");
            skybox.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "rigth1.jpg");
            skybox.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "front1.jpg");
            skybox.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "back1.jpg");
            skybox.SkyEpsilon = 25f;

            skybox.Init();
        }

        public void configurarTerrain()
        {
            terrain.Effect = TgcShaders.loadEffect(ShadersDir + "shaderTierra.fx"); ;
            terrain.Technique = "normal";
            terrain.loadTexture(MediaDir + "texturas\\terrain\\TerrainTexture3.jpg");
            terrain.loadHeightmap(MediaDir + "texturas\\terrain\\Heightmap3.jpg", 255f, 2.5f, new TGCVector3(0, 0, 0));
        }
    }
}