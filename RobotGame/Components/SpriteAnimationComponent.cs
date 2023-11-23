using Arch.Core;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace RobotGame.Components
{
    public struct SpriteAnimationComponent
    {
        public List<Rectangle> Frames;
        public float FrameDuration;

        public float Time;
    }
}
