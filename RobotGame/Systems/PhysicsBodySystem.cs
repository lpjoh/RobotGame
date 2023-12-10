using Arch.Core;
using Arch.Core.Extensions;
using RobotGame.Components;
using System.Collections.Generic;

namespace RobotGame.Systems
{
    public class PhysicsBodySystem : ISystem
    {
        public QueryDescription MoverQuery, ColliderQuery;

        public PhysicsBodySystem()
        {
            MoverQuery = new QueryDescription().WithAll<PhysicsBodyComponent, PositionComponent, VelocityComponent>();
            ColliderQuery = new QueryDescription().WithAll<PhysicsBodyComponent, PositionComponent>();
        }

        // Gets the rect of a body 
        public static GameRect GetRect(PhysicsBodyComponent body, PositionComponent position)
        {
            return new GameRect(position.Position, body.Size);
        }

        // Returns whether or not two entities have matching layers
        public static bool BodyLayersMatch(
            ref PhysicsBodyComponent moverBody,
            ref PhysicsBodyComponent collderBody)
        {
            return (moverBody.MoverMask & collderBody.ColliderMask) != 0;
        }

        // Moves an entity along the X axis
        public float MoveAndCollideX(
            ref PhysicsBodyComponent moverBody,
            ref PositionComponent moverPosition,
            ref VelocityComponent moverVelocity,
            
            ref PhysicsBodyComponent colliderBody,
            ref PositionComponent colliderPosition,

            float endPosition)
        {
            // Check vertical alignment
            if (moverPosition.Position.Y + moverBody.Size.Y <= colliderPosition.Position.Y ||
                moverPosition.Position.Y >= colliderPosition.Position.Y + colliderBody.Size.Y)
            {
                return endPosition;
            }

            if (moverVelocity.Velocity.X < 0.0f)
            {
                // Left
                if (moverPosition.Position.X + moverBody.Size.X <= colliderPosition.Position.X)
                {
                    return endPosition;
                }

                float wall = colliderPosition.Position.X + colliderBody.Size.X;

                if (endPosition < wall)
                {
                    return wall;
                }
            }
            else
            {
                // Right
                if (moverPosition.Position.X >= colliderPosition.Position.X + colliderBody.Size.X)
                {
                    return endPosition;
                }

                float wall = colliderPosition.Position.X - moverBody.Size.X;

                if (endPosition > wall)
                {
                    return wall;
                }
            }

            // No collision
            return endPosition;
        }

        // Moves an entity along the Y axis
        public float MoveAndCollideY(
            ref PhysicsBodyComponent moverBody,
            ref PositionComponent moverPosition,
            ref VelocityComponent moverVelocity,

            ref PhysicsBodyComponent colliderBody,
            ref PositionComponent colliderPosition,

            float endPosition)
        {
            // Check horizontal alignment
            if (moverPosition.Position.X + moverBody.Size.X <= colliderPosition.Position.X ||
                moverPosition.Position.X >= colliderPosition.Position.X + colliderBody.Size.X)
            {
                return endPosition;
            }

            if (moverVelocity.Velocity.Y < 0.0f)
            {
                // Up
                if (moverPosition.Position.Y + moverBody.Size.Y <= colliderPosition.Position.Y)
                {
                    return endPosition;
                }

                float wall = colliderPosition.Position.Y + colliderBody.Size.Y;

                if (endPosition < wall)
                {
                    return wall;
                }
            }
            else
            {
                // Down
                if (moverPosition.Position.Y >= colliderPosition.Position.Y + colliderBody.Size.Y)
                {
                    return endPosition;
                }

                float wall = colliderPosition.Position.Y - moverBody.Size.Y;

                if (endPosition > wall)
                {
                    return wall;
                }
            }

            // No collision
            return endPosition;
        }

        public void Initialize()
        {

        }

        public void Update(World entities, float delta)
        {
            // Put matching entities into list
            List<Entity> movers = new(), colliders = new();

            entities.GetEntities(MoverQuery, movers);
            entities.GetEntities(ColliderQuery, colliders);

            foreach (Entity mover in movers)
            {
                ref PhysicsBodyComponent moverBody = ref mover.Get<PhysicsBodyComponent>();
                ref PositionComponent moverPosition = ref mover.Get<PositionComponent>();
                ref VelocityComponent moverVelocity = ref mover.Get<VelocityComponent>();

                // Collide X
                if (moverVelocity.Velocity.X != 0.0f)
                {
                    float endPosition = moverPosition.Position.X + moverVelocity.Velocity.X * delta;

                    foreach (Entity collider in colliders)
                    {
                        // Skip matches
                        if (mover == collider)
                        {
                            continue;
                        }

                        ref PhysicsBodyComponent colliderBody = ref collider.Get<PhysicsBodyComponent>();

                        if (!BodyLayersMatch(ref moverBody, ref colliderBody))
                        {
                            continue;
                        }

                        ref PositionComponent colliderPosition = ref collider.Get<PositionComponent>();

                        endPosition = MoveAndCollideX(
                            ref moverBody, ref moverPosition, ref moverVelocity,
                            ref colliderBody, ref colliderPosition,
                            endPosition);
                    }

                    moverPosition.Position.X = endPosition;
                }

                // Collide Y
                if (moverVelocity.Velocity.Y != 0.0f)
                {
                    float endPosition = moverPosition.Position.Y + moverVelocity.Velocity.Y * delta;

                    foreach (Entity collider in colliders)
                    {
                        // Skip matches
                        if (mover == collider)
                        {
                            continue;
                        }

                        ref PhysicsBodyComponent colliderBody = ref collider.Get<PhysicsBodyComponent>();

                        if (!BodyLayersMatch(ref moverBody, ref colliderBody))
                        {
                            continue;
                        }

                        ref PositionComponent colliderPosition = ref collider.Get<PositionComponent>();

                        endPosition = MoveAndCollideY(
                            ref moverBody, ref moverPosition, ref moverVelocity,
                            ref colliderBody, ref colliderPosition,
                            endPosition);
                    }

                    moverPosition.Position.Y = endPosition;
                }
            }
        }
    }
}
