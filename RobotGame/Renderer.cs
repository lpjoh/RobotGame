using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RobotGame
{
    public class Renderer
    {
        public GraphicsDeviceManager Graphics;
        public GraphicsDevice GraphicsDevice;

        public RenderTarget2D RenderTarget;
        public Rectangle RenderTargetRect;

        public SpriteBatch SpriteBatch;

        public Texture2D
            PlayerUpTexture,
            PlayerDownTexture,
            PlayerLeftTexture,
            PlayerRightTexture;


        public void Initialize(GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice)
        {
            Graphics = graphics;
            GraphicsDevice = graphicsDevice;

            Graphics.PreferredBackBufferWidth = 320 * 2;
            Graphics.PreferredBackBufferHeight = 240 * 2;

            Graphics.ApplyChanges();

            RenderTarget = new RenderTarget2D(GraphicsDevice, 320, 240);

            RenderTargetRect = new Rectangle(0, 0,
                Graphics.PreferredBackBufferWidth,
                Graphics.PreferredBackBufferHeight);

            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public void LoadContent(ContentManager content)
        {

            PlayerUpTexture = content.Load<Texture2D>("Textures/player_up");
            PlayerDownTexture = content.Load<Texture2D>("Textures/player_down");
            PlayerLeftTexture = content.Load<Texture2D>("Textures/player_left");
            PlayerRightTexture = content.Load<Texture2D>("Textures/player_right");
        }

        public void Draw()
        {
            GraphicsDevice.SetRenderTarget(RenderTarget);

            GraphicsDevice.Clear(Color.Black);

            GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin();
            SpriteBatch.Draw(RenderTarget, RenderTargetRect, Color.White);
            SpriteBatch.End();
        }
    }
}
