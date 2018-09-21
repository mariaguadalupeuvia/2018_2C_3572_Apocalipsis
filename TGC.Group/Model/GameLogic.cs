using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Group.Model.GameObjects;
using TGC.Group.Model.GameObjects.BulletObjects;

namespace TGC.Group.Model
{
    public class GameLogic
    {
        #region variables
        public static int cantidadEnergia = 2000;
        private GamePhysics physicWorld;
        List<Planta> plantas = new List<Planta>();
        private Picking picker = new Picking();
        TGCVector3 posicionSeleccionada;
        #endregion

        public void Init( GamePhysics physicWorld, TgcD3dInput Input)
        {
            this.physicWorld = physicWorld;
            picker.Init(Input);
        }

        private void addPlanta(Planta unaPlanta)
        {
            if (unaPlanta.getCostoEnSoles() > cantidadEnergia)
            {
                unaPlanta.cambiarTecnicaShader("Transparente");
            }
            else
            {
                cantidadEnergia -= unaPlanta.getCostoEnSoles();
            }

            plantas.Add(unaPlanta);
        }

        public void Update(TgcD3dInput Input)
        {
            posicionSeleccionada = picker.Update(Input);

            #region chequearInput
            if (Input.keyDown(Key.NumPad1))
            {
                addPlanta(FactoryPlanta.crearCanion(physicWorld, posicionSeleccionada));// new TGCVector3(200f, 420f, 1500f)));
            }
            if (Input.keyDown(Key.NumPad2))
            {
                addPlanta(FactoryPlanta.crearGirasol(physicWorld, posicionSeleccionada));// new TGCVector3(500f, 420f, 1300f)));
            }
            if (Input.keyDown(Key.NumPad3))
            {
                addPlanta(FactoryPlanta.crearChile(physicWorld, posicionSeleccionada));//new TGCVector3(700f, 220f, 1500f)));
            }
            if (Input.keyDown(Key.NumPad4))
            {
                addPlanta(FactoryPlanta.crearMina(physicWorld, posicionSeleccionada));// new TGCVector3(500f, 220f, 1700f)));
            }
            if (Input.keyDown(Key.NumPad5))
            {
                addPlanta(FactoryPlanta.crearCongelador(physicWorld, posicionSeleccionada));// new TGCVector3(200f, 420f, 1800f)));
            }
            if (Input.keyDown(Key.Z))//esto hacerlo automatico segun el tiempo trancurrido
            {
                crearZombies();
            }
            #endregion

            plantas.ForEach(P => P.Update(Input));
        }

        public void Render()
        {
            picker.Render();
            plantas.ForEach(P => P.Render());
        }

        public void Dispose()
        {
            plantas.ForEach(P => P.Dispose());
            picker.Dispose();
        }

        private void crearZombies()
        {
            physicWorld.addBulletObject(new Zombie(new TGCVector3(1000f, 900f, 5200f), physicWorld));
            physicWorld.addBulletObject(new Zombie(new TGCVector3(1500f, 700f, 5000f), physicWorld));
            physicWorld.addBulletObject(new Zombie(new TGCVector3(1860f, 900f, 5400f), physicWorld));
            physicWorld.addBulletObject(new Zombie(new TGCVector3(2000f, 1000f, 4800f), physicWorld));
        }
    }
}
