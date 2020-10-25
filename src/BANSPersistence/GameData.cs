// Code written by Gabriel Mailhot, 11/09/2020.

namespace TalesPersistence
{
    public class GameData
    {
        private static GameData instance;

        public bool IsDebug = true;

        public static GameData Instance
        {
            get => instance ?? (instance = new GameData());
            set => instance = value;
        }

        public GameContext GameContext { get; set; } = new GameContext();

        public StoryContext StoryContext { get; set; } = new StoryContext();
    }
}