using System;
using System.Diagnostics;
using Godot;
using Godot.Collections;

namespace projecttamasuccessor.Inventory
{
    public partial class ContainerManager : BaseContainer
    {
        [Export] public Control InteractMarker {get; set;}

        private bool canOpen = false;
        private bool isOpen = false;
        private Node2D _currentActor = null;

        private ContainerEventBus _eventBus;

        public override void _Ready()
        {
            base._Ready();

            _eventBus = GetNode<ContainerEventBus>("/root/ContainerEventBus");
        }

        public override void _PhysicsProcess(double delta)
        {
            if(canOpen && Input.IsActionJustReleased(InteractKey)){
                
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
                _eventBus.EmitSignal(ContainerEventBus.SignalName.ContainerClosed, this, actor);
            }
        }

        public void Open(Node2D actor)
        {
            if(isOpen){
                isOpen = false;
                _eventBus.EmitSignal(ContainerEventBus.SignalName.ContainerClosed, this, actor);
                return; 
            }
            isOpen = true;
            _eventBus.EmitSignal(ContainerEventBus.SignalName.ContainerOpened, this, actor);
        }
    }
}
