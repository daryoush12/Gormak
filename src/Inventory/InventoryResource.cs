using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class InventoryResource : Resource
{
    [Export] public string Name { get; set; } = "";
    [Export] public bool IsStackable { get; set; } = true;
    [Export] public Texture2D Icon { get; set; } = null;
    [Export] public int MaxStackSize { get; set; } = 99;

    public virtual void Use(Node2D actor)
    {
        GD.Print($"Using {Name}");
    }
}
