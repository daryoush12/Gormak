using System;
using System.Diagnostics;
using Godot;
using projecttamasuccessor.Inventory;

public partial class PlayerInventoryManager : BaseContainer
{
	private const string PLAYER_INVENTORY_OPEN_ACTION = "player_inventory";

	[Export] private Node2D _actor;

	public override void _Ready()
	{
		_actor = this.GetParent() as Node2D;
		base._Ready();
		ContainerEventBus.inventoryItemMoved += ReceiveItem;
		ContainerEventBus.EmitInventoryInstantiated(this);
	}

	private void ReceiveItem(ulong from, int fromIndex, ulong to, int toIndex)
	{
		Debug.WriteLine(String.Format("Received from {0} index {1} to {2} index {3}", from, fromIndex, to, toIndex));
		// Is authored instance receiving item?
		if (this.GetInstanceId() == to)
		{
			GodotObject rawObject = GodotObject.InstanceFromId(from);

			if (rawObject is BaseContainer)
			{	
				BaseContainer tg = rawObject as BaseContainer;
				InventorySlot fromSlot = tg.GetSlot(fromIndex);
				SetItem(toIndex, fromSlot);
				tg.EmptySlot(fromIndex);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public override void _Process(double delta)
	{
		if (Input.IsActionJustReleased(PLAYER_INVENTORY_OPEN_ACTION))
		{
			Open(_actor);
		}
	}
}
