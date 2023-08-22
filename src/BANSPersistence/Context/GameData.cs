// Code written by Gabriel Mailhot, 02/12/2023.  Updated by  Gabriel Mailhot on 02/19/2023.

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

        public void Reset()
        {
            _instance = new GameData();
        }
    }
}