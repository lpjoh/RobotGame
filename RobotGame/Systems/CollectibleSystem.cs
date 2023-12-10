using Arch.Core;
using Arch.Core.Extensions;
using RobotGame.Components;

namespace RobotGame.Systems
{
    public class CollectibleSystem : ISystem
    {
        public RobotGame Game;
        public QueryDescription Query;

        public CollectibleSystem(RobotGame game)
        {
            Game = game;

            Query = new QueryDescription().WithAll<
                CollectibleComponent, 
                PhysicsAreaComponent>();
        }

        public void CollectEntity(Entity entity)
        {
            Game.World.QueueDestroyEntity(entity);
        }

        public void Initialize()
        {

        }

        public void Update(World entities, float delta)
        {
            entities.Query(in Query, (
                Entity entity,
                ref CollectibleComponent collectible,
                ref PhysicsAreaComponent area) =>
            {
                // Check if collected by player
                foreach (PhysicsAreaCollision collision in area.Collisions)
                {
                    if (collision.Entity.Has<PlayerComponent>())
                    {
                        collectible.Collected = true;
                    }
                }
            });
        }
    }
}
