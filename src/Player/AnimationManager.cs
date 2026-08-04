using Godot;
using System;
using System.Diagnostics;

namespace projecttamasuccessor {
public partial class AnimationManager : AnimatedSprite2D
    {
        private const string IDLE_ANIMATION = "Idle";
        private const string RUN_ANIMATION = "Run";
        private const string HURT_ANIMATION = "Hurt";
        private const string JUMP_ANIMATION = "Jump";
        private const string JUMP_LAND_ANIMATION = "Jump_land";

        private float lastDirection = 1;
        private float lastVelY = 0;
        private int _lastFrame = 0;

        public override void _Ready()
        {
           
        }

        private void OnAnimationFinished()
        {
            Debug.WriteLine($"Animation finished: {this.Animation}, last frame: {_lastFrame}");
            // Reset the animation to the first frame when it finishes.
            if (this.Animation == IDLE_ANIMATION || this.Animation == RUN_ANIMATION)
            {
                this.Frame = _lastFrame;
                return;
            }   
        }

        public void PlayAnimation(string animationName, bool flipH)
        {
           this.FlipH = flipH;     
           this.Play(animationName);
        }

        public void OnPlayerMoved(float dir)
        {
            if(dir == 0)
            {
                PlayAnimation(IDLE_ANIMATION, lastDirection < 0);
                return;
            }
            PlayAnimation(RUN_ANIMATION, dir < 0);
            lastDirection = dir;
        }

        public void OnPlayerHurt()
        {
            PlayAnimation("Hurt", lastDirection < 0);
        }

        public void OnPLayerInAir(int velY)
        {
            Debug.WriteLine($"Player vertical velocity: {velY}, last vertical velocity: {lastVelY}");

            if (velY < 0 && lastVelY > -1)
            {
               Debug.WriteLine($"Player is jumping.");
                lastVelY = velY;
                
                PlayAnimation(JUMP_ANIMATION, lastDirection < 0);
                return;
            }

            if (velY > 0 && lastVelY < 1)
            {
                lastVelY = velY;
                PlayAnimation(JUMP_LAND_ANIMATION, lastDirection < 0);
                return;
            }

         

        }
    }
}
