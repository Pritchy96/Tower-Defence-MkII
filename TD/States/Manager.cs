using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tower_Defence.Util;

public class Manager
{
    #region Variables
    private Basic_State currentState;
    private List<GUI_Button> buttons = new List<GUI_Button>();
    #endregion

    #region Properties
    public Basic_State CurrentState
    {
        get { return currentState; }
    }

    public List<GUI_Button> Buttons
    {
        get { return buttons; }
        set { buttons = value; }
    }
    #endregion

    public Manager()
    {
        ChangeState(new Menu_State(this));
    }

    public void Update()
    {
        currentState.Update();
    }

    public void ChangeState(Basic_State newState)
    {
        buttons.Clear();
        currentState = newState;
        currentState.CreateGUI();
        
    }

    #region Events
    public void MouseMoved(MouseEventArgs e)
    {
        currentState.MouseMoved(e);
    }

    public void MouseClicked(MouseEventArgs e)
    {
        bool ButtonClicked = false; //Event does not get called in state if it is a GUI element.

        foreach (GUI_Button b in buttons)
        {
            if (b.Rectangle.Contains(e.Location))
            {
                b.Press(e);
                ButtonClicked = true;
                break;
            }
        }

        if (!ButtonClicked)
            currentState.MouseClicked(e);
    }

    public void KeyPress(KeyPressEventArgs e)
    {
        currentState.KeyPress(e);
    }

    public void KeyDown(KeyEventArgs e)
    {
        currentState.KeyDown(e);
    }

    public void KeyUp(KeyEventArgs e)
    {
        currentState.KeyUp(e);
    }
    #endregion

    public void Redraw(PaintEventArgs e)
    {
        currentState.Redraw(e);

        foreach (GUI_Button b in buttons)
            b.Redraw(e);
    }
}


