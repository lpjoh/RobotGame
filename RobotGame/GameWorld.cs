using Arch.Core;
using RobotGame.Systems;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using RobotGame.Components;

namespace RobotGame
{
    public class GameWorld
    {
        public const float WallThickness = 8.0f;

        public GameRect WallsRect = new(new Vector2(30.0f, 18.0f), new Vector2(180.0f, 144.0f));

        public World Entities;

        public Entity Player;

        public PhysicsBodySystem PhysicsBodySystem;
        public PhysicsAreaSystem PhysicsAreaSystem;

        public PlayerSystem PlayerSystem;
        public BulletSystem BulletSystem;

        public EnemySystem EnemySystem;
        public AlienEnemySystem AlienEnemySystem;

        public CollectibleSystem CollectibleSystem;
        public GearSystem GearSystem;
        public BatterySystem BatterySystem;

        public HealthBar HealthBar;
        public GearDisplay GearDisplay;

        public Random Random = new();

        public List<Entity> EntityDestructionQueue = new();

        public List<ISystem> Systems = new();

        public RobotGame Game;

        public GameWorld(RobotGame game)
        {
            Game = game;

            // Generic systems
            PhysicsBodySystem = new PhysicsBodySystem();
            Systems.Add(PhysicsBodySystem);

            PhysicsAreaSystem = new PhysicsAreaSystem();
            Systems.Add(PhysicsAreaSystem);

            // Main entities
            PlayerSystem = new PlayerSystem(Game);
            Systems.Add(PlayerSystem);

            BulletSystem = new BulletSystem(Game);
            Systems.Add(BulletSystem);

            EnemySystem = new EnemySystem(Game);
            Systems.Add(EnemySystem);

            AlienEnemySystem = new AlienEnemySystem(Game);
            Systems.Add(AlienEnemySystem);

            // Collectibles
            CollectibleSystem = new CollectibleSystem(Game);
            Systems.Add(CollectibleSystem);

            GearSystem = new GearSystem(Game);
            Systems.Add(GearSystem);

            BatterySystem = new BatterySystem(Game);
            Systems.Add(BatterySystem);

            // Create stat displays
            HealthBar = new HealthBar(Game);
            GearDisplay = new GearDisplay(Game);
        }

        // Creates a wall
        public void CreateWall(GameRect rect)
        {
            Entities.Create(
                new PositionComponent { Position = rect.Position },
                new PhysicsBodyComponent { Size = rect.Size });
        }

        // Creates the game's walls
        public void CreateWalls()
        {
            // Get rect sizes
            Vector2 wideSize = new(WallsRect.Size.X, WallThickness), tallSize = new(WallThickness, WallsRect.Size.Y);

            // Left
            CreateWall(new GameRect(
                new Vector2(WallsRect.Position.X - WallThickness, WallsRect.Position.Y), tallSize));

            // Right
            CreateWall(new GameRect(
                new Vector2(WallsRect.Position.X + WallsRect.Size.X, WallsRect.Position.Y), tallSize));

            // Top
            CreateWall(new GameRect(
                new Vector2(WallsRect.Position.X, WallsRect.Position.Y - WallThickness), wideSize));

            // Bottom
            CreateWall(new GameRect(
                new Vector2(WallsRect.Position.X, WallsRect.Position.Y + WallsRect.Size.Y), wideSize));
        }

        // Queues an entity to be destroyed
        public void QueueDestroyEntity(Entity entity)
        {
            EntityDestructionQueue.Add(entity);
        }

        public void Initialize()
        {
            Entities = World.Create();

            // Start each system by interface
            foreach (ISystem system in Systems)
            {
                system.Initialize();
            }

            CreateWalls();

            Player = PlayerSystem.CreatePlayer(Entities, new Vector2(120.0f, 90.0f));

            GearSystem.CreateGear(Entities, Vector2.One * 32.0f);
            BatterySystem.CreateBattery(Entities, Vector2.One * 64.0f);

            AlienEnemySystem.CreateAlienEnemy(Entities, Vector2.One * 96.0f);

            HealthBar.UpdateDisplay();
        }

        public void Update(float delta)
        {
            // Update each system by interface
            foreach (ISystem system in Systems)
            {
                system.Update(Entities, delta);
            }

            // Delete queued entities
            foreach (Entity entity in EntityDestructionQueue)
            {
                Entities.Destroy(entity);
            }

            EntityDestructionQueue.Clear();
        }
    }
}
