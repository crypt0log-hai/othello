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
        private Token currentToken;
        private List<Token> lTokenPlayable;


        public int[,] TiBoard { get => tiBoard; set => tiBoard = value; }
        public int[] TPlayerPoints { get => tPlayerPoints; set => tPlayerPoints = value; }


        public GameBoard()
        {
            TiBoard = new int[iSize, iSize];
            tPlayerPoints = new int[2];
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
                        FindAction(tiBoard,iActualPlayerId,j, i, x, y, ref tempTarget);
                        foreach (int[] coord in tempTarget) { token.LTokenCoordTarget.Add(coord); }
                    }
            if (token.LTokenCoordTarget.Count > 0)
            {
                token.IIsPlayable = true;
                return true;
            }
            return false;
        }

        public bool IsCellPlayable(int[,] board, int iActualPlayerId, int x, int y, ref Token token)
        {
            if (board[x, y] == -1)
            {
                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                        if (!(i == 0 && j == 0))
                        {
                            List<int[]> tempTarget = new List<int[]>();
                            FindAction(board, iActualPlayerId, j, i, x, y, ref tempTarget);
                            foreach (int[] coord in tempTarget) { token.LTokenCoordTarget.Add(coord); }
                        }
                if (token.LTokenCoordTarget.Count > 0)
                {
                    token.IIsPlayable = true;
                    return true;
                }
            }
            return false;
        }


        private bool FindAction(int[,] board, int iActualPlayerId, int j, int i, int x, int y, ref List<int[]> tempTokenRefs)
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
                            blFindExtremis = FindAction(board,iActualPlayerId,j, i, x, y, ref tempTokenRefs);
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
           
        public void ComputeScore()
        {
            tPlayerPoints[0] = tPlayerPoints[1] = 0;
            for (int i = 0; i < iSize; i++)
                for (int j = 0; j < iSize; j++)
                {
                    if (tiBoard[j, i] != -1)
                        TPlayerPoints[tiBoard[j, i]]++;
                }
        }

        public void PushToken(ref int[,] boardTest, int iPlayerId, int x, int y, Token token)
        {
            boardTest[x, y] = iPlayerId;
            foreach (int[] coord in currentToken.lTokenCoordTarget)
                boardTest[coord[0], coord[1]] = iPlayerId;
        }



        #region IPlayable

        public string GetName()
        {
            return "Othello Vs He-Arc";
        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            if (column >= 0 && column < iSize && line >= 0 && line < iSize)
            {
                int iActualPlayerId = Tools.IsWhiteToId(isWhite);
                currentToken.ResetTokenList();
                currentToken = new Token(column, line);
                if (tiBoard[column, line] == -1)
                    return IsCellPlayable(iActualPlayerId, column, line, ref currentToken);
            }
            return false;
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            int iActualPlayer = Tools.IsWhiteToId(isWhite);
            if (IsPlayable(column, line, isWhite))
            {
                PushToken(ref tiBoard, iActualPlayer, column, line, currentToken);
                return true;
            }
            return false;
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            int iActualPlayer = Tools.IsWhiteToId(whiteTurn);
            int score = ComputeMoveScore(iActualPlayer, tiBoard);

            int resVal = 0;
            Tuple<int, int> actualMove;

            AlphaBeta(tiBoard, iActualPlayer, level, score, iActualPlayer, out actualMove, out resVal);
            return actualMove;
        }
        
        public int[,] GetBoard()
        {
            return TiBoard;
        }

        public int GetWhiteScore()
        {
            ComputeScore();
            return TPlayerPoints[0];
        }

        public int GetBlackScore()
        {
            ComputeScore();
            return TPlayerPoints[1];
        }

        #endregion

        #region IA

        private void AlphaBeta(int[,] boardTest, int minOrMax, int depth,int parentScoreMove, int iActualPlayerId, out Tuple<int, int> actualMove, out int resVal)
        {
            if (depth > 0)
            {
                actualMove = Tuple.Create(-1, -1);
                resVal = minOrMax * -Int32.MaxValue - 1;

                for (int i = 0; i < iSize; i++)
                {
                    for (int j = 0; j < iSize; j++)
                    {
                        Token token = new Token(j,i);
                        if (IsCellPlayable(boardTest, iActualPlayerId, j, i, ref token))
                        {
                            int[,] newboard = (int[,])boardTest.Clone();
                            PushToken(ref newboard, iActualPlayerId, i, j, token);
                            int newResVal;
                            Tuple<int, int> actualNewMove;
                            AlphaBeta(newboard, minOrMax * -1, depth - 1, resVal, iActualPlayerId,  out actualNewMove, out newResVal);
                            if (newResVal * minOrMax > resVal * minOrMax)
                            {
                                resVal = newResVal;
                                actualMove = Tuple.Create(j,i);
                                if (resVal * minOrMax > parentScoreMove * minOrMax)
                                {
                                    depth = 0;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                actualMove = Tuple.Create(-1, -1);
                resVal = ComputeMoveScore(iActualPlayerId, boardTest);
            }
        }

        private int ComputeMoveScore(int iActualPlayerId, int[,] boardTest)
        {
            int iScoreMove = 0;
            for (int i = 0; i < iSize; i++)
            {
                for (int j = 0; j < iSize; j++)
                {
                    if (boardTest[j,i] != iActualPlayerId)
                    {
                        int iValPower = 1;
                        if (j == 0 || j == iSize - 1)
                        {
                            if (i == 0 || i == iSize - 1)
                                iValPower *= 4;
                            else
                                iValPower *= 2;
                        }
                        else
                            if (i == 0 || i == iSize - 1)
                                iValPower *= 2;
                        if (j == 0 || j == 7)
                            iValPower *= 2;
                        if (boardTest[i, j] == iActualPlayerId)
                            iScoreMove += iValPower;
                        else if (boardTest[i, j] == Tools.InverseBin(iActualPlayerId))
                            iScoreMove -= iValPower;
                    }
                }
            }
            return iScoreMove;
        }


        #endregion

    }

}
