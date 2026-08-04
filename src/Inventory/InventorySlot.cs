using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class InventorySlot : Resource
{
    [Export] public InventoryResource Item { get; set; } = null;
    [Export] public int Quantity { get; set; } = 0;
    
    bool IsEmpty => Item == null || Quantity <= 0;

    public void Clear()
    {
        Item = null;
        Quantity = 0;
    }
}