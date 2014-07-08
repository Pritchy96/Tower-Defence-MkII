using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class Manager
{
    #region Variables
    private BasicState currentState;
    #endregion

    #region Properties
    public BasicState CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }
    #endregion

    public Manager()
    {
        currentState = new MenuState(this);
    }

    public void Update()
    {
        currentState.Update();
    }

    public void MouseMoved(MouseEventArgs e)
    {
        currentState.MouseMoved(e);
    }

    public void MouseClicked(MouseEventArgs e)
    {
        currentState.MouseClicked(e);
    }

    public void Redraw(PaintEventArgs e)
    {
        currentState.Redraw(e);
    }
}


