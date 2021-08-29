using Godot;
using System;
using System.Collections.Generic;

public class ContenerResult : Panel
{

    [Export (PropertyHint.ResourceType, "PackedScene")]
    PackedScene PackedScenelabelPipeDraw;

    VBoxContainer vBoxContainer;


    public override void _Ready()
    {
        vBoxContainer = GetNode<VBoxContainer>("VBoxContainer");
    }

    Label CreateLabelPipeDraw(int length)
    {
        Label labelPipeDraw = PackedScenelabelPipeDraw.Instance<Label>();
        labelPipeDraw.Text = length.ToString();
        labelPipeDraw.RectMinSize = new Vector2( length/7 , 30);
        return labelPipeDraw;
    }

    public void FillDataPipe(IEnumerable<IEnumerable<int>> data)
    {
        int numPipe = 0;

        foreach (IEnumerable<int> pipe in data)
        {
            HBoxContainer actualHBoxContainer = new HBoxContainer();
            var actualLabelPipe = new Label();
            numPipe++;
            actualLabelPipe.Text  = $" * tube n°{numPipe}";
            actualLabelPipe.AddFontOverride("font",ResourceLoader.Load<Font>("res://fonts/fontsmall.tres"));
            actualLabelPipe.AddColorOverride("font_color",Colors.Black);
            vBoxContainer.AddChild(actualLabelPipe);

            vBoxContainer.AddChild(actualHBoxContainer);
            var hSeparator = new HSeparator();
            StyleBoxLine sbl = new StyleBoxLine(){Color = new Color("ad4c4c"),Thickness = 5 };
            hSeparator.AddStyleboxOverride("separator",sbl);
            vBoxContainer.AddChild(hSeparator);

            foreach (int length in pipe)
            {
                actualHBoxContainer.AddChild(CreateLabelPipeDraw(length));    
            }
        }
        RectSize = new Vector2(RectSize.x,(numPipe)*68);
    }

    public void ClearDataPipe()
    {
        foreach (Control child in vBoxContainer.GetChildren())
        {
            child.QueueFree();
        }
        RectSize = new Vector2(RectSize.x,68);
    }

}
