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
    public ref struct PlayerData
    {
        public ref PlayerComponent Player;
        public ref PositionComponent Position;
        public ref PhysicsBodyComponent Body;
        public ref SpriteComponent Sprite;
        public ref SpriteAnimatorComponent SpriteAnimator;
    }

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

            SpriteAnimatorSystem.PlayAnimation(ref entity.Get<SpriteAnimatorComponent>(), IdleAnimation);

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

        public void Move(ref PlayerData playerData, Vector2 moveDirection, float delta)
        {
            playerData.Body.Velocity.X =
                ApplyMovement(playerData.Body.Velocity.X, moveDirection.X, delta);

            playerData.Body.Velocity.Y =
                ApplyMovement(playerData.Body.Velocity.Y, moveDirection.Y, delta);
        }

        public void Shoot(
            ref PlayerData playerData,
            World entities,
            Vector2 shootDirection,
            float delta)
        {
            if (playerData.Player.ShootTimer <= 0)
            {
                if (shootDirection != Vector2.Zero)
                {
                    BulletSystem bulletSystem = Game.World.BulletSystem;

                    Vector2 bulletPosition =
                        playerData.Position.Position +
                        (playerData.Body.Size - bulletSystem.BulletSize) * 0.5f;

                    bulletSystem.CreateBullet(
                        entities, bulletPosition, shootDirection);

                    playerData.Player.ShootTimer = 0.2f;
                }
            }
            else
            {
                playerData.Player.ShootTimer -= delta;
            }
        }

        public void Animate(ref PlayerData playerData, Vector2 moveDirection)
        {
            if (moveDirection == Vector2.Zero)
            {
                SpriteAnimatorSystem.PlayAnimation(ref playerData.SpriteAnimator, IdleAnimation);
            }
            else
            {
                if (Vector2.Dot(playerData.Player.FacingDirection, moveDirection) <= 0.0)
                {
                    playerData.Player.FacingDirection = GetFacingDirection(moveDirection);
                    playerData.Sprite.Texture = GetFacingTexture(playerData.Player.FacingDirection);
                }

                SpriteAnimatorSystem.PlayAnimation(ref playerData.SpriteAnimator, WalkAnimation);
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
                ref Entity entity,
                ref PlayerComponent player,
                ref PositionComponent position,
                ref PhysicsBodyComponent body,
                ref SpriteComponent sprite,
                ref SpriteAnimatorComponent spriteAnimator) =>
            {
                PlayerData playerData = new()
                {
                    Player = ref player,
                    Position = ref position,
                    Body = ref body,
                    Sprite = ref sprite,
                    SpriteAnimator = ref spriteAnimator
                };

                Input input = Game.Input;

                Vector2 moveDirection = input.GetAxis(Keys.W, Keys.S, Keys.A, Keys.D);
                Vector2 shootDirection = input.GetAxis(Keys.Up, Keys.Down, Keys.Left, Keys.Right);

                Move(ref playerData, moveDirection, delta);
                Shoot(ref playerData, entities, shootDirection, delta);
                Animate(ref playerData, moveDirection);
            });
        }
    }
}
