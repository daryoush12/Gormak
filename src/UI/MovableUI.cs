using Godot;
using System;
using System.Diagnostics;

public partial class MovableUI : Control
{
	private bool _dragging = false;
    private Vector2 _dragOffset = Vector2.Zero;

    // Adjust the NodePath in GetNode to match your scene tree
	[Export] private NodePath _dragHandlePath;
    private BaseButton _dragHandle;

    public override void _Ready()
    {
        _dragHandle = GetNode<Button>(_dragHandlePath);
        
        // Connect the handle button signals using C# events or lambda expressions
        _dragHandle.ButtonDown += this.OnDragHandleDown;
        _dragHandle.ButtonUp += this.OnDragHandleUp;
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (_dragging && @event is InputEventMouseMotion mouseMotionEvent)
        {
            Vector2 mouseScreenPos = GetViewport().GetMousePosition();
            GlobalPosition = mouseScreenPos - _dragOffset;
        }
    }


    private void OnDragHandleDown()
    {
		Debug.WriteLine("halleluja");
        _dragging = true;
        // Calculate offset so the window doesn't "snap" to the cursor center
		_dragOffset = GetViewport().GetMousePosition() - GlobalPosition;
    }

    private void OnDragHandleUp()
    {
        _dragging = false;
    }
}
