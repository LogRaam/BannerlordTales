// Code written by Gabriel Mailhot, 11/09/2020.

#region

using TalesPersistence;
using TalesTaleWorlds.Menu;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;

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

        /// <summary>
        ///     This event is the starting point to show game menus.  It is triggered by the game's designed event probability.
        /// </summary>
        /// <param name="menu">MenuCallbackArgs given by the game engine</param>
        private void AfterGameMenuOpenedEventRaised(MenuCallbackArgs menu)
        {
            ShowWaitingMenu(menu);
        }


        private void DailyTickEventRaised()
        {
            GameData.Instance.GameContext.ResetEventChanceBonus();
        }

        private void GameMenuOpenedEventRaised(MenuCallbackArgs obj)
        {
            GameData.Instance.GameContext.LastGameMenuOpened = obj.MenuContext.GameMenu.StringId;
        }


        private void HourlyTickEventRaised()
        {
            ShowActMenu();

            new MenuBroker().ShowCaptiveWaiting();
            new GameFunction().UnPauseGame();
        }


        private void ShowActMenu()
        {
            if (!GameData.Instance.GameContext.ReadyToShowNewEvent()) return;

            var act = GameData.Instance.GameContext.RetrieveActToPlay();

            if (act == null) act = GameData.Instance.GameContext.RetrieveAlreadyPlayedActToPlay();

            new MenuBroker().ShowMenuFor(act);
        }


        private void ShowCustomWaitingMenu(MenuCallbackArgs menuCallback)
        {
            var m = new MenuBroker();
            m.ShowMenuFor(m.GetWaitingMenu());
        }


        /// <summary>
        ///     Will show waiting menu if callback result is either: menu_captivity_end_wilderness_escape or
        ///     menu_captivity_end_propose_ransom_wilderness.
        /// </summary>
        /// <param name="menuCallback"></param>
        /// <returns></returns>
        private void ShowWaitingMenu(MenuCallbackArgs menuCallback)
        {
            if (menuCallback.MenuContext.GameMenu?.StringId != "menu_captivity_end_wilderness_escape"
                && menuCallback.MenuContext.GameMenu?.StringId != "menu_captivity_end_propose_ransom_wilderness") return;

            ShowCustomWaitingMenu(menuCallback);
        }

        #endregion
    }
}