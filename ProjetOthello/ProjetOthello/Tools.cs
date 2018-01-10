using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProjetOthello
{
    public static class Tools
    {
        public static int InverseBin(int x) { return (x == 0) ? 1 : 0; }
        public static int IsWhiteToId(bool x) { return (x) ? 0 : 1; }
        public static bool IdToIsWhite(int x) { return (x == 0) ? true : false;}



        public static void UidToIJ(Button btn, ref int j, ref int i)
        {
            string[] strUid = btn.Uid.Split(';');
            try { j = Convert.ToInt32(strUid[0]); i = Convert.ToInt32(strUid[1]); }
            catch { Console.WriteLine("Btn.Uid is not integer."); }
        }
    }
}
