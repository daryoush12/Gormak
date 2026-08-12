using System;
using System.Diagnostics;
using Godot;
using projecttamasuccessor.Constants;
using projecttamasuccessor.Inventory;
using static Godot.TextServer;
namespace projecttamasuccessor.Player
{
    public partial class PlayerManager : CharacterBody2D
    {
        private const string PLAYER_JUMP_ACTION = "player_jump";

        [Export] private int MAX_HEALTH = 100;
        [Export] private int MOVEMENT_MODIFIER = 150;
        [Export] private int JUMP_MODIFIER = -350;
        [Export] private int MAX_SPEED = 20;

        [Export] private AudioStream _jumpSound;
        [Export] private AudioStream _moveSound;
        [Export] private AudioStream _hurtSound;

        private int _health;
        private float _currentMovementDirection;

        // Get the gravity from the project settings to match the engine physics
        public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
        public int Health { get { return _health; } }
        private float _velX;
        private float _velY;
        [Export] private Node2D _sword;

        private bool _isAttacking = false;
        private double _idleTime = 0.0;

        // Configurable floating speeds and ranges
        private float _orbitSpeedX = 2.0f;
        private float _orbitSpeedY = 4.0f;
        private float _radiusX = 15.0f;
        private float _radiusY = 8.0f;
        private float _hoverHeightOffset = -20.0f; // Floats slightly above player center
        private float _direction;
        [Signal]
        public delegate void PlayerMovedEventHandler(float direction);

        [Signal]
        public delegate void PlayerJumpEventHandler(float velY);


        public override void _Ready()
        {

        }

        public void Damage(int amount)
        {
            _health = Math.Clamp(_health - amount, 0, MAX_HEALTH);
            EmitSignal("player_damaged", [_health, amount]);
        }

        public void Bounce()
        {
            Debug.WriteLine("Shiver me bouncy.");
            _velY = JUMP_MODIFIER + ((int)(JUMP_MODIFIER * 0.2));
        }

        private void HandleMechanics()
        {
            if (Input.IsActionJustPressed(PLAYER_JUMP_ACTION) && this.IsOnFloor())
            {
                _velY = JUMP_MODIFIER;
                SFXManager._sfxmanager.PlaySFX(_jumpSound);
            }

            _direction = Input.GetAxis("ui_left", "ui_right");
       
            if (_direction != 0)
            {
                _velX = _direction * MOVEMENT_MODIFIER;
                this.EmitSignal(SignalName.PlayerMoved, [_direction]);
            }
            else
            {
                Velocity = new Vector2(0, Velocity.Y);
                this.EmitSignal(SignalName.PlayerMoved, [_direction]);
            }
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            if (!this.IsOnFloor())
            {
                _velY = Velocity.Y + ((float)(delta * Gravity));
                this.EmitSignal(SignalName.PlayerJump, Mathf.Sign(_velY));
            }
            HandleMechanics();
            MoveAndSlide();
            HandleTelekinesis(delta);
            Velocity = new Vector2(_velX, _velY);
        }
    }
}
