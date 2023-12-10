﻿using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RobotGame.Components;

namespace RobotGame.Systems
{
    public class GearSystem : ISystem
    {
        public Vector2 AreaSize = new(16.0f, 16.0f);

        public GameRect[] AreaRects;

        public SpriteAnimation TurnAnimation;

        public RobotGame Game;
        public QueryDescription Query;

        public GearSystem(RobotGame game)
        {
            Game = game;

            Query = new QueryDescription().WithAll<
                GearComponent,
                PositionComponent,
                PhysicsAreaComponent,
                CollectibleComponent,
                SpriteComponent,
                SpriteAnimatorComponent>();
        }

        // Spawns a new gear
        public Entity CreateGear(World entities, Vector2 position)
        {
            AreaRects = new GameRect[]
            {
                new GameRect(Vector2.Zero, AreaSize)
            };

            Entity entity = entities.Create(
                new GearComponent(),
                new PositionComponent { Position = position },
                new PhysicsAreaComponent { Rects = AreaRects },
                new CollectibleComponent(),
                new SpriteComponent { Texture = Game.Renderer.GearTexture },
                new SpriteAnimatorComponent());

            // Starting animation
            SpriteAnimatorSystem.PlayAnimation(ref entity.Get<SpriteAnimatorComponent>(), TurnAnimation);

            return entity;
        }

        public void Initialize()
        {
            // Create animation
            Texture2D texture = Game.Renderer.GearTexture;

            TurnAnimation = new SpriteAnimation(
                SpriteAnimatorSystem.GetFrames(texture, 2), 10.0f);
        }

        public void Update(World entities, float delta)
        {
            entities.Query(in Query, (
                Entity entity,
                ref GearComponent gear,
                ref PositionComponent position,
                ref PhysicsAreaComponent area,
                ref CollectibleComponent collectible,
                ref SpriteComponent sprite,
                ref SpriteAnimatorComponent spriteAnimator) =>
            {
                if (collectible.Collected)
                {
                    Game.World.CollectibleSystem.CollectEntity(entity);
                }
            });
        }
    }
}
