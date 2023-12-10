using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Text.Json;

namespace RobotGame
{
    public class TextRenderer
    {
        public Texture2D Texture;
        public Point CharSize;
        public int CharsPerRow;
        public int[] CharSpacings;

        public TextRenderer(Texture2D texture, Point charSize, string spacingsPath)
        {
            Texture = texture;
            CharSize = charSize;

            CharsPerRow = Texture.Width / CharSize.X;

            // Load font spacing from json
            string spacingsJson = File.ReadAllText(spacingsPath);
            CharSpacings = JsonSerializer.Deserialize<int[]>(spacingsJson);
        }

        public void DrawText(Renderer renderer, string text, Vector2 position)
        {
            Vector2 charPosition = position;

            foreach (char c in text)
            {
                // Get relative ASCII index
                int charIndex = c - 32;

                // Create source rectangle
                Rectangle srcRect = new(new Point(charIndex % CharsPerRow, charIndex / CharsPerRow) * CharSize, CharSize);

                renderer.SpriteBatch.Draw(Texture, charPosition, srcRect, Color.White);

                // Add character spacing
                if (charIndex >= 0 && charIndex < CharSpacings.Length)
                {
                    charPosition.X += CharSpacings[charIndex];
                }
            }
        }
    }
}
