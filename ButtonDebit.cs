using Godot;
using System;

public class ButtonDebit : Button
{

    private LineEdit lineEdit;
    private Button buttonDelete;

    public override void _Ready()
    {
        lineEdit = GetNode<LineEdit>("LineEdit");
        lineEdit.Connect("text_entered", this, nameof(ValidationLine));
        buttonDelete = GetNode<Button>("ButtonDelete");
        buttonDelete.Hide();
        this.Disabled = true;
        buttonDelete.Connect("pressed", this, nameof(Delete));
        Connect("pressed", this, nameof(NewLineEdition));
    }

    void ValidationLine(string text)
    {
        bool resultStatus = int.TryParse(text, out int value);

        if ( resultStatus == true && value > 5 && value <= 6000)
        {
            this.Text = value.ToString() + " mm";
            lineEdit.Hide();
            Disabled = false;
            buttonDelete.Show();
        }
        else
        {
            lineEdit.Text = string.Empty;
        }
    }

    void NewLineEdition()
    {
        Text = string.Empty;
        Disabled = true;
        lineEdit.Show();
        lineEdit.Text = "";
        lineEdit.GrabFocus();
        buttonDelete.Hide();
    }

    void Delete()
    {
        this.QueueFree();
    }

}
