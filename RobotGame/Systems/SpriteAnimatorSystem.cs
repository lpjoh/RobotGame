using Arch.Core;
using Arch.Core.Extensions;
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

        public static void PlayAnimation(Entity entity, SpriteAnimation animation)
        {
            ref SpriteAnimatorComponent animator = ref entity.Get<SpriteAnimatorComponent>();

            if (animator.Animation == animation)
            {
                return;
            }

            animator.Animation = animation;
            animator.Time = 0.0f;
        }

        public void Update(World entities, float delta)
        {
            entities.Query(in Query, (
                Entity entity,
                ref SpriteComponent sprite,
                ref SpriteAnimatorComponent animator) =>
            {
                SpriteAnimation animation = animator.Animation;

                int frameIndex = (int)animator.Time;
                sprite.Frame = animation.Frames[frameIndex];

                animator.Time += delta * animation.FramesPerSecond;
                animator.Time %= animation.Frames.Count;
            });
        }
    }
}
