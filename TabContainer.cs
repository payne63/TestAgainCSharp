using Godot;
using System;

public class TabContainer : Godot.TabContainer
{

    [Export(PropertyHint.ResourceType, "PackedScene")]
    private PackedScene packedSceneContener;
    private ConternerButton conternerButton;
    private Vector2 verticalPosition = new Vector2(0,0);

    int tabPipeIndex = 1;

    public override void _Ready()
    {
        Connect("tab_changed",this, nameof(TabChanged));
        conternerButton = FindNode("ConternerButton") as ConternerButton; 
    }

    public override void _Input(InputEvent ev){
    if (ev is InputEventMouseButton){
        InputEventMouseButton emb = (InputEventMouseButton)ev;
        if (emb.IsPressed()){
            if (emb.ButtonIndex == (int)ButtonList.WheelUp){
                conternerButton.RectPosition += new Vector2(0,20);
                conternerButton.RectPosition = conternerButton.RectPosition.Clamped(100);
            }
            if (emb.ButtonIndex == (int)ButtonList.WheelDown){
                conternerButton.RectPosition += new Vector2(0,-20);
                if (conternerButton.RectPosition.y <0) conternerButton.RectPosition = Vector2.Zero ; 
            }
        }
    }
}

    private void TabChanged( int tab)
    {
        if (tab == GetTabCount()-1)
        {
            var newTab = new Tabs();
            newTab.Name = GetNextName();
            AddChild(newTab);
            ScrollContainer scrollContainer = new ScrollContainer();
            scrollContainer.ScrollHorizontalEnabled = false;
            scrollContainer.AnchorRight = 1f;
            scrollContainer.AnchorBottom = 1f;
            newTab.AddChild(scrollContainer);
            scrollContainer.AddChild(packedSceneContener.Instance<ConternerButton>());
            MoveChild(GetNode<Tabs>("+"),GetChildCount()-1);
            CurrentTab = GetTabCount()-2;
        }
    }

    string GetNextName()
    {
        tabPipeIndex++;
        return "Tube " +tabPipeIndex.ToString(); 
    }
}
