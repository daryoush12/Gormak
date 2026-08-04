using System;
using Godot;

namespace projecttamasuccessor.Inventory
{
   public partial class ContainerEventBus : Node
    {
        [Signal] public delegate void ContainerOpenedEventHandler(ContainerManager container, Node2D actor);
        [Signal] public delegate void ContainerClosedEventHandler(ContainerManager container, Node2D actor);
        [Signal] public delegate void InventoryItemMovedEventHandler(string fromContainer, int fromIndex, string toContainer, int toIndex);

    }
}
