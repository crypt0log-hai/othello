﻿using System;
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

        BitmapImage portrait = new BitmapImage();

        public SelectionMenu()
        {
            InitializeComponent();
            BitmapImage bitmapImage1 = new BitmapImage();
            bitmapImage1.BeginInit();
            bitmapImage1.UriSource = new Uri("pack://application:,,,/Assets/Tokens/BlackToken.png", UriKind.RelativeOrAbsolute);
            bitmapImage1.EndInit();
            BitmapImage bitmapImage2 = new BitmapImage();
            bitmapImage2.BeginInit();
            bitmapImage2.UriSource = new Uri("pack://application:,,,/Assets/Tokens/WhiteToken.png", UriKind.RelativeOrAbsolute);
            bitmapImage2.EndInit();
            Image img1 = new Image();
            img1.Source = bitmapImage1;
            Image img2 = new Image();
            img2.Source = bitmapImage2;

            portrait.BeginInit();
            portrait.UriSource = new Uri("pack://application:,,,/Assets/Menu/Portrait/Carrino.png", UriKind.RelativeOrAbsolute);
            portrait.EndInit();
            Image img3 = new Image();
            img3.Source = portrait;

            GameParameter.imageIndex[0] = bitmapImage2;
            GameParameter.imageIndex[1] = bitmapImage1;
            btn0.Content = img3;
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
            btnNext.Content = btnEvent.Uid;
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = portrait;
            rPlayer1.Fill = imageBrush;
        }
    }
}