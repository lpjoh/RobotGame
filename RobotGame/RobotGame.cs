using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RobotGame
{
    public class RobotGame : Game
    {
        public GraphicsDeviceManager Graphics;

        public GameWorld World = new();
        public Renderer Renderer = new();

        public RobotGame()
        {
            Graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            World.Initialize(this);
            Renderer.Initialize(this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Renderer.LoadContent(this);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Renderer.Update(this, delta);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Renderer.Draw(this);

            base.Draw(gameTime);
        }
    }
}