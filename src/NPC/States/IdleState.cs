using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace projecttamasuccessor.NPC.States
{
    public class IdleState: BaseState
    {
        private NPCManager _actor;
        private float _moveDir;
        private Vector2 currentTargetNode;
        private Tween _tween;

        public IdleState(NPCManager actor) : base(actor)
        {
            _actor = actor;
        }

        public override void OnStart()
        {
            Debug.WriteLine("IdleState: OnStart called");
            LoopMovement();
        }

        public override void OnStop()
        {
            _tween.Stop();
        }

        private void LoopMovement()
        {
             _tween = _actor.CreateTween().SetLoops();

            // 2. Queue up movements sequentially 
            // The tween will read these line-by-line, waiting for each to finish
            foreach (Node2D targetNode in _actor.PatrolPath)
            {
                _tween.TweenCallback(Callable.From(() => UpdateDirection(targetNode.GlobalPosition)));

                _tween.TweenProperty(_actor, "global_position", targetNode.GlobalPosition, _actor.DurationPerLeg);
                currentTargetNode = targetNode.GlobalPosition; 
                _moveDir = 0; // Reset move direction after reaching the target node
                // OPTIONAL: Add a 0.5 second pause at this platform stop before moving to the next
                _tween.TweenCallback(Callable.From(() =>  UpdateDirection(_actor.GlobalPosition)));
                _tween.TweenInterval(1f);
            }
        }


        private void UpdateDirection(Vector2 destination)
        {
            float difference = destination.X - _actor.GlobalPosition.X;

            // Use Sign to get -1 (Left), 0 (Idle), or 1 (Right)
            _moveDir = Mathf.Sign(difference);

            // Ignore tiny sub-pixel floating point errors
            if (Mathf.Abs(difference) < 0.1f)
            {
                _moveDir = 0;
            }

            _actor.EmitSignal(NPCManager.SignalName.NPCMoved, _moveDir);
        }

        public override BaseState Tick()
        {
            return null;
        }
    }
}
