using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Media;

namespace ProjetOthello
{
    class SoundEngine
    {
        MediaPlayer mediaPlayer;
        public SoundEngine(string musicPaths)
        {
            mediaPlayer = new MediaPlayer();
            mediaPlayer.Open();
        }   
    }
}
