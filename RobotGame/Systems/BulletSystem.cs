using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RobotGame.Components;

namespace RobotGame.Systems
{
    public class BulletSystem : ISystem
    {
        public const float Speed = 100.0f;

        public Vector2 BulletSize = new Vector2(8.0f, 8.0f);

        public QueryDescription Query;

        public SpriteAnimation FlashAnimation;

        public RobotGame Game;

        public BulletSystem(RobotGame game)
        {
            Game = game;

            Query = new QueryDescription().WithAll<
                BulletComponent,
                PositionComponent,
                PhysicsBodyComponent,
                SpriteComponent,
                SpriteAnimatorComponent>();
        }

        // Spawns a new bullet
        public Entity CreateBullet(World entities, Vector2 position, Vector2 direction)
        {
            Entity entity = entities.Create(
                new BulletComponent(),
                new PositionComponent { Position = position },
                new PhysicsBodyComponent { Size = BulletSize, Velocity = direction * Speed },
                new SpriteComponent { Texture = Game.Renderer.PlayerBulletTexture },
                new SpriteAnimatorComponent());

            // Start with flashing animation
            SpriteAnimatorSystem.PlayAnimation(ref entity.Get<SpriteAnimatorComponent>(), FlashAnimation);

            return entity;
        }

        public void Initialize()
        {
            Texture2D texture = Game.Renderer.PlayerBulletTexture;

            FlashAnimation = new SpriteAnimation(
                SpriteAnimation.GetFrames(texture, 2), 10.0f);
        }

        public void Update(World entities, float delta)
        {
            
        }
    }
}
