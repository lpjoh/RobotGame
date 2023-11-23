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

        public void Update(World entities, float delta)
        {
            entities.Query(in Query, (
                Entity entity,
                ref PhysicsBodyComponent body,
                ref PositionComponent position) =>
            {
                position.Position += body.Velocity * delta;
            });
        }
    }
}