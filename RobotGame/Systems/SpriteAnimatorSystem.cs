using Arch.Core;
using RobotGame.Components;

namespace RobotGame.Systems
{
    public class SpriteAnimatorSystem : ISystem
    {
        public QueryDescription Query;

        public SpriteAnimatorSystem()
        {
            Query = new QueryDescription().WithAll<SpriteComponent, SpriteAnimatorComponent>();
        }

        // Plays an animation on an animator
        public static void PlayAnimation(ref SpriteAnimatorComponent animator, SpriteAnimation animation)
        {
            // Stop if animation already playing
            if (animator.Animation == animation)
            {
                return;
            }

            // Play new animation with resetted time
            animator.Animation = animation;
            animator.Time = 0.0f;
        }

        public void Initialize()
        {

        }

        public void Update(World entities, float delta)
        {
            entities.Query(in Query, (
                Entity entity,
                ref SpriteComponent sprite,
                ref SpriteAnimatorComponent animator) =>
            {
                SpriteAnimation animation = animator.Animation;

                // Stop if no animation 
                if (animation == null)
                {
                    return;
                }

                // Get frame index by flooring time
                int frameIndex = (int)animator.Time;

                // Load frame rect
                sprite.Frame = animation.Frames[frameIndex];

                // Advance animation by FPS with looping
                animator.Time += delta * animation.FramesPerSecond;
                animator.Time %= animation.Frames.Count;
            });
        }
    }
}
