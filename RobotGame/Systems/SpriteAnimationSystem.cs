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
                int frameIndex = (int)animation.Time;
                sprite.Frame = animation.Frames[frameIndex];

                animation.Time += delta * animation.FramesPerSecond;
                animation.Time %= animation.Frames.Count;
            });
        }
    }
}
