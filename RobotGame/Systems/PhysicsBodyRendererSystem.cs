using Arch.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RobotGame.Components;

namespace RobotGame.Systems
{
    public class PhysicsBodyRendererSystem : ISystem
    {
        public Texture2D RectTexture;

        public Renderer Renderer;
        public QueryDescription Query;

        public PhysicsBodyRendererSystem(Renderer renderer)
        {
            Renderer = renderer;

            Query = new QueryDescription().WithAll<PhysicsBodyComponent, PositionComponent>();
        }

        public void Draw(World entities, Renderer renderer)
        {
            entities.Query(in Query, (
                Entity entity,
                ref PhysicsBodyComponent body,
                ref PositionComponent position) =>
            {
                // Draw bounding box of physics body
                Rectangle entityRect = new Rectangle(
                    (int)position.Position.X,
                    (int)position.Position.Y,
                    (int)body.Size.X,
                    (int)body.Size.Y);

                renderer.SpriteBatch.Draw(RectTexture, entityRect, Color.White);
            });
        }

        public void Initialize()
        {
            // Create 1x1 texture to represent hitboxes
            RectTexture = new Texture2D(Renderer.GraphicsDevice, 1, 1);
            RectTexture.SetData(new[] { new Color(Color.Red, 0.1f) });
        }

        public void Update(World entities, float delta)
        {
        }
    }
}
