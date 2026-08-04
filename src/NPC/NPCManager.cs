using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using System.Collections;
using System.Collections.Generic;
using projecttamasuccessor.NPC.States;
using projecttamasuccessor.Player;
using System.Diagnostics;


namespace projecttamasuccessor.NPC
{
    public partial class NPCManager : Node2D
    {
        private int _health;

        public int Health { get { return _health; } }

        private BaseState _currentState;
        private BaseState _nextState;

        public Dictionary<string, BaseState> States;

        [Export] private Godot.Collections.Array<Node2D> _patrolPath { get; set; }
        [Export] private float _durationPerPath { get; set; } = 2.0f;
        [Export] private int _maxHealth { get; set; } = 100;

        [Export] private AudioStream _slimeHurt;
        [Export ] private AudioStreamPlayer2D _audioPlayer;

        public Godot.Collections.Array<Node2D> PatrolPath { get { return _patrolPath; } }
        public float DurationPerLeg { get { return _durationPerPath; } }


        [Signal]
        public delegate void NPCMovedEventHandler(float direction);

        [Signal]
        public delegate void NPCHealthEventHandler(int current, int change, int max);

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            States = new Dictionary<string, BaseState> {
            { "Idle", new IdleState(this) },
            { "Wander", new WanderState(this) },
        };
            _currentState = States["Idle"] as BaseState;
            _currentState.OnStart();
            _health = _maxHealth;
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            if(_health == 0 && _currentState != null)
            {  
                _currentState.OnStop();
                _currentState = null;
                _nextState = null;
                return;
            }
            if (_currentState != null)
            {
                _nextState = _currentState.Tick();
            }
            if(_nextState != null)
            {
                _currentState.OnStop();
                _nextState.OnStart();
                _currentState = _nextState;
            }
        }

        public void Damage(int amount)
        {
            _health = Mathf.Clamp(_health-amount, 0, _maxHealth);
            _audioPlayer.Stream = _slimeHurt;
            _audioPlayer.Play();

            EmitSignal(SignalName.NPCHealth, _health, -amount, 100);
        }

        public void _on_area_2d_body_entered(Node2D actor)
        {
            if(actor.IsInGroup("Player"))
            {
                Debug.WriteLine("Player on top of slime");
                var player = actor as PlayerManager;
                if (player == null) return;

                Damage(50);
                player.Bounce();

            }
        }
    }
}
