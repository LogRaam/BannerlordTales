// unset

#region

using _47_TalesMath;
using System;
using System.Collections.Generic;
using System.Linq;
using TalesBase.Stories;
using TalesContract;
using TalesDAL;
using TalesEnums;
using TalesPersistence.Context;
using TalesPersistence.Stories;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.Engine.GauntletUI;

#endregion

namespace TalesRuntime.Menu
{
    #region

    #endregion

    public class MenuBroker
    {
        public void ExitToLastAndUnpause()
        {
            if (!GameData.Instance.GameContext.Heroes.Player.IsPrisoner) return;

            SetBackgroundImage("LogCaptivePrisoner");

            GameMenu.ExitToLast();

            new GameFunction().UnPauseGame();
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

            PlayAct(act);
        }

        public void PlayAct(IAct act)
        {
            GameMenu.ExitToLast();
            GameMenu.ActivateGameMenu(act.Id);
            RegisterPlayedAct(act);
        }


        public void SetBackgroundImage(string imageName)
        {
            if (imageName == "None") return;

            GameData.Instance.GameContext.OriginalBackgroundSpriteSheets = UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets;

            for (var i = 0; i < UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets.Count; i++)
                if (UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[i].Width == 445)
                    UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[i] = GameData.Instance.StoryContext.BackgroundImages.TextureList[imageName];
        }

        public void ShowActMenu()
        {
            var stories = GameData.Instance.GameContext.Acts.GetNewStories().Where(n => n.Header.Name != "Test"
                                                                                        && n.Header.TypeOfStory != StoryType.WAITING
                                                                                        && n.Header.TypeOfStory != StoryType.PLAYER_SURRENDER
                                                                                        && n.Header.TypeOfStory != StoryType.TEST
                                                                                        && n.Header.TypeOfStory != StoryType.UNKNOWN).ToList();

            var act = GameData.Instance.GameContext.Acts.ChooseQualifiedActFrom(stories);

            if (act == null)
            {
                stories = GameData.Instance.GameContext.Acts.GetAlreadyPlayedStories().Where(n => n.Header.Name != "Test"
                                                                                                  && n.Header.TypeOfStory != StoryType.WAITING
                                                                                                  && n.Header.TypeOfStory != StoryType.PLAYER_SURRENDER
                                                                                                  && n.Header.TypeOfStory != StoryType.TEST
                                                                                                  && n.Header.TypeOfStory != StoryType.UNKNOWN).ToList();
                act = GameData.Instance.GameContext.Acts.ChooseQualifiedActFrom(stories);
            }

            if (act == null) return;

            PlayAct(act);
        }

        public void ShowSurrenderMenu()
        {
            var act = GameData.Instance.GameContext.Acts.RetrieveActToPlay(StoryType.PLAYER_SURRENDER);

            if (act == null) return;

            PlayAct(act);
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

            var acts = new StoryBroker().RetrieveNonPlayedActsFrom(stories);

            if (acts.Count == 0) acts = GameData.Instance.StoryContext.PlayedActs.ToAct();

            if (acts.Count == 0) return null;

            var selectedAct = new Act(acts[TalesRandom.GenerateRandomNumber(acts.Count)]);

            return selectedAct.IsQualifiedRightNow()
                ? selectedAct
                : null;
        }

        private void RegisterPlayedAct(IAct act)
        {
            if (act.GetType() == typeof(BaseSequence)) return;

            GameData.Instance.StoryContext.AddToPlayedActs(act);
        }


        private List<IStory> RetrieveWaitingStories()
        {
            var result = new List<IStory>();
            foreach (var story in GameData.Instance.StoryContext.Stories)
                if (story.Header.TypeOfStory == StoryType.WAITING)
                    result.Add(story);

            return result;
        }

        #endregion
    }
}