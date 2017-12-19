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

        List<Image> lImgPortraits;
        int iSelectedPortrait = -1;

        BitmapImage btmVoidPortait;

        public SelectionMenu()
        {
            InitializeComponent();
            
            string[] tPathsToSelectedImage = { "Carrino.png" };
            tbtmPortrait = new BitmapImage[6];
            lImgPortraits = new List<Image>();
            for (int i = 0; i < tPathsToSelectedImage.Length; i++)
            {
                tbtmPortrait[i] = new BitmapImage();
                tbtmPortrait[i].BeginInit();
                tbtmPortrait[i].UriSource = new Uri("pack://application:,,,/Assets/Menu/Portrait/" + tPathsToSelectedImage[i], UriKind.RelativeOrAbsolute);
                tbtmPortrait[i].EndInit();
                Image imgSelected = new Image();
                imgSelected.Source = tbtmPortrait[i];
                lImgPortraits.Add(imgSelected);
            }

            btmVoidPortait = new BitmapImage();
            btmVoidPortait.BeginInit();
            btmVoidPortait.UriSource = new Uri("pack://application:,,,/Assets/Menu/Portrait/Inconnu.png", UriKind.RelativeOrAbsolute);
            btmVoidPortait.EndInit();

            rSelected = new Rectangle[2];
            rSelected[0] = rPlayer1;
            rSelected[1] = rPlayer2;
            btnPortrait0.Content = lImgPortraits[0];


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
