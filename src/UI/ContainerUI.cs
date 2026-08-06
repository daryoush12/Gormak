using System.ComponentModel;
using System.Diagnostics;
using Godot;
using Godot.Collections;
using projecttamasuccessor.Inventory;

namespace projecttamasuccessor.UI
{
    public partial class ContainerUI : Control
    {
        [Export] public Control SlotContainer { get; set; }
        [Export] public PackedScene SlotPrefab { get; set; }
        [Export] public PackedScene SlotItem { get; set; }

        [Export] public PackedScene SlotItemUiPrefab { get; set; }

        private ContainerEventBus _events;
        private Array<Control> ContainerUISlots { get; set; } = new Array<Control>();

        private ulong _currentContainer;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _events = GetNode<ContainerEventBus>("/root/ContainerEventBus");
            _events.ContainerOpened += DisplayContainer;
            _events.ContainerClosed += CloseContainer;
            this.Visible = false;

        }

        private void CloseContainer(BaseContainer container, Node2D actor){
            //If match fails:
            if(container.GetInstanceId() != _currentContainer) return;

            this.Visible = false;
            // Reset id, for next container to take over.
            _currentContainer = 0;
        }

        private void DisplayContainer(BaseContainer container, Node2D actor)
        {
            Debug.WriteLine(container);
            // Only open lootable containers.
            if (!container.IsInGroup("general_containers")) return;
            if (this.Visible)
            {
                this.Visible = false;
                return;
            }else {
                _currentContainer = container.GetInstanceId();
                GD.Print(container.Items[0].Item.Name);
                this.Refresh(container);
                this.Visible = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _events.ContainerOpened -= DisplayContainer;
            }
            base.Dispose(disposing);
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
                SlotContainer.AddChild(slot);
                ContainerUISlots.Add(slot);
                if (container.Items[i] != null && !container.Items[i].IsEmpty())
                {
                    var itemNode = SlotItem.Instantiate<ItemUINode>();
                    itemNode.RenderItem(container.Items[i]);
                    slot.AddChild(itemNode);
                }
            }
        }
    }
}
