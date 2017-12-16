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
        int iSize = 8;
        Token[][] tokensBoard;
        public MainWindow()
        {
            InitializeComponent();
            Initialization();
        }

        private void Initialization()
        {
            tokensBoard = new Token[iSize][];
            for(int i = 0; i < iSize; i++)
            {
                tokensBoard[i] = new Token[iSize];
                for (int j = 0; j < iSize; j++)
                {
                    System.Windows.Controls.Button newBtn = new Button();
                    newBtn.IsEnabled = true;
                    
                    Grid.SetRow(newBtn, i);
                    Grid.SetColumn(newBtn, j);
                    tokensBoard[i][j] = new Token(newBtn);
                    gridBoard.Children.Add(newBtn);
                }
            }


        }
    }
}
