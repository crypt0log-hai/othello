using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjetOthello
{
    public static class GameParameter
    {
        public static BitmapImage[] tbtmTokenIndex = new BitmapImage[2];
        public static string[] tCharacterNames = new string[2];

        public static string[] tNameCharacter = { "Carrino", "Cortinovis", "Gorbel", "Husser", "Tieche", "Gobron", "Ouerhani" };

        public static Brush[] tColorBackgroundCell = { Brushes.White, Brushes.Red };

        public static List<Tuple<int, int[,], string>> lHistoryGame;

        public static int iNbCharacter = 7;
        public static int iSize = 8;
        public static int iGameMod = 0;
        public static int iNbTurn = 0;
        public static int iWinner = -1;
    }
}
