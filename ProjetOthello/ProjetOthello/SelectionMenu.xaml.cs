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
using System.Windows.Threading;

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
        
        int iSelectPortrait = -1;

        BitmapImage btmVoidPortait;

        int iSelectedPortrait = -1;

        //Passer en ressource
        int iNbChar = GameParameter.iNbCharacter;
        Button[] tbtnSelection;
        string[] tNameCharacter = GameParameter.tNameCharacter;


        DispatcherTimer timerAnimation;
        int iValMaxAnimation = 0;
        int iValAnimation = 0;

        public SelectionMenu()
        {
            InitializeComponent();
            tbtmPortrait = new BitmapImage[iNbChar];
            tbtnSelection = new Button[iNbChar];
            for (int i = 0; i < iNbChar; i++)
            {
                Button btnSelection = new Button();
                btnSelection.Uid = i.ToString();
                btnSelection.Click += btnPortraits_click;
                btnSelection.MouseEnter += btnPortrait_mouseEnter;
                btnSelection.MouseLeave += btnPortrait_mouseLeave;
                panelSelection.Children.Add(btnSelection);
                tbtnSelection[i] = btnSelection;


                tbtmPortrait[i] = new BitmapImage();
                tbtmPortrait[i].BeginInit();
                tbtmPortrait[i].UriSource = new Uri("pack://application:,,,/Assets/Menu/Portrait/" + tNameCharacter[i] + ".png",  UriKind.RelativeOrAbsolute);
                tbtmPortrait[i].EndInit();
                

                Image imgSelection = new Image();
                imgSelection.Source = tbtmPortrait[i];
                tbtnSelection[i].Content = imgSelection;
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

            ControlIA();

            
        }

        private void UpdatePlayerImage(int iPlayerId)
        {
            ImageBrush imageBrush = new ImageBrush();
            if (iSelectPortrait == -1)
            {
                imageBrush.ImageSource = btmVoidPortait;
                rSelected[iPlayerId].Fill = imageBrush;
            }
            else
            {
                if (iPlayerId == 0)
                    tbxNamePlayer1.Text = tNameCharacter[iSelectPortrait];
                else
                    tbxNamePlayer2.Text = tNameCharacter[iSelectPortrait];
                imageBrush.ImageSource = tbtmPortrait[iSelectPortrait];
                rSelected[iPlayerId].Fill = imageBrush;
            }

        }

        private void ControlIA()
        {
            if(GameParameter.isIA[iChooseTurn])
            {
                iValMaxAnimation = new Random().Next(50, 75);
                iValAnimation = 0;
                timerAnimation = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 25), DispatcherPriority.Background,
                timerAnimation_Update, Dispatcher.CurrentDispatcher);
                timerAnimation.IsEnabled = true;
                timerAnimation.Start();
            }
        }


        private void PortraitSelect()
        {
            BitmapImage btmChoose = new BitmapImage();
            btmChoose.BeginInit();
            btmChoose.UriSource = new Uri("pack://application:,,,/Assets/Game/Tokens/" + tNameCharacter[iSelectPortrait] + ".png", UriKind.RelativeOrAbsolute);
            btmChoose.EndInit();
            GameParameter.tCharacterNames[iChooseTurn] = tNameCharacter[iSelectPortrait];
            GameParameter.tbtmTokenIndex[iChooseTurn] = btmChoose;

            iSelectedPortrait = iSelectPortrait;
            if (iChooseTurn > 0)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                iChooseTurn++;
                ControlIA();
            }
        }

        #region Events

        private void timerAnimation_Update(object sender, EventArgs e)
        {
            if(iValAnimation >= iValMaxAnimation)
            {
                timerAnimation.IsEnabled = false;
                timerAnimation.Stop();
                PortraitSelect();
            }
            else
            {
                iValAnimation++;
                iSelectPortrait++;
                if (iSelectPortrait == iSelectedPortrait)
                    iSelectPortrait++;
                if (iSelectPortrait >= iNbChar)
                    iSelectPortrait = 0;
                UpdatePlayerImage(iChooseTurn);
            }
        }

        private void btnPortraits_click(object sender, RoutedEventArgs e)
        {
            if(!GameParameter.isIA[iChooseTurn])
                PortraitSelect();
        }

        private void btnPortrait_mouseEnter(object sender, MouseEventArgs e)
        {
            if (!GameParameter.isIA[iChooseTurn])
            {
                Button btnEvent = (Button)sender;
                try { iSelectPortrait = Convert.ToInt32(btnEvent.Uid); }
                catch { Console.Write("Select Id is not integer"); iSelectPortrait = -1; }
                if (iSelectPortrait != iSelectedPortrait)
                    UpdatePlayerImage(iChooseTurn);
            }
        }

        private void btnPortrait_mouseLeave(object sender, MouseEventArgs e)
        {
            if (!GameParameter.isIA[iChooseTurn])
            {
                Button btnEvent = (Button)sender;
                if (iSelectPortrait.ToString() == btnEvent.Uid)
                {
                    iSelectPortrait = -1;
                    UpdatePlayerImage(iChooseTurn);
                }
            }

        }

#endregion

    }
}
