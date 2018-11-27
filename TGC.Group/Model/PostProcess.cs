using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Group.Model.EstadosJuego;

namespace TGC.Group.Model
{
    public class PostProcess
    {
        #region variables
        private Effect effect;
        private Surface depthStencil;
        private Texture renderTarget, renderTarget4, renderTargetGlow;
        private VertexBuffer vertexBuffer;

        private string shadersDir;
        GameModel gameModel;
        Device d3dDevice;
        int aux = 0;
        internal Play estado;
        #endregion

        #region variablesShadowMap
        private readonly int SHADOWMAP_SIZE = 1024;
        private readonly float far_plane = 50000f;
        private readonly float near_plane = 1f;

        private TGCVector3 lightDir; // direccion de la luz 
        private TGCVector3 lightPos; // posicion de la luz 
        private TGCMatrix lightView; // matriz de view del light
        private TGCMatrix projMatrix; // Projection matrix for shadow map
        private Surface stencilBuff; // Depth-stencil buffer for rendering to shadow map

        private Texture shadowTex; // Texture to which the shadow map is rendered
        #endregion

        static List<IPostProcess> postProcessObjects = new List<IPostProcess>();

        public static void agregarPostProcessObject(IPostProcess objeto)
        {
            postProcessObjects.Add(objeto);
        }

        //public void Update(float[] eyePosition)
        //{
        //    effect.SetValue("fvEyePosition", eyePosition); 
        //}

        public void Init(GameModel model, Device device)
        {
            this.d3dDevice = device;
            this.gameModel = model;

            #region configurarEfecto

            string compilationErrors;
            effect = Effect.FromFile(d3dDevice, GameModel.shadersDir + "PostProcess.fx",
                null, null, ShaderFlags.PreferFlowControl, null, out compilationErrors);
            Console.WriteLine(compilationErrors);
            effect.Technique = "DefaultTechnique";
            #endregion

            depthStencil = d3dDevice.CreateDepthStencilSurface(d3dDevice.PresentationParameters.BackBufferWidth,
                d3dDevice.PresentationParameters.BackBufferHeight,
                DepthFormat.D24S8, MultiSampleType.None, 0, true);

            #region inicializarRenderTargets
            renderTarget = new Texture(d3dDevice, d3dDevice.PresentationParameters.BackBufferWidth
                , d3dDevice.PresentationParameters.BackBufferHeight, 1, Usage.RenderTarget,
                Format.X8R8G8B8, Pool.Default);

            renderTarget4 = new Texture(d3dDevice, d3dDevice.PresentationParameters.BackBufferWidth / 4
                , d3dDevice.PresentationParameters.BackBufferHeight / 4, 1, Usage.RenderTarget,
                Format.X8R8G8B8, Pool.Default);

            renderTargetGlow = new Texture(d3dDevice, d3dDevice.PresentationParameters.BackBufferWidth / 4
                , d3dDevice.PresentationParameters.BackBufferHeight / 4, 1, Usage.RenderTarget,
                Format.X8R8G8B8, Pool.Default);
            #endregion

            #region setEffectValues
            effect.SetValue("g_RenderTarget", renderTarget);
            // Resolucion de pantalla
            effect.SetValue("screen_dx", d3dDevice.PresentationParameters.BackBufferWidth);
            effect.SetValue("screen_dy", d3dDevice.PresentationParameters.BackBufferHeight);
            #endregion

            #region inicializarVertexBuffer
            //Creamos un FullScreen Quad
            CustomVertex.PositionTextured[] vertices =
            {
                new CustomVertex.PositionTextured(-1, 1, 1, 0, 0),
                new CustomVertex.PositionTextured(1, 1, 1, 1, 0),
                new CustomVertex.PositionTextured(-1, -1, 1, 0, 1),
                new CustomVertex.PositionTextured(1, -1, 1, 1, 1)
            };
            //vertex buffer de los triangulos
            vertexBuffer = new VertexBuffer(typeof(CustomVertex.PositionTextured),
                4, d3dDevice, Usage.Dynamic | Usage.WriteOnly,
                CustomVertex.PositionTextured.Format, Pool.Default);
            vertexBuffer.SetData(vertices, 0, LockFlags.None);
            #endregion

            #region shadowMap
            shadowTex = new Texture(D3DDevice.Instance.Device, SHADOWMAP_SIZE, SHADOWMAP_SIZE, 1, Usage.RenderTarget, Format.R32F, Pool.Default);
            stencilBuff = D3DDevice.Instance.Device.CreateDepthStencilSurface(SHADOWMAP_SIZE, SHADOWMAP_SIZE, DepthFormat.D24S8, MultiSampleType.None, 0, true);

            var aspectRatio = D3DDevice.Instance.AspectRatio;
            projMatrix = TGCMatrix.PerspectiveFovLH(Geometry.DegreeToRadian(80), aspectRatio, 50, 5000);
            D3DDevice.Instance.Device.Transform.Projection = TGCMatrix.PerspectiveFovLH(Geometry.DegreeToRadian(45.0f), aspectRatio, near_plane, far_plane).ToMatrix();
            #endregion

            #region light
            lightPos = new TGCVector3(760, 520, 60);
            lightDir = TGCVector3.Empty - lightPos;
            lightDir.Normalize();
            lightView = TGCMatrix.LookAtLH(lightPos, lightPos + lightDir, new TGCVector3(0, 0, 1));
            #endregion
        }

