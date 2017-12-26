using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProjetOthello
{
    class LoadSaveHandler
    {
        public LoadSaveHandler() { }

        public void SaveGame()
        {
            string strNameFile = GameParameter.tCharacterNames[0] + "_" + GameParameter.tCharacterNames[1] + "_Turn_" + GameParameter.iNbTurn + "_" + DateTime.Now.ToString("dd_MM_yyyy_H_mm_ss");
            string strName = "Name," + GameParameter.tCharacterNames[0] + "," + GameParameter.tCharacterNames[1];
            int iSize = GameParameter.iSize;
            string strSize = "Size," + iSize;
            List<Tuple<int, int[,], string>> lHistoryGame = GameParameter.lHistoryGame;

            if (!Directory.Exists("./Save"))
                Directory.CreateDirectory("./Save");
            using (StreamWriter file = new StreamWriter("./Save/"+ strNameFile + ".txt"))
            {
                file.WriteLine(strName);
                file.WriteLine(strSize);
                foreach (Tuple<int,int[,], string> historyValue in lHistoryGame)
                {
                    string strIdPlayer = "Turn," + historyValue.Item1.ToString();
                    string strTime = "Time," + historyValue.Item3;
                    int[,] iBoards = historyValue.Item2;
                    file.WriteLine(strIdPlayer);
                    file.WriteLine(strTime);
                    file.WriteLine("Board");
                    for(int i = 0; i < iSize; i++)
                    {
                        string strLine = "";
                        for (int j = 0; j < iSize; j++)
                        {
                            strLine += iBoards[j, i];
                            strLine += ",";
                        }
                        file.WriteLine(strLine);
                    }
                    file.WriteLine("-");
                }
                file.Flush();
                file.Close();
            }
        }

        public void LoadGame(string strNameFile)
        {
            using (StreamReader file = new StreamReader(strNameFile))
            {
                int iSize = 0;
                bool blReadBoard = false;
                int iTurn = 0;
                string strTime = "";
                int[,] tBoard = new int[0,0];
                int iY = 0;
                GameParameter.lHistoryGame = new List<Tuple<int, int[,], string>>();

                while(!file.EndOfStream)
                {
                    string line = file.ReadLine();                    
                    string[] tComponent = line.Split(',');

                    if (line.Equals("-"))
                    {
                        GameParameter.lHistoryGame.Add(new Tuple<int, int[,], string>(iTurn, tBoard, strTime));
                        blReadBoard = false;
                    }
                    else
                    {
                        if (blReadBoard)
                        {
                            for (int iX = 0; iX < iSize; iX++)
                                tBoard[iX, iY] = Convert.ToInt32(tComponent[iX]);
                            iY++;
                        }
                        else
                        {
                            if (line.Equals("Board"))
                            {
                                blReadBoard = true;
                                tBoard = new int[iSize,iSize];
                                iY = 0;
                            }
                            else
                            {
                                switch (tComponent[0])
                                {
                                    case "Name":
                                        GameParameter.tCharacterNames[0] = tComponent[1];
                                        GameParameter.tCharacterNames[1] = tComponent[2];
                                        break;
                                    case "Size":
                                        GameParameter.iSize = iSize = Convert.ToInt32(tComponent[1]);
                                        break;
                                    case "Turn":
                                        iTurn = Convert.ToInt32(tComponent[1]);
                                        break;
                                    case "Time":
                                        strTime = tComponent[1];
                                        break;
                                }
                            }
                        }
                    }

                }
            }
        }

    }
}
