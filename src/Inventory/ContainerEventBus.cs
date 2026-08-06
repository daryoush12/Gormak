using System;
using Godot;

namespace projecttamasuccessor.Inventory
{
    public partial class ContainerEventBus : Node
    {
        [Signal] public delegate void ContainerOpenedEventHandler(BaseContainer container, Node2D actor);
        [Signal] public delegate void ContainerClosedEventHandler(BaseContainer container, Node2D actor);

        public delegate void InventoryEventHandler(BaseContainer container);
        public static InventoryEventHandler inventoryInstantiated;

        public delegate void InventoryItemMovedEventHandler(ulong fromContainer, int fromIndex, ulong toContainer, int toIndex);
        public static InventoryItemMovedEventHandler inventoryItemMoved;
        public static void EmitInventoryItemMoved(ulong fromContainer, int fromIndex, ulong toContainer, int toIndex)
        {
            inventoryItemMoved?.Invoke(fromContainer, fromIndex, toContainer, toIndex);
        }

        public static void EmitInventoryInstantiated(BaseContainer container){
            inventoryInstantiated?.Invoke(container);
        }

    }
}
