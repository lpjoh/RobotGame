using Microsoft.Xna.Framework;

namespace RobotGame
{
    public class GameRect
    {
        public Vector2 Position, Size;

        public static bool Overlaps(GameRect rect1, GameRect rect2)
        {
            return false;
        }
    }
}
