using Godot;
using System;
using PipeCalculator;
using System.Collections.Generic;

public class ConternerButtonSave : Control
{
    private VBoxContainer vBoxContainer;

    private PanelContainer panelContainer;
    private LineEdit lineEdit;

    [Export(PropertyHint.ResourceType, "PackedScene")]
    private PackedScene packedSceneButton;//= ResourceLoader.Load<PackedScene>("res://ButtonDebit.tscn");

    [Export(PropertyHint.ResourceType, "PackedScene")]
    PackedScene packedSceneButtonResolution;

    PipeManager pipeManager;

    public override void _Ready()
    {
        vBoxContainer = GetNode<VBoxContainer>("PanelContainer/VBoxContainer");
        panelContainer = GetNode<PanelContainer>("PanelContainer");
        lineEdit = GetNode<LineEdit>("PanelContainer/VBoxContainer/LineEdit");
        AddButtonDebit();
        AddButtonResolution();
    }

    public override void _Process(float delta)
    {
    }

    private void AddButtonDebit()
    {
        ButtonDebit instance = packedSceneButton.Instance<ButtonDebit>();
        instance.Connect("hasDelete", this, nameof(ResizeHeigth));
        instance.Connect("hasValidate", this, nameof(AddButtonDebit));
        vBoxContainer.AddChild(instance);
        vBoxContainer.MoveChild(instance, vBoxContainer.GetChildCount() - 2);
        instance.GetFocusLineEdit();
    }

    private void AddButtonResolution()
    {
        ButtonStartResolution instance = packedSceneButtonResolution.Instance<ButtonStartResolution>();
        instance.Connect("hasResolve", this, nameof(StartResolve));
        vBoxContainer.AddChild(instance);
    }

    public void ResizeHeigth()
    {
        panelContainer.RectSize -= new Vector2(0, 68);
    }

    void StartResolve()
    {
        GD.Print("has been resolve");

        List<int> listCut = new List<int>();
        foreach (var item in vBoxContainer.GetChildren())
        {
            switch (item)
            {
                case ButtonDebit buttonDebit when buttonDebit.length != null:
                    listCut.Add((int)buttonDebit.length);
                    break;
            }
        }

        pipeManager = new PipeManager(lineEdit.Text, 6005, listCut, 5);

        pipeManager.Process();
        foreach (var pipe in pipeManager.GetEnumerable())
        {
            String cutString = $"{lineEdit.Text} : ";
            foreach (var cut in pipe)
            {
                cutString += cut.ToString();
                cutString += "-";
            }
            GD.Print(cutString);
        }
    }

}
