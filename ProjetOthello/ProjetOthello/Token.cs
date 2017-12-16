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
        public Button btn;
        public int iTokenValue = -1;
        public Token(Button _btn)
        {
            btn = _btn;
            btn.Content = GameParameter.imageIndex[0];
        }

        
    }
}
