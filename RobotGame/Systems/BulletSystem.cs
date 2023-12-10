using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RobotGame.Components;
using System;

namespace RobotGame.Systems
{
    public class BulletSystem : ISystem
    {
        public const float Speed = 100.0f;

        public Vector2 BodySize = new(4.0f, 4.0f);

        public Vector2 AreaSize = new(6.0f, 6.0f);
        public GameRect[] AreaRects;

        public Vector2 SpriteOffset = new(-2.0f, -2.0f);

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
                PhysicsAreaComponent,
                SpriteComponent,
                SpriteAnimatorComponent>();

            // Create area rect
            AreaRects = new GameRect[]
            {
                new GameRect((BodySize - AreaSize) * 0.5f, AreaSize)
            };
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
                new BulletComponent { Type = type },
                new PositionComponent { Position = position - BodySize * 0.5f },
                new PhysicsBodyComponent { Size = BodySize, Velocity = direction * Speed },
                new PhysicsAreaComponent { Rects = AreaRects },
                new SpriteComponent { Texture = texture, Offset = SpriteOffset },
                new SpriteAnimatorComponent());

            // Starting animation
            SpriteAnimatorSystem.PlayAnimation(ref entity.Get<SpriteAnimatorComponent>(), FlashAnimation);

            return entity;
        }

        // Destroys a bullet
        public void DestroyBullet(Entity entity)
        {
            Game.World.QueueDestroyEntity(entity);
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
            entities.Query(in Query, (
                Entity entity,
                ref BulletComponent bullet,
                ref PositionComponent position,
                ref PhysicsBodyComponent body,
                ref PhysicsAreaComponent area,
                ref SpriteComponent sprite,
                ref SpriteAnimatorComponent spriteAnimator) =>
            {
                switch (bullet.Type)
                {
                    case BulletType.Player:
                        // Check for enemy collisions
                        foreach (PhysicsAreaCollision collision in area.Collisions)
                        {
                            if (collision.Entity.Has<EnemyComponent>())
                            {
                                Game.World.EnemySystem.DestroyEnemy(collision.Entity);
                                DestroyBullet(entity);
                            }
                        }

                        break;

                    case BulletType.Enemy:
                        // Check for player collisions
                        foreach (PhysicsAreaCollision collision in area.Collisions)
                        {
                            if (collision.Entity.Has<PlayerComponent>())
                            {
                                Game.World.PlayerSystem.DamagePlayer(collision.Entity, 1);
                                DestroyBullet(entity);
                            }
                        }

                        break;

                    default:
                        break;
                }
            });
        }
    }
}
