using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace RobotGame
{
    public class AudioController
    {
        public Song Music;

        public RobotGame Game;

        public AudioController(RobotGame game)
        {
            Game = game;
        }

        public void Initialize()
        {
            ContentManager content = Game.Content;

            Music = content.Load<Song>("Sounds/music");
        }
    }
}
