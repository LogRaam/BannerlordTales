// unset

#region

using System.Collections.Generic;
using TalesEnums;

#endregion

namespace TalesContract
{
    #region

    #endregion

    public interface ISettlement
    {
        public BattleSide BattleSide { get; set; }

        public IList<IMobileParty> BoundParties { get; set; }

        public IList<IVillage> BoundVillages { get; set; }

        public int BribePaid { get; set; }

        public int CanBeClaimed { get; set; }

        public IHero ClaimedBy { get; set; }

        public float ClaimValue { get; set; }

        public ICultureObject Culture { get; set; }

        public ISettlement CurrentSettlement { get; set; }

        public string EncyclopediaLink { get; set; }

        public string EncyclopediaLinkWithName { get; set; }

        public string EncyclopediaText { get; set; }

        public int GetNumberOfAvailableRecruits { get; set; }

        public bool HasFestival { get; set; }

        public bool HasMultipleRecruits { get; set; }

        public bool HasRecruits { get; set; }

        public bool HasVisited { get; set; }

        public List<IHero> HeroesWithoutParty { get; set; }

        public IHideout Hideout { get; set; }

        public bool IsActive { get; set; }

        public bool IsAlerted { get; set; }

        public bool IsBooming { get; set; }

        public bool IsCastle { get; set; }

        public bool IsFortification { get; set; }

        public bool IsHideout { get; set; }

        public bool IsInspected { get; set; }

        public bool IsMinorFactionBase { get; set; }

        public bool IsQuestSettlement { get; set; }

        public bool IsRaided { get; set; }

        //public bool IsRebelling { get; set; }

        public bool IsStarving { get; set; }

        public bool IsTown { get; set; }

        public bool IsUnderRaid { get; set; }

        public bool IsUnderRebellionAttack { get; set; }

        public bool IsUnderSiege { get; set; }

        public bool IsVillage { get; set; }

        public bool IsVisible { get; set; }

        public IMobileParty LastAttackerParty { get; set; }

        public float LastVisitTimeOfOwner { get; set; }

        public IFaction MapFaction { get; set; }

        public float MaxHitPointsOfOneWallSection { get; set; }

        public float MaxWallHitPoints { get; set; }

        public IMobileParty MilitaParty { get; set; }

        public float Militia { get; set; }

        public string Name { get; set; }

        public IList<IHero> Notables { get; set; }

        public float NumberOfAlliesSpottedAround { get; set; }

        public float NumberOfEnemiesSpottedAround { get; set; }

        public int NumberOfLordPartiesAt { get; set; }

        public int NumberOfLordPartiesTargeting { get; set; }

        public int NumberOfTroopsKilledOnSide { get; set; }

        public IMobileParty oldMilitiaParty { get; set; }

        public IClan OwnerClan { get; set; }

        public IList<IMobileParty> Parties { get; set; }

        public IPartyBase Party { get; set; }

        public int PassedHoursAfterLastThreat { get; set; }

        public float Prosperity { get; set; }

        public float SettlementHitPoints { get; set; }

        public bool SettlementTaken { get; set; }

        public float SettlementTotalWallHitPoints { get; set; }

        public IList<float> SettlementWallSectionHitPointsRatioList { get; set; }

        public IList<IPartyBase> SiegeParties { get; set; }

        public ITown Town { get; set; }

        public IVillage Village { get; set; }

        public int WallSectionCount { get; set; }
    }
}