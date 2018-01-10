using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjetOthello
{
    class Token
    {

        #region Propreties

        //Reference of grid buttons
        private Button btnContainer;
        private bool iIsPlayable = false;
        public List<int[]> lTokenCoordTarget;
        public int x, y;
        #endregion


        #region Getter/Setter

        public Button BtnContainer { get => btnContainer; set => btnContainer = value; }
        public bool IIsPlayable { get => iIsPlayable; set => iIsPlayable = value; }
        public List<int[]> LTokenCoordTarget { get => lTokenCoordTarget; set => lTokenCoordTarget = value; }

        #endregion


        #region Constructor

        public Token(Button _btnContainer, int x, int y)
        {            
            BtnContainer = _btnContainer;
            LTokenCoordTarget = new List<int[]>();
            this.x = x;
            this.y = y;
        }

        public Token(int x, int y)
        {
            LTokenCoordTarget = new List<int[]>();
            this.x = x;
            this.y = y;
        }

        #endregion

        #region Function        

        public void UpdateToken(int iPlayerId)
        {
            Image imgToken = new Image();
            imgToken.Source = GameParameter.tbtmTokenIndex[iPlayerId];
            btnContainer.Content = imgToken;
            btnContainer.Background = GameParameter.tColorBackgroundCell[iPlayerId];
        }

        public void TokenResetDisplay()
        {
            btnContainer.Content = "";
        }

        public void ResetTokenList()
        {
            IIsPlayable = false;
            LTokenCoordTarget = new List<int[]>();
        }

        #endregion



    }
}
