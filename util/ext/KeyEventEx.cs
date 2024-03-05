using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace util.ext
{
    public static class KeyEventEx
    {
        public static bool CtrlV(this KeyEventArgs e)
            => e.Control && e.KeyCode == Keys.V;

        public static bool CtrlS(this KeyEventArgs e)
            => e.Control && e.KeyCode == Keys.S;
    }
}
