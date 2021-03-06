using Godot;
using System;

public class ButtonDebit : Button
{

    private LineEdit lineEdit;
    private Button buttonDelete;

    [Signal]
    public delegate void hasDelete();
    [Signal]
    public delegate void hasValidate();

    public int? length = null; 

    public override void _Ready()
    {
        lineEdit = GetNode<LineEdit>("LineEdit");
        lineEdit.Connect("text_entered", this, nameof(ValidationLine));
        buttonDelete = GetNode<Button>("ButtonDelete");
        buttonDelete.Hide();
        this.Disabled = true;
        buttonDelete.Connect("pressed", this, nameof(Delete));
        Connect("pressed", this, nameof(NewLineEdition));
        Connect("focus_entered",this, nameof(GetFocusLineEdit));
    }

    void ValidationLine(string text)
    {
        bool resultStatus = int.TryParse(text, out int value);

        if ( resultStatus == true && value > 5 && value <= 6000)
        {
            length = value;
            this.Text = value.ToString() + " mm";
            lineEdit.Hide();
            Disabled = false;
            buttonDelete.Show();
            EmitSignal(nameof(hasValidate));
        }
        else
        {
            length = null;
            lineEdit.Text = string.Empty;
        }
    }

    void NewLineEdition()
    {
        length = null;
        Text = string.Empty;
        Disabled = true;
        lineEdit.Show();
        lineEdit.Text = "";
        lineEdit.GrabFocus();
        buttonDelete.Hide();
    }

    void Delete()
    {
        EmitSignal(nameof(hasDelete));
        this.QueueFree();
    }

    public void GetFocusLineEdit()
    {
        lineEdit.GrabFocus();
    }
}
