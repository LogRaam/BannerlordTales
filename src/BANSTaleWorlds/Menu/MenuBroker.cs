// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System;
using System.Collections.Generic;
using _47_TalesMath;
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
        public void ExitToCaptiveWaitingMenu()
        {
            GameFunction.Log("ExitToCaptiveWaitingMenu()...");

            //if (Campaign.Current.CurrentMenuContext != null && Campaign.Current.CurrentMenuContext.GameMenu.IsWaitMenu)
            if (!GameData.Instance.GameContext.Player.IsPrisoner) return;

            SetBackgroundImage("LogCaptivePrisoner");

            GameFunction.Log("... call => GameMenu.ExitToLast()");
            GameMenu.ExitToLast();

            GameFunction.Log("... call => UnPauseGame()");
            new GameFunction().UnPauseGame();
        }

        public IAct GetWaitingMenu()
        {
            var ev = PickEventFromStories();

            GameFunction.Log("GetWaitingMenu() return => " + ev.Id);

            return ev;
        }

        public void GotoMenuFor(string triggerLink)
        {
            GameFunction.Log("GotoMenuFor(string triggerLink) => triggerLink = " + triggerLink);


            var act = GameData.Instance.StoryContext.FindSequence(triggerLink) ?? GameData.Instance.StoryContext.FindAct(triggerLink);

            if (act == null) throw new NullReferenceException("ERROR: Cannot find IAct: " + triggerLink);

            ShowMenuFor(act);
        }


        public void SetBackgroundImage(string imageName)
        {
            GameFunction.Log("SetBackgroundImage(string imageName) => imageName = " + imageName);

            if (imageName == "None") return;

            for (var i = 0; i < UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets.Count; i++)
            {
                UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[i] = GameData.Instance.StoryContext.BackgroundImages.TextureList[imageName];
            }
        }

        public bool ShowActMenu()
        {
            GameFunction.Log("ShowActMenu()...");

            if (GameData.Instance.GameContext.Player.IsPrisoner)
            {
                GameFunction.Log("... Player is not prisoner, return => false");

                return false;
            }

            if (!GameData.Instance.GameContext.ReadyToShowNewEvent())
            {
                GameFunction.Log("... ReadyToShowNewEvent return => false");

                return false;
            }

            var act = GameData.Instance.GameContext.RetrieveActToPlay() ?? GameData.Instance.GameContext.RetrieveAlreadyPlayedActToPlay();

            if (act == null)
            {
                GameFunction.Log("... act is null return => false");

                return false;
            }

            GameFunction.Log("... call => ShowMenuFor(act), act = " + act.Id);
            ShowMenuFor(act);

            GameFunction.Log("... return => true");

            return true;
        }

        public bool ShowSurrenderMenu()
        {
            GameFunction.Log("ShowSurrenderMenu()");

            var act = GameData.Instance.GameContext.RetrieveActToPlay(StoryType.PLAYER_SURRENDER);

            if (act == null) return false;

            GameFunction.Log("... call => ShowMenuFor(act) act => " + act.Id);

            ShowMenuFor(act); //GameMenu.ActivateGameMenu(act.Id);

            return true;
        }

        public void ShowWaitingMenu(MenuCallbackArgs menuCallback)
        {
            GameFunction.Log("ShowWaitingMenu(MenuCallbackArgs menuCallback) menuCallback => " + menuCallback.MenuContext.GameMenu.StringId);

            if (menuCallback.MenuContext.GameMenu?.StringId != "menu_captivity_end_wilderness_escape"
                && menuCallback.MenuContext.GameMenu?.StringId != "menu_captivity_end_propose_ransom_wilderness") return;

            GameFunction.Log("... call => ShowCustomWaitingMenu(menuCallback)");
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
            GameFunction.Log("PickEventFromStories() ...");

            var stories = RetrieveWaitingStories();

            if (stories.Count == 0) return null;

            var acts = new StoryBroker().RetrieveNonPlayedActsFrom(stories);

            if (acts.Count == 0) acts = GameData.Instance.StoryContext.PlayedActs.ToAct();

            if (acts.Count == 0) return null;

            var selectedAct = new Act(acts[TalesRandom.GenerateRandomNumber(acts.Count)]);

            GameFunction.Log("... selectedAct => " + selectedAct.Id);

            return selectedAct.IsQualifiedRightNow()
                ? selectedAct
                : null;
        }

        private void RegisterPlayedAct(IAct act)
        {
            GameFunction.Log("RegisterPlayedAct(IAct act) act => " + act.Id);

            if (act.GetType() == typeof(BaseSequence)) return;

            GameData.Instance.StoryContext.AddToPlayedActs(act);
        }


        private List<IStory> RetrieveWaitingStories()
        {
            var result = new List<IStory>();
            foreach (var story in GameData.Instance.StoryContext.Stories)
                if (story.Header.TypeOfStory == StoryType.WAITING)
                    result.Add(story);

            GameFunction.Log("RetrieveWaitingStories() return count => " + result.Count);

            return result;
        }

        private void ShowCustomWaitingMenu(MenuCallbackArgs menuCallback)
        {
            GameFunction.Log("ShowCustomWaitingMenu(MenuCallbackArgs menuCallback) menuCallback => " + menuCallback.MenuContext.GameMenu.StringId);

            ShowMenuFor(GetWaitingMenu());
        }

        private void ShowMenuFor(IAct act)
        {
            GameFunction.Log("ShowMenuFor(IAct act) act => " + act.Id);


            GameFunction.Log("... call => ExitToLast()");
            GameMenu.ExitToLast(); //NOTE: Should use this to enable options with Surrender menu

            //if (!string.IsNullOrEmpty(act.Image) || act.Image.ToUpper() == "NONE") SetBackgroundImage(act.Image);

            if (act.ParentStory.Header.TypeOfStory == StoryType.WAITING)
            {
                GameFunction.Log("... call => ActivateGameMenu(act.Id) id => " + act.Id);
                GameMenu.ActivateGameMenu(act.Id);
            }

            if (act.ParentStory.Header.TypeOfStory == StoryType.PLAYER_SURRENDER)
            {
                GameFunction.Log("... call => ActivateGameMenu(act.Id) id => " + act.Id);
                GameMenu.ActivateGameMenu(act.Id);
            }
            else
            {
                GameFunction.Log("... call => SwitchToMenu(act.Id) id => " + act.Id);
                GameMenu.SwitchToMenu(act.Id);
            }

            //if (!string.IsNullOrEmpty(act.Image) || act.Image.ToUpper() != "NONE") SetBackgroundImage(act.Image);

            GameFunction.Log("... call => RegisterPlayedAct(act) act => " + act.Id);
            RegisterPlayedAct(act);
        }

        #endregion
    }
}