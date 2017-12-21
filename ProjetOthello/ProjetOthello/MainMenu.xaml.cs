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
    /// Logique d'interaction pour MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void buttonsClicked(object sender, RoutedEventArgs e)
        {
            Button btnEvent = (Button)sender;
            switch(btnEvent.Uid)
            {
                case "0":
                    GameParameter.iStateGame = 0;
                    break;
                case "1":
                    GameParameter.iStateGame = 1;
                    break;
                case "2":
                    GameParameter.iStateGame = 2;
                    break;
            }
            /*
            OptionMenu optionMenu = new OptionMenu();
            optionMenu.Show();
            this.Close();
            */
            SelectionMenu selectionMenu = new SelectionMenu();
            selectionMenu.Show();
            this.Close();
            
        }
    }
}
