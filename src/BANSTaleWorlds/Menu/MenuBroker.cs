// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System;
using System.Collections.Generic;
using System.Linq;
using TalesContract;
using TalesDAL;
using TalesEntities.Stories;
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
            GameFunction.Log("GetWaitingMenu()");

            var ev = PickEventFromStories();

            return ev;
        }

        public void ShowCaptiveWaiting()
        {
            if (!GameData.Instance.GameContext.Player.IsPrisoner)
                return;

            GameFunction.Log("ShowCaptiveWaiting()");
            UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[13] = GameData.Instance.StoryContext.BackgroundImages.TextureList["LogCaptivePrisoner"];
            GameMenu.ExitToLast();
            //GameData.Instance.GameContext.UnPauseGame();
        }

        public void ShowMenuFor(string triggerLink)
        {
            GameFunction.Log("ShowMenuFor(string triggerLink)");
            var s = GameData.Instance.StoryContext.FindSequence(triggerLink) ?? GameData.Instance.StoryContext.FindAct(triggerLink);

            if (s == null) throw new NullReferenceException("ERROR: Cannot find IAct: " + triggerLink);

            ShowMenuFor(s);
        }

        public void ShowMenuFor(IAct act)
        {
            GameFunction.Log("ShowMenuFor(IAct act)");

            if (act == null) return;

            //GameData.Instance.GameContext.PauseGame();


            if (act.ParentStory.Header.TypeOfStory == StoryType.WAITING) GameMenu.ActivateGameMenu(act.Id);
            else GameMenu.SwitchToMenu(act.Id);

            if (!string.IsNullOrEmpty(act.Image) || act.Image.ToUpper() != "NONE") SetBackgroundImage(act.Image);

            if (act.GetType() != typeof(BaseSequence))
                if (GameData.Instance.StoryContext.PlayedActs.FirstOrDefault(n => n.Id == act.Id) == null)
                    GameData.Instance.StoryContext.PlayedActs.Add(act);
        }


        internal void CreateGameMenuFor(CampaignGameStarter gameStarter, IStory story)
        {
            GameFunction.Log("CreateGameMenuFor(CampaignGameStarter gameStarter, IStory story)");
            CreateActGameMenuFor(gameStarter, story);
            CreateSequenceGameMenuFor(gameStarter, story);
        }

        #region private

        private void CreateActGameMenuFor(CampaignGameStarter gameStarter, IStory story)
        {
            GameFunction.Log("CreateActGameMenuFor(CampaignGameStarter gameStarter, IStory story)");
            foreach (var act in story.Acts)
            {
                var m = new MenuCallBackDelegate(act);

                gameStarter.AddGameMenu(act.Id, act.Intro, m.ActMenuSetup, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.none, "BannerlordTales");

                foreach (var choice in act.Choices)
                {
                    var o = new OptionCallBackDelegate(choice);
                    gameStarter.AddGameMenuOption(act.Id, choice.Id, choice.Text, o.OnConditionDelegate, o.OnConsequenceDelegate, m.IsLeave(choice.Id), m.Index(choice.Id), m.IsRepeatable(choice.Id));
                }
            }
        }

        private void CreateSequenceGameMenuFor(CampaignGameStarter gameStarter, IStory story)
        {
            GameFunction.Log("CreateSequenceGameMenuFor(CampaignGameStarter gameStarter, IStory story)");
            foreach (var sequence in story.Sequences)
            {
                var m = new MenuCallBackDelegate(sequence);

                gameStarter.AddGameMenu(sequence.Id, sequence.Intro, m.ActMenuSetup, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.none, "BannerlordTales");

                foreach (var choice in sequence.Choices)
                {
                    var o = new OptionCallBackDelegate(choice);
                    gameStarter.AddGameMenuOption(sequence.Id, choice.Id, choice.Text, o.OnConditionDelegate, o.OnConsequenceDelegate, m.IsLeave(choice.Id), m.Index(choice.Id), m.IsRepeatable(choice.Id));
                }
            }
        }

        private IAct PickEventFromStories()
        {
            GameFunction.Log("PickEventFromStories()");
            var stories = RetrieveWaitingStories();

            if (stories.Count == 0) return null;

            var s = new StoryBroker();
            var acts = s.RetrieveNonPlayedActsFrom(stories);
            if (acts.Count == 0) acts = GameData.Instance.StoryContext.PlayedActs.ToAct();

            if (acts.Count == 0) return null;

            var i = TalesRandom.GenerateRandomNumber(acts.Count);
            var selectedAct = new Act(acts[i]);

            return selectedAct.IsQualifiedRightNow()
                ? selectedAct
                : null;
        }

        private List<IStory> RetrieveWaitingStories()
        {
            GameFunction.Log("RetrieveWaitingStories()");
            var result = new List<IStory>();
            foreach (var story in GameData.Instance.StoryContext.Stories)
                if (story.Header.TypeOfStory == StoryType.WAITING)
                    result.Add(story);

            return result;
        }


        private void SetBackgroundImage(string imageName)
        {
            GameFunction.Log("SetBackgroundImage(string imageName)");

            if (imageName == "None") return;

            //UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[34] = GameData.Instance.StoryContext.BackgroundImages.TextureList[backGround];
            UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[13] = GameData.Instance.StoryContext.BackgroundImages.TextureList[imageName];
            //UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[28] = GameData.Instance.StoryContext.BackgroundImages.TextureList[backGround];
            //UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[12] = GameData.Instance.StoryContext.BackgroundImages.TextureList[backGround];

            //if (Game.Current.GameStateManager.ActiveState is MapState mapState) mapState.MenuContext.SetBackgroundMeshName(imageName);
        }

        #endregion
    }
}