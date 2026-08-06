using System;
using System.Diagnostics;
using Godot;
using Godot.Collections;
using projecttamasuccessor.Events;
using projecttamasuccessor.Inventory;

namespace projecttamasuccessor.UI
{
    public partial class PlayerHotbarUI : Control
    {
        [Export] private Control _slotContainer;
        [Export] private PackedScene SlotItem { get; set; }
        [Export] public PackedScene SlotPrefab { get; set; }
        
        [Export] public PackedScene SlotItemUiPrefab { get; set; }

        private ContainerEventBus _events;
        [Export] private Array<Control> ContainerUISlots { get; set; }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _events = GetNode<ContainerEventBus>("/root/ContainerEventBus");
            _events.ContainerOpened += DisplayContainer;
            ContainerEventBus.inventoryInstantiated += InstantiateHotbar;
        }

        private void InstantiateHotbar(BaseContainer container){
            if(!container.IsInGroup("player_inventory_hotbar")) return;

            Refresh(container);
        }

        /// <summary>
        /// Displays player inventory UI
        /// </summary>
        private void DisplayContainer(BaseContainer container, Node2D actor)
        {
            // Only open lootable containers.
            if(!container.IsInGroup("player_inventory_hotbar")) return;
            
            if(this.GetChildCount() > 0) Refresh(container);
            for (int i = 0; i < container.Items.Count; i++)
            {
                InventoryUISlot slot = SlotPrefab.Instantiate<InventoryUISlot>();
                slot.SetRefrence(container.GetInstanceId(), i);
                _slotContainer.AddChild(slot);
                ContainerUISlots.Add(slot);
                if (container.Items[i] != null)
                {
                    var Item = SlotItem.Instantiate<ItemUINode>();
                    Item.RenderItem(container.Items[i]);
                    slot.AddChild(Item);
                }
            }
        }

        /// <summary>
        /// Refreshes the player inventory UI, re-rendering all slots and their contained items.
        /// </summary>
        private void Refresh(BaseContainer container)
        {
            // Clear existing slots
            foreach (var slot in ContainerUISlots)
            {
                if (slot.HasNode(""))
                {
                    foreach (Node child in slot.GetChildren())
                        child.QueueFree();
                }
                slot.QueueFree();
            }
            ContainerUISlots.Clear();

            // Recreate slots and add associated items if present
            for (int i = 0; i < container.Items.Count; i++)
            {
                InventoryUISlot slot = SlotPrefab.Instantiate<InventoryUISlot>();
                slot.SetRefrence(container.GetInstanceId(), i);
                _slotContainer.AddChild(slot);
                ContainerUISlots.Add(slot);
                if (container.Items[i] != null && !container.Items[i].IsEmpty())
                {
                    var itemNode = SlotItem.Instantiate<ItemUINode>();
                    itemNode.RenderItem(container.Items[i]);
                    slot.AddChild(itemNode);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _events.ContainerOpened -= DisplayContainer;
                ContainerEventBus.inventoryInstantiated -= Refresh;
            }
            base.Dispose(disposing);
        }
    }
}