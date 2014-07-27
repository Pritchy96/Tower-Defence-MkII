using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public abstract class Basic_State
{
    protected Manager manager;

    public Basic_State(Manager manager)
    {
        this.manager = manager;
    }

    public abstract void CreateGUI();

    public virtual void Update()
    {
    }

    #region Events
    public virtual void MouseMoved(MouseEventArgs e)
    {
    }

    public virtual void MouseClicked(MouseEventArgs e)
    {
    }

    public virtual void KeyPress(KeyPressEventArgs e)
    {
    }

    public virtual void KeyDown(KeyEventArgs e)
    {
    }

    public virtual void KeyUp(KeyEventArgs e)
    {
    }
    #endregion

    public virtual void Redraw(PaintEventArgs e)
    {
    }
}
