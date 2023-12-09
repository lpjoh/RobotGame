using Arch.Core;
using RobotGame.Components;

namespace RobotGame.Systems
{
    public class PhysicsAreaSystem : ISystem
    {
        public QueryDescription Query;

        public PhysicsAreaSystem()
        {
            Query = new QueryDescription().WithAll<PhysicsAreaComponent, PositionComponent>();
        }

        public static GameRect GetGameRect(PhysicsAreaComponent area, PositionComponent position)
        {
            return new GameRect(position.Position + area.Offset, area.Size);
        }

        public void Initialize()
        {

        }

        public void Update(World entities, float delta)
        {
            entities.Query(in Query, (
                Entity entity,
                ref PhysicsAreaComponent area,
                ref PositionComponent position) =>
            {

            });
        }
    }
}
