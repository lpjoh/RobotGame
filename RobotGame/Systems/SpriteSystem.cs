using Arch.Core;
using Microsoft.Xna.Framework;
using RobotGame.Components;

namespace RobotGame.Systems
{
    public class SpriteSystem : ISystem
    {
        public QueryDescription Query;

        public SpriteSystem()
        {
            Query = new QueryDescription().WithAll<SpriteComponent, PositionComponent>();
        }

        public void Draw(World entities, Renderer renderer)
        {
            entities.Query(in Query, (
                Entity entity,
                ref SpriteComponent sprite,
                ref PositionComponent position) =>
            {
                // Draw sprite
                renderer.SpriteBatch.Draw(
                    sprite.Texture, position.Position, sprite.Frame, Color.White);
            });
        }

        public void Initialize()
        {

        }

        public void Update(World entities, float delta)
        {

        }
    }
}
