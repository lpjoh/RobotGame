using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RobotGame
{
    public class RobotGame : Game
    {
        public GraphicsDeviceManager Graphics;

        public Renderer Renderer = new();

        public RobotGame()
        {
            Graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Renderer.Initialize(Graphics, GraphicsDevice);
        }

        protected override void LoadContent()
        {
            Renderer.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Renderer.Draw();

            base.Draw(gameTime);
        }
    }
}