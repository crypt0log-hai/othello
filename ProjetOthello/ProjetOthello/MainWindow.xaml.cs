using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        int[] tPlayerPoints;
        PlayerInfo pInfo;

        //Id of the player to whom it is the turn
        int iActualPlayerId = 1;

        //Represent the cells on the board
        Token[,] tTokensBoard;
        int[,] tiBoard;
        Grid gridCell;

        //Represent the cells who can be played this turn
        List<Token> lTokenPlayable;

        public List<Tuple<int, int[,], string>> lHistoryGame;

        int iTime = 0;
        TextBlock[] tbxTimers = new TextBlock[2];
        TimeSpan[] timePlayer = new TimeSpan[2];
        public DispatcherTimer dispatcherTimer;

        bool[] tblNoMoreMove = { false, false };

        #endregion
        
        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            InititializeAll();
        }

        public MainWindow(Tuple<int, int[,], string> loadedGame)
        {
            InitializeComponent();
            InitializationParameter();
            InitializationBoard();
            InitializationLoadedGame(loadedGame);
            ResetPlayableToken();
        }

        #endregion

        #region Initialization

        private void InititializeAll()
        {
            InitializationParameter();
            InitializationBoard();
            InitializationGame();
            ResetPlayableToken();
        }

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


            pInfo = new PlayerInfo { ScoreP1 = "02", NbTokenP1 = "30", ScoreP2 = "02", NbTokenP2 = "30" };
            this.DataContext = pInfo;

            lHistoryGame = new List<Tuple<int, int[,], string>>();

        }

        //Initialize the cells, with the buttons and their events
        private void InitializationBoard()
        {
            tTokensBoard = new Token[iSize,iSize];
            tiBoard = new int[iSize, iSize];
            gridCell = new Grid();
            canvaBoard.Children.Add(gridCell);
            for (int i = 0; i < iSize; i++)
            {                
                for (int j = 0; j < iSize; j++)
                {
                    //[j,i] correspond to [x,y]
                    if(i==0)
                    {
                        RowDefinition row = new RowDefinition();
                        ColumnDefinition column = new ColumnDefinition();         
                        gridCell.RowDefinitions.Add(row);
                        gridCell.ColumnDefinitions.Add(column);
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

                    tiBoard[j, i] = -1;
                    tTokensBoard[j,i] = new Token(btnNewCell);
                }
            }
            lTokenPlayable = new List<Token>();

            GridCellResize();
        }
       

        //Initialize the fourth first tokens
        private void InitializationGame()
        {
            tiBoard[(int)iSize / 2, (int)iSize / 2 - 1] = iActualPlayerId;
            tiBoard[(int)iSize / 2 - 1, (int)iSize / 2] = iActualPlayerId;
            tiBoard[(int)iSize / 2 - 1, (int)iSize / 2 - 1] = InverseBin(iActualPlayerId);
            tiBoard[(int)iSize / 2, (int)iSize / 2] = InverseBin(iActualPlayerId);
            UpdateTokenBoard();
        }

        private void InitializationLoadedGame(Tuple<int, int[,], string> loadedGame)
        {
            iActualPlayerId = loadedGame.Item1;
            tiBoard = loadedGame.Item2;
            UpdateTokenBoard();
        }

        #endregion
        
        #region Function

        #region sub_TurnChange

        private void ChangeTurn()
        {
            string strTimer;
            string strFormatPlayer1, strFormatPlayer2;
            int[,] board = new int[iSize,iSize];

            for (int i = 0; i < iSize; i++)
                for (int j = 0; j < iSize; j++)
                    board[j, i] = tiBoard[j, i];

            iActualPlayerId = InverseBin(iActualPlayerId);
            tblNoMoreMove[iActualPlayerId] = false;

            strFormatPlayer1 = (timePlayer[0].Minutes < 10) ? @"\0m\:ss" : @"\mm\:ss";
            strFormatPlayer2 = (timePlayer[1].Minutes < 10) ? @"\0m\:ss" : @"\mm\:ss";
            strTimer = timePlayer[0].ToString(strFormatPlayer1) + "_" + timePlayer[1].ToString(strFormatPlayer2);
            lHistoryGame.Add(new Tuple<int, int[,], string>(iActualPlayerId, board, strTimer));
            GameParameter.iNbTurn++;
            ResetPlayableToken();
        }

        private int InverseBin(int x) { return (x == 0) ? 1 : 0; }

        //Reset every IsPlayable for each cells
        private void ResetPlayableToken()
        {
            lTokenPlayable.ForEach(token => token.ResetTokenList());
            lTokenPlayable = new List<Token>();
            UpCellInformations();
            if (lTokenPlayable.Count == 0)
            {
                NoMoreMoves();
            }

        }

        private void UpCellInformations()
        {
            tPlayerPoints[0] = tPlayerPoints[1] = 0;
            for (int i = 0; i < iSize; i++)
                for (int j = 0; j < iSize; j++)
                {
                    if (tiBoard[j,i] == -1)
                        IsCellPlayable(j, i, ref tTokensBoard[j,i]);
                    else
                        tPlayerPoints[tiBoard[j, i]]++;
                }
            DisplayPlayerInformation();
        }

        private void IsCellPlayable(int x, int y, ref Token token)
        {
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                    if (!(i == 0 && j == 0))
                    {
                        List<int[]> tempTarget = new List<int[]>();
                        FindAction(j, i, x, y, ref tempTarget);
                        foreach(int[] coord in tempTarget) { token.LTokenCoordTarget.Add(coord); }
                    }

            if (token.LTokenCoordTarget.Count > 0)
            {
                lTokenPlayable.Add(token);
                token.IIsPlayable = true;
            }
            

        }

        private bool FindAction(int j,int i, int x, int y, ref List<int[]> tempTokenRefs)
        {
            x += j;
            y += i;
            bool blFindExtremis = false;
            if(x >= 0 && x < iSize)
                if(y >= 0 && y < iSize)
                {
                    if (iActualPlayerId == 0)
                    {
                        if (tiBoard[x,y] == 1)
                        {
                            tempTokenRefs.Add(new int[]{x,y});
                            blFindExtremis = FindAction(j, i, x, y, ref tempTokenRefs);
                        }
                        else if (tiBoard[x, y] == 0)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        if (tiBoard[x, y] == 0)
                        {
                            tempTokenRefs.Add(new int[] {x, y});
                            blFindExtremis = FindAction(j, i, x, y, ref tempTokenRefs);
                        }
                        else if (tiBoard[x, y] == 1)
                            return true;
                        else
                            return false;

                    }
                }
            if (!blFindExtremis)
            {
                tempTokenRefs = new List<int[]>();
                return false;
            }
            return true;
        }


        private void UpdateTokenBoard()
        {
            for (int i = 0; i < iSize; i++)
                for (int j = 0; j < iSize; j++)
                    if (tiBoard[j, i] != -1)
                        tTokensBoard[j, i].UpdateToken(tiBoard[j, i]);
        }

        #endregion

        #region sub_Rules

        public void Restart()
        {
            gridCell.Children.RemoveRange(0, gridCell.Children.Count);
            canvaBoard.Children.Remove(gridCell);
            GameParameter.iWinner = -1;
            InititializeAll();
        }



        private void NoMoreMoves()
        {
            if (tblNoMoreMove[InverseBin(iActualPlayerId)])
                GameOver();
            else
            {
                tblNoMoreMove[iActualPlayerId] = true;
                ChangeTurn();
            }
        }


        private void GameOver()
        {
            if (tPlayerPoints[0] > tPlayerPoints[1])
                GameParameter.iWinner = 0;
            else if (tPlayerPoints[0] < tPlayerPoints[1])
                GameParameter.iWinner = 1;
            ShowScore(true);         
        }

        private void ShowScore(bool blGameEnd)
        {
            dispatcherTimer.IsEnabled = false;
            mainWindow.IsEnabled = false;
            ScoreDisplay scoreDisplay = new ScoreDisplay(this, blGameEnd);
            scoreDisplay.Show();
        }

        #endregion

        #region sub_Tools

        private void DisplayPlayerInformation()
        {
            pInfo.ScoreP1 = ((tPlayerPoints[0] < 10) ? "0" : "") + tPlayerPoints[0].ToString();
            pInfo.ScoreP2 = ((tPlayerPoints[1] < 10) ? "0" : "") + tPlayerPoints[1].ToString();
            pInfo.NbTokenP1 = ((tnbTokensRemain[0] < 10) ? "0" : "") + tnbTokensRemain[0].ToString();
            pInfo.NbTokenP2 = ((tnbTokensRemain[1] < 10) ? "0" : "") + tnbTokensRemain[1].ToString();
        }

        private void UidToIJ(Button btn, ref int j, ref int i)
        {
            string[] strUid = btn.Uid.Split(';');
            try { j = Convert.ToInt32(strUid[0]); i = Convert.ToInt32(strUid[1]); }
            catch { Console.WriteLine("Btn.Uid is not integer."); }
        }

        #endregion

        private void GridCellResize()
        {
            double dblWidth = mainWindow.ActualWidth;
            double dblHeight = mainWindow.ActualHeight;
            double dblCellSize = 0;


            //if (dblWidth / dblHeight < ProgramParameter.dblProgramSizeRatio)
            //{
            //    dblCellSize = dblWidth - 20;
            //    dblCellSize = (dblCellSize / 2) - 264;
            //    dblCellSize /= iSize;
            //}
            //else
            //{
                dblCellSize = dblHeight - 40;
                dblCellSize = ((dblCellSize / 4) * 3) - 120;
                dblCellSize = dblCellSize / iSize;
                
            //}
            if (dblHeight != 0)
            {
                for (int i = 0; i < iSize; i++)
                    for (int j = 0; j < iSize; j++)
                        tTokensBoard[j, i].BtnContainer.Width = tTokensBoard[j, i].BtnContainer.Height = dblCellSize;
            }
        }

        #endregion

        #region Event

        private void MouseClickCell(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int iX = 0;
            int iY = 0;
            UidToIJ(btn, ref iX, ref iY);
            Token tokenRef = tTokensBoard[iX,iY];

            if(tokenRef.IIsPlayable)
            {
                if(tnbTokensRemain[iActualPlayerId] > 0)
                    tnbTokensRemain[iActualPlayerId]--;
                else
                {
                    int iOtherPlayerId = InverseBin(iActualPlayerId);
                    if (tnbTokensRemain[iOtherPlayerId] > 0)
                        tnbTokensRemain[iOtherPlayerId]--;
                    else
                        GameOver();

                }
                tiBoard[iX,iY] = iActualPlayerId;
                foreach (int[] coord in tokenRef.LTokenCoordTarget){ tiBoard[coord[0], coord[1]] = iActualPlayerId;}
                UpdateTokenBoard();
                ChangeTurn();
            }

        }


        private void MouseEnterCell(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            int iX = 0;
            int iY = 0;
            UidToIJ(btn, ref iX, ref iY);
            Token tokenRef = tTokensBoard[iX,iY];
            
            
            if (tiBoard[iX,iY] == -1)
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
            if(tiBoard[iX,iY] == -1)
                tTokensBoard[iX,iY].TokenResetDisplay();
        }

        private void dispatcherTimer_Update(object sender, EventArgs e)
        {
            TimeSpan timeInterval = TimeSpan.FromSeconds(1);
            timePlayer[iActualPlayerId] =  timePlayer[iActualPlayerId].Add(timeInterval);
            tbxTimers[iActualPlayerId].Text = timePlayer[iActualPlayerId].ToString(@"\0m\:ss");
        }


        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridCellResize();
        }


        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            ShowScore(false);
        }

        #endregion

    }

    #region ClassBinding


    public class PlayerInfo : INotifyPropertyChanged
    {
        private string strScoreP1;
        private string strNbTokenP1;
        private string strScoreP2;
        private string strNbTokenP2;

        public string ScoreP1
        {
            get { return strScoreP1; }
            set
            {
                if (strScoreP1 != value)
                {
                    strScoreP1 = value;
                    RaisePropertyChanged("ScoreP1");
                }
            }
        }
        public string NbTokenP1
        {
            get { return strNbTokenP1; }
            set
            {
                if (strNbTokenP1 != value)
                {
                    strNbTokenP1 = value;
                    RaisePropertyChanged("NbTokenP1");
                }
            }
        }
        public string ScoreP2
        {
            get { return strScoreP2; }
            set
            {
                if (strScoreP2 != value)
                {
                    strScoreP2 = value;
                    RaisePropertyChanged("ScoreP2");
                }
            }
        }
        public string NbTokenP2
        {
            get { return strNbTokenP2; }
            set
            {
                if (strNbTokenP2 != value)
                {
                    strNbTokenP2 = value;
                    RaisePropertyChanged("NbTokenP2");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    #endregion
}
