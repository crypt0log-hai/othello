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


        List<Button> lbtnMenu;
        List<string[]> lButtonName;

        int iMenuState = 0;

        public MainMenu()
        {
            InitializeComponent();
            lButtonName = new List<string[]>();
            string[] tNames0 = { "NewGame", "Load", "Option", "Exit" };
            string[] tNames1 = { "1 vs 1", "1 vs IA", "IA vs IA", "Back" };
            lButtonName.Add(tNames0);
            lButtonName.Add(tNames1);
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            spMenuButton.Children.RemoveRange(0, spMenuButton.Children.Count);
            foreach(string name in lButtonName[iMenuState])
            {
                Button btnMenu = new Button();
                btnMenu.Uid = name;
                btnMenu.Content = name;
                btnMenu.Click += buttonsClicked;
                spMenuButton.Children.Add(btnMenu);
            }
        }


        private void buttonsClicked(object sender, RoutedEventArgs e)
        {
            Button btnEvent = (Button)sender;
            switch(btnEvent.Uid)
            {
                case "NewGame":
                    iMenuState = 1;
                    UpdateButtons();
                    break;
                case "Load":
                    break;
                case "Option":
                    OptionMenu optionMenu = new OptionMenu();
                    optionMenu.Show();
                    this.Close();
                    break;
                    break;
                case "Exit":
                    this.Close();
                    break;
                case "1 vs 1":
                    GameParameter.iGameMod = 0;
                    SelectionMenu selectionMenu = new SelectionMenu();
                    selectionMenu.Show();
                    this.Close();
                    break;
                case "1 vs IA":
                    break;
                case "IA vs IA":
                    break;
                case "Back":
                    iMenuState = 0;
                    UpdateButtons();
                    break;
            }
            
        }
    }
}
