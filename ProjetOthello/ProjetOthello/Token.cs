using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace ProjetOthello
{
    class Token
    {

        #region Propreties

        //Reference of grid buttons
        private Button btnContainer;
        private int iTokenValue = -1;
        private bool isPlayable = false;

        #endregion


        #region Getter/Setter

        public Button BtnContainer { get => btnContainer; set => btnContainer = value; }
        public int ITokenValue { get => iTokenValue;}
        public bool IsPlayable { get => isPlayable; set => isPlayable = value; }

        #endregion


        #region Constructor

        public Token(Button _btnContainer)
        {
            BtnContainer = _btnContainer;
        }

        #endregion

        #region Function

        public void UpdateToken(int iPlayerId)
        {
            iTokenValue = iPlayerId;
            TokenChangeDisplay(iPlayerId);
        }

        public void TokenChangeDisplay(int iPlayerId)
        {
            Image imgToken = new Image();
            imgToken.Source = GameParameter.imageIndex[iPlayerId];
            btnContainer.Content = imgToken;
        }

        public void TokenResetDisplay()
        {
            if (iTokenValue == -1)
                btnContainer.Content = "";
        }

        #endregion



    }
}
