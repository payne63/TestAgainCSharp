using Godot;
using System;

public class ButtonStartResolution : Button
{
    [Signal]
    public delegate void hasResolve();

    public override void _Ready()
    {
        Connect("pressed",this, nameof(ClickResoudre));
    }

    void ClickResoudre()
    {
        EmitSignal(nameof(hasResolve));
    }
}
