using System;
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
using System.Windows.Threading;

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
        int[] tnbTokensRemain;

        //Id of the player to whom it is the turn
        int iActualPlayerId = 0;

        //Represent the cells on the board
        Token[][] tokensBoard;

        //Represent the cells who can be played this turn
        List<Token> lTokenPlayable;


        int[] tPlayerPoints;

        int iTime = 60 * 5;
        TextBlock[] tbxTimers = new TextBlock[2];
        TimeSpan[] timePlayer = new TimeSpan[2];
        DispatcherTimer dispatcherTimer;
        
        #endregion



        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            
            InitializationParameter();
            InitializationBoard();
            InitializationGame();
           
        }

        #endregion

        #region Initialization

        private void InitializationParameter()
        {
            tbxTimers[0] = tbxTimerPlay1;
            tbxTimers[1] = tbxTimerPlay2;
            timePlayer[0] = TimeSpan.FromSeconds(iTime);
            timePlayer[1] = TimeSpan.FromSeconds(iTime);
            dispatcherTimer = new DispatcherTimer(new TimeSpan(0, 0, 0, 1, 0), DispatcherPriority.Background,
                dispatcherTimer_Update, Dispatcher.CurrentDispatcher);
            dispatcherTimer.IsEnabled = true;
            dispatcherTimer.Start();

            tbxPlayerName1.Text = GameParameter.tCharacterNames[0];
            tbxPlayerName2.Text = GameParameter.tCharacterNames[1];

            rPortraitPlayer1.Fill = new ImageBrush(GameParameter.tbtmTokenIndex[0]);
            rPortraitPlayer2.Fill = new ImageBrush(GameParameter.tbtmTokenIndex[1]);

            tPlayerPoints = new int[2];
            tPlayerPoints[0] = tPlayerPoints[1] = 2;

            iSize = GameParameter.iSize;
            int tokenRemains = Convert.ToInt32(iSize*iSize / 2) - 2;
            tnbTokensRemain = new int[2];
            tnbTokensRemain[0] = tnbTokensRemain[1] = tokenRemains;

        }

        //Initialize the cells, with the buttons and their events
        private void InitializationBoard()
        {
            tokensBoard = new Token[iSize][];


            for (int i = 0; i < iSize; i++)
            {
                
                for (int j = 0; j < iSize; j++)
                {
                    //[j,i] correspond to [x,y], so first of all the collumns are created and only after the rows are made
                    if(i==0)
                    {
                        RowDefinition row = new RowDefinition();
                        ColumnDefinition column = new ColumnDefinition();
                        

                        gridCell.RowDefinitions.Add(row);
                        gridCell.ColumnDefinitions.Add(column);
                        tokensBoard[j] = new Token[iSize];
                    }
                        

                    Button btnNewCell = new Button();
                    btnNewCell.IsEnabled = true;
                    btnNewCell.Uid = j + ";" + i;
                    btnNewCell.MouseEnter +=  new MouseEventHandler(MouseEnterCell);
                    btnNewCell.MouseLeave += new MouseEventHandler(MouseLeaveCell);
                    btnNewCell.Click += MouseClickCell;

                    //Put the buttons into the grid
                    Grid.SetRow(btnNewCell, i);
                    Grid.SetColumn(btnNewCell, j);
                    gridCell.Children.Add(btnNewCell);

                   
                    tokensBoard[j][i] = new Token(btnNewCell);
                }
            }
            lTokenPlayable = new List<Token>();

            GridCellResize();
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



        #region sub_TurnChange

        private void ChangeTurn(){ iActualPlayerId = InverseBin(iActualPlayerId);  ResetPlayableToken(); }

        private int InverseBin(int x) { return (x == 0) ? 1 : 0; }

        //Reset every IsPlayable for each cells
        private void ResetPlayableToken()
        {
            lTokenPlayable.ForEach(token => token.ResetTokenList());
            lTokenPlayable = new List<Token>();
            UpCellInformations();
            if (lTokenPlayable.Count == 0)
                NoMoreMoves();

        }

        private void UpCellInformations()
        {
            tPlayerPoints[0] = tPlayerPoints[1] = 0;
            for (int i = 0; i < iSize; i++)
                for (int j = 0; j < iSize; j++)
                {
                    if (tokensBoard[j][i].ITokenValue == -1)
                        IsCellPlayable(j, i, ref tokensBoard[j][i]);
                    else
                        tPlayerPoints[tokensBoard[j][i].ITokenValue]++;
                }
            DisplayPlayerInformation();
        }

        private void IsCellPlayable(int x, int y, ref Token token)
        {
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                    if (!(i == 0 && j == 0))
                    {
                        List<Token> tempTokenRefs = new List<Token>();
                        FindAction(j, i, x, y, ref tempTokenRefs);
                        foreach(Token tokTarget in tempTokenRefs) { token.LTokenActionList.Add(tokTarget);}
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

        #endregion

        private void DisplayPlayerInformation()
        {
            tbxScorePlayer1.Text = ((tPlayerPoints[0] < 10) ? "0" : "") + tPlayerPoints[0].ToString();
            tbxScorePlayer2.Text = ((tPlayerPoints[1] < 10) ? "0" : "") + tPlayerPoints[1].ToString();
            tbxTokenPlayer1.Text = ((tnbTokensRemain[0] < 10) ? "0" : "") + tnbTokensRemain[0].ToString();
            tbxTokenPlayer2.Text = ((tnbTokensRemain[0] < 10) ? "0" : "") + tnbTokensRemain[0].ToString();
        }


        private void NoMoreMoves()
        {

        }


        private void GameOver()
        {

        }


        private void UidToIJ(Button btn, ref int j, ref int i)
        {
            string[] strUid = btn.Uid.Split(';');
            try { j = Convert.ToInt32(strUid[0]); i = Convert.ToInt32(strUid[1]); }
            catch { Console.WriteLine("Btn.Uid is not integer."); }
        }

        private void GridCellResize()
        {
            double dblWidth = mainWindow.Width;
            double dblHeight = mainWindow.Height;
            double dblCellSize = 0;

            if (dblWidth / dblHeight < ProgramParameter.dblProgramSizeRatio)
            {
                dblCellSize = mainWindow.Width - 20;
                dblCellSize = (dblCellSize / 2) / iSize;
            }
            else
            {
                dblCellSize = mainWindow.Height - 40;
                dblCellSize = ((dblCellSize / 4) * 3) - 120;
                dblCellSize = dblCellSize / iSize;
            }          
            for(int i = 0; i < iSize; i++)
                for(int j = 0; j < iSize; j++)
                    tokensBoard[j][i].BtnContainer.Width = tokensBoard[j][i].BtnContainer.Height = dblCellSize;
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
                if(tnbTokensRemain[iActualPlayerId] > 0)
                    tnbTokensRemain[iActualPlayerId]--;
                else
                {
                    int iOtherPlayerId = InverseBin(iActualPlayerId);
                    if (tnbTokensRemain[iOtherPlayerId] > 0)
                        tnbTokensRemain[iActualPlayerId]--;
                    else
                        GameOver();

                }


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
                    imgToken.Source = GameParameter.tbtmTokenIndex[iActualPlayerId];
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

        private void dispatcherTimer_Update(object sender, EventArgs e)
        {
            TimeSpan timeInterval = TimeSpan.FromSeconds(1);
            timePlayer[iActualPlayerId] =  timePlayer[iActualPlayerId].Subtract(timeInterval);
            tbxTimers[iActualPlayerId].Text = timePlayer[iActualPlayerId].ToString(@"\0m\:ss");
        }


        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridCellResize();
        }

        #endregion

    }
}
