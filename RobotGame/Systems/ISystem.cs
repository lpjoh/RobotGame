using Arch.Core;

namespace RobotGame.Systems
{
    public interface ISystem
    {
        void Update(World entities, float delta);
    }
}
