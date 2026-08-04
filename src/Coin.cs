using Godot;
using projecttamasuccessor.Events;
using System;

public partial class Coin : Node
{
	[Export] private AudioStream _coinSound;

    private EventBus _events;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _events = GetNode<EventBus>("/root/EventBus");
    }

    private void CollectCoin()
    {
        SFXManager._sfxmanager.PlaySFX(_coinSound);
        _events.EmitSignal(EventBus.SignalName.CoinCollected, 1);
        this.QueueFree();
    }

    private void _on_coin_area_entered(Node2D body)
    {
        if (body.IsInGroup("Player"))
        {
            CollectCoin();
        }
    }
}
