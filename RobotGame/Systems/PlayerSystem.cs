using Arch.Core;
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

        public QueryDescription Query;

        public SpriteAnimation IdleAnimation, WalkAnimation;

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
                new PhysicsBodyComponent(),
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
            if (direction.Y == 0.0f)
            {
                if (direction.X < 0.0f)
                {
                    return Game.Renderer.PlayerLeftTexture;
                }

                return Game.Renderer.PlayerRightTexture;
            }

            if (direction.Y < 0.0f)
            {
                return Game.Renderer.PlayerUpTexture;
            }

            return Game.Renderer.PlayerDownTexture;
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

                Vector2 moveDirection = new(
                    (input.IsKeyPressed(Keys.D) ? 1.0f : 0.0f) -
                    (input.IsKeyPressed(Keys.A) ? 1.0f : 0.0f),
                    (input.IsKeyPressed(Keys.S) ? 1.0f : 0.0f) -
                    (input.IsKeyPressed(Keys.W) ? 1.0f : 0.0f));

                body.Velocity.X = ApplyMovement(body.Velocity.X, moveDirection.X, delta);
                body.Velocity.Y = ApplyMovement(body.Velocity.Y, moveDirection.Y, delta);

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
            });
        }
    }
}
