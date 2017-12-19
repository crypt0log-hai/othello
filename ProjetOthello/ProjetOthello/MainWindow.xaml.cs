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

namespace ProjetOthello
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Proprety

        int iSize = 0;
        int iActualPlayerId = 0;
        Token[][] tokensBoard;

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            iSize = GameParameter.iSize;
            InitializationBoard();
            InitializationGame();
        }

        #endregion

        #region Initialization

        private void InitializationBoard()
        {
            tokensBoard = new Token[iSize][];
            for(int i = 0; i < iSize; i++)
            {
                for (int j = 0; j < iSize; j++)
                {
                    if(i==0)
                        tokensBoard[j] = new Token[iSize];
                    Button btnNewCell = new Button();
                    btnNewCell.IsEnabled = true;
                    btnNewCell.Uid = j + ";" + i;
                    btnNewCell.MouseEnter +=  new MouseEventHandler(MouseEnterCell);
                    btnNewCell.MouseLeave += new MouseEventHandler(MouseLeaveCell);
                    Grid.SetRow(btnNewCell, i);
                    Grid.SetColumn(btnNewCell, j);
                    tokensBoard[j][i] = new Token(btnNewCell);
                    gridBoard.Children.Add(btnNewCell);
                }
            }

        }

        

        #endregion

        private void InitializationGame()
        {
            //Initialize the fourth first tokens
            tokensBoard[(int)iSize / 2 - 1][(int)iSize / 2 - 1].UpdateToken(iActualPlayerId);
            tokensBoard[(int)iSize / 2][(int)iSize / 2].UpdateToken(iActualPlayerId);

            ChangeTurn();

            tokensBoard[(int)iSize / 2][(int)iSize / 2 - 1].UpdateToken(iActualPlayerId);
            tokensBoard[(int)iSize / 2 - 1][(int)iSize / 2].UpdateToken(iActualPlayerId);
        }

        #region Function

        private void ChangeTurn(){iActualPlayerId = (iActualPlayerId == 0) ? 1 : 0;}

        private void ResetPlayableToken() { tokensBoard.ToList().ForEach(list => list.ToList().ForEach(token => token.IsPlayable = false)); }

        private bool IsCellPlayable(int x, int y)
        {
            return true;
        }

        #endregion

        #region Event

        private void MouseEnterCell(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            string[] strUid = btn.Uid.Split(';');
            int iX = 0;
            int iY = 0;
            try { iX = Convert.ToInt32(strUid[0]); iY = Convert.ToInt32(strUid[1]); }
            catch{ Console.WriteLine("Btn.Uid is not integer.");}
            Token tokenRef = tokensBoard[iX][iY];
            if (tokenRef.ITokenValue == -1)
            {
                if (!tokenRef.IsPlayable)
                    tokenRef.IsPlayable = IsCellPlayable(iX, iY);
                Image imgToken = new Image();
                imgToken.Source = GameParameter.imageIndex[iActualPlayerId];
                btn.Content = imgToken;
            }
        }

        private void MouseLeaveCell(object sender, MouseEventArgs e)
        {            
            Button btn = (Button)sender;
            string[] strUid = btn.Uid.Split(';');
            if(tokensBoard[Convert.ToInt32(strUid[1])][Convert.ToInt32(strUid[0])].ITokenValue == -1)
                btn.Content = "";
        }
        #endregion
    }
}
