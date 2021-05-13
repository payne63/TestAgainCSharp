using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Engine : Node2D
{
    int numberOfSprite = 5;
    PackedScene packScene = ResourceLoader.Load<PackedScene>("res://MainScene.tscn");
    List<Node2D> listNode;
    Label labelFPS;

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
    }

    public override void _Process(float delta)
    {
        labelFPS.Text = $"FPS:{Godot.Engine.GetFramesPerSecond()}";
    }

}
