using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projecttamasuccessor.NPC
{
    public partial class CreatureAnimationManager : AnimatedSprite2D
    {
        private const string IDLE_ANIMATION = "Idle";
        private const string MOVE_ANIMATION = "Move";
        private const string HURT_ANIMATION = "Hurt";
        private const string DEATH_ANIMATION = "Dying";
        private const string DEAD_ANIMATION = "Dead";

        private float lastDirection = 1;

        public void PlayAnimation(string animationName, bool flipH)
        {
            this.FlipH = flipH;
            this.Play(animationName);
        }

        public void PlayAnimations(string[] animationNames, bool flipH, int cur = 0)
        {
            this.FlipH = flipH;

            this.Play(animationNames[cur]);
            //TODO: I have to look into if arrow function here is optimized or not. It looks wacky, and was my first instict.
            this.AnimationFinished += () =>
            {
                cur++;
                if (cur < animationNames.Length)
                {
                    PlayAnimations(animationNames, flipH, cur);
                }
            };
        }

        public void OnCreatureMoved(float dir)
        {
            if (dir == 0)
            {
                PlayAnimation(MOVE_ANIMATION, lastDirection < 0);
                return;
            }
            PlayAnimation(MOVE_ANIMATION, dir < 0);
            lastDirection = dir;
        }

        public void OnCreatureHurt(int current, int change, int max)
        {

            // If the creature is dead, we want to play the death animation, and then the dead animation.
            // We don't want to play the hurt animation in this case.
            Debug.WriteLine($"Creature health changed from {current - change} to {current}. Change: {change}. Max: {max}");
            if (current == 0)
            {
                Debug.WriteLine("Creature is dead, playing death animation");
                PlayAnimation(DEAD_ANIMATION, lastDirection < 0);
                return;
            }

            // If change is not negative sign, we don't want to play the hurt animation
            if (change > 0) return;
            Debug.WriteLine("Creature is hurt, playing hurt animation");
            PlayAnimation(HURT_ANIMATION, lastDirection < 0);
        }
    }
}
