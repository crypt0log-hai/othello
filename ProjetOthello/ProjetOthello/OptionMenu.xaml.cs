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
    /// Logique d'interaction pour OptionMenu.xaml
    /// </summary>
    public partial class OptionMenu : Window
    {
        public OptionMenu()
        {
            InitializeComponent();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            SelectionMenu selectionMenu = new SelectionMenu();
            selectionMenu.Show();
            this.Close();
        }
    }
}
