﻿using System;
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
            Token_ChangeDisplay(iPlayerId);
        }

        public void Token_ChangeDisplay(int iPlayerId)
        {
            Image imgToken = new Image();
            imgToken.Source = GameParameter.imageIndex[iPlayerId];
            btnContainer.Content = imgToken;
        }

        #endregion



    }
}
