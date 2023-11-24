using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RobotGame.Components;
using System;
using System.Collections.Generic;

namespace RobotGame.Systems
{
    public class PlayerSystem : ISystem
    {
        public const float Acceleration = 400.0f;
        public const float MaxSpeed = 60.0f;

        public SpriteAnimation IdleAnimation, WalkAnimation;

        public QueryDescription Query;

        public RobotGame Game;

        public PlayerSystem(RobotGame game)
        {
            Game = game;

            Query = new QueryDescription().WithAll<
                PlayerComponent,
                PositionComponent,
                PhysicsBodyComponent,
                SpriteComponent,
                SpriteAnimatorComponent>();
        }

        public Entity CreatePlayer(World entities)
        {
            Entity entity = entities.Create(
                new PlayerComponent { FacingDirection = new Vector2(0, 1) },
                new PositionComponent { Position = new Vector2(0, 0) },
                new PhysicsBodyComponent { Size = new Vector2(16, 16) },
                new SpriteComponent { Texture = Game.Renderer.PlayerDownTexture },
                new SpriteAnimatorComponent());

            SpriteAnimatorSystem.PlayAnimation(entity, IdleAnimation);

            return entity;
        }

        public Vector2 GetFacingDirection(Vector2 direction)
        {
            if (direction.Y == 0.0f)
            {
                return new Vector2(direction.X, 0.0f);
            }

            return new Vector2(0.0f, direction.Y);
        }

        public Texture2D GetFacingTexture(Vector2 direction)
        {
            Renderer renderer = Game.Renderer;

            if (direction.Y == 0.0f)
            {
                if (direction.X < 0.0f)
                {
                    return renderer.PlayerLeftTexture;
                }

                return renderer.PlayerRightTexture;
            }

            if (direction.Y < 0.0f)
            {
                return renderer.PlayerUpTexture;
            }

            return renderer.PlayerDownTexture;
        }

        public float ApplyMovement(float velocity, float moveDirection, float delta)
        {
            if (moveDirection == 0.0f)
            {
                float velocityChange = Acceleration * delta;

                if (velocity < 0.0f)
                {
                    return MathF.Min(velocity + velocityChange, 0.0f);
                }

                return MathF.Max(velocity - velocityChange, 0.0f);
            }
            else
            {
                float newVelocity = velocity + Acceleration * moveDirection * delta;
                return Math.Clamp(newVelocity, -MaxSpeed, MaxSpeed);
            }
        }

        public void Move(PhysicsBodyComponent body, Vector2 moveDirection, float delta)
        {
            body.Velocity.X = ApplyMovement(body.Velocity.X, moveDirection.X, delta);
            body.Velocity.Y = ApplyMovement(body.Velocity.Y, moveDirection.Y, delta);
        }

        public void Shoot(
            PlayerComponent player,
            PositionComponent position,
            PhysicsBodyComponent body,
            World entities,
            Vector2 shootDirection,
            float delta)
        {
            if (player.ShootTimer <= 0)
            {
                if (shootDirection != Vector2.Zero)
                {
                    BulletSystem bulletSystem = Game.World.BulletSystem;

                    Vector2 bulletPosition =
                        position.Position + (body.Size - bulletSystem.BulletSize) * 0.5f;

                    bulletSystem.CreateBullet(
                        entities, bulletPosition, shootDirection);

                    player.ShootTimer = 0.2f;
                }
            }
            else
            {
                player.ShootTimer -= delta;
            }
        }

        public void Animate(
            Entity entity,
            PlayerComponent player,
            SpriteComponent sprite,
            Vector2 moveDirection)
        {
            if (moveDirection == Vector2.Zero)
            {
                SpriteAnimatorSystem.PlayAnimation(entity, IdleAnimation);
            }
            else
            {
                if (Vector2.Dot(player.FacingDirection, moveDirection) <= 0.0)
                {
                    player.FacingDirection = GetFacingDirection(moveDirection);
                    sprite.Texture = GetFacingTexture(player.FacingDirection);
                }

                SpriteAnimatorSystem.PlayAnimation(entity, WalkAnimation);
            }
        }

        public void Initialize()
        {
            Texture2D texture = Game.Renderer.PlayerDownTexture;

            List<Rectangle>
                spriteFrames = SpriteAnimation.GetFrames(texture, 3),
                idleFrames = new() { spriteFrames[0] },
                walkFrames = new() { spriteFrames[0], spriteFrames[1], spriteFrames[2] };

            IdleAnimation = new SpriteAnimation(idleFrames, 0.0f);
            WalkAnimation = new SpriteAnimation(walkFrames, 10.0f);
        }

        public void Update(World entities, float delta)
        {
            entities.Query(in Query, (
                Entity entity,
                ref PlayerComponent player,
                ref PositionComponent position,
                ref PhysicsBodyComponent body,
                ref SpriteComponent sprite,
                ref SpriteAnimatorComponent animator) =>
            {
                Input input = Game.Input;

                Vector2 moveDirection = input.GetAxis(Keys.W, Keys.S, Keys.A, Keys.D);
                Vector2 shootDirection = input.GetAxis(Keys.Up, Keys.Down, Keys.Left, Keys.Right);

                Move(body, moveDirection, delta);
                Shoot(player, position, body, entities, shootDirection, delta);
                Animate(entity, player, sprite, moveDirection);
            });
        }
    }
}
