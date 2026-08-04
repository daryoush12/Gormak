using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class Weapon : InventoryResource
{
    [Export] public int Damage { get; set; } = 10;
    [Export] public float AttackSpeed { get; set; } = 1.0f;
    [Export] public float Range { get; set; } = 1.0f;
    
    public override void Use(Node2D actor)
    {
        GD.Print($"Using {Name}");
        // Implement weapon-specific behavior here
    }
}
