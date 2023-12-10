using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using RobotGame.Components;

namespace RobotGame.Systems
{
    public class EnemySystem : ISystem
    {
        public RobotGame Game;

        public EnemySystem(RobotGame game)
        {
            Game = game;
        }

        // Destroys an enemy
        public void DestroyEnemy(Entity entity)
        {
            World entities = Game.World.Entities;

            // Create a random drop
            Vector2 dropPosition = entity.Get<PositionComponent>().Position + entity.Get<PhysicsBodyComponent>().Size * 0.5f;

            if (Game.World.Random.Next() % 4 == 0)
            {
                Game.World.BatterySystem.CreateBattery(entities, dropPosition);
            }
            else
            {
                Game.World.GearSystem.CreateGear(entities, dropPosition);
            }

            Game.World.QueueDestroyEntity(entity);
        }

        public void Initialize()
        {
            
        }

        public void Update(World entities, float delta)
        {
            
        }
    }
}
