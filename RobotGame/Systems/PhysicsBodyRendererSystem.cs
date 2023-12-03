using Arch.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RobotGame.Components;

namespace RobotGame.Systems
{
    public class PhysicsBodyRendererSystem : ISystem
    {
        public Texture2D RectTexture;

        public QueryDescription Query;

        public Renderer Renderer;

        public PhysicsBodyRendererSystem(Renderer renderer)
        {
            Renderer = renderer;

            Query = new QueryDescription().WithAll<PhysicsBodyComponent, PositionComponent>();
        }

        public void Draw(World entities, Renderer renderer)
        {
            entities.Query(in Query, (
                ref Entity entity,
                ref PhysicsBodyComponent body,
                ref PositionComponent position) =>
            {
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
            RectTexture = new Texture2D(Renderer.GraphicsDevice, 1, 1);
            RectTexture.SetData(new[] { new Color(Color.Red, 0.1f) });
        }

        public void Update(World entities, float delta)
        {
        }
    }
}
