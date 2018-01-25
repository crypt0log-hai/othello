using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Media;

namespace ProjetOthello
{
    class SoundEngine
    {
        SoundPlayer mediaPlayer;
        public SoundEngine(string musicPaths)
        {
            mediaPlayer = new SoundPlayer(musicPaths);
            if (GameParameter.isMusiqueEnabled)
            {
                mediaPlayer.Load();
                mediaPlayer.Play();
            }
        }   

        public void StopSound()
        {
            mediaPlayer.Stop();
            mediaPlayer.Dispose();
        }
    }
}
