using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetOthello
{
    class GameBoard : IPlayable
    {
        private int[,] tiBoard;
        private int[] tPlayerPoints;
        private int iSize = 8;


        public int[,] TiBoard { set => tiBoard = value; }
        public int[] TPlayerPoints { set => tPlayerPoints = value; }
        public int ISize { set => iSize = value; }

        public string GetName()
        {
            return "Othello Vs He-Arc";
        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            int iActualPlayerId = (isWhite) ? 0 : 1;
            if (tiBoard[column, line] == -1)
            {
                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                        if (!(i == 0 && j == 0))
                            if (FindAction(j, i, column, line, iActualPlayerId))
                            {
                                return true;
                            }
            }
            return false;
        }

        private bool FindAction(int j, int i, int column, int line, int iActualPlayerId)
        {
            column += j;
            line += i;
            if (column >= 0 && column < iSize)
                if (line >= 0 && line < iSize)
                {
                    if (tiBoard[column, line] == InverseBin(iActualPlayerId))
                        return FindAction(j, i, column, line, tiBoard[column, line]);
                    else if (tiBoard[column, line] == iActualPlayerId)
                        return true;
                }

            return false;
        }


        private int InverseBin(int x) { return (x == 0) ? 1 : 0; }


        public bool PlayMove(int column, int line, bool isWhite)
        {
            if (IsPlayable(column, line, isWhite))
                return true;
            return false;
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            throw new NotImplementedException();
        }

        public int[,] GetBoard()
        {
            return tiBoard;
        }

        public int GetWhiteScore()
        {
            return tPlayerPoints[0];
        }

        public int GetBlackScore()
        {
            return tPlayerPoints[1];
        }

    }

}
