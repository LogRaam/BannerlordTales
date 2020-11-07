// Code written by Gabriel Mailhot, 26/10/2020.

namespace TalesPersistence.Context
{
    public class GameData
    {
        private static GameData _instance;


        public static GameData Instance
        {
            get => _instance ?? (_instance = new GameData());
            set => _instance = value;
        }

        public GameContext GameContext { get; set; } = new GameContext();

        public StoryContext StoryContext { get; set; } = new StoryContext();
    }
}