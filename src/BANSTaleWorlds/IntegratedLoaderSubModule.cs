// Code written by Gabriel Mailhot, 12/02/2023.

#region

using System.IO;
using TalesBase;
using TalesDAL;
using TalesPersistence.Context;
using TalesRuntime.Behavior;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using Path = System.IO.Path;

#endregion

namespace TalesRuntime
{
    public class IntegratedLoaderSubModule : MBSubModuleBase
    {
        //public override void BeginGameStart(Game game) { }

        public override bool DoLoading(Game game)
        {
            // TODO:May be used to load saved data
            return base.DoLoading(game);
        }

        //public override void OnCampaignStart(Game game, object starterObject) { }

        //public override void OnGameEnd(Game game) { }

        //public override void OnGameInitializationFinished(Game game) { }

        //public override void OnGameLoaded(Game game, object initializerObject) { }

        //public override void OnMissionBehaviourInitialize(Mission mission) { }

        //public override void OnMultiplayerGameStart(Game game, object starterObject) { }

        //public override void OnNewGameCreated(Game game, object initializerObject) { }

        //protected override void OnApplicationTick(float dt) { }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            InformationManager.DisplayMessage(new InformationMessage(new Welcome().WelcomeMessage, Colors.Yellow));
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            if (game.GameType is Campaign)
            {
                LoadStoryAssets();
                AddBehaviorsTo(gameStarterObject);
            }

            base.OnGameStart(game, gameStarterObject);
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            GameData.Instance.StoryContext.ModuleFolder = new StoryLoader().GetModuleDirectoryInfo();
        }

        #region private

        private static void AddBehaviorsTo(IGameStarter gameStarterObject)
        {
            var campaignStarter = (CampaignGameStarter)gameStarterObject;
            campaignStarter.AddBehavior(new TalesCampaignBehavior());
            new StoryBroker().AddStoriesToGame(campaignStarter);
        }

        private static void LoadBodyArmorsIntoRuntime()
        {
            GameData.Instance.GameContext.Inventory.BodyArmors = new BodyArmorImporter().ImportBodyArmorsFrom(GameData.Instance.GameContext.Inventory.BodyArmorsFile);
        }

        private static void LoadCustomStoriesIntoRuntime()
        {
            GameData.Instance.StoryContext.CustomStoriesFolder = new DirectoryInfo(GameData.Instance.StoryContext.ModuleFolder.FullName + "\\CustomStories");

            GameData.Instance.StoryContext.Stories = GameData.Instance.StoryContext.ImportStoriesFromDisk();
        }

        private static void LoadStoryAssets()
        {
            LoadCustomStoriesIntoRuntime();
            LoadTexturesIntoRuntime();
            LoadBodyArmorsIntoRuntime();
        }

        private static void LoadTexturesIntoRuntime()
        {
            foreach (var image in GameData.Instance.StoryContext.StoryImagesFolder.GetFiles("*.png"))
            {
                var texture = Texture.LoadTextureFromPath(image.Name, image.DirectoryName);
                texture.PreloadTexture(false); //NOTE: param "blocking" is new.

                var texture2D = new TaleWorlds.TwoDimension.Texture(new EngineTexture(texture));
                var key = Path.GetFileNameWithoutExtension(image.Name);

                if (GameData.Instance.StoryContext.BackgroundImages.TextureList.ContainsKey(key)) return;

                GameData.Instance.StoryContext.BackgroundImages.TextureList.Add(key, texture2D);
            }
        }

        #endregion

        //protected override void OnSubModuleUnloaded() { }
    }
}