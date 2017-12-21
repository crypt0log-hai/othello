﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjetOthello
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Proprety
        
        //Number if collumns and rows of the board, iSize*iSize = Number of cells
        int iSize = 0;

        //Id of the player to whom it is the turn
        int iActualPlayerId = 0;

        //Represent the cells on the board
        Token[][] tokensBoard;

        //Represent the cells who can be played this turn
        List<Token> lTokenPlayable;
        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();

            //ZONE A MOURRIR
            BitmapImage btm1 = new BitmapImage();
            btm1.BeginInit();
            btm1.UriSource = new Uri("pack://application:,,,/Assets/Tokens/BlackToken.png");
            btm1.EndInit();
            GameParameter.imageIndex[0] = btm1;
            btm1 = new BitmapImage();
            btm1.BeginInit();
            btm1.UriSource = new Uri("pack://application:,,,/Assets/Tokens/WhiteToken.png");
            btm1.EndInit();
            GameParameter.imageIndex[1] = btm1;


            /////////////////


            iSize = GameParameter.iSize;
            InitializationBoard();
            InitializationGame();
        }

        #endregion

        #region Initialization
        //Initialize the cells, with the buttons and their events
        private void InitializationBoard()
        {
            tokensBoard = new Token[iSize][];
            for(int i = 0; i < iSize; i++)
            {
                for (int j = 0; j < iSize; j++)
                {
                    //[j,i] correspond to [x,y], so first of all the collumns are created and only after the rows are made
                    if(i==0)
                        tokensBoard[j] = new Token[iSize];

                    Button btnNewCell = new Button();
                    btnNewCell.IsEnabled = true;
                    btnNewCell.Uid = j + ";" + i;
                    btnNewCell.MouseEnter +=  new MouseEventHandler(MouseEnterCell);
                    btnNewCell.MouseLeave += new MouseEventHandler(MouseLeaveCell);
                    btnNewCell.Click += MouseClickCell;

                    //Put the buttons into the grid
                    Grid.SetRow(btnNewCell, i);
                    Grid.SetColumn(btnNewCell, j);
                    gridBoard.Children.Add(btnNewCell);

                   
                    tokensBoard[j][i] = new Token(btnNewCell);
                }
            }
            lTokenPlayable = new List<Token>();

        }
       

        //Initialize the fourth first tokens
        private void InitializationGame()
        {
            tokensBoard[(int)iSize / 2 - 1][(int)iSize / 2 - 1].UpdateToken(iActualPlayerId);
            tokensBoard[(int)iSize / 2][(int)iSize / 2].UpdateToken(iActualPlayerId);
            ChangeTurn();
            tokensBoard[(int)iSize / 2][(int)iSize / 2 - 1].UpdateToken(iActualPlayerId);
            tokensBoard[(int)iSize / 2 - 1][(int)iSize / 2].UpdateToken(iActualPlayerId);
            ChangeTurn();
        }



       

        #endregion


        #region Function

        
        private void ChangeTurn(){iActualPlayerId = (iActualPlayerId == 0) ? 1 : 0; ResetPlayableToken(); }

        //Reset every IsPlayable for each cells
        private void ResetPlayableToken()
        {
            lTokenPlayable.ForEach(token => token.ResetTokenList());
            lTokenPlayable = new List<Token>();
            CellPlayableThisTurn();
        }

        private void CellPlayableThisTurn()
        {
            for (int i = 0; i < iSize; i++)
                for (int j = 0; j < iSize; j++)
                    IsCellPlayable(j, i, ref tokensBoard[j][i]);
        }

        private void IsCellPlayable(int x, int y, ref Token token)
        {
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                    if (!(i == 0 && j == 0))
                    {
                        List<Token> tempTokenRefs = new List<Token>();
                        FindAction(j, i, x, y, ref tempTokenRefs);
                        foreach(Token tokTarget in tempTokenRefs) { token.LTokenActionList.Add(tokTarget); }
                    }

            if (token.LTokenActionList.Count > 0)
            {
                lTokenPlayable.Add(token);
                token.IIsPlayable = true;
            }
            

        }

        private bool FindAction(int j,int i, int x, int y, ref List<Token> tempTokenRefs)
        {
            x += j;
            y += i;
            bool blFindExtremis = false;
            if(x >= 0 && x < iSize)
                if(y >= 0 && y < iSize)
                {
                    if (iActualPlayerId == 0)
                    {
                        if (tokensBoard[x][y].ITokenValue == 1)
                        {
                            tempTokenRefs.Add(tokensBoard[x][y]);
                            blFindExtremis = FindAction(j, i, x, y, ref tempTokenRefs);
                        }
                        else if (tokensBoard[x][y].ITokenValue == 0)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        if (tokensBoard[x][y].ITokenValue == 0)
                        {
                            tempTokenRefs.Add(tokensBoard[x][y]);
                            blFindExtremis = FindAction(j, i, x, y, ref tempTokenRefs);
                        }
                        else if (tokensBoard[x][y].ITokenValue == 1)
                            return true;
                        else
                            return false;

                    }
                }
            if (!blFindExtremis)
            {
                tempTokenRefs = new List<Token>();
                return false;
            }
            return true;
        }

        private void UidToIJ(Button btn, ref int j, ref int i)
        {
            string[] strUid = btn.Uid.Split(';');
            try { j = Convert.ToInt32(strUid[0]); i = Convert.ToInt32(strUid[1]); }
            catch { Console.WriteLine("Btn.Uid is not integer."); }
        }

        #endregion

        #region Event

        private void MouseClickCell(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int iX = 0;
            int iY = 0;
            UidToIJ(btn, ref iX, ref iY);
            Token tokenRef = tokensBoard[iX][iY];

            if(tokenRef.IIsPlayable)
            {
                tokenRef.UpdateToken(iActualPlayerId);
                foreach (Token tokTarget in tokenRef.LTokenActionList){tokTarget.UpdateToken(iActualPlayerId);}
                ChangeTurn();
            }

        }


        private void MouseEnterCell(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            int iX = 0;
            int iY = 0;
            UidToIJ(btn, ref iX, ref iY);
            Token tokenRef = tokensBoard[iX][iY];
            
            
            if (tokenRef.ITokenValue == -1)
            {
                if (tokenRef.IIsPlayable)
                {
                    Image imgToken = new Image();
                    imgToken.Source = GameParameter.imageIndex[iActualPlayerId];
                    btn.Content = imgToken;
                }
            }
        }

        private void MouseLeaveCell(object sender, MouseEventArgs e)
        {            
            Button btnEvent = (Button)sender;
            int iX = 0;
            int iY = 0;
            UidToIJ(btnEvent, ref iX, ref iY);
            tokensBoard[iX][iY].TokenResetDisplay();
        }
        #endregion
    }
}
