using Arch.Core;
using RobotGame.Systems;
using System.Collections.Generic;
using System.Numerics;

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
        public GearSystem GearSystem;

        public HealthBar HealthBar;

        public List<ISystem> Systems = new();

        public GameWorld(RobotGame game)
        {
            Game = game;

            // Create systems
            PhysicsBodySystem = new PhysicsBodySystem();
            Systems.Add(PhysicsBodySystem);

            PhysicsAreaSystem = new PhysicsAreaSystem();
            Systems.Add(PhysicsAreaSystem);

            PlayerSystem = new PlayerSystem(Game);
            Systems.Add(PlayerSystem);

            BulletSystem = new BulletSystem(Game);
            Systems.Add(BulletSystem);

            GearSystem = new GearSystem(Game);
            Systems.Add(GearSystem);

            HealthBar = new HealthBar(Game);
        }

        public void Initialize()
        {
            Entities = World.Create();

            // Start each system by interface
            foreach (ISystem system in Systems)
            {
                system.Initialize();
            }

            Player = PlayerSystem.CreatePlayer(Entities);

            GearSystem.CreateGear(Entities, Vector2.One * 32.0f);
        }

        public void Update(float delta)
        {
            // Update each system by interface
            foreach (ISystem system in Systems)
            {
                system.Update(Entities, delta);
            }
        }
    }
}
