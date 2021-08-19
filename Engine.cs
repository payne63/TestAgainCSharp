using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using PipeCalculator;

public class Engine : Node2D
{
    int numberOfSprite = 5;
    PackedScene packScene = ResourceLoader.Load<PackedScene>("res://MainScene.tscn");
    List<Node2D> listNode;
    Label labelFPS;
    PipeManager pipeManager;
    MenuButton menuButton;
    Camera2D camera2D;

    bool isMiddleMouseButtonPressed = false;
    Vector2 positionmouseMiddleClic = new Vector2(0, 0);

    public override void _Ready()
    {
        labelFPS = new Label() { RectPosition = new Vector2(0, 0), Text = "FPS:" };
        AddChild(labelFPS);
        listNode = new List<Node2D>();
        foreach (var a in Enumerable.Range(0, numberOfSprite))
        {
            listNode.Add(packScene.Instance<Node2D>());
        }
        listNode.ForEach(node => AddChild(node));
        GD.Print(labelFPS.ToString());
        GD.Print("****************");
        GD.Print(GD.Var2Str(labelFPS));
        menuButton = GetNode<MenuButton>("VBoxContainer/HBoxContainer2/MenuButton");
        menuButton.GetPopup().Connect("id_pressed", this, nameof(OnSelectMenu));
        camera2D = GetNode<Camera2D>("Camera2D");
    }

    private void OnSelectMenu(int idx)
    {
        GD.Print(idx);
        menuButton.Text = menuButton.GetPopup().GetItemText(idx);
    }

    public override void _Process(float delta)
    {
        labelFPS.Text = $"FPS:{Godot.Engine.GetFramesPerSecond()}";
        if (isMiddleMouseButtonPressed)
        {
            camera2D.Position = positionmouseMiddleClic- GetLocalMousePosition();
        }
    }

    //public override void _UnhandledInput(InputEvent ev)
    public override void _Input(InputEvent ev)
    {
        if (ev is InputEventMouseButton)
        {
            InputEventMouseButton emb = (InputEventMouseButton)ev;
            if (emb.IsPressed())
            {
                if (emb.ButtonIndex == (int)ButtonList.WheelUp)
                {
                    GD.Print(emb.AsText());
                    camera2D.Zoom *= 1.05f;
                }
                if (emb.ButtonIndex == (int)ButtonList.WheelDown)
                {
                    GD.Print(emb.AsText());
                    camera2D.Zoom *= (1 / 1.05f);
                }
            }
        }

        
    }

    public override void _UnhandledInput(InputEvent ev)
    {
        if (ev.IsActionPressed("mousemiddle"))
        {
            GD.Print("middle");
            isMiddleMouseButtonPressed = true;
            positionmouseMiddleClic = GetLocalMousePosition();
        }
        if (ev.IsActionReleased("mousemiddle"))
        {
            GD.Print("middle release");
            isMiddleMouseButtonPressed = false;
        }
    }
}
