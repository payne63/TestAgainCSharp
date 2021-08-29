using Godot;
using System;

public class ButtonDebit2 : Button
{

    private LineEdit lineEditLength;
    private LineEdit lineEditQt;
    private Button buttonDelete;

    [Signal]
    public delegate void hasValidate();

    public int? length = null;
    public int? qt = null;

    public override void _Ready()
    {
        lineEditLength = GetNode<LineEdit>("HBoxContainer/LineEdit");
        lineEditQt = GetNode<LineEdit>("HBoxContainer/LineEdit2");
        lineEditLength.Connect("text_entered", this, nameof(ValidationLineLength));

        lineEditQt.Connect("text_entered", this, nameof(ValidationLineQt));
        this.Disabled = true;
        buttonDelete = GetNode<Button>("HBoxContainer/ButtonDelete");
        buttonDelete.Connect("pressed", this, nameof(Delete));
        Connect("focus_entered", this, nameof(GetFocusLineEdit));
    }


    void ValidationLineLength(string text)
    {
        bool resultStatus = int.TryParse(text, out int value);

        int maxLength = 6000;
        var conternerButton = FindParent("ConternerButton") as ConternerButton;

        if (conternerButton != null) maxLength = conternerButton.GetLengthMaxPipe(); 

        if (resultStatus == true && value > 5 && value <= maxLength)
        {
            length = value;
            lineEditLength.Editable = false;
            
            lineEditQt.GrabFocus();
            lineEditQt.Text = "1";
            lineEditQt.Select();
        }
        else
        {
            length = null;
            lineEditLength.Text = string.Empty;
        }
    }



    void ValidationLineQt(string text)
    {
        bool resultStatus = int.TryParse(text, out int value);
        if (resultStatus == true && value > 0 && value <= 999)
        {
            qt = value;
            lineEditQt.Editable = false;
            lineEditQt.Deselect();
            EmitSignal(nameof(hasValidate));
        }
        else
        {
            qt = null;
            lineEditQt.Text = string.Empty;
        }
    }


    void Delete()
    {
        this.QueueFree();
    }

    public void GetFocusLineEdit()
    {
        lineEditLength.GrabFocus();
    }
}
