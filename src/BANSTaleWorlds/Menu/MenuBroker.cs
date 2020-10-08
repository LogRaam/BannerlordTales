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

        public string GetWaitingMenu()
        {
            var ev = PickEventFromStories();

            if (ev != "NoActFound") return ev;

            var n = new List<string>
            {
                "menu_captivity_end_wilderness_escape", "menu_captivity_end_propose_ransom_wilderness"
            };

            TalesRandom.InitRandomNumber(Guid.NewGuid().GetHashCode());

            var i = TalesRandom.GenerateRandomNumber(1);

            return n[i];
        }

        internal void CreateGameMenuFor(CampaignGameStarter gameStarter, IStory story)
        {
            if (string.IsNullOrEmpty(story.Id)) story.Id = Guid.NewGuid().ToString();

            foreach (var act in story.Acts)
            {
                var menuId = story.Header.Name.Replace(" ", "") + "_" + act.Name.Replace(" ", "");
                var m = new MenuCallBackDelegate(act);

                gameStarter.AddGameMenu(menuId, act.Intro, m.ActMenuSetup, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.none, "BannerlordTales");

                foreach (var choice in act.Choices)
                {
                    var o = new OptionCallBackDelegate(choice);
                    gameStarter.AddGameMenuOption(menuId, choice.Id, choice.Text, o.OnConditionDelegate, o.OnConsequenceDelegate, m.IsLeave(choice.Id), m.Index(choice.Id), m.IsRepeatable(choice.Id));
                }
            }
        }

        internal void ShowMenuFor(MenuCallbackArgs menuCallbackArgs, IAct act)
        {
            if (act == null) return;

            new GameFunction().PauseGame();

            GameMenu.ActivateGameMenu(act.Id);
            if (!string.IsNullOrEmpty(act.Image)) menuCallbackArgs.MenuContext.SetBackgroundMeshName(act.Image);
        }

        #region private

        private string PickEventFromStories()
        {
            var stories = RetrieveWaitingStories();

            if (stories.Count == 0) return "NoActFound";

            var s = new StoryBroker();
            var acts = s.RetrieveUnplayedActsFrom(stories);
            if (acts.Count == 0)
            {
                acts = GameData.Instance.StoryContext.PlayedActs.ToAct();
            }

            if (acts.Count == 0) return "NoActFound";

            TalesRandom.InitRandomNumber(Guid.NewGuid().GetHashCode());

            var i = TalesRandom.GenerateRandomNumber(acts.Count);
            var selectedAct = new Act(acts[i]);

            if (selectedAct.IsQualifiedRightNow()) return selectedAct.ParentStory + "_" + selectedAct.Name;

            return "NoActFound";
        }

        private List<IStory> RetrieveWaitingStories()
        {
            var Result = new List<IStory>();
            foreach (var story in GameData.Instance.StoryContext.Stories)
                if (story.Header.TypeOfStory == StoryType.WAITING)
                    Result.Add(story);

            return Result;
        }

        #endregion
    }
}