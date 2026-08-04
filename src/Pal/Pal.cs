using Godot;
using System;

public partial class Pal : Node
{
	private int _health;
	private int _hunger;
	private int _happiness;

	public int Health {get { return _health; }}
	public int Hunger { get { return _hunger; } }
	public int Happiness { get { return _happiness; } }


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
