﻿using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using RobotGame.Components;
using System.Collections.Generic;

namespace RobotGame
{
    public class EnemyFactory
    {
        public const float SpawnTime = 3.0f;
        public const float SpawnPointPadding = 16.0f;
        public const int MaxEnemies = 8;
        public const float MinPlayerDistance = 32.0f;

        public RobotGame Game;
        public float SpawnTimer;

        public Vector2[] SpawnPoints;

        public QueryDescription EnemyQuery;

        public EnemyFactory(RobotGame game)
        {
            Game = game;

            GameRect wallsRect = Game.World.WallsRect;

            Vector2
                spawnPointsStart = wallsRect.Position + new Vector2(SpawnPointPadding),
                spawnPointsEnd = wallsRect.Position + wallsRect.Size - new Vector2(SpawnPointPadding);

            SpawnPoints = new Vector2[]
            {
                spawnPointsStart,
                new Vector2(spawnPointsEnd.X, spawnPointsStart.Y),
                new Vector2(spawnPointsStart.X, spawnPointsEnd.Y),
                spawnPointsEnd
            };

            EnemyQuery = new QueryDescription().WithAll<EnemyComponent>();
        }

        public void Update(float delta)
        {
            if (SpawnTimer <= 0.0f)
            {
                SpawnTimer = SpawnTime;

                // Check enemy threshold
                List<Entity> enemies = new();
                Game.World.Entities.GetEntities(EnemyQuery, enemies);

                if (enemies.Count < MaxEnemies)
                {
                    // Spawn at random position, away from player
                    Entity player = Game.World.Player;
                    Vector2 playerPosition = player.Get<PositionComponent>().Position + player.Get<PhysicsBodyComponent>().Size * 0.5f;

                    Vector2 spawnPoint;

                    while (true)
                    {
                        spawnPoint = SpawnPoints[Game.World.Random.Next() % SpawnPoints.Length];
                        
                        if (Vector2.Distance(playerPosition, spawnPoint) >= MinPlayerDistance)
                        {
                            break;
                        }
                    }

                    Game.World.AlienEnemySystem.CreateAlienEnemy(Game.World.Entities, spawnPoint);
                }
            }
            else
            {
                SpawnTimer -= delta;
            }
        }
    }
}
