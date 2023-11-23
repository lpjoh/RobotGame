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
            PlayerRightTexture,

            PlayerBulletTexture;

        public SpriteSystem SpriteSystem = new();
        public SpriteAnimatorSystem SpriteAnimatorSystem = new();

        public RobotGame Game;

        public Renderer(RobotGame game)
        {
            Game = game;
        }

        public void DrawWorld()
        {
            World entities = Game.World.Entities;

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            SpriteSystem.Draw(entities, this);

            SpriteBatch.End();
        }

        public void Initialize()
        {
            GraphicsDeviceManager graphics = Game.Graphics;
            GraphicsDevice graphicsDevice = Game.GraphicsDevice;

            graphics.PreferredBackBufferWidth = 240 * 3;
            graphics.PreferredBackBufferHeight = 180 * 3;

            graphics.ApplyChanges();

            RenderTarget = new RenderTarget2D(graphicsDevice, 240, 180);

            RenderTargetRect = new Rectangle(0, 0,
                graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight);

            SpriteBatch = new SpriteBatch(graphicsDevice);
        }

        public void LoadContent()
        {
            ContentManager content = Game.Content;

            PlayerUpTexture = content.Load<Texture2D>("Textures/player_up");
            PlayerDownTexture = content.Load<Texture2D>("Textures/player_down");
            PlayerLeftTexture = content.Load<Texture2D>("Textures/player_left");
            PlayerRightTexture = content.Load<Texture2D>("Textures/player_right");

            PlayerBulletTexture = content.Load<Texture2D>("Textures/player_bullet");
        }

        public void Update(float delta)
        {
            World entities = Game.World.Entities;

            SpriteAnimatorSystem.Update(entities, delta);
        }

        public void Draw()
        {
            GraphicsDevice graphicsDevice = Game.GraphicsDevice;

            graphicsDevice.SetRenderTarget(RenderTarget);
            graphicsDevice.Clear(Color.Black);

            DrawWorld();

            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            SpriteBatch.Draw(RenderTarget, RenderTargetRect, Color.White);
            SpriteBatch.End();
        }
    }
}
