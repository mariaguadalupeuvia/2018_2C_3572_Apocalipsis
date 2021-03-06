﻿using BulletSharp.Math;
using System;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;
using TGC.Group.Model.GameObjects.BulletObjects;

namespace TGC.Group.Model.GameObjects
{
    public class Sol : Disparo
    {
        public Sol(TgcMesh girasol, GameLogic logica)
        {
            masa = 10.0f;
            body = FactoryBody.crearBodyConImpulso(girasol.Position, radio, masa, new TGCVector3(1, 2, 1));
            logica.addBulletObject(this);
            callback = new CollisionCallbackFloor(logica, this);
            GameLogic.cantidadEnergia += 50;
        }

        public override void dañarZombie(Zombie zombie)
        {
            //el sol no daña ni colisiona con zombie :)
            zombie.avanza();
        }

        public override void Render()
        {
            //el body muere antes al colisionar y tira exception
            body.Translate(new Vector3(1, -3, -10));
            esfera.Transform = TGCMatrix.Scaling(15, 15, 15) * new TGCMatrix(body.InterpolationWorldTransform);
            esfera.Render();
        }

        public void agrandarMesh()
        {
            esfera.Scale = new TGCVector3(85, 85, 85);
        }
    }
}