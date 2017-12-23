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
                case "1":
                case "2":
                    GameParameter.iGameMod = Convert.ToInt32(btnEvent.Uid);
                    SelectionMenu selectionMenu = new SelectionMenu();
                    selectionMenu.Show();
                    this.Close();
                    break;
                case "3":
                    OptionMenu optionMenu = new OptionMenu();
                    optionMenu.Show();
                    this.Close();
                    break;
                case "4":
                    this.Close();
                    break;
            }
            
        }
    }
}
