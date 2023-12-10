using Microsoft.Xna.Framework;

namespace RobotGame
{
    public class EnemyFactory
    {
        public const float SpawnTime = 3.0f;
        public const float SpawnPointPadding = 16.0f;

        public RobotGame Game;
        public float SpawnTimer;

        public Vector2[] SpawnPoints;

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
        }

        public void Update(float delta)
        {
            if (SpawnTimer <= 0.0f)
            {
                SpawnTimer = SpawnTime;

                // Spawn at random position
                Vector2 spawnPoint = SpawnPoints[Game.World.Random.Next() % SpawnPoints.Length];

                Game.World.AlienEnemySystem.CreateAlienEnemy(Game.World.Entities, spawnPoint);
            }
            else
            {
                SpawnTimer -= delta;
            }
        }
    }
}
