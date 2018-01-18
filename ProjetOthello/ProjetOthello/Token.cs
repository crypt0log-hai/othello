using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetOthello
{
    class Token
    {

        #region Propreties        

        private bool IsPlayable = false;        //Usefull to not reverified if a cell is playable, everytime user enter in it
        public List<int[]> lTokenCoordTarget;   //List of token who would be changed if you play this move
        public int x, y;                        //Position of the token

        #endregion

        #region Getter/Setter
        
        public bool IIsPlayable { get => IsPlayable; set => IsPlayable = value; }
        public List<int[]> LTokenCoordTarget { get => lTokenCoordTarget; set => lTokenCoordTarget = value; }

        #endregion
        
        #region Constructor
        
        public Token(int x, int y)
        {
            LTokenCoordTarget = new List<int[]>();
            this.x = x;
            this.y = y;
        }

        #endregion

        #region Function        

        /// <summary>
        /// Reset the list of target and IsPlayable, call every end of a turn
        /// </summary>
        public void ResetTokenList()
        {
            IsPlayable = false;
            lTokenCoordTarget = new List<int[]>();
        }

        #endregion
        
    }
}
