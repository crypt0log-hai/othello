using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetOthello
{
    public static class Tools
    {
        public static int InverseBin(int x) { return (x == 0) ? 1 : 0; }         //Inverse the  binnary value
        public static int IsWhiteToId(bool x) { return (x) ? 0 : 1; }            //IsWhite from IPlayable to ActualPlayerId
        public static bool IdToIsWhite(int x) { return (x == 0) ? true : false;} //ActualPlayerId to IsWhite from IPlayable
        
      
    }
}
