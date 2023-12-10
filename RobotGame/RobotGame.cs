using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RobotGame
{
    public class RobotGame : Game
    {
        public GraphicsDeviceManager Graphics;

        public GameWorld World;
        public Renderer Renderer;

        public Input Input = new();

        public RobotGame()
        {
            Graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            World = new GameWorld(this);
            Renderer = new Renderer(this);
        }

        protected override void Initialize()
        {
            Renderer.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Renderer.LoadContent();

            World.Initialize();

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            World.Update(delta);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Renderer.Draw();

            base.Draw(gameTime);
        }
    }
}