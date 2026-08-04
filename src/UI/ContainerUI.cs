using System;
using System.Diagnostics;
using Godot;
using Godot.Collections;
using projecttamasuccessor.Events;
using projecttamasuccessor.Inventory;

namespace projecttamasuccessor.UI
{
    public partial class ContainerUI : Control
    {
        [Export] private Label _scoreLabel;
        [Export] private Control _slotContainer;
        [Export] public PackedScene SlotPrefab { get; set; }
        [Export] private PackedScene SlotItem { get; set; }

        private ContainerEventBus _events;
        [Export] private Array<Control> ContainerUISlots { get; set; }
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _events = GetNode<ContainerEventBus>("/root/ContainerEventBus");
            _events.ContainerOpened += DisplayContainer;
            this.Visible = false;

        }

        private void DisplayContainer(ContainerManager container, Node2D actor)
        {
            Debug.WriteLine("Rendering chest ui");
            for (int i = 0; i < container.Items.Count; i++)
            {
                Control slot = SlotPrefab.Instantiate<Control>();
                _slotContainer.AddChild(slot);
                ContainerUISlots.Add(slot);
                if (container.Items[i] != null)
                {
                    var Item = SlotItem.Instantiate<ItemUINode>();
                    Item.RenderItem(container.Items[0]);
                    //Temp. Get this shit fixed. We are not calling N Array to just put shit into first child.
                    slot.GetChildren()[0].AddChild(Item);
                }
            }

            this.Visible = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _events.ContainerOpened -= DisplayContainer;
            }
            base.Dispose(disposing);
        }
    }
}
