using Microsoft.Xna.Framework.Input;

namespace RobotGame
{
    public class Input
    {
        public bool IsKeyPressed(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }
    }
}
