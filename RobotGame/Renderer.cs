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

        public SpriteBatch SpriteBatch;

        public Texture2D
            PlayerUpTexture,
            PlayerDownTexture,
            PlayerLeftTexture,
            PlayerRightTexture;

        public SpriteSystem SpriteSystem = new();
        public SpriteAnimationSystem SpriteAnimationSystem = new();

        public static void InitializePlayer(RobotGame game)
        {
            Texture2D texture = game.Renderer.PlayerDownTexture;
            List<Rectangle> frames = SpriteSystem.GetFrames(texture, 3);

            game.World.Player.Add(
                new SpriteComponent { Texture = texture, Frame = frames[0] },
                new SpriteAnimationComponent { Frames = frames, FramesPerSecond = 10.0f });
        }

        public void DrawWorld(RobotGame game)
        {
            World entities = game.World.Entities;

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            SpriteSystem.Draw(entities, this);

            SpriteBatch.End();
        }

        public void Initialize(RobotGame game)
        {
            GraphicsDeviceManager graphics = game.Graphics;
            GraphicsDevice graphicsDevice = game.GraphicsDevice;

            graphics.PreferredBackBufferWidth = 320 * 2;
            graphics.PreferredBackBufferHeight = 240 * 2;

            graphics.ApplyChanges();

            RenderTarget = new RenderTarget2D(graphicsDevice, 320, 240);

            RenderTargetRect = new Rectangle(0, 0,
                graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight);

            SpriteBatch = new SpriteBatch(graphicsDevice);
        }

        public void LoadContent(RobotGame game)
        {
            ContentManager content = game.Content;

            PlayerUpTexture = content.Load<Texture2D>("Textures/player_up");
            PlayerDownTexture = content.Load<Texture2D>("Textures/player_down");
            PlayerLeftTexture = content.Load<Texture2D>("Textures/player_left");
            PlayerRightTexture = content.Load<Texture2D>("Textures/player_right");

            InitializePlayer(game);
        }

        public void Update(RobotGame game, float delta)
        {
            World entities = game.World.Entities;

            SpriteAnimationSystem.Update(entities, delta);
        }

        public void Draw(RobotGame game)
        {
            GraphicsDevice graphicsDevice = game.GraphicsDevice;

            graphicsDevice.SetRenderTarget(RenderTarget);
            graphicsDevice.Clear(Color.Black);

            DrawWorld(game);

            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            SpriteBatch.Draw(RenderTarget, RenderTargetRect, Color.White);
            SpriteBatch.End();
        }
    }
}
