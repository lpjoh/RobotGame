using Arch.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RobotGame.Components;
using System.Collections.Generic;

namespace RobotGame.Systems
{
    public class SpriteSystem
    {
        QueryDescription Query;

        public SpriteSystem()
        {
            Query = new QueryDescription().WithAll<SpriteComponent, PositionComponent>();
        }

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

        public void Draw(World entities, Renderer renderer)
        {
            entities.Query(in Query, (
                ref SpriteComponent sprite,
                ref PositionComponent position) =>
            {
                renderer.SpriteBatch.Draw(
                    sprite.Texture, position.Position, sprite.Frame, Color.White);
            });
        }
    }
}
