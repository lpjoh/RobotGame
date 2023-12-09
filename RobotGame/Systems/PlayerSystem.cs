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
        public ref PhysicsAreaComponent Area;
        public ref HealthComponent Health;
        public ref SpriteComponent Sprite;
        public ref SpriteAnimatorComponent SpriteAnimator;
    }

    public class PlayerSystem : ISystem
    {
        public Vector2 BodySize = new(8.0f, 8.0f);
        public Vector2 AreaSize = new(12.0f, 12.0f);

        public Vector2 SpriteOffset = new(-4.0f, -7.0f);
        public Vector2 AreaOffset;

        public const float Acceleration = 400.0f;
        public const float MaxSpeed = 60.0f;
        public const float ShootTime = 0.2f;
        public const int MaxHealth = 4;

        public SpriteAnimation IdleAnimation, WalkAnimation;

        public RobotGame Game;
        public QueryDescription Query;

        public PlayerSystem(RobotGame game)
        {
            Game = game;

            Query = new QueryDescription().WithAll<
                PlayerComponent,
                PositionComponent,
                PhysicsBodyComponent,
                PhysicsAreaComponent,
                HealthComponent,
                SpriteComponent,
                SpriteAnimatorComponent>();

            // Center area relative to body
            AreaOffset = (BodySize - AreaSize) / 2.0f;
        }

        // Spawns a new player
        public Entity CreatePlayer(World entities)
        {
            Entity entity = entities.Create(
                new PlayerComponent { FacingDirection = new Vector2(0.0f, 1.0f) },
                new PositionComponent { Position = new Vector2(0.0f, 0.0f) },
                new PhysicsBodyComponent { Size = BodySize },
                new PhysicsAreaComponent { Size = AreaSize, Offset = AreaOffset },
                new HealthComponent() { Health = MaxHealth - 1, MaxHealth = MaxHealth },
                new SpriteComponent { Texture = Game.Renderer.PlayerDownTexture, Offset = SpriteOffset },
                new SpriteAnimatorComponent());

            // Start with idle animation
            SpriteAnimatorSystem.PlayAnimation(ref entity.Get<SpriteAnimatorComponent>(), IdleAnimation);

            return entity;
        }

        public Vector2 GetFacingDirection(Vector2 direction)
        {
            if (direction.Y == 0.0f)
            {
                // Return left or right
                return new Vector2(direction.X, 0.0f);
            }

            // Return up or down
            return new Vector2(0.0f, direction.Y);
        }

        public Texture2D GetFacingTexture(Vector2 direction)
        {
            Renderer renderer = Game.Renderer;

            if (direction.Y == 0.0f)
            {
                // Return left or right
                if (direction.X < 0.0f)
                {
                    return renderer.PlayerLeftTexture;
                }

                return renderer.PlayerRightTexture;
            }

            // Return up or down
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
                // Decelerate by slowing towards zero
                float velocityChange = Acceleration * delta;

                if (velocity < 0.0f)
                {
                    return MathF.Min(velocity + velocityChange, 0.0f);
                }

                return MathF.Max(velocity - velocityChange, 0.0f);
            }
            else
            {
                // Accelerate with maximum speed
                float newVelocity = velocity + Acceleration * moveDirection * delta;
                return Math.Clamp(newVelocity, -MaxSpeed, MaxSpeed);
            }
        }

        public void Move(ref PlayerData playerData, Vector2 moveDirection, float delta)
        {
            // Move across both axes
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
            // Check shoot timer is finished
            if (playerData.Player.ShootTimer <= 0)
            {
                // Check if shooting in a direction
                if (shootDirection != Vector2.Zero)
                {
                    // Summon bullet centered on the player's hitbox
                    BulletSystem bulletSystem = Game.World.BulletSystem;

                    Vector2 bulletPosition =
                        playerData.Position.Position +
                        (playerData.Body.Size - bulletSystem.BulletSize) * 0.5f;

                    bulletSystem.CreateBullet(
                        entities, bulletPosition, shootDirection);

                    // Reset shoot timer
                    playerData.Player.ShootTimer = ShootTime;
                }
            }
            else
            {
                // Advance shoot timer
                playerData.Player.ShootTimer -= delta;
            }
        }

        public void Animate(ref PlayerData playerData, Vector2 moveDirection)
        {
            if (moveDirection == Vector2.Zero)
            {
                // Show idle animation
                SpriteAnimatorSystem.PlayAnimation(ref playerData.SpriteAnimator, IdleAnimation);
            }
            else
            {
                // Check if movement is facing away from current direction
                if (Vector2.Dot(playerData.Player.FacingDirection, moveDirection) <= 0.0)
                {
                    // Change sprite direction
                    playerData.Player.FacingDirection = GetFacingDirection(moveDirection);
                    playerData.Sprite.Texture = GetFacingTexture(playerData.Player.FacingDirection);
                }

                // Play walking animation
                SpriteAnimatorSystem.PlayAnimation(ref playerData.SpriteAnimator, WalkAnimation);
            }
        }

        public void Initialize()
        {
            // Create animations
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
                ref PhysicsAreaComponent area,
                ref HealthComponent health,
                ref SpriteComponent sprite,
                ref SpriteAnimatorComponent spriteAnimator) =>
            {
                // Pack component references into struct
                PlayerData playerData = new()
                {
                    Player = ref player,
                    Position = ref position,
                    Body = ref body,
                    Health = ref health,
                    Sprite = ref sprite,
                    SpriteAnimator = ref spriteAnimator
                };

                // Obtain input variables
                Input input = Game.Input;

                Vector2 moveDirection = input.GetAxis(Keys.W, Keys.S, Keys.A, Keys.D);
                Vector2 shootDirection = input.GetAxis(Keys.Up, Keys.Down, Keys.Left, Keys.Right);

                // Perform actions
                Move(ref playerData, moveDirection, delta);
                Shoot(ref playerData, entities, shootDirection, delta);
                Animate(ref playerData, moveDirection);
            });
        }
    }
}
