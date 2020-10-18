// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System;
using System.Collections.Generic;
using TalesContract;
using TalesDAL;
using TalesEnums;
using TalesPersistence;
using TalesPersistence.Stories;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.Engine.GauntletUI;

#endregion

namespace TalesTaleWorlds.Menu
{
    #region

    #endregion

    public class MenuBroker
    {
        public string GetBackgroundFrom(string id)
        {
            var s = id.Split('_')[1];
            foreach (var story in GameData.Instance.StoryContext.Stories)
            {
                foreach (var sequence in story.Sequences)
                    if (sequence.Name == s)
                        return sequence.Image;

                foreach (var act in story.Acts)
                    if (act.Name == s)
                        return act.Image;
            }

            return "LogCaptivePrisoner";
        }

        public IAct GetWaitingMenu()
        {
            var ev = PickEventFromStories();

            return ev;
        }

        internal void CreateGameMenuFor(CampaignGameStarter gameStarter, IStory story)
        {
            if (string.IsNullOrEmpty(story.Id)) story.Id = Guid.NewGuid().ToString();

            CreateActGameMenuFor(gameStarter, story);
            CreateSequenceGameMenuFor(gameStarter, story);
        }

        internal void ShowMenuFor(IAct act)
        {
            if (act == null) return;

            new GameFunction().PauseGame();

            GameMenu.ActivateGameMenu(act.Id);
            if (!string.IsNullOrEmpty(act.Image)) SetBackgroundImage(act.Image);
            GameData.Instance.StoryContext.PlayedActs.Add(act);
        }

        #region private

        private void CreateActGameMenuFor(CampaignGameStarter gameStarter, IStory story)
        {
            foreach (var act in story.Acts)
            {
                var menuId = act.Id;
                var m = new MenuCallBackDelegate(act);

                gameStarter.AddGameMenu(menuId, act.Intro, m.ActMenuSetup, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.none, "BannerlordTales");

                foreach (var choice in act.Choices)
                {
                    var o = new OptionCallBackDelegate(choice);
                    gameStarter.AddGameMenuOption(menuId, choice.Id, choice.Text, o.OnConditionDelegate, o.OnConsequenceDelegate, m.IsLeave(choice.Id), m.Index(choice.Id), m.IsRepeatable(choice.Id));
                }
            }
        }

        private void CreateSequenceGameMenuFor(CampaignGameStarter gameStarter, IStory story)
        {
            foreach (var sequence in story.Sequences)
            {
                var menuId = story.Header.Name.Replace(" ", "") + "_" + sequence.Name.Replace(" ", "");
                var m = new MenuCallBackDelegate(sequence);

                gameStarter.AddGameMenu(menuId, sequence.Intro, m.ActMenuSetup, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.none, "BannerlordTales");

                foreach (var choice in sequence.Choices)
                {
                    var o = new OptionCallBackDelegate(choice);
                    gameStarter.AddGameMenuOption(menuId, choice.Id, choice.Text, o.OnConditionDelegate, o.OnConsequenceDelegate, m.IsLeave(choice.Id), m.Index(choice.Id), m.IsRepeatable(choice.Id));
                }
            }
        }

        private IAct PickEventFromStories()
        {
            var stories = RetrieveWaitingStories();

            if (stories.Count == 0) return null;

            var s = new StoryBroker();
            var acts = s.RetrieveNonPlayedActsFrom(stories);
            if (acts.Count == 0) acts = GameData.Instance.StoryContext.PlayedActs.ToAct();

            if (acts.Count == 0) return null;

            var i = TalesRandom.GenerateRandomNumber(acts.Count);
            var selectedAct = new Act(acts[i]);

            if (selectedAct.IsQualifiedRightNow()) return selectedAct;

            return null;
        }

        private List<IStory> RetrieveWaitingStories()
        {
            var Result = new List<IStory>();
            foreach (var story in GameData.Instance.StoryContext.Stories)
                if (story.Header.TypeOfStory == StoryType.WAITING)
                    Result.Add(story);

            return Result;
        }


        private void SetBackgroundImage(string imageName)
        {
            if (imageName == "None") return;

            //UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[34] = GameData.Instance.StoryContext.BackgroundImages.TextureList[backGround];
            UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[13] = GameData.Instance.StoryContext.BackgroundImages.TextureList[imageName];
            //UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[28] = GameData.Instance.StoryContext.BackgroundImages.TextureList[backGround];
            //UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[12] = GameData.Instance.StoryContext.BackgroundImages.TextureList[backGround];
        }

        #endregion
    }
}