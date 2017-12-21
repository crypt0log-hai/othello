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
using System.Windows.Shapes;

namespace ProjetOthello
{
    /// <summary>
    /// Logique d'interaction pour SelectionMenu.xaml
    /// </summary>
    public partial class SelectionMenu : Window
    {

        BitmapImage[] tbtmPortrait;

        Rectangle[] rSelected;
        int iChooseTurn = 0;
        
        int iSelectedPortrait = -1;

        BitmapImage btmVoidPortait;
        //Passer en ressource
        int nbSelection = 6;
        Button[] btnSelection;

        public SelectionMenu()
        {
            InitializeComponent();

            string[] tPathsToSelectedImage = GameParameter.tNameToken;
            tbtmPortrait = new BitmapImage[nbSelection];
            btnSelection = new Button[nbSelection];
            btnSelection[0] = btnPortrait0;
            btnSelection[1] = btnPortrait1;
            btnSelection[2] = btnPortrait2;
            btnSelection[3] = btnPortrait3;
            btnSelection[4] = btnPortrait4;
            for (int i = 0; i < tPathsToSelectedImage.Length; i++)
            {
                tbtmPortrait[i] = new BitmapImage();
                tbtmPortrait[i].BeginInit();
                tbtmPortrait[i].UriSource = new Uri("pack://application:,,,/Assets/Menu/Portrait/" + tPathsToSelectedImage[i] + ".png",  UriKind.RelativeOrAbsolute);
                tbtmPortrait[i].EndInit();

                BitmapImage btmSelection = new BitmapImage();
                btmSelection.BeginInit();
                btmSelection.UriSource = new Uri("pack://application:,,,/Assets/Menu/Selection/" + tPathsToSelectedImage[i] + ".png", UriKind.RelativeOrAbsolute);
                btmSelection.EndInit();

                Image imgSelection = new Image();
                imgSelection.Source = btmSelection;
                btnSelection[i].Content = imgSelection;
            }

            btmVoidPortait = new BitmapImage();
            btmVoidPortait.BeginInit();
            btmVoidPortait.UriSource = new Uri("pack://application:,,,/Assets/Menu/Selection/QuestionMark.png", UriKind.RelativeOrAbsolute);
            btmVoidPortait.EndInit();

            rSelected = new Rectangle[2];
            rSelected[0] = rPlayer1;
            rSelected[1] = rPlayer2;


            UpdatePlayerImage(0);
            UpdatePlayerImage(1);

        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btnPortraits_click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPortrait_mouseEnter(object sender, MouseEventArgs e)
        {
            Button btnEvent = (Button)sender;
            try { iSelectedPortrait = Convert.ToInt32(btnEvent.Uid); }
            catch { Console.Write("Select Id is not integer"); iSelectedPortrait = -1;}
            UpdatePlayerImage(iChooseTurn);
        }

        private void btnPortrait_mouseLeave(object sender, MouseEventArgs e)
        {
            Button btnEvent = (Button)sender;
            if (iSelectedPortrait.ToString() == btnEvent.Uid)
            {
                iSelectedPortrait = -1;
                UpdatePlayerImage(iChooseTurn);
            }

        }

        private void UpdatePlayerImage(int iPlayerId)
        {
            ImageBrush imageBrush = new ImageBrush();
            if (iSelectedPortrait == -1)
            {
                imageBrush.ImageSource = btmVoidPortait;
                rSelected[iPlayerId].Fill = imageBrush;
            }
            else
            {
                imageBrush.ImageSource = tbtmPortrait[iSelectedPortrait];
                rSelected[iPlayerId].Fill = imageBrush;
            }

        }
    }
}
