using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RobotGame.Components;
using RobotGame.Systems;
using System.Collections.Generic;

namespace RobotGame
{
    public class Renderer
    {
        public RenderTarget2D RenderTarget;
        public Rectangle RenderTargetRect;

        public GraphicsDeviceManager Graphics;
        public GraphicsDevice GraphicsDevice;

        public SpriteBatch SpriteBatch;

        public Texture2D
            BackgroundTexture,

            PlayerUpTexture,
            PlayerDownTexture,
            PlayerLeftTexture,
            PlayerRightTexture,

            PlayerBulletTexture,
            EnemyBulletTexture,

            BatteryTexture,
            GearTexture,

            HealthBackTexture,
            HealthFrontTexture;

        public SpriteSystem SpriteSystem = new();
        public SpriteAnimatorSystem SpriteAnimatorSystem = new();
        public PhysicsBodyRendererSystem PhysicsBodyRendererSystem;

        public RobotGame Game;

        public Renderer(RobotGame game)
        {
            Game = game;
            PhysicsBodyRendererSystem = new PhysicsBodyRendererSystem(this);
        }

        // Draws the game world
        public void DrawWorld()
        {
            World entities = Game.World.Entities;

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            SpriteBatch.Draw(BackgroundTexture, Vector2.Zero, Color.White);

            SpriteSystem.Draw(entities, this);
            //PhysicsBodyRendererSystem.Draw(entities, this);

            Game.World.HealthBar.Draw(this);

            SpriteBatch.End();
        }

        public void Initialize()
        {
            // Get graphics objects from game
            Graphics = Game.Graphics;
            GraphicsDevice = Game.GraphicsDevice;

            // Set window resolution
            Graphics.PreferredBackBufferWidth = 240 * 3;
            Graphics.PreferredBackBufferHeight = 180 * 3;

            Graphics.ApplyChanges();

            // Create viewport
            RenderTarget = new RenderTarget2D(GraphicsDevice, 240, 180);

            // Span viewport rect across entire screen
            RenderTargetRect = new Rectangle(0, 0,
                Graphics.PreferredBackBufferWidth,
                Graphics.PreferredBackBufferHeight);

            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // Start debug renderer
            PhysicsBodyRendererSystem.Initialize();
        }

        public void LoadContent()
        {
            ContentManager content = Game.Content;

            // Load textures
            BackgroundTexture = content.Load<Texture2D>("Textures/background");

            PlayerUpTexture = content.Load<Texture2D>("Textures/player_up");
            PlayerDownTexture = content.Load<Texture2D>("Textures/player_down");
            PlayerLeftTexture = content.Load<Texture2D>("Textures/player_left");
            PlayerRightTexture = content.Load<Texture2D>("Textures/player_right");

            PlayerBulletTexture = content.Load<Texture2D>("Textures/player_bullet");
            EnemyBulletTexture = content.Load<Texture2D>("Textures/enemy_bullet");

            BatteryTexture = content.Load<Texture2D>("Textures/battery");
            GearTexture = content.Load<Texture2D>("Textures/gear");

            HealthBackTexture = content.Load<Texture2D>("Textures/health_back");
            HealthFrontTexture = content.Load<Texture2D>("Textures/health_front");
        }

        public void Update(float delta)
        {
            World entities = Game.World.Entities;

            SpriteAnimatorSystem.Update(entities, delta);
        }

        public void Draw()
        {
            // Draw to viewport
            GraphicsDevice.SetRenderTarget(RenderTarget);
            GraphicsDevice.Clear(Color.Black);

            DrawWorld();

            // Draw to main window
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            // Put viewport to screen
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            SpriteBatch.Draw(RenderTarget, RenderTargetRect, Color.White);
            SpriteBatch.End();
        }
    }
}
