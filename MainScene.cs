using Godot;
using System;
using System.Globalization;
using System.Collections.Generic;

public class MainScene : Node2D
{

    [Export(PropertyHint.Range, "0,10")]
    float speed = 1.0f;

     [Export(PropertyHint.ResourceType, "Texture")]
    private Texture tex ;

    Sprite sprite;

    public override void _Ready()
    {
        var reference = GetScript();
        sprite = GetNode<Sprite>("SpriteGodot");
        sprite.Modulate = Colors.Aqua;
        // sprite.Texture = tex;
        Position = new Vector2().Random();
        //this is a comment for test
        //and another
    }

    public override void _Process(float delta)
    {
        if (Input.IsMouseButtonPressed(1))
        {
            Position += new Vector2(speed, 0);
        }
        if (Input.IsActionJustPressed("mouseright"))
        {
            if (IsVisibleInTree()) Hide(); else Show();
        }
    }
}

public static class Vector2Extension
{
    static Random rnd = new Random(DateTime.Now.Millisecond);

    public static Vector2 Random(this Vector2 v)
    {
        return new Vector2(rnd.Next(0, 400), rnd.Next(0, 400));
    }

}