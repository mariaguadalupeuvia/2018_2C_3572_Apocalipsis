using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX.DirectSound;
using TGC.Core.Sound;

namespace TGC.Group.Model
{
    public class GameSound
    {
        #region variables
        private static TgcStaticSound disparo;
        private static TgcStaticSound explosion;
        private static TgcStaticSound zombie;
        private TgcStaticSound musica;
        private TgcStaticSound piano;
        #endregion

        public GameSound(Device directSound)
        {
            #region inicializarSonidos
            disparo = new TgcStaticSound();
            string filePath = GameModel.mediaDir + "sonidos\\plop.wav";
            disparo.loadSound(filePath, directSound);

            explosion = new TgcStaticSound();
            filePath = GameModel.mediaDir + "sonidos\\explosion.wav";
            explosion.loadSound(filePath, directSound);

            zombie = new TgcStaticSound();
            filePath = GameModel.mediaDir + "sonidos\\zombie2.wav";
            zombie.loadSound(filePath, directSound);

            musica = new TgcStaticSound();
            filePath = GameModel.mediaDir + "sonidos\\musica.wav";
            musica.loadSound(filePath, directSound);
            musica.play(true);

            piano = new TgcStaticSound();
            filePath = GameModel.mediaDir + "sonidos\\piano.wav";
            piano.loadSound(filePath, directSound);
           // piano.play(true);
            #endregion
        }
        public void stop()
        {
            musica.stop();
        }
        public void play()
        {
            musica.play(true);
        }
        public static void disparar()
        {
            disparo.play(); 
        }
        public static void explotar()
        {
            explosion.play();
        }
        public static void hablar()
        {
            zombie.play();
        }
    }
}
