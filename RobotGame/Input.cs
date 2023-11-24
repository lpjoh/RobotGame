using Microsoft.Xna.Framework.Input;
using System.Numerics;

namespace RobotGame
{
    public class Input
    {
        public bool IsKeyPressed(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }

        public Vector2 GetAxis(Keys up, Keys down, Keys left, Keys right)
        {
            return new Vector2(
                (IsKeyPressed(right) ? 1.0f : 0.0f) -
                (IsKeyPressed(left) ? 1.0f : 0.0f),
                (IsKeyPressed(down) ? 1.0f : 0.0f) -
                (IsKeyPressed(up) ? 1.0f : 0.0f));
        }
    }
}
