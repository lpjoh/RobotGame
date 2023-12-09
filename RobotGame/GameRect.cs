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
            return
                rect1.Position.X < rect2.Position.X + rect2.Size.X &&
                rect1.Position.X + rect1.Size.X > rect2.Position.X &&
                rect1.Position.Y < rect2.Position.Y + rect2.Size.Y &&
                rect1.Position.Y + rect1.Size.Y > rect2.Position.Y;
        }
    }
}
