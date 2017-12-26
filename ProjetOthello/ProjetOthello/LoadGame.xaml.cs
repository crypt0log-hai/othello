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
using System.Windows.Shapes;

namespace ProjetOthello
{
    /// <summary>
    /// Logique d'interaction pour LoadGame.xaml
    /// </summary>
    public partial class LoadGame : Window
    {
        int iMaxIndex = 0;
        int iSize;
        List<Tuple<int, int[,], string>> lHistoryGame;
        Rectangle[,] tRectangle;
        GameInfo gInfo = new GameInfo { Index = 0 };
        BitmapImage[] tbtmToken = new BitmapImage[2];


        public LoadGame()
        {
            InitializeComponent();
            int iCpt = 0;
            lHistoryGame = GameParameter.lHistoryGame;
            foreach(string name in GameParameter.tCharacterNames)
            {
                tbtmToken[iCpt] = new BitmapImage();
                tbtmToken[iCpt].BeginInit();
                tbtmToken[iCpt].UriSource = new Uri("pack://application:,,,/Assets/Game/Tokens/" + name + ".png", UriKind.RelativeOrAbsolute);
                tbtmToken[iCpt].EndInit();
                iCpt++;
            }
            GameParameter.tbtmTokenIndex = tbtmToken;

            iSize = GameParameter.iSize;
            tRectangle = new Rectangle[iSize, iSize];
            for (int i = 0; i < iSize;i++)
            {
                for (int j = 0; j < iSize; j++)
                {
                    if (i == 0)
                    {
                        RowDefinition row = new RowDefinition();
                        ColumnDefinition column = new ColumnDefinition();
                        gBoard.RowDefinitions.Add(row);
                        gBoard.ColumnDefinitions.Add(column);
                    }

                    Rectangle rectangle = new Rectangle();
                    tRectangle[j, i] = rectangle;
                    Grid.SetColumn(rectangle, j);
                    Grid.SetRow(rectangle, i);
                    gBoard.Children.Add(rectangle);
                }
            }

            this.DataContext = gInfo;

            iMaxIndex = GameParameter.lHistoryGame.Count - 1;
            sIndex.Minimum = 0;
            sIndex.Maximum = iMaxIndex;
            IndexChanged();
        }


        private void IndexChanged()
        {
            int[,] tiBoard = lHistoryGame[gInfo.Index].Item2;
            for (int i = 0; i < iSize; i++)
                for (int j = 0; j < iSize; j++)
                    if (tiBoard[j, i] != -1)
                        tRectangle[j, i].Fill = new ImageBrush(tbtmToken[tiBoard[j, i]]);
                    else
                        tRectangle[j, i].Fill = new ImageBrush();
        }

        private void UpButtons()
        {
            btnAdd.IsEnabled = btnMinus.IsEnabled = true;
            if (gInfo.Index == 0)
                btnMinus.IsEnabled = false;
            if (gInfo.Index == iMaxIndex)
                btnAdd.IsEnabled = false;
        }


        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainMenu menu = new MainMenu();
            menu.Show();
            this.Close();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            MainWindow gameWindow = new MainWindow(lHistoryGame[gInfo.Index]);
            gameWindow.Show();
            this.Close();
        }
        
        private void btnParameterClick(object sender, RoutedEventArgs e)
        {
            Button btnParameter = (Button)sender;
            
            if(btnParameter.Uid.Equals("0"))
                gInfo.Index--;
            else
                gInfo.Index++;
            IndexChanged();
            UpButtons();
        }

        private void sIndex_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpButtons();
            IndexChanged();
        }
    }

    public class GameInfo : INotifyPropertyChanged
    {
        private int iIndex;

        public int Index
        {
            get { return iIndex; }
            set
            {
                if (iIndex != value)
                {
                    iIndex = value;
                    RaisePropertyChanged("Index");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
