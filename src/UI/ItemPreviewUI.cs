using System;
using System.Diagnostics;
using Godot;
using Godot.Collections;
using projecttamasuccessor.Inventory;

namespace projecttamasuccessor.UI
{
    public partial class ItemPreviewUI : Control
    {
        [Export] ulong CurrentContainerId { get; set; }
        [Export] int CurrentSlotIndex { get; set; }

        [ExportSubgroup("UI Elements")]
        [Export] Label NameLabel { get; set; }
        [Export] Label DescriptionLabel { get; set; }
        [Export] TextureRect IconHolder {get; set;}
        [Export] Control AttributesContainer { get; set; }
        [Export] PackedScene AttributePrefab { get; set; }

        public override void _Ready()
        {

            ContainerEventBus.onItemHovered += PreviewItem;
            ContainerEventBus.onItemHoverExit += Hide;
            this.Visible = false;
        }

        private void PreviewItem(ulong id, int index){
            if(id == 0){
                this.Visible = false;
                return;
            }
            BaseContainer c = InstanceFromId(id) as BaseContainer;
            InventoryResource item = c.Items[index].Item;
            
            NameLabel.Text = item.Name;
            DescriptionLabel.Text = item.Description;
            IconHolder.Texture = item.Icon;

            //Here we want to check item type, and display data accordingly:



            this.Visible = true;
        }

        private void Hide(ulong id, int index){
            this.Visible = false;
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing){
            ContainerEventBus.onItemHovered -= PreviewItem;
            ContainerEventBus.onItemHoverExit -= Hide;
            }
            base.Dispose(disposing);
        }
    }
}