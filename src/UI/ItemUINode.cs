using System;
using Godot;

namespace projecttamasuccessor.UI
{
	public partial class ItemUINode : TextureRect
	{
		[Export] Label QuantityLabel {get; set;}
		

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			
		}

		public void RenderItem(InventorySlot slot){
			if(slot.Item.IsStackable && slot.Quantity > 1) {
				QuantityLabel.Text = string.Format("{0}", slot.Quantity.ToString());
			}

			SetAnchorsAndOffsetsPreset(LayoutPreset.Center, LayoutPresetMode.KeepSize, 2);
			
			this.Texture = slot.Item.Icon;
			QuantityLabel.Text = "";
		}
	}
}