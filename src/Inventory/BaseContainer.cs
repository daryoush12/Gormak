using System;
using System.Diagnostics;
using Godot;
using Godot.Collections;

namespace projecttamasuccessor.Inventory
{
    public partial class BaseContainer : Node2D
    {
        [Export] public Array<InventorySlot> Items { get; set; } = new Array<InventorySlot>();
        [Export] public int MaxSlots { get; set; } = 10;
        [Export] public string InteractKey { get; set; }

        private bool isOpen = false;
        private Node2D _currentActor = null;

        private ContainerEventBus _eventBus;

        public override void _Ready()
        {
            if(Items.Count != MaxSlots)
                Items.Resize(MaxSlots);
                
            for (int i = 0; i < MaxSlots; i++)
            {
                if (Items[i] == null)
                    Items[i] = new InventorySlot();
            }
            _eventBus = GetNode<ContainerEventBus>("/root/ContainerEventBus");
        }

        public override void _PhysicsProcess(double delta)
        {
            if (InteractKey != null && Input.IsActionJustReleased(InteractKey))
            {

                Open(_currentActor);
            }
        }

        public void SetItem(int index, InventorySlot slot)
        {
            // Holy shit, what a condition check. 
            // TODO: Maybe we can put refrences to vars, and create bool arrow functions in InventorySlot, to make code cleaner.  
            // if (Items[index] == null)
            // {
            //     Items[index] = new InventorySlot();
            // }
            Debug.WriteLine("Emptier From"+slot.IsEmpty());
            Debug.WriteLine("Emptier Origin"+Items[index].IsEmpty());
            if (!slot.IsEmpty() && !Items[index].IsEmpty())
            {
                Items[index].Quantity += slot.Quantity;
                return;
            }
            
            Items[index].Item = slot.Item;
            Items[index].Quantity = slot.Quantity;
        }

        public void EmptySlot(int index)
        {
            Items[index].Item = null;
            Items[index].Quantity = 0;
        }

        public InventorySlot GetSlot(int index)
        {
            return Items[index];
        }

        public void Open(Node2D actor)
        {
            if (isOpen)
            {
                isOpen = false;
                return;
            }
            isOpen = true;
            _eventBus.EmitSignal(ContainerEventBus.SignalName.ContainerOpened, this, actor);
        }
    }
}