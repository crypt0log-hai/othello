using System;
using System.Collections.Generic;
using System.IO;
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
    /// Logique d'interaction pour OptionMenu.xaml
    /// </summary>
    public partial class OptionMenu : Window
    {
        bool blOnLoad = true;
        public OptionMenu()
        {
            InitializeComponent();
            if (GameParameter.isMusiqueEnabled)
                cbxMusic.IsChecked = true;
            ReadCredit();
        }

        private void ReadCredit()
        {
            string strCredits = "";
            using (StreamReader file = new StreamReader("./Assets/Credit.txt"))
            {
                while (!file.EndOfStream)
                    strCredits += file.ReadLine() +"\n";
            }
            lblCredit.Content = strCredits;
        }

        
        private void btnBack_Clic(object sender, RoutedEventArgs e)
        {
            MainMenu mainMenu = new MainMenu();
            mainMenu.Show();
            this.Close();
        }

        private void cbxMusicClick(object sender, RoutedEventArgs e)
        {
            if (cbxMusic.IsChecked.Value)
                GameParameter.isMusiqueEnabled = true;
            else
                GameParameter.isMusiqueEnabled = false;
            
        }
    }
}
