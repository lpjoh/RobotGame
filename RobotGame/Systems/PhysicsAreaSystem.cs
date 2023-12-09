﻿using Arch.Core;
using Arch.Core.Extensions;
using RobotGame.Components;
using System.Collections.Generic;

namespace RobotGame.Systems
{
    public class PhysicsAreaSystem : ISystem
    {
        public QueryDescription Query;

        public PhysicsAreaSystem()
        {
            Query = new QueryDescription().WithAll<PhysicsAreaComponent, PositionComponent>();
        }

        public static GameRect GetRect(PhysicsAreaComponent area, PositionComponent position, int index)
        {
            // Get area rect at index
            GameRect rect = area.Rects[index];

            return new GameRect(position.Position + rect.Position, rect.Size);
        }

        // Creates or empties collisions in an area
        public static void ResetCollisions(ref PhysicsAreaComponent area)
        {
            if (area.Collisions == null)
            {
                area.Collisions = new List<PhysicsAreaCollision>();
            }
            else
            {
                area.Collisions.Clear();
            }
        }

        public void Initialize()
        {

        }

        public void Update(World entities, float delta)
        {
            // Put every matching entity into list
            List<Entity> areaEntities = new();
            entities.Query(in Query, areaEntities.Add);

            // Test every entity against every other
            for (int ei1 = 0; ei1 < areaEntities.Count; ei1++)
            {
                Entity entity1 = areaEntities[ei1];

                ref PhysicsAreaComponent area1 = ref entity1.Get<PhysicsAreaComponent>();
                ref PositionComponent position1 = ref entity1.Get<PositionComponent>();

                ResetCollisions(ref area1);

                for (int ei2 = 0; ei2 < areaEntities.Count; ei2++)
                {
                    // End loop when indices match
                    if (ei1 == ei2)
                    {
                        break;
                    }

                    Entity entity2 = areaEntities[ei2];

                    ref PhysicsAreaComponent area2 = ref entity2.Get<PhysicsAreaComponent>();
                    ref PositionComponent position2 = ref entity2.Get<PositionComponent>();

                    ResetCollisions(ref area2);

                    // Test every rect against every other
                    for (int ri1 = 0; ri1 < area1.Rects.Length; ri1++)
                    {
                        GameRect rect1 = GetRect(area1, position1, ri1);

                        for (int ri2 = 0; ri2 < area2.Rects.Length; ri2++)
                        {
                            GameRect rect2 = GetRect(area2, position2, ri2);

                            // Signal collision for both sides on overlap
                            if (GameRect.Overlaps(rect1, rect2))
                            {
                                area1.Collisions.Add(new PhysicsAreaCollision
                                {
                                    Entity = entity2,
                                    RectIndex = ri1,
                                    EntityRectIndex = ri2
                                });

                                area2.Collisions.Add(new PhysicsAreaCollision
                                {
                                    Entity = entity1,
                                    RectIndex = ri2,
                                    EntityRectIndex = ri1
                                });
                            }
                        }
                    }
                }
            }
        }
    }
}