﻿using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RobotGame.Components;
using System;

namespace RobotGame
{
    public class HealthBar
    {
        public Vector2 Position = new(2.0f, 2.0f);
        public Vector2 FrontOffset = new(2.0f, 7.0f);

        public RobotGame Game;

        public HealthBar(RobotGame game)
        {
            Game = game;
        }

        public void Draw(Renderer renderer)
        {
            Texture2D backTexture = renderer.HealthBackTexture;
            Texture2D frontTexture = renderer.HealthFrontTexture;

            // Draw back
            renderer.SpriteBatch.Draw(backTexture, Position, Color.White);

            // Get ratio between player's health and max health
            HealthComponent health = Game.World.Player.Get<HealthComponent>();
            float healthRatio = health.Value / (float)health.MaxValue;

            // Scale rect by ratio, with ceiling to fill entire space
            Vector2 frontSize = new(frontTexture.Width, MathF.Ceiling(frontTexture.Height * healthRatio));

            // Move rect to bottom of bar
            Vector2 frontSrcPos = new(0, frontTexture.Height - frontSize.Y);
            Vector2 frontDstPos = Position + FrontOffset + frontSrcPos;

            // Create front rects
            Rectangle frontDstRect = new(
                (int)frontDstPos.X, (int)frontDstPos.Y,
                (int)frontSize.X, (int)frontSize.Y);

            Rectangle frontSrcRect = new(
                (int)frontSrcPos.X, (int)frontSrcPos.Y,
                (int)frontSize.X, (int)frontSize.Y);

            renderer.SpriteBatch.Draw(frontTexture, frontDstRect, frontSrcRect, Color.White);
        }
    }
}
