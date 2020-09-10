// Code written by Gabriel Mailhot, 05/08/2020.

namespace TalesTaleWorlds.Behavior
{
   #region

   using BannerlordTales.Events;
   using TalesContract;
   using TalesEntities;
   using TalesTaleWorlds.Menu;
   using TaleWorlds.CampaignSystem;
   using TaleWorlds.CampaignSystem.Actions;
   using TaleWorlds.CampaignSystem.GameMenus;
   using Hero = TaleWorlds.CampaignSystem.Hero;
   using IFaction = TaleWorlds.CampaignSystem.IFaction;

   #endregion

   internal class TalesCampaignBehavior : CampaignBehaviorBase
   {
      public override void RegisterEvents()
      {
         CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, this.HourlyTickEventRaised);
         CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, this.DailyTickEventRaised);
         //CampaignEvents.OnChildConceivedEvent.AddNonSerializedListener(this, this.ChildConceivedEventRaised);
         //CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, this.HeroKilledEventRaised);
         //CampaignEvents.PrisonerTaken.AddNonSerializedListener(this, this.PrisonerTakenEventRaised);
         //CampaignEvents.PrisonerReleased.AddNonSerializedListener(this, this.PrisonerReleasedEventRaised);
         //CampaignEvents.CharacterBecameFugitive.AddNonSerializedListener(this, this.CharacterBecameFugitiveEventRaised);
         //CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, this.GameMenuOpenedEventRaised);
         //CampaignEvents.AfterGameMenuOpenedEvent.AddNonSerializedListener(this, this.AfterGameMenuOpenedEventEventRaised);
         //CampaignEvents.BeforeGameMenuOpenedEvent.AddNonSerializedListener(this, this.BeforeGameMenuOpenedEventEventRaised);
         //CampaignEvents.GameMenuOptionSelectedEvent.AddNonSerializedListener(this, this.GameMenuOptionSelectedEventRaised);
         //CampaignEvents.SettlementEntered.AddNonSerializedListener(this, this.SettlementEnteredRaised);
         //CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, this.OnSettlementLeftEventRaised);
         //CampaignEvents.BattleStarted.AddNonSerializedListener(this, this.BattleStartedRaised);
      }

      public override void SyncData(IDataStore dataStore)
      {
         // Do nothing
      }

      #region private

  

      private void DailyTickEventRaised()
      {
         var a = PartyBase.MainParty;
         var b = Hero.MainHero;
         var c = Hero.HeroWoundedHealthLevel;
         var d = Campaign.Current.MainParty;
         var e = Campaign.Current.IsNight;
         var f = Campaign.Current.CurrentMenuContext;
         
         IEventRequest storyAction = new DailyEvent().Execute();

         new MenuBroker().ShowMenuFor(storyAction);
      }

     

      private void HourlyTickEventRaised()
      {
         new HourlyEvent().Execute();
      }

     

      #endregion
   }
}