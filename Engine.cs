using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using PipeCalculator;

public class Engine : Node2D
{
    Label labelFPS;
    MenuButton menuButton;
    Camera2D camera2D;

    bool isMiddleMouseButtonPressed = false;
    Vector2 positionmouseMiddleClic = new Vector2(0, 0);

    public override void _Ready()
    {
        labelFPS = new Label() { RectPosition = new Vector2(0, 0), Text = "FPS:" };
        AddChild(labelFPS);
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
            camera2D.Position = positionmouseMiddleClic- GetGlobalMousePosition();
            // GD.Print( "pmc = "+ positionmouseMiddleClic.ToString());
            // GD.Print( "mouse = "+ GetGlobalMousePosition().ToString());
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
                if (emb.ButtonIndex == (int)ButtonList.WheelUp) camera2D.Zoom *= (1 / 1.05f);
                if (emb.ButtonIndex == (int)ButtonList.WheelDown) camera2D.Zoom *= 1.05f;
            }
        }
    }

    public override void _UnhandledInput(InputEvent ev)
    {
        if (ev.IsActionPressed("mousemiddle"))
        {
            // GD.Print("middle");
            isMiddleMouseButtonPressed = true;
            positionmouseMiddleClic = camera2D.Position+GetGlobalMousePosition();
        }
        if (ev.IsActionReleased("mousemiddle"))
        {
            // GD.Print("middle release");
            isMiddleMouseButtonPressed = false;
            positionmouseMiddleClic = Vector2.Zero;
        }
    }
}
