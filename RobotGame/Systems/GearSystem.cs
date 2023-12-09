using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RobotGame.Components;

namespace RobotGame.Systems
{
    public class GearSystem : ISystem
    {
        public Vector2 GearSize = new(16.0f, 16.0f);

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
            Entity entity = entities.Create(
                new GearComponent(),
                new PositionComponent { Position = position },
                new PhysicsAreaComponent { Size = GearSize },
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

        }
    }
}
