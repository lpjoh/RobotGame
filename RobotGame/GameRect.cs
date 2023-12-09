using Microsoft.Xna.Framework;

namespace RobotGame
{
    public class GameRect
    {
        public Vector2 Position, Size;

        public GameRect(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        // Returns whether or not two rects overlap
        public static bool Overlaps(GameRect rect1, GameRect rect2)
        {
            return false;
        }
    }
}
