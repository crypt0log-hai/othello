using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProjetOthello
{
    class LoadSaveHandler
    {
        public LoadSaveHandler() { }

        public void SaveGame()
        {
            string strName = GameParameter.tCharacterNames[0] + "_" + GameParameter.tCharacterNames[1] + "_Turn_" + GameParameter.iNbTurn + "_" + DateTime.Now.ToString("dd_MM_yyyy_H_mm_ss");
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter("pack://application:,,,/Save/"+strName+".txt"))
            {
                //foreach (string line in lines)
                //{
                //    // If the line doesn't contain the word 'Second', write the line to the file.
                //    if (!line.Contains("Second"))
                //    {
                //        file.WriteLine(line);
                //    }
                //}
            }
        }

    }
}
