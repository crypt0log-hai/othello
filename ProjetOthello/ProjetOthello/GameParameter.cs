using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ProjetOthello
{
    public static class GameParameter
    {
        public static BitmapImage[] imageIndex = new BitmapImage[2];

        public static string[] tNameCharacter = { "Carrino", "Cortinovis", "Gorbel", "Husser", "Tieche" };

        public static int iSize = 8;
        public static int iStateGame = 0;
    }
}
