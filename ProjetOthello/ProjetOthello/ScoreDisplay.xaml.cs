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
    /// Logique d'interaction pour ScoreDisplay.xaml
    /// </summary>
    public partial class ScoreDisplay : Window
    {
        MainWindow gameWindow;
        bool blIsOver;
        public ScoreDisplay(MainWindow gameWindow, bool blIsOver)
        {
            InitializeComponent();
            this.Closing += OnWindowClosing;
            this.blIsOver = blIsOver;
            this.gameWindow = gameWindow;
           
            string strName;
            if (blIsOver)
            {
                strName = "Menu,Restart,Save,Exit";
                ShowScore();
            }
            else
                strName = "Resume,Menu,Restart,Save,Exit";
            string[] tButtonsName = strName.Split(',');
            foreach(string name in tButtonsName)
            {
                Button btnName = new Button();
                btnName.Uid = name;
                btnName.Content = name;
                btnName.Click += Button_Click;
                spMenuButtons.Children.Add(btnName);
            }
        }

        private void ShowScore()
        {
            int iWinner = GameParameter.iWinner;
            rWinner.Fill = new ImageBrush(GameParameter.tbtmTokenIndex[iWinner]);
            lblWinner.Visibility = Visibility.Visible;
            lblScore.Content = "Score : " + GameParameter.tScore[0] + " - " + GameParameter.tScore[1];
            lblTime.Content = "Timer : " + GameParameter.tTime[0] + " - " + GameParameter.tTime[1];
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btnSend = (Button)sender;
            switch(btnSend.Uid)
            {
                case "Resume":
                    gameWindow.dispatcherTimer.IsEnabled = true;
                    gameWindow.dispatcherTimer.Start();
                    gameWindow.IsEnabled = true;
                    this.Close();
                    break;
                case "Menu":
                    MainMenu mainMenu = new MainMenu();
                    mainMenu.Show();
                    gameWindow.Close();
                    this.Close();
                    break;
                case "Restart":
                    gameWindow.IsEnabled = true;
                    gameWindow.Restart();
                    this.Close();
                    break;
                case "Save":
                    btnSend.IsEnabled = false;
                    GameParameter.lHistoryGame = gameWindow.lHistoryGame;
                    LoadSaveHandler saveHandler = new LoadSaveHandler();
                    saveHandler.SaveGame();
                    break;
                case "Exit":
                    gameWindow.Close();
                    this.Close();
                    break;

            }
            
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (blIsOver)
                gameWindow.Close();
            else
            {
                gameWindow.dispatcherTimer.IsEnabled = true;
                gameWindow.IsEnabled = true;
            }
        }
    }
}
