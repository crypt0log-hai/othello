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
        
        
        public int[,] TiBoard { get => tiBoard; set => tiBoard = value; }

        private Token currentToken;

        public GameBoard()
        {
            TiBoard = new int[iSize, iSize];
            currentToken = new Token(0,0);
        }

        /// <summary>
        /// For not tournament purpose only, update size of boards
        /// </summary>
        public void InitializationSolo(int iSize)
        {
            this.iSize = iSize;
            TiBoard = new int[iSize, iSize];
        }        

        public bool IsCellPlayable(int iActualPlayerId, int x, int y, ref Token token)
        {
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                    if (!(i == 0 && j == 0))
                    {
                        List<int[]> tempTarget = new List<int[]>();
                        FindAction(iActualPlayerId,j, i, x, y, ref tempTarget);
                        foreach (int[] coord in tempTarget) { token.LTokenCoordTarget.Add(coord); }
                    }
            if (token.LTokenCoordTarget.Count > 0)
            {
                token.IIsPlayable = true;
                return true;
            }
            return false;
        }

        private bool FindAction(int iActualPlayerId, int j, int i, int x, int y, ref List<int[]> tempTokenRefs)
        {
            x += j;
            y += i;
            bool blFindExtremis = false;
            if (x >= 0 && x < iSize)
                if (y >= 0 && y < iSize)
                {
                        if (tiBoard[x, y] == Tools.InverseBin(iActualPlayerId))
                        {
                            tempTokenRefs.Add(new int[] { x, y });
                            blFindExtremis = FindAction(iActualPlayerId,j, i, x, y, ref tempTokenRefs);
                        }
                        else if (tiBoard[x, y] == iActualPlayerId)
                            return true;
                        else
                            return false;
                }
            if (!blFindExtremis)
            {
                tempTokenRefs = new List<int[]>();
                return false;
            }
            return true;
        }
           
        
        #region IPlayable

        public string GetName()
        {
            return "Othello Vs He-Arc";
        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            int iActualPlayerId = Tools.IsWhiteToId(isWhite);
            currentToken.ResetTokenList();
            currentToken = new Token(column,line);
            if (tiBoard[column, line] == -1)
                return IsCellPlayable(iActualPlayerId,column,line, ref currentToken);
            return false;
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            int iActualPlayer = Tools.IsWhiteToId(isWhite);
            if (IsPlayable(column, line, isWhite))
            {
                tiBoard[column, line] = iActualPlayer;
                foreach(int[] coord in currentToken.lTokenCoordTarget)
                {
                    tiBoard[coord[0], coord[1]] = iActualPlayer;
                }
                return true;
            }
            return false;
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            
            return new Tuple<int, int>(-1, -1);
        }

        public int[,] GetBoard()
        {
            return TiBoard;
        }

        public int GetWhiteScore()
        {
            return tPlayerPoints[0];
        }

        public int GetBlackScore()
        {
            return tPlayerPoints[1];
        }

        #endregion

    }

}
