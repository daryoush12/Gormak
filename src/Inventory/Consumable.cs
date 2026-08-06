using Godot;
using System;
using Godot.Collections;
using Godot.NativeInterop;

namespace projecttamasuccessor.Inventory {

[Flags]
public enum AffectedAttribute
{
    Health = 1 << 1,
    Mana = 1 << 2,
    HealthAndMana = Health | Mana,
}


[GlobalClass]
public partial class Consumable : InventoryResource
{
    [Export] public int Amount { get; set; } = 10;

    [Export] public AffectedAttribute affectedAttribute { get; set; } = 0;
 
    public override void Use(Node2D actor)
    {
        GD.Print($"Using {Name}");
        // Implement weapon-specific behavior here
    }
}
}
