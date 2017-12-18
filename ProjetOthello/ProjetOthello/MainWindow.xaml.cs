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

        #endregion

        #region Event

        private void MouseEnterCell(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            Image imgToken = new Image();
            imgToken.Source = GameParameter.imageIndex[iActualPlayerId];
            btn.Content = imgToken;
        }

        private void MouseLeaveCell(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            btn.Content = "";
        }
        #endregion
    }
}
