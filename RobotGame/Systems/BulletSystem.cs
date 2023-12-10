using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RobotGame.Components;

namespace RobotGame.Systems
{
    public enum BulletType
    {
        Player, Enemy
    }

    public class BulletSystem : ISystem
    {
        public const float Speed = 100.0f;

        public Vector2 BodySize = new Vector2(8.0f, 8.0f);

        public SpriteAnimation FlashAnimation;

        public RobotGame Game;
        public QueryDescription Query;

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
        public Entity CreateBullet(World entities, Vector2 position, Vector2 direction, BulletType type)
        {
            Texture2D texture = type switch
            {
                BulletType.Player => Game.Renderer.PlayerBulletTexture,
                BulletType.Enemy => Game.Renderer.EnemyBulletTexture,
                _ => Game.Renderer.PlayerBulletTexture
            };

            Entity entity = entities.Create(
                new BulletComponent(),
                new PositionComponent { Position = position - BodySize * 0.5f },
                new PhysicsBodyComponent { Size = BodySize, Velocity = direction * Speed },
                new SpriteComponent { Texture = texture },
                new SpriteAnimatorComponent());

            // Starting animation
            SpriteAnimatorSystem.PlayAnimation(ref entity.Get<SpriteAnimatorComponent>(), FlashAnimation);

            return entity;
        }

        public void Initialize()
        {
            // Create animation
            Texture2D texture = Game.Renderer.PlayerBulletTexture;

            FlashAnimation = new SpriteAnimation(
                SpriteAnimatorSystem.GetFrames(texture, 2), 10.0f);
        }

        public void Update(World entities, float delta)
        {
            
        }
    }
}
