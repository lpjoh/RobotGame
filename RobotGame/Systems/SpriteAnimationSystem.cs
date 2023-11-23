using Arch.Core;
using RobotGame.Components;

namespace RobotGame.Systems
{
    public class SpriteAnimationSystem
    {
        QueryDescription Query;

        public SpriteAnimationSystem()
        {
            Query = new QueryDescription().WithAll<SpriteComponent, SpriteAnimationComponent>();
        }

        public void Update(World entities, float delta)
        {
            entities.Query(in Query, (
                ref SpriteComponent sprite,
                ref SpriteAnimationComponent animation) =>
            {
                float duration = animation.FrameDuration * animation.Frames.Count;
                int frame = (int)(animation.Time / animation.FrameDuration);

                sprite.Frame = animation.Frames[frame];
                animation.Time = (animation.Time + delta) % duration;
            });
        }
    }
}
