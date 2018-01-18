using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetOthello
{
    class GameBoard : IPlayable.IPlayable
    {

        #region Proprety

        public int[,] tiBoard;          //Represent the value of each cells
        public Token[,] tToken;         //Reference each cells
        private int[] tPlayerPoints;    //Game score of each player
        private int iSize = 8;          //Size of the board (could be change in a solo game)         

        #endregion

        #region Getter/Setter
        public int[] TPlayerPoints { get => tPlayerPoints; set => tPlayerPoints = value; }
        #endregion

        #region Constructor
        public GameBoard()
        {
            tiBoard = new int[iSize, iSize];
            tPlayerPoints = new int[2];
            tToken = new Token[iSize, iSize];
            for (int i = 0; i < iSize; i++)
                for (int j = 0; j < iSize; j++)
                {
                    tiBoard[j, i] = -1;
                    tToken[j, i] = new Token(j, i);
                }
            InitializationGame();
        }

        #endregion

        #region Initialization

        /// <summary>
        /// For not tournament purpose only, update size of boards
        /// </summary>
        public void InitializationSolo(int iSize)
        {
            this.iSize = iSize;
            tiBoard = new int[iSize, iSize];
        }

        /// <summary>
        /// Initialise the 4th first tokens
        /// </summary>
        private void InitializationGame()
        {
            tiBoard[(int)iSize / 2, (int)iSize / 2 - 1] = 1;
            tiBoard[(int)iSize / 2 - 1, (int)iSize / 2] = 1;
            tiBoard[(int)iSize / 2 - 1, (int)iSize / 2 - 1] = 0;
            tiBoard[(int)iSize / 2, (int)iSize / 2] = 0;
        }

        #endregion

        #region Function
        /// <summary>
        /// Find if a cells is playable
        /// </summary>
        /// <param name="iActualPlayerId">Id of the player who have the turn</param>
        /// <param name="x">Horizontal position of the cells </param>
        /// <param name="y">Vertical position of the cells</param>
        /// <param name="token">Reference from the token table tToken, to update the target list & token</param>
        /// <returns>True if the cell is playable, false if not</returns>
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
        
        //Same as the first method, but this one take a board in parameter. 
        //Usefull for the AlphaBeta testing
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

        /// <summary>
        /// Will find every target, who will be turned back if we play this cell
        /// </summary>
        /// <param name="board">The board we will use to find the action</param>
        /// <param name="iActualPlayerId">Id of the player who have the turn</param>
        /// <param name="j">The horizontal direction where we try to find action</param>
        /// <param name="i">The vertical direction where we try to find action</param>
        /// <param name="x">Horizontal position of the cells</param>
        /// <param name="y">Vertical position of the cells</param>
        /// <param name="tempTokenRefs">Temp token where the target will be save</param>
        /// <returns>Return false if we didn't find actual action</returns>
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
        
        /// <summary>
        /// Compute the game score of the game, one point by token
        /// </summary>
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

        /// <summary>
        /// Remove the list of Playable token, after every move
        /// </summary>
        public void ResetTokenTarget()
        {
            foreach (Token t in tToken)
                t.ResetTokenList();
        }

        /// <summary>
        /// Put a token on the board & Update everyone of his taget
        /// </summary>
        /// <param name="boardTest"></param>
        /// <param name="iPlayerId"></param>
        /// <param name="x">Horizontale position of the token</param>
        /// <param name="y">Vertical position of the token</param>
        /// <param name="token">Reference of the token put in the board</param>
        public void PutToken(ref int[,] boardTest, int iPlayerId, int x, int y, Token token)
        {
            boardTest[x, y] = iPlayerId;
            foreach (int[] coord in tToken[x,y].lTokenCoordTarget)
                boardTest[coord[0], coord[1]] = iPlayerId;
            ResetTokenTarget();
        }

        #endregion

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
                if (tiBoard[column, line] == -1)
                    return IsCellPlayable(iActualPlayerId, column, line, ref tToken[column,line]);
            }
            return false;
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            int iActualPlayer = Tools.IsWhiteToId(isWhite);
            if (IsPlayable(column, line, isWhite))
            {
                PutToken(ref tiBoard, iActualPlayer, column, line, tToken[column, line]);
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

            AlphaBeta(out actualMove, out resVal, tiBoard, 1, level, score, iActualPlayer, iActualPlayer);
            return actualMove;
        }
        
        public int[,] GetBoard()
        {
            return tiBoard;
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

        /// <summary>
        /// Will do the AlphaBeta algorithe to find one of the most optimised move to play
        /// </summary>
        /// <param name="actualMove">The chosen move</param>
        /// <param name="resVal"></param>
        /// <param name="boardTest"></param>
        /// <param name="minOrMax">Represent if we are in a min part or a max part of the algoritme, to compare the opponent score</param>
        /// <param name="depth">The number of phase the algoritm pass trough</param>
        /// <param name="parentScoreMove">Score of the parent</param>
        /// <param name="iActualPlayerId">Global player id</param>
        /// <param name="iPlayerId">Local player id </param>
        private void AlphaBeta(out Tuple<int, int> actualMove, out int resVal, int[,] boardTest, int minOrMax, int depth,int parentScoreMove, int iActualPlayerId, int iPlayerId)
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
                        if (IsCellPlayable(boardTest, iPlayerId, j, i, ref token))
                        {
                            int[,] newboard = (int[,])boardTest.Clone();
                            PutToken(ref newboard, iPlayerId, i, j, token);
                            int newResVal;
                            Tuple<int, int> actualNewMove;
                            AlphaBeta( out actualNewMove, out newResVal, newboard, minOrMax * -1, depth - 1, resVal, iActualPlayerId, Tools.InverseBin(iPlayerId));
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
                resVal = ComputeMoveScore(iActualPlayerId, boardTest);
                actualMove = Tuple.Create(-1, -1);
            }
        }

        /// <summary>
        /// Will compute the power score of the board 
        /// </summary>
        /// <param name="iActualPlayerId">Id of the player who actually play</param>
        /// <param name="boardTest">The board that will be compute</param>
        /// <returns></returns>
        private int ComputeMoveScore(int iActualPlayerId, int[,] boardTest)
        {
            int iScoreMove = 0;
            int[,] tValPower = 
                { 
                    { 32,1,16,16,16,16,1,32},
                    { 1,1,1,1,1,1,1,1},
                    { 16,1,2,2,2,2,1,16},
                    { 16,1,2,2,2,2,1,16},
                    { 16,1,2,2,2,2,1,16},
                    { 16,1,2,2,2,2,1,16},
                    { 1,1,1,1,1,1,1,1},
                    { 32,1,16,16,16,16,1,32}
            };

            for (int i = 0; i < iSize; i++)
            {
                for (int j = 0; j < iSize; j++)
                {
                    if (boardTest[j,i] != iActualPlayerId)
                    {
                        if (boardTest[i, j] == iActualPlayerId)
                            iScoreMove += tValPower[j,i];
                        else if (boardTest[i, j] == Tools.InverseBin(iActualPlayerId))
                            iScoreMove -= tValPower[j, i];
                    }
                }
            }
            return iScoreMove;
        }


        #endregion

    }

}
