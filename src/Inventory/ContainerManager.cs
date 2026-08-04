using System;
using System.Diagnostics;
using Godot;
using Godot.Collections;

namespace projecttamasuccessor.Inventory
{
    public partial class ContainerManager : Node2D
    {
        [Export] public Array<InventorySlot> Items { get; set; } = new Array<InventorySlot>();
        [Export] public int MaxSlots { get; set; } = 10;
        [Export] public string InteractKey {get; set;}

        [Export] public Control InteractMarker {get; set;}

        private bool canOpen = false;
        private bool isOpen = false;
        private Node2D _currentActor = null;

        private ContainerEventBus _eventBus;

        public override void _Ready()
        {
            Items.Resize(MaxSlots);
            _eventBus = GetNode<ContainerEventBus>("/root/ContainerEventBus");
        }

        public override void _PhysicsProcess(double delta)
        {
            if(Input.IsActionJustReleased(InteractKey)){
                
                Open(_currentActor);
            }
        }

        public void _on_area_2d_body_entered(Node2D actor)
        {
            if (actor.IsInGroup("Player"))
            {
                canOpen = true;
                _currentActor = actor;
                InteractMarker.Visible = true;
            }
        }

        public void _on_area_2d_body_exited(Node2D actor)
        {
            if (actor.IsInGroup("Player"))
            {
                canOpen = false;
                _currentActor = null;
                InteractMarker.Visible = false;
            }
        }

        public void Open(Node2D actor)
        {
            if(isOpen){
                isOpen = false;
                return; 
            }
            Debug.WriteLine("Open chest");
            isOpen = true;
            _eventBus.EmitSignal(ContainerEventBus.SignalName.ContainerOpened, this, actor);
        }
    }
}