        private void pasada(string technique, string map, Texture renderTarget, int indice)
        {
            #region pasada
            effect.Technique = technique;
            d3dDevice.VertexFormat = CustomVertex.PositionTextured.Format;
            d3dDevice.SetStreamSource(0, vertexBuffer, 0);
            effect.SetValue(map, renderTarget);
            effect.SetValue("g_GlowMap", renderTargetGlow);

            d3dDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            effect.Begin(FX.None);
            effect.BeginPass(indice);
            d3dDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            effect.EndPass();
            effect.End();
            #endregion
        }

        public void Render()
        {
            gameModel.clearTextures();

            if (!gameModel.postProcessActivo)
            {
                D3DDevice.Instance.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
                RenderShadowMap();
                D3DDevice.Instance.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
                renderNormal();
                return;
            }

            #region renderTarget
            effect.Technique = "DefaultTechnique";

            // guardo el Render target anterior y seteo la textura como render target
            var renderTargetAnterior = d3dDevice.GetRenderTarget(0);
            var SurfaceLevel = renderTarget.GetSurfaceLevel(0);
            d3dDevice.SetRenderTarget(0, SurfaceLevel);
            SurfaceLevel.Dispose();

            // hago lo mismo con el depthbuffer
            var depthStencilAnterior = d3dDevice.DepthStencilSurface;
            d3dDevice.DepthStencilSurface = depthStencil;
            d3dDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            #endregion

            renderNormal();

            SurfaceLevel = renderTarget4.GetSurfaceLevel(0);
            d3dDevice.SetRenderTarget(0, SurfaceLevel);
            pasada("filter4", "g_RenderTarget", renderTarget, 0);

            renderGlow();

            SurfaceLevel.Dispose();
            d3dDevice.DepthStencilSurface = depthStencilAnterior;

            SurfaceLevel = renderTargetGlow.GetSurfaceLevel(0);
            d3dDevice.SetRenderTarget(0, SurfaceLevel);
            pasada("blur", "g_RenderTarget", renderTarget4, 0);

            // Ultima pasada va sobre la pantalla pp dicha
            d3dDevice.SetRenderTarget(0, renderTargetAnterior);
            pasada("separable", "g_RenderTarget", renderTarget, 0);

        }

        public void RenderShadowMap()
        {
            // Primero genero el shadow map, para ello dibujo desde el pto de vista de luz
            // a una textura, con el VS y PS que generan un mapa de profundidades.
            var pOldRT = D3DDevice.Instance.Device.GetRenderTarget(0);
            var pShadowSurf = shadowTex.GetSurfaceLevel(0);
            D3DDevice.Instance.Device.SetRenderTarget(0, pShadowSurf);
            var pOldDS = D3DDevice.Instance.Device.DepthStencilSurface;
            D3DDevice.Instance.Device.DepthStencilSurface = stencilBuff;
            D3DDevice.Instance.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);

            // Hago el render de la escena pp dicha
            //efecto.SetValue("g_txShadow", shadowTex);
            renderShadow(shadowTex);

            // restuaro el render target y el stencil
            D3DDevice.Instance.Device.DepthStencilSurface = pOldDS;
            D3DDevice.Instance.Device.SetRenderTarget(0, pOldRT);
        }

        void renderGlow()
        {
            postProcessObjects.ForEach(ppo => ppo.cambiarTecnicaPostProceso());
            postProcessObjects.ForEach(ppo => ppo.Render());
        }

        void renderNormal()
        {
            postProcessObjects.ForEach(ppo => ppo.cambiarTecnicaDefault());
            gameModel.renderPostProcess();
            estado.renderPostProcess();//hace el render de los objetos de la escena

        }
        void renderShadow(Texture shadowTex)
        {
            postProcessObjects.ForEach(ppo => ppo.cambiarTecnicaShadow(shadowTex));
            gameModel.renderPostProcess();
            estado.renderPostProcess();//hace el render de los objetos de la escena
        }

        public void Dispose()
        {
            effect.Dispose();
            renderTarget.Dispose();
            renderTargetGlow.Dispose();
            renderTarget4.Dispose();
            vertexBuffer.Dispose();
            depthStencil.Dispose();

            shadowTex.Dispose();
            stencilBuff.Dispose();
        }
    }
}