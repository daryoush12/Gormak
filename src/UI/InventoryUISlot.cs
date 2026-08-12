using System;
using System.Diagnostics;
using Godot;
using Godot.Collections;
using projecttamasuccessor.Inventory;

namespace projecttamasuccessor.UI
{
    public partial class InventoryUISlot : Control
    {
        [Export] ulong ContainerId { get; set; } = 0;
        [Export] int SlotIndex { get; set; }

        public void SetRefrence(ulong containerId, int index)
        {
            this.ContainerId = containerId;
            this.SlotIndex = index;

        }

        public Control GetHeldItem()
        {
            return this.GetChildCount() > 0 ? this.GetChild<Control>(0) : null;
        }

        public void OnMouseEnter()
        {
            ContainerEventBus.EmitItemHovered(ContainerId, SlotIndex);
        }

        public void OnMouseExit()
        {
            ContainerEventBus.EmitItemHoverExit(0, 0);
        }

        public override Variant _GetDragData(Vector2 atPosition)
        {
            Control item = GetHeldItem();
            if (item == null) return default; // Return empty Variant if empty slot

            // Use Godot's Dictionary to pass dynamic structured data across the Variant barrier
            var dragData = new Dictionary
        {
            { "type", "inventory_item" },
            { "origin_slot", this },
            { "origin_container", ContainerId },
            { "origin_index", SlotIndex },
            { "item_node", item }
        };

            // Create a visual duplicate for the drag preview
            Control preview = (Control)item.Duplicate();
            SetDragPreview(preview);

            return dragData;
        }

        // Step 2: Accept any valid inventory item dictionary data
        public override bool _CanDropData(Vector2 atPosition, Variant data)
        {
            if (data.VariantType != Variant.Type.Dictionary) return false;

            var dict = data.AsGodotDictionary();
            return dict.ContainsKey("type") && dict["type"].AsString() == "inventory_item";
        }

        // Step 3: Handle the drop and manage swaps
        public override void _DropData(Vector2 atPosition, Variant data)
        {
            var dict = data.AsGodotDictionary();

            InventoryUISlot sourceSlot = dict["origin_slot"].As<InventoryUISlot>();
            Control sourceItem = dict["item_node"].As<Control>();
            Control targetItem = GetHeldItem();

            // Prevent unnecessary processing if dropped back onto itself
            if (sourceSlot == this) return;

            // Scenario A: Swap Needed (Target slot already has an item)
            if (targetItem != null)
            {
                RemoveChild(targetItem);
                sourceSlot.RemoveChild(sourceItem);

                AddChild(sourceItem);
                sourceSlot.AddChild(targetItem);

                targetItem.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);
            }
            // Scenario B: Empty Slot (Clean addition)
            else
            {
                sourceSlot.RemoveChild(sourceItem);
                AddChild(sourceItem);
            }

            // Always reset layouts so nodes stretch to fit their new parent layout cleanly
            sourceItem.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);

            // Step 4: Fire the static C# event bus to notify the backend data systems
            ContainerEventBus.EmitInventoryItemMoved(
                ulong.Parse(dict["origin_container"].ToString()),
                dict["origin_index"].AsInt32(),
                ContainerId,
                SlotIndex
            );
        }
    }
}