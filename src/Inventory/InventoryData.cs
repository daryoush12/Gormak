using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class InventoryData : Resource
{
    [Export] public Array<InventorySlot> Items { get; set; } = new Array<InventorySlot>();
    
    [Signal] public delegate void InventoryChangedEventHandler();

    public void SetupSlots(int size)
    {
        Items.Resize(size);
    }

    public void AddItem(InventoryResource item, int quantity = 1)
    {
      for(int i = 0; i < Items.Count; i++)
        {
            if (Items[i] != null && Items[i].Item == item && item.IsStackable)
            {
                Items[i].Quantity += quantity;
                EmitSignal(SignalName.InventoryChanged);
                return;
            }
        }
    }
    
    public InventoryResource EmptySlot(int index)
    {
        if (index >= 0 && index < Items.Count)
        {
            var temp = Items[index].Item;
            Items[index].Clear();
            EmitSignal(SignalName.InventoryChanged);
            return temp;
        }
        return null;
    }

    public InventorySlot GetSlot(int index)
    {
        if (index >= 0 && index < Items.Count)
        {
            return Items[index];
        }
        return null;
    }
}
