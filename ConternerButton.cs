using Godot;
using System;
using PipeCalculator;
using System.Collections.Generic;

public class ConternerButton : Control
{
    private VBoxContainer vBoxContainer;
    private PanelContainer panelContainer;
    private LineEdit lineEditDescription;
    private LineEdit lineEditLengthBase;
    private LineEdit lineEditCutSize;
    private Button buttonResolution;
    private ContenerResult contenerResult;

    [Export(PropertyHint.ResourceType, "PackedScene")]
    private PackedScene packedSceneButton;

    PipeManager pipeManager;
    Boolean GenerateDone = false;

    public override void _Ready()
    {
        vBoxContainer = GetNode<VBoxContainer>("PanelContainer/GridContainer/VBoxContainer");
        panelContainer = GetNode<PanelContainer>("PanelContainer");
        lineEditDescription = GetNode<LineEdit>("PanelContainer/GridContainer/LineEditDescription");
        lineEditLengthBase = GetNode<LineEdit>("PanelContainer/GridContainer/LineEditLength");
        lineEditCutSize = GetNode<LineEdit>("PanelContainer/GridContainer/LineEditCutSize");
        buttonResolution = GetNode<Button>("PanelContainer/GridContainer/ButtonResolution");
        contenerResult = GetNode<ContenerResult>("ConteneurResult");

        lineEditDescription.Connect("text_entered",this, nameof(DescriptionValidation));

        lineEditLengthBase.Connect("text_entered",this, nameof(LengthBaseValidation));

        lineEditCutSize.Connect("text_entered",this, nameof(CutSizeValidation));

        buttonResolution.Connect("pressed",this , nameof(StartResolve));
        var buttonDebit = AddButtonDebit();
        lineEditCutSize.FocusNext = buttonDebit.GetPath();

        lineEditDescription.GrabFocus();
        contenerResult.Hide();
    }


    private void DescriptionValidation( string text)
    {
        if (GetParent<ScrollContainer>() == null) return;
        GetParent<ScrollContainer>().GetParent<Tabs>().Name = text;
        lineEditDescription.FocusNext =lineEditLengthBase.GetPath();
        lineEditLengthBase.FocusNext =lineEditCutSize.GetPath();
        lineEditCutSize.FocusNext = vBoxContainer.GetChild<Control>(0)?.GetPath();
        lineEditLengthBase.GrabFocus();
        lineEditLengthBase.SelectAll();
    }

    private void LengthBaseValidation( string text)
    {
        bool resultStatus = int.TryParse(text, out int value);
        if (resultStatus == true && value > 0 && value <= 6000)
        {
            lineEditLengthBase.Deselect();
            lineEditCutSize.GrabFocus();
            lineEditCutSize.SelectAll();
        }
        else
        {
            lineEditLengthBase.Text = string.Empty;
        }
    }

    private void CutSizeValidation( string text)
    {
        bool resultStatus = int.TryParse(text, out int value);
        if (resultStatus == true && value > 0 && value <= 10)
        {
            lineEditCutSize.Deselect();
            GetNode<ButtonDebit2>(lineEditCutSize.FocusNext).GrabFocus();
        }
        else
        {
            lineEditCutSize.Text = string.Empty;
        }
    }

    private ButtonDebit2 AddButtonDebit()
    {
        ButtonDebit2 instance = packedSceneButton.Instance<ButtonDebit2>();
        instance.Connect("hasValidate", this, nameof(AddButtonDebit));
        instance.Connect("tree_exited",this, nameof(ItemDelete));
        vBoxContainer.AddChild(instance);
        instance.GetFocusLineEdit();
        return instance;
    }



    public void ItemDelete()
    {
        panelContainer.RectSize -= new Vector2(0, 50);
        bool noFreeItem = false;
        foreach (var item in vBoxContainer.GetChildren())
        {
            switch (item)
            {
                case ButtonDebit2 buttonDebit when buttonDebit.length == null || buttonDebit.qt == null:
                    noFreeItem = true;
                    GD.PrintErr(true, buttonDebit);
                    break;
            }
        }
        if (noFreeItem == false) AddButtonDebit();
        if (vBoxContainer.GetChildCount() == 0) AddButtonDebit();
    }

    void StartResolve()
    {

        if (GenerateDone)
        {
            contenerResult.ClearDataPipe();
        }

        List<int> listCuts = new List<int>();
        foreach (var item in vBoxContainer.GetChildren())
        {
            switch (item)
            {
                case ButtonDebit2 buttonDebit when buttonDebit.length != null:
                    for (int i = 1; i <= buttonDebit.qt; i++)
                    {
                        listCuts.Add((int)buttonDebit.length);
                    }
                    break;
            }
        }

        int.TryParse(lineEditLengthBase.Text, out int lengthbase); 
        int.TryParse(lineEditCutSize.Text, out int cutSize);

        pipeManager = new PipeManager(lineEditDescription.Text, lengthbase+cutSize, listCuts, cutSize);

        pipeManager.Process();

        contenerResult.Show();
        contenerResult.FillDataPipe(pipeManager.GetEnumerable());

        GenerateDone = true;

    }


    public int GetLengthMaxPipe()
    {
        return lineEditLengthBase.Text.ToInt();
    }

}
