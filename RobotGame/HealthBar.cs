using Arch.Core;
using Microsoft.Xna.Framework;

namespace RobotGame
{
    public class HealthBar
    {
        public Entity Player;
        public Vector2 Position = new(2.0f, 2.0f);

        public void Draw(Renderer renderer)
        {
            renderer.SpriteBatch.Draw(renderer.HealthBackTexture, Position, Color.White);
        }
    }
}
