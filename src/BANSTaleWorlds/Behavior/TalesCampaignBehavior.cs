// Code written by Gabriel Mailhot, 11/09/2020.

#region

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
            //CampaignEvents.HeroPrisonerReleased.AddNonSerializedListener(this, HeroPrisonerReleasedRaised);
            CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, HeroPrisonerTakenRaised);
            CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, HourlyTickEventRaised);
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, DailyTickEventRaised);
            CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, GameMenuOpenedEventRaised);
            CampaignEvents.AfterGameMenuOpenedEvent.AddNonSerializedListener(this, AfterGameMenuOpenedEventRaised);
            CampaignEvents.BeforeGameMenuOpenedEvent.AddNonSerializedListener(this, BeforeGameMenuOpenedEventEventRaised);
            CampaignEvents.GameMenuOptionSelectedEvent.AddNonSerializedListener(this, GameMenuOptionSelectedEventRaised);

            //CampaignEvents.AfterDailyTickEvent.AddNonSerializedListener(this, AfterDailyTickEventRaised);
            //CampaignEvents.CharacterDefeated.AddNonSerializedListener(this, CharacterDefeatedRaised);
            //CampaignEvents.ConversationEnded.AddNonSerializedListener(this, ConversationEndedRaised);
            //CampaignEvents.MapEventStarted.AddNonSerializedListener(this, MapEventStartedRaised);
            //CampaignEvents.MapEventEnded.AddNonSerializedListener(this, MapEventEndedRaised);
            //CampaignEvents.OnChildConceivedEvent.AddNonSerializedListener(this, OnChildConceivedEventRaised);
            //CampaignEvents.OnBeforeSaveEvent.AddNonSerializedListener(this, OnBeforeSaveEventRaised);
            //CampaignEvents.OnGivenBirthEvent.AddNonSerializedListener(this, OnGivenBirthEventRaised);
            //CampaignEvents.OnPrisonerReleasedEvent.AddNonSerializedListener(this, OnPrisonerReleasedEventRaised);
            //CampaignEvents.OnPrisonerTakenEvent.AddNonSerializedListener(this, OnPrisonerTakenEventRaised);
            //CampaignEvents.OnPrisonerSoldEvent.AddNonSerializedListener(this, OnPrisonerSoldEventRaised);
            //CampaignEvents.OnPlayerBattleEndEvent.AddNonSerializedListener(this, OnPlayerBattleEndEventRaised);
            //CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, this.HeroKilledEventRaised);
            //CampaignEvents.CharacterBecameFugitive.AddNonSerializedListener(this, this.CharacterBecameFugitiveEventRaised);
            //CampaignEvents.SettlementEntered.AddNonSerializedListener(this, this.SettlementEnteredRaised);
            //CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, this.OnSettlementLeftEventRaised);
            //CampaignEvents.BattleStarted.AddNonSerializedListener(this, this.BattleStartedRaised);
        }

        public override void SyncData(IDataStore dataStore)
        {
            // Do nothing, sorry for the missed segregation.
        }

        #region private

        private static void IfVanillaMenuThenDestroyMenuContext(MenuCallbackArgs menu)
        {
            if (GameData.Instance.StoryContext.MenuExist(menu.MenuContext.GameMenu.StringId)) return;

            menu.MenuContext.Destroy();
        }

        private static void SetupEncounterBackground(MenuCallbackArgs obj)
        {
            if (obj.MenuContext.CurrentBackgroundMeshName == "encounter_looter") new MenuBroker().SetBackgroundImage("LogEncounterLooter2");
            if (obj.MenuContext.CurrentBackgroundMeshName == "encounter_forest_bandit") new MenuBroker().SetBackgroundImage("LogEncounterLooter");
            if (obj.MenuContext.CurrentBackgroundMeshName == "encounter_desert_bandits") new MenuBroker().SetBackgroundImage("LogAserai2");
            if (obj.MenuContext.CurrentBackgroundMeshName == "encounter_mountain_bandit") new MenuBroker().SetBackgroundImage("LogBattleEnemies");
            if (obj.MenuContext.CurrentBackgroundMeshName == "encounter_shore_bandit") new MenuBroker().SetBackgroundImage("LogBattleEnemies2");

            if (obj.MenuContext.CurrentBackgroundMeshName == "encounter_aserai") new MenuBroker().SetBackgroundImage("LogEncounterAserai");
            if (obj.MenuContext.CurrentBackgroundMeshName == "encounter_empire") new MenuBroker().SetBackgroundImage("LogEncounterEmpire");
            if (obj.MenuContext.CurrentBackgroundMeshName == "encounter_sturgia") new MenuBroker().SetBackgroundImage("LogBattleNordman");
            if (obj.MenuContext.CurrentBackgroundMeshName == "encounter_vlandia") new MenuBroker().SetBackgroundImage("LogEncounterVlandia");
            if (obj.MenuContext.CurrentBackgroundMeshName == "encounter_khuzait") new MenuBroker().SetBackgroundImage("LogKhuzait3");
            if (obj.MenuContext.CurrentBackgroundMeshName == "encounter_battania") new MenuBroker().SetBackgroundImage("LogEncounterBattania2");

            //TODO: complete other encounters.
        }


        private void AfterGameMenuOpenedEventRaised(MenuCallbackArgs menu)
        {
            IfVanillaMenuThenDestroyMenuContext(menu);

            if (IsVanillaEscapingMenu(menu)) ReplaceVanillaEscapeByCustomWaiting();
        }

        private void BeforeGameMenuOpenedEventEventRaised(MenuCallbackArgs obj)
        {
            SetupEncounterBackground(obj);
        }


        private void DailyTickEventRaised()
        {
            GameData.Instance.GameContext.ResetEventChanceBonus();
        }

        private void GameMenuOpenedEventRaised(MenuCallbackArgs obj) //3
        {
            GameData.Instance.GameContext.LastGameMenuOpened = obj.MenuContext.GameMenu.StringId;
        }

        private void GameMenuOptionSelectedEventRaised(GameMenuOption obj) //2
        {
            if (obj.OptionLeaveType != GameMenuOption.LeaveType.Surrender) return;

            new MenuBroker().ShowSurrenderMenu();
        }


        private void HeroPrisonerTakenRaised(PartyBase party, Hero hero) //1
        {
            if (!hero.IsHumanPlayerCharacter) return;

            new MenuBroker().SetBackgroundImage("LogCaptivePrisoner");
        }


        private void HourlyTickEventRaised()
        {
            if (!GameData.Instance.GameContext.Player.IsPrisoner) return;
            if (PlayerCaptivity.CaptivityStartTime.ElapsedHoursUntilNow < 1) return;
            if (!GameData.Instance.GameContext.ReadyToShowNewEvent()) return;

            new MenuBroker().ShowActMenu();
        }

        private bool IsVanillaEscapingMenu(MenuCallbackArgs menu)
        {
            if (menu.MenuContext.GameMenu == null) return false;

            var menuId = menu.MenuContext.GameMenu.StringId;

            if (menuId == "menu_captivity_end_wilderness_escape") return true;
            if (menuId == "menu_captivity_end_propose_ransom_wilderness") return true;

            return false;
        }


        private void ReplaceVanillaEscapeByCustomWaiting()
        {
            var mb = new MenuBroker();
            var act = mb.GetWaitingMenu();

            new MenuBroker().PlayAct(act);

            if (!string.IsNullOrEmpty(act.Image) || act.Image.ToUpper() != "NONE") mb.SetBackgroundImage(act.Image);
        }

        #endregion
    }
}