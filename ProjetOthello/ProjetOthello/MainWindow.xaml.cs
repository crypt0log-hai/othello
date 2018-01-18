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
using System.Threading;

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
        Grid gridCell;


        /// <summary>
        /// 
        /// </summary>
        List<Token> lTokenPlayable;

        Button[,] tTokensBoard;


        public List<Tuple<int, int[,], string>> lHistoryGame;

        int iTime = 0;
        TextBlock[] tbxTimers = new TextBlock[2];
        TimeSpan[] timePlayer = new TimeSpan[2];
        public DispatcherTimer dispatcherTimer;

        bool[] tblNoMoreMove = { false, false };
        bool gameOver = false;


        private GameBoard gameBoard;

        #endregion
        
        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            InititializeAll();
            ResetPlayableToken();
            if (GameParameter.isIA[iActualPlayerId])
                IaPlay();
        }

        public MainWindow(Tuple<int, int[,], string> loadedGame)
        {
            InitializeComponent();
            InititializeAll();
            InitializationLoadedGame(loadedGame);
            ResetPlayableToken();
            if (GameParameter.isIA[iActualPlayerId])
                IaPlay();
        }

        #endregion

        #region Initialization

        private void InititializeAll()
        {
            InitializationParameter();
            InitializationBoard();
            UpdateTokenBoard();
        }

        private void InitializationParameter()
        {
            
            tbxTimers[0] = tbxTimerPlay1;
            tbxTimers[1] = tbxTimerPlay2;
            tbxTimers[0].Text = tbxTimers[1].Text = "00:00";
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
            gameBoard = new GameBoard();

            tTokensBoard = new Button[iSize, iSize];
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
                    
                    tTokensBoard[j,i] = btnNewCell;
                }
            }
            lTokenPlayable = new List<Token>();
            GridCellResize();
        }
       

        //Initialize the fourth first tokens
        

        private void InitializationLoadedGame(Tuple<int, int[,], string> loadedGame)
        {
            iActualPlayerId = loadedGame.Item1;
            gameBoard.tiBoard = loadedGame.Item2;
            UpdateTokenBoard();
        }

        #endregion

        #region Function

        private void UpdateTokenBoard()
        {
            gameBoard.GetBoard();
            for (int i = 0; i < iSize; i++)
                for (int j = 0; j < iSize; j++)
                    if (gameBoard.tiBoard[j, i] != -1)
                        UpdateToken(gameBoard.tiBoard[j, i], j, i);
        }

        public void UpdateToken(int iPlayerId, int x, int y)
        {
            Image imgToken = new Image();
            imgToken.Source = GameParameter.tbtmTokenIndex[iPlayerId];
            tTokensBoard[x,y].Content = imgToken;
            tTokensBoard[x, y].Background = GameParameter.tColorBackgroundCell[iPlayerId];
        }

        public void TokenResetDisplay(int x, int y)
        {
            tTokensBoard[x, y].Content = "";
        }


        #region sub_TurnChange

        private void ChangeTurn()
        {
            string strTimer;
            string strFormatPlayer1, strFormatPlayer2;
            int[,] board = new int[iSize,iSize];

            for (int i = 0; i < iSize; i++)
                for (int j = 0; j < iSize; j++)
                    board[j, i] = gameBoard.tiBoard[j, i];

            iActualPlayerId = Tools.InverseBin(iActualPlayerId);
            tblNoMoreMove[iActualPlayerId] = false;

            strFormatPlayer1 = (timePlayer[0].Minutes < 10) ? @"\0m\:ss" : @"\mm\:ss";
            strFormatPlayer2 = (timePlayer[1].Minutes < 10) ? @"\0m\:ss" : @"\mm\:ss";
            strTimer = timePlayer[0].ToString(strFormatPlayer1) + "_" + timePlayer[1].ToString(strFormatPlayer2);
            lHistoryGame.Add(new Tuple<int, int[,], string>(iActualPlayerId, board, strTimer));
            GameParameter.iNbTurn++;
            ResetPlayableToken();
            DisplayPlayerInformation();
            if (lTokenPlayable.Count == 0)
                NoMoreMoves();
            else
            {
                if (GameParameter.isIA[iActualPlayerId])
                    IaPlay();
            }

        }

        public void IaPlay()
        {
            Tuple<int, int> nextMove = gameBoard.GetNextMove(gameBoard.tiBoard, 6, Tools.IdToIsWhite(iActualPlayerId));
            if (gameBoard.PlayMove(nextMove.Item1, nextMove.Item2, Tools.IdToIsWhite(iActualPlayerId)))
                DecreaseToken();
            UpdateTokenBoard();

            if (!gameOver)
                ChangeTurn();
        }

        public void ResetPlayableToken()
        {
            lTokenPlayable.ForEach(token => token.ResetTokenList());
            lTokenPlayable = new List<Token>();
            UpCellInformations();
        }

        private void UpCellInformations()
        {
            tPlayerPoints[0] = tPlayerPoints[1] = 0;
            for (int i = 0; i < iSize; i++)
                for (int j = 0; j < iSize; j++)
                {
                    if (gameBoard.tiBoard[j, i] == -1)
                    {


                        Token token = new Token(j, i);
                        if (gameBoard.IsCellPlayable(iActualPlayerId, j, i, ref gameBoard.tToken[j,i]))
                            lTokenPlayable.Add(token);
                    }
                }
            gameBoard.ComputeScore();
            tPlayerPoints = gameBoard.TPlayerPoints;
        }       

        #endregion

        #region sub_Rules
        
        private void NoMoreMoves()
        {
            if (tblNoMoreMove[Tools.InverseBin(iActualPlayerId)])
                GameOver();
            else
            {
                tblNoMoreMove[iActualPlayerId] = true;
                ChangeTurn();
            }
        }


        private void GameOver()
        {
            gameOver = true;
            GameParameter.tScore = tPlayerPoints;
            GameParameter.tTime[0] = timePlayer[0].ToString(@"\0m\:ss");
            GameParameter.tTime[1] = timePlayer[0].ToString(@"\0m\:ss");
            if (tPlayerPoints[0] > tPlayerPoints[1])
                GameParameter.iWinner = 0;
            else if (tPlayerPoints[0] < tPlayerPoints[1])
                GameParameter.iWinner = 1;
            else
                GameParameter.iWinner = -1;
            ShowScore(true);         
        }

        private void ShowScore(bool blGameEnd)
        {
            dispatcherTimer.IsEnabled = false;
            dispatcherTimer.Stop();
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
            UpdateTokenBoard();
        }

        public static void UidToIJ(Button btn, ref int j, ref int i)
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


            if (dblWidth / dblHeight < ProgramParameter.dblProgramSizeRatio)
            {
                dblCellSize = dblWidth - 20;
                dblCellSize = (dblCellSize / 2) - 264;
                dblCellSize /= iSize;
            }
            else
            {
                dblCellSize = dblHeight - 40;
                dblCellSize = ((dblCellSize / 4) * 3) - 120;
                dblCellSize = dblCellSize / iSize;
            }
            if (dblCellSize > 0)
            {
                for (int i = 0; i < iSize; i++)
                    for (int j = 0; j < iSize; j++)
                        tTokensBoard[j, i].Width = tTokensBoard[j, i].Height = dblCellSize;
            }
        }


        private void DecreaseToken()
        {
            if (tnbTokensRemain[iActualPlayerId] > 0)
                tnbTokensRemain[iActualPlayerId]--;
            else
            {
                int iOtherPlayerId = Tools.InverseBin(iActualPlayerId);
                if (tnbTokensRemain[iOtherPlayerId] > 0)
                    tnbTokensRemain[iOtherPlayerId]--;
                else
                    GameOver();
            }
        }

        #endregion

        #region Event

        private void MouseClickCell(object sender, RoutedEventArgs e)
        {
            if (!GameParameter.isIA[iActualPlayerId])
            {
                Button btn = (Button)sender;
                int iX = 0;
                int iY = 0;
                UidToIJ(btn, ref iX, ref iY);
                Token tokenRef = gameBoard.tToken[iX, iY];
            
                if (tokenRef.IIsPlayable)
                {
                    gameBoard.PlayMove(iX, iY, Tools.IdToIsWhite(iActualPlayerId));
                    UpdateTokenBoard();
                    DecreaseToken();
                    if (!gameOver)
                        ChangeTurn();
                }
            }

        }


        private void MouseEnterCell(object sender, MouseEventArgs e)
        {

            if (!GameParameter.isIA[iActualPlayerId])
            {
                Button btn = (Button)sender;
                int iX = 0;
                int iY = 0;
                UidToIJ(btn, ref iX, ref iY);
                Token tokenRef = gameBoard.tToken[iX, iY];
               


                if (gameBoard.tiBoard[iX, iY] == -1)
                {
                    if (iX == 4 && iY == 5)
                    {
                        Console.WriteLine("");
                    }
                    if (tokenRef.IIsPlayable)
                    {
                        Image imgToken = new Image();
                        imgToken.Source = GameParameter.tbtmTokenIndex[iActualPlayerId];
                        btn.Content = imgToken;
                    }
                }
            }
        }

        private void MouseLeaveCell(object sender, MouseEventArgs e)
        {
            if (!GameParameter.isIA[iActualPlayerId])
            {
                Button btnEvent = (Button)sender;
                int iX = 0;
                int iY = 0;
                UidToIJ(btnEvent, ref iX, ref iY);
                if (gameBoard.tiBoard[iX, iY] == -1)
                    TokenResetDisplay(iX,iY);
            }
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
