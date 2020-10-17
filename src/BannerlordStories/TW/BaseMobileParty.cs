// Code written by Gabriel Mailhot, 15/09/2020.

#region

using System.Collections.Generic;
using TalesContract;
using TalesEnums;
using TaleWorlds.CampaignSystem;
using IFaction = TalesContract.IFaction;

#endregion

namespace TalesEntities.TW
{
    public class BaseMobileParty : IMobileParty
    {
        public BaseMobileParty(MobileParty mobileParty)
        {
            if (mobileParty == null) return;

            Name = mobileParty.Name.ToString();
        }

        public BaseMobileParty()
        {
        }


        public IClan ActualClan { get; set; }
        public float Aggressiveness { get; set; }
        public IList<IMobileParty> All { get; set; }
        public IList<IMobileParty> AllCaravans { get; set; }
        public string ArmyName { get; set; }
        public bool AtCampMode { get; set; }
        public IList<IMobileParty> AttachedParties { get; set; }
        public IMobileParty AttachedTo { get; set; }
        public ISettlement BesiegedISettlement { get; set; }
        public IMobileParty ConversationParty { get; set; }
        public int Count { get; set; }
        public ISettlement CurrentISettlement { get; set; }
        public IHero EffectiveEngineer { get; set; }
        public IHero EffectiveQuartermaster { get; set; }
        public IHero EffectiveScout { get; set; }
        public IHero EffectiveSurgeon { get; set; }
        public IHero Engineer { get; set; }
        public float FoodChange { get; set; }
        public bool ForceAiNoPathMode { get; set; }
        public int GetNumDaysForFoodToLast { get; set; }
        public float HasUnpaidWages { get; set; }
        public float HealingRateForHeroes { get; set; }
        public float HealingRateForRegulars { get; set; }
        public ISettlement HomeISettlement { get; set; }
        public int InventoryCapacity { get; set; }
        public bool IsActive { get; set; }
        public bool IsAlerted { get; set; }
        public bool IsBandit { get; set; }
        public bool IsBanditBossParty { get; set; }
        public bool IsCaravan { get; set; }
        public bool IsCommonAreaParty { get; set; }
        public bool IsCurrentlyGoingToSettlement { get; set; }
        public bool IsCurrentlyUsedByAQuest { get; set; }
        public bool IsDeserterParty { get; set; }
        public bool IsDisbanding { get; set; }
        public bool IsDisorganized { get; set; }
        public bool IsEngaging { get; set; }
        public bool IsGarrison { get; set; }
        public bool IsGoingToISettlement { get; set; }
        public bool IsHolding { get; set; }
        public bool IsInspected { get; set; }
        public bool IsJoiningArmy { get; set; }
        public bool IsLeaderless { get; set; }
        public bool IsLordParty { get; set; }
        public bool IsMainParty { get; set; }
        public bool IsMilitia { get; set; }
        public bool IsMoving { get; set; }
        public bool IsPartyTradeActive { get; set; }
        public bool IsRaiding { get; set; }
        public bool IsVillager { get; set; }
        public bool IsVisible { get; set; }
        public float LastCachedSpeed { get; set; }
        public ISettlement LastVisitedSettlement { get; set; }
        public ICharacterObject Leader { get; set; }
        public IHero LeaderHero { get; set; }
        public IMobileParty MainParty { get; set; }
        public IFaction MapFaction { get; set; }
        public ITroopRoster MemberRoster { get; set; }
        public float Morale { get; set; }
        public IMobileParty MoveTargetParty { get; set; }
        public string Name { get; set; }
        public bool NeedTargetReset { get; set; }
        public int NumberOfFleeingsAtLastTravel { get; set; }
        public IPartyBase Party { get; set; }
        public float PartySizeRatio { get; set; }
        public int PartyTradeGold { get; set; }
        public int PartyTradeTaxGold { get; set; }
        public PartyType PartyType { get; set; }
        public ITroopRoster PrisonRoster { get; set; }
        public IHero Quartermaster { get; set; }
        public float RecentEventsMorale { get; set; }
        public IHero Scout { get; set; }
        public float SeeingRange { get; set; }
        public ISettlement ShortTermTargetISettlement { get; set; }
        public IMobileParty ShortTermTargetParty { get; set; }
        public bool ShouldBeIgnored { get; set; }
        public bool ShouldJoinPlayerBattles { get; set; }
        public IHero Surgeon { get; set; }
        public ISettlement TargetISettlement { get; set; }
        public IMobileParty TargetParty { get; set; }
        public int TotalFoodAtInventory { get; set; }
        public float TotalStrengthWithFollower { get; set; }
        public float TotalWeightCarried { get; set; }
    }
}