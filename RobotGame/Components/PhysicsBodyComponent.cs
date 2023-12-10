using Microsoft.Xna.Framework;

namespace RobotGame.Components
{
    public struct PhysicsBodyComponent
    {
        public Vector2 Size;
        public int MoverMask, ColliderMask;

        public PhysicsBodyComponent()
        {
            MoverMask = 1;
            ColliderMask = 1;
        }
    }
}
