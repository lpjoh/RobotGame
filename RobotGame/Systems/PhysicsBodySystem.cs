﻿using Arch.Core;
using RobotGame.Components;

namespace RobotGame.Systems
{
    public class PhysicsBodySystem : ISystem
    {
        public QueryDescription Query;

        public PhysicsBodySystem()
        {
            Query = new QueryDescription().WithAll<PhysicsBodyComponent, PositionComponent>();
        }

        public static GameRect GetGameRect(PhysicsBodyComponent body, PositionComponent position)
        {
            return new GameRect(position.Position, body.Size);
        }

        public void Initialize()
        {

        }

        public void Update(World entities, float delta)
        {
            entities.Query(in Query, (
                Entity entity,
                ref PhysicsBodyComponent body,
                ref PositionComponent position) =>
            {
                // Move body by velocity
                position.Position += body.Velocity * delta;
            });
        }
    }
}
