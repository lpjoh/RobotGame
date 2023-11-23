using Arch.Core;
using RobotGame.Components;
using Microsoft.Xna.Framework;

namespace RobotGame
{
    public class GameWorld
    {
        public World Entities;

        public Entity Player;

        public void Initialize(RobotGame game)
        {
            Entities = World.Create();

            Player = Entities.Create(
                new PositionComponent { Position = new Vector2(0, 0) });
        }
    }
}
