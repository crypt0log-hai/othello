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

        private Button btnContainer;
        private int iTokenValue = -1;

        #endregion


        #region Getter/Setter

        public Button BtnContainer { get => btnContainer; set => btnContainer = value; }

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
            Image imgToken = new Image();
            imgToken.Source = GameParameter.imageIndex[iTokenValue];
            btnContainer.Content = imgToken;
        }

        #endregion



    }
}
