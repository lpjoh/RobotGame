using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RobotGame
{
    public class SpriteAnimation
    {
        public List<Rectangle> Frames;
        public float FramesPerSecond;

        public static List<Rectangle> GetFrames(Texture2D texture, int frameCount)
        {
            List<Rectangle> frames = new();

            int frameWidth = texture.Width / frameCount;
            int frameHeight = texture.Height;

            for (int i = 0; i < frameCount; i++)
            {
                frames.Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
            }

            return frames;
        }

        public SpriteAnimation(List<Rectangle> frames, float framesPerSecond)
        {
            Frames = frames;
            FramesPerSecond = framesPerSecond;
        }
    }
}
