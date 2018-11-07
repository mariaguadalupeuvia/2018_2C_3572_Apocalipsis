using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulletSharp;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.Textures;
using TGC.Group.Model.GameObjects.BulletObjects;
using Microsoft.DirectX.Direct3D;
using BulletSharp.Math;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class BulletObject : GameObject
    {
        #region variables
        public RigidBody body { get; set; }
        public ContactResultCallback callback { get; set; }

        public float radio = 10;
        public float masa = 0.1f;
        #endregion

        public override void Init()
        {
        }

        public override void Dispose()
        {
            body.Dispose();
            base.Dispose();
        }

        public override void Update()
        {
        }

        public virtual bool enCaidaLibre()
        {
            return false;
        }

    }
}
