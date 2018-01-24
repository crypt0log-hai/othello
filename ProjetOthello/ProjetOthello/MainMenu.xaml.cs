using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ProjetOthello
{
    /// <summary>
    /// Logique d'interaction pour MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {


        List<Button> lbtnMenu;
        List<string[]> lButtonName;
        string[] filePaths;
        int iMenuState = 0;
        SoundEngine soundEngine;

        public MainMenu()
        {
            InitializeComponent();
            soundEngine = new SoundEngine("./Assets/Sound/menu_theme.mp3");
            lButtonName = new List<string[]>();
            string[] tNames0 = { "NewGame", "Load", "Option", "Exit" };
            string[] tNames1 = { "1 vs 1", "1 vs IA", "IA vs IA", "Back" };
            string[] tNames2 = { "Back"};
            lButtonName.Add(tNames0);
            lButtonName.Add(tNames1);
            lButtonName.Add(tNames2);
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            spMenuButton.Children.RemoveRange(0, spMenuButton.Children.Count);
            if (iMenuState == 2)
            {
                ScrollViewer svLoadName = new ScrollViewer();
                svLoadName.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                svLoadName.MaxHeight = 250;
                spMenuButton.Children.Add(svLoadName);

                StackPanel spLoadName = new StackPanel();
                if (!Directory.Exists("./Save"))
                    Directory.CreateDirectory("./Save");
                filePaths = Directory.GetFiles("./Save");
                int iCpt = 0;
                foreach(string path in filePaths)
                {
                    string[] tComponent = path.Split('\\');
                    tComponent = tComponent[1].Split('.');
                    tComponent = tComponent[0].Split('_');
                    string strName = tComponent[0] + "VS" + tComponent[1] + " T" + tComponent[3] + " " + tComponent[4] + "." + tComponent[5] + "." + tComponent[6] +
                        " " + tComponent[7] + ":" + tComponent[8] + ":" + tComponent[9];
                    Button btn = new Button();
                    btn.Content = strName;
                    btn.FontSize = 15;
                    btn.Uid = ""+iCpt;
                    btn.Click += LoadSavedGame;
                    spLoadName.Children.Add(btn);
                    iCpt++;
                }
                svLoadName.Content = spLoadName;
            } 

            foreach (string name in lButtonName[iMenuState])
            {
                Button btnMenu = new Button();
                btnMenu.Uid = name;
                btnMenu.Content = name;
                btnMenu.Click += buttonsClicked;
                spMenuButton.Children.Add(btnMenu);
            }
            
        }

        private void LoadSavedGame(object sender, RoutedEventArgs e)
        {
            Button btnEvent = (Button)sender;
            int iId = Convert.ToInt32(btnEvent.Uid);
            LoadSaveHandler loadHandler = new LoadSaveHandler();
            loadHandler.LoadGame(filePaths[iId]);
            LoadGame loadGame = new LoadGame();
            loadGame.Show();
            this.Close();
        }

        private void buttonsClicked(object sender, RoutedEventArgs e)
        {
            Button btnEvent = (Button)sender;
            SelectionMenu selectionMenu;
            switch (btnEvent.Uid)
            {
                case "NewGame":
                    iMenuState = 1;
                    UpdateButtons();
                    break;
                case "Load":
                    iMenuState = 2;
                    UpdateButtons();
                    break;
                case "Option":
                    OptionMenu optionMenu = new OptionMenu();
                    optionMenu.Show();
                    this.Close();
                    break;
                case "Exit":
                    this.Close();
                    break;
                case "1 vs 1":
                    GameParameter.isIA = new bool[2] { false, false };
                    selectionMenu = new SelectionMenu();
                    selectionMenu.Show();
                    this.Close();
                    break;
                case "1 vs IA":
                    GameParameter.isIA = new bool[2] { false, true };
                    selectionMenu = new SelectionMenu();
                    selectionMenu.Show();
                    this.Close();
                    break;
                case "IA vs IA":
                    GameParameter.isIA = new bool[2] { true, true };
                    selectionMenu = new SelectionMenu();
                    selectionMenu.Show();
                    this.Close();
                    break;
                case "Back":
                    iMenuState = 0;
                    UpdateButtons();
                    break;
            }
            
        }
    }
}
