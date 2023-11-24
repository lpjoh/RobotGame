using Arch.Core;
using RobotGame.Components;
using Microsoft.Xna.Framework;
using RobotGame.Systems;
using System.Collections.Generic;

namespace RobotGame
{
    public class GameWorld
    {
        public World Entities;

        public Entity Player;

        public RobotGame Game;

        public PhysicsBodySystem PhysicsBodySystem;
        public PlayerSystem PlayerSystem;
        public BulletSystem BulletSystem;

        public List<ISystem> Systems = new();

        public GameWorld(RobotGame game)
        {
            Game = game;

            PhysicsBodySystem = new PhysicsBodySystem();
            Systems.Add(PhysicsBodySystem);

            PlayerSystem = new PlayerSystem(Game);
            Systems.Add(PlayerSystem);

            BulletSystem = new BulletSystem(Game);
            Systems.Add(BulletSystem);
        }

        public void Initialize()
        {
            Entities = World.Create();

            PlayerSystem.Initialize();
            BulletSystem.Initialize();

            Player = PlayerSystem.CreatePlayer(Entities);
        }

        public void Update(float delta)
        {
            foreach (ISystem system in Systems)
            {
                system.Update(Entities, delta);
            }
        }
    }
}
