using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Muse2
{
    public static class CloseWindow
    {
        public static Window win = null;
        public static void CloseParent()
        { 
            if (win != null) 
            {
                win.Close();
            }
        }
    }
}
