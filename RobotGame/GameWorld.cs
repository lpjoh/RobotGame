using Arch.Core;
using RobotGame.Systems;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;

namespace RobotGame
{
    public class GameWorld
    {
        public World Entities;

        public Entity Player;

        public RobotGame Game;

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

        public Queue<Entity> EntityDestructionQueue = new();

        public List<ISystem> Systems = new();

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

        // Queues an entity to be destroyed
        public void QueueDestroyEntity(Entity entity)
        {
            EntityDestructionQueue.Enqueue(entity);
        }

        public void Initialize()
        {
            Entities = World.Create();

            // Start each system by interface
            foreach (ISystem system in Systems)
            {
                system.Initialize();
            }

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
            for (int i = 0; i < EntityDestructionQueue.Count; i++)
            {
                Entities.Destroy(EntityDestructionQueue.Dequeue());
            }
        }
    }
}
