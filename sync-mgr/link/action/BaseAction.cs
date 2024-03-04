using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace link
{
    public abstract class BaseAction
    {
        public LinkView ui;
        public virtual bool OnMouseDown(MouseEventArgs e) => false;
        public virtual bool OnMouseMove(MouseEventArgs e) => false;
        public virtual bool OnMouseUp(MouseEventArgs e) => false;
        public virtual bool OnKeyDown(KeyEventArgs e) => false;
        public virtual bool OnKeyUp(KeyEventArgs e) => false;
        public virtual bool OnMouseDoubleClick(MouseEventArgs e) => false;
        public abstract void stop();
        public void reset()
        {
            if (null != ui)
                ui.action = null;
        }
    }
}
