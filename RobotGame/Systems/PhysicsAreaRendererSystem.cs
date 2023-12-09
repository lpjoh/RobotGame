using Arch.Core;
using Microsoft.Xna.Framework;
using RobotGame.Components;

namespace RobotGame.Systems
{
    public class PhysicsAreaRendererSystem : ISystem
    {
        public RectRenderer RectRenderer;

        public Renderer Renderer;
        public QueryDescription Query;

        public PhysicsAreaRendererSystem(Renderer renderer)
        {
            Renderer = renderer;

            Query = new QueryDescription().WithAll<PhysicsAreaComponent, PositionComponent>();
        }

        public void Draw(Renderer renderer, World entities)
        {
            entities.Query(in Query, (
                Entity entity,
                ref PhysicsAreaComponent area,
                ref PositionComponent position) =>
            {
                // Draw bounding box of body
                RectRenderer.DrawRect(renderer, PhysicsAreaSystem.GetGameRect(area, position));
            });
        }

        public void Initialize()
        {
            RectRenderer = new RectRenderer(Renderer, Color.Green);
        }

        public void Update(World entities, float delta)
        {
        }
    }
}
