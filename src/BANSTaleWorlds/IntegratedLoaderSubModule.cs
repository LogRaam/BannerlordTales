// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System.IO;
using TalesEntities;
using TalesPersistence;
using TalesTaleWorlds.Behavior;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using Path = System.IO.Path;

#endregion

namespace TalesTaleWorlds
{
    #region

    #endregion

    public class IntegratedLoaderSubModule : MBSubModuleBase
    {
        public override void BeginGameStart(Game game)
        {
        }

        public override bool DoLoading(Game game)
        {
            // TODO: Clear listeners for prisoner escape behaviors
            return base.DoLoading(game);
        }

        public override void OnCampaignStart(Game game, object starterObject)
        {
        }

        public override void OnGameEnd(Game game)
        {
            // TODO: Unpatch harmony
        }

        public override void OnGameInitializationFinished(Game game)
        {
        }

        public override void OnGameLoaded(Game game, object initializerObject)
        {
        }

        public override void OnMissionBehaviourInitialize(Mission mission)
        {
        }

        public override void OnMultiplayerGameStart(Game game, object starterObject)
        {
        }

        public override void OnNewGameCreated(Game game, object initializerObject)
        {
        }

        protected override void OnApplicationTick(float dt)
        {
            // BUG: OnApplicationTick() happens when loading screen...it may be preferable to not process stages during loading.

            // TODO: Process Captive Stages
            // TODO: Process Animations
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            // TODO: Init Settings for MCM
            // TODO: Apply Harmony Patches
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

            GameData.Instance.StoryContext.ModuleFolder = new DirectoryInfo(Path.GetDirectoryName(ModuleInfo.GetPath("LogRaamBannerlordTales")) ?? string.Empty);

            // TODO: Retrieve mod version.  Alert user if wrong version used.
            // TODO: Init Harmony
            // TODO: Validate all module loaded successful

            // TODO: Load the notification Sprites
        }

        protected override void OnSubModuleUnloaded()
        {
        }

        #region private

        private static void AddBehaviorsTo(IGameStarter gameStarterObject)
        {
            var campaignStarter = (CampaignGameStarter)gameStarterObject;
            campaignStarter.AddBehavior(new TalesCampaignBehavior());
            new StoryBroker().AddStoriesToGame(campaignStarter);
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
        }

        private static void LoadTexturesIntoRuntime()
        {
            GameData.Instance.StoryContext.StoryImagesFolder = new DirectoryInfo(GameData.Instance.StoryContext.ModuleFolder.FullName + "\\StoryImages");

            foreach (var image in GameData.Instance.StoryContext.StoryImagesFolder.GetFiles("*.png"))
            {
                var texture = Texture.LoadTextureFromPath(image.Name, image.DirectoryName);
                texture.PreloadTexture();

                var texture2D = new TaleWorlds.TwoDimension.Texture(new EngineTexture(texture));

                GameData.Instance.StoryContext.BackgroundImages.TextureList.Add(Path.GetFileNameWithoutExtension(image.Name), texture2D);
            }
        }

        #endregion
    }
}