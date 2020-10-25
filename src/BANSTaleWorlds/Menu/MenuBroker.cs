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
        public void ExitToCaptiveWaitingMenu()
        {
            if (!GameData.Instance.GameContext.Player.IsPrisoner)
                return;

            UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[13] = GameData.Instance.StoryContext.BackgroundImages.TextureList["LogCaptivePrisoner"];
            GameMenu.ExitToLast();
        }

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

        public void GotoMenuFor(string triggerLink)
        {
            var act = GameData.Instance.StoryContext.FindSequence(triggerLink) ?? GameData.Instance.StoryContext.FindAct(triggerLink);

            if (act == null) throw new NullReferenceException("ERROR: Cannot find IAct: " + triggerLink);

            ShowMenuFor(act);
        }

        public void ShowActMenu()
        {
            if (!GameData.Instance.GameContext.ReadyToShowNewEvent()) return;

            var act = GameData.Instance.GameContext.RetrieveActToPlay() ?? GameData.Instance.GameContext.RetrieveAlreadyPlayedActToPlay();

            if (act != null) ShowMenuFor(act);
        }

        public void ShowWaitingMenu(MenuCallbackArgs menuCallback)
        {
            if (menuCallback.MenuContext.GameMenu?.StringId != "menu_captivity_end_wilderness_escape"
                && menuCallback.MenuContext.GameMenu?.StringId != "menu_captivity_end_propose_ransom_wilderness") return;

            ShowCustomWaitingMenu(menuCallback);
        }


        internal void CreateGameMenuFor(CampaignGameStarter gameStarter, IStory story)
        {
            CreateActGameMenuFor(gameStarter, story);
            CreateSequenceGameMenuFor(gameStarter, story);
        }

        #region private

        private void CreateActGameMenuFor(CampaignGameStarter gameStarter, IStory story)
        {
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
            var result = new List<IStory>();
            foreach (var story in GameData.Instance.StoryContext.Stories)
                if (story.Header.TypeOfStory == StoryType.WAITING)
                    result.Add(story);

            return result;
        }


        private void SetBackgroundImage(string imageName)
        {
            if (imageName == "None") return;

            //UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[34] = GameData.Instance.StoryContext.BackgroundImages.TextureList[backGround];
            UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[13] = GameData.Instance.StoryContext.BackgroundImages.TextureList[imageName];
            //UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[28] = GameData.Instance.StoryContext.BackgroundImages.TextureList[backGround];
            //UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[12] = GameData.Instance.StoryContext.BackgroundImages.TextureList[backGround];

            //if (Game.Current.GameStateManager.ActiveState is MapState mapState) mapState.MenuContext.SetBackgroundMeshName(imageName);
        }

        private void ShowCustomWaitingMenu(MenuCallbackArgs menuCallback)
        {
            ShowMenuFor(GetWaitingMenu());
        }

        private void ShowMenuFor(IAct act)
        {
            if (act == null) return;

            if (act.ParentStory.Header.TypeOfStory == StoryType.WAITING) GameMenu.ActivateGameMenu(act.Id);
            else GameMenu.SwitchToMenu(act.Id);

            if (!string.IsNullOrEmpty(act.Image) || act.Image.ToUpper() != "NONE") SetBackgroundImage(act.Image);

            if (act.GetType() != typeof(BaseSequence))
                if (GameData.Instance.StoryContext.PlayedActs.FirstOrDefault(n => n.Id == act.Id) == null)
                    GameData.Instance.StoryContext.PlayedActs.Add(act);
        }

        #endregion
    }
}