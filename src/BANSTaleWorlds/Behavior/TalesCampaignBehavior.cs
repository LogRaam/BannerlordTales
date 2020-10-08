// Code written by Gabriel Mailhot, 11/09/2020.

#region

using TalesPersistence;
using TalesPersistence.Events;
using TalesTaleWorlds.Menu;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.Engine.GauntletUI;

#endregion

namespace TalesTaleWorlds.Behavior
{
    internal class TalesCampaignBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, HourlyTickEventRaised);
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, DailyTickEventRaised);
            //CampaignEvents.OnChildConceivedEvent.AddNonSerializedListener(this, this.ChildConceivedEventRaised);
            //CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, this.HeroKilledEventRaised);
            //CampaignEvents.PrisonerTaken.AddNonSerializedListener(this, this.PrisonerTakenEventRaised);
            //CampaignEvents.PrisonerReleased.AddNonSerializedListener(this, this.PrisonerReleasedEventRaised);
            //CampaignEvents.CharacterBecameFugitive.AddNonSerializedListener(this, this.CharacterBecameFugitiveEventRaised);
            CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, GameMenuOpenedEventRaised);
            CampaignEvents.AfterGameMenuOpenedEvent.AddNonSerializedListener(this, AfterGameMenuOpenedEventRaised);
            //CampaignEvents.BeforeGameMenuOpenedEvent.AddNonSerializedListener(this, BeforeGameMenuOpenedEventEventRaised);
            //CampaignEvents.GameMenuOptionSelectedEvent.AddNonSerializedListener(this, this.GameMenuOptionSelectedEventRaised);
            //CampaignEvents.SettlementEntered.AddNonSerializedListener(this, this.SettlementEnteredRaised);
            //CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, this.OnSettlementLeftEventRaised);
            //CampaignEvents.BattleStarted.AddNonSerializedListener(this, this.BattleStartedRaised);
        }

        public override void SyncData(IDataStore dataStore)
        {
            // Do nothing, sorry for the missed segregation.
        }

        #region private

        private void AfterGameMenuOpenedEventRaised(MenuCallbackArgs menu)
        {
            var waitShowed = ShowWaitingMenu(menu);

            if (waitShowed) return;

            ShowEventMenu(menu);

            /*
            if (Game.Current.GameStateManager.ActiveState is MapState mapState)
            {
               //GameMenu.ActivateGameMenu("Default_KisstheBanner");
               mapState.MenuContext.SetBackgroundMeshName("encounter_looter");
            }
            */
        }


        private void DailyTickEventRaised()
        {
            //GameData.Instance.GameContext.ResetEventChanceBonus();  //TODO: Maybe I don<t need to reset the chance bonus here..
        }

        private void GameMenuOpenedEventRaised(MenuCallbackArgs obj)
        {
            GameData.Instance.GameContext.LastGameMenuOpened = obj.MenuContext.GameMenu.StringId;
        }


        private void HourlyTickEventRaised()
        {
            //TODO: Evaluate if it is preferable to use the HourlyTickEventRaised to trigger a menu.
        }

        private void SetBackgroundImage(MenuBroker m, string id)
        {
            var backGround = m.GetBackgroundFrom(id);
            //UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[34] = GameData.Instance.StoryContext.BackgroundImages.TextureList[backGround];
            UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[13] = GameData.Instance.StoryContext.BackgroundImages.TextureList[backGround];
            //UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[28] = GameData.Instance.StoryContext.BackgroundImages.TextureList[backGround];
            //UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[12] = GameData.Instance.StoryContext.BackgroundImages.TextureList[backGround];
        }

        private void ShowCustomWaitingMenu(MenuCallbackArgs menuCallback)
        {
            var m = new MenuBroker();
            new GameFunction().PauseGame();
            var waitingMenu = m.GetWaitingMenu();


            GameMenu.ActivateGameMenu(waitingMenu);
            SetBackgroundImage(m, waitingMenu);
            GameData.Instance.StoryContext.AddToPlayedActs(waitingMenu);
        }

        private void ShowEventMenu(MenuCallbackArgs menuCallbackArgs)
        {
            if (!GameData.Instance.GameContext.ReadyToShowNewEvent()) return;

            var act = new HourlyEvent().Execute();

            if (act == null) return;

            new MenuBroker().ShowMenuFor(menuCallbackArgs, act);
        }

        private bool ShowWaitingMenu(MenuCallbackArgs menuCallback)
        {
            if (menuCallback.MenuContext.GameMenu?.StringId != "menu_captivity_end_wilderness_escape" && menuCallback.MenuContext.GameMenu?.StringId != "menu_captivity_end_propose_ransom_wilderness") return false;

            ShowCustomWaitingMenu(menuCallback);

            return true;
        }

        #endregion
    }
}