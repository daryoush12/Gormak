using System;
using Godot;
using Godot.Collections;

namespace projecttamasuccessor.UI
{
	public partial class ItemUINode : TextureRect
	{
		[Export] Label QuantityLabel { get; set; }

		public void RenderItem(InventorySlot slot)
		{
			if(slot.Item == null) return;
			if (slot.Item.IsStackable && slot.Quantity > 1)
			{
				QuantityLabel.Text = string.Format("{0}", slot.Quantity.ToString());
			}

			SetAnchorsAndOffsetsPreset(LayoutPreset.Center, LayoutPresetMode.KeepSize, 2);


			this.Texture = slot.Item.Icon;
			QuantityLabel.Text = "";
		}
	}
}