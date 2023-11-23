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

        public PlayerSystem PlayerSystem;
        public PhysicsBodySystem PhysicsBodySystem;

        public List<ISystem> Systems = new();

        public GameWorld(RobotGame game)
        {
            Game = game;

            PlayerSystem = new PlayerSystem(Game);
            Systems.Add(PlayerSystem);

            PhysicsBodySystem = new PhysicsBodySystem();
            Systems.Add(PhysicsBodySystem);
        }

        public void Initialize()
        {
            Entities = World.Create();

            PlayerSystem.Initialize();
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
