﻿using Arch.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RobotGame.Components;

namespace RobotGame.Systems
{
    public class PhysicsBodyRendererSystem : ISystem
    {
        public RectRenderer RectRenderer;

        public Renderer Renderer;
        public QueryDescription Query;

        public PhysicsBodyRendererSystem(Renderer renderer)
        {
            Renderer = renderer;

            Query = new QueryDescription().WithAll<PhysicsBodyComponent, PositionComponent>();
        }

        public void Draw(Renderer renderer, World entities)
        {
            entities.Query(in Query, (
                Entity entity,
                ref PhysicsBodyComponent body,
                ref PositionComponent position) =>
            {
                // Draw bounding box of body
                RectRenderer.DrawRect(renderer, PhysicsBodySystem.GetGameRect(body, position));
            });
        }

        public void Initialize()
        {
            RectRenderer = new RectRenderer(Renderer, Color.Red);
        }

        public void Update(World entities, float delta)
        {
        }
    }
}
