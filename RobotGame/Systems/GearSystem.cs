using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
                new SpriteComponent { Texture = Game.Renderer.GearTexture },
                new SpriteAnimatorComponent());

            // Start with flashing animation
            SpriteAnimatorSystem.PlayAnimation(ref entity.Get<SpriteAnimatorComponent>(), TurnAnimation);

            return entity;
        }

        public void Initialize()
        {
            // Create animation
            Texture2D texture = Game.Renderer.GearTexture;

            TurnAnimation = new SpriteAnimation(
                SpriteAnimation.GetFrames(texture, 2), 10.0f);
        }

        public void Update(World entities, float delta)
        {
            entities.Query(in Query, (
                Entity entity,
                ref GearComponent gear,
                ref PositionComponent position,
                ref PhysicsAreaComponent area,
                ref SpriteComponent sprite,
                ref SpriteAnimatorComponent spriteAnimator) =>
            {
                // Check if collected by player
                foreach (PhysicsAreaCollision collision in area.Collisions)
                {
                    if (collision.Entity.Has<PlayerComponent>())
                    {
                        Game.World.QueueDestroyEntity(entity);
                    }
                }
            });
        }
    }
}
