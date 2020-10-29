// Code written by Gabriel Mailhot, 11/09/2020.

#region

using _47_TalesMath;
using TalesEnums;
using TalesPersistence.Context;
using TalesRuntime.Menu;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;

#endregion

namespace TalesRuntime.Behavior
{
    internal class TalesCampaignBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, HourlyTickEventRaised);
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, DailyTickEventRaised);
            //CampaignEvents.OnChildConceivedEvent.AddNonSerializedListener(this, this.ChildConceivedEventRaised);
            //CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, this.HeroKilledEventRaised);
            CampaignEvents.PrisonerTaken.AddNonSerializedListener(this, PrisonerTakenEventRaised);
            //CampaignEvents.PrisonerReleased.AddNonSerializedListener(this, this.PrisonerReleasedEventRaised);
            //CampaignEvents.CharacterBecameFugitive.AddNonSerializedListener(this, this.CharacterBecameFugitiveEventRaised);
            CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, GameMenuOpenedEventRaised);
            CampaignEvents.AfterGameMenuOpenedEvent.AddNonSerializedListener(this, AfterGameMenuOpenedEventRaised);
            //CampaignEvents.BeforeGameMenuOpenedEvent.AddNonSerializedListener(this, BeforeGameMenuOpenedEventEventRaised);
            CampaignEvents.GameMenuOptionSelectedEvent.AddNonSerializedListener(this, GameMenuOptionSelectedEventRaised);
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
            GameFunction.Log("AfterGameMenuOpenedEventRaised(MenuCallbackArgs menu) menu => " + menu.MenuContext.GameMenu.StringId);

            var mb = new MenuBroker();

            if (GameData.Instance.StoryContext.GetStoryTypeFor(menu.MenuContext.GameMenu.StringId) == StoryType.PLAYER_SURRENDER)
            {
                GameFunction.Log("... call => ShowSurrenderMenu()");

                mb.ShowSurrenderMenu();

                return;
            }

            GameFunction.Log("... call => menu.MenuContext.Destroy()");
            menu.MenuContext.Destroy();

            GameFunction.Log("... call => ShowWaitingMenu(menu) menu => " + menu.MenuContext.StringId);
            mb.ShowWaitingMenu(menu);
        }


        private void DailyTickEventRaised()
        {
            GameFunction.Log("DailyTickEventRaised()");

            GameData.Instance.GameContext.ResetEventChanceBonus();
        }

        private void GameMenuOpenedEventRaised(MenuCallbackArgs obj)
        {
            GameFunction.Log("GameMenuOpenedEventRaised(MenuCallbackArgs obj) new menu => " + obj.MenuContext.GameMenu.StringId);

            GameData.Instance.GameContext.LastGameMenuOpened = obj.MenuContext.GameMenu.StringId;
        }

        private void GameMenuOptionSelectedEventRaised(GameMenuOption obj)
        {
            GameFunction.Log("GameMenuOptionSelectedEventRaised(GameMenuOption obj) obj => " + obj.Text);

            if (obj.OptionLeaveType == GameMenuOption.LeaveType.Surrender)
            {
                obj.OnConsequence = null;
            }
        }


        private void HourlyTickEventRaised()
        {
            GameFunction.Log("HourlyTickEventRaised()...");

            var mb = new MenuBroker();

            if (PlayerCaptivity.CaptivityStartTime.ElapsedHoursUntilNow < 1)
                if (mb.ShowSurrenderMenu())
                {
                    GameFunction.Log("... ShowSurrenderMenu returned true => return");

                    return;
                }


            if (mb.ShowActMenu())
            {
                GameFunction.Log("HourlyTickEventRaised() ShowActMenu returned true => return");

                return;
            }

            GameFunction.Log("... call => ExitToCaptiveWaitingMenu()");
            mb.ExitToCaptiveWaitingMenu();
        }

        private void PrisonerTakenEventRaised(PartyBase party, Hero hero)
        {
            if (!hero.IsHumanPlayerCharacter) return;

            GameFunction.Log("PrisonerTakenEventRaised(PartyBase party, Hero hero) hero => Player");

            //new MenuBroker().ShowSurrenderMenu();
        }

        #endregion
    }
}