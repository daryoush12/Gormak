using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class Platform : AnimatableBody2D
{
	[Export] private Godot.Collections.Array<Node2D> Array { get; set; }
    [Export] private float DurationPerLeg { get; set; } = 2.0f;

    private Vector2 _currentDest;
    private Transform2D _startTransform;
    private int _currentIndex = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        if (Array != null && Array.Count > 0)
            StartLoopingMovement();

    }

    private void StartLoopingMovement()
    {
        // 1. Create a tween and set it to loop infinitely
        Tween tween = CreateTween().SetLoops();

        // 2. Queue up movements sequentially 
        // The tween will read these line-by-line, waiting for each to finish
        foreach (Node2D targetNode in Array)
        {
            tween.TweenProperty(this, "global_position", targetNode.GlobalPosition, DurationPerLeg)
                 .SetTrans(Tween.TransitionType.Quad)
                 .SetEase(Tween.EaseType.InOut);

            // OPTIONAL: Add a 0.5 second pause at this platform stop before moving to the next
            tween.TweenInterval(0.5f);
        }
    }
}
