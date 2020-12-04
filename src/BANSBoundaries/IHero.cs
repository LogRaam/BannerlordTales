// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System.Collections.Generic;
using TalesEnums;
using TaleWorlds.ObjectSystem;

#endregion

namespace TalesContract
{
    #region

    #endregion

    public interface IHero
    {
        public float Age { get; set; }

        public bool AlwaysUnconscious { get; set; }

        public int Athletics { get; set; }


        public bool AwaitingTrial { get; set; }

        public ICampaignTime BirthDay { get; set; }

        public ISettlement BornSettlement { get; set; }

        public int Bow { get; set; }
        int Calculating { get; set; }

        public bool CanBeCompanion { get; }

        public bool CanHaveRecruits { get; }

        public ICampaignTime CaptivityStartTime { get; set; }

        public ICharacterObject CharacterObject { get; set; }

        public int Charm { get; set; }

        public IList<IHero> Children { get; }

        public IClan Clan { get; set; }

        public IClan CompanionOf { get; set; }

        public IList<IHero> CompanionsInParty { get; set; }

        public int Control { get; set; }

        public float Controversy { get; set; }

        public int Crafting { get; set; }

        public int Crossbow { get; set; }

        public IBasicCultureObject Culture { get; set; }

        public int Cunning { get; set; }

        public ISettlement CurrentSettlement { get; set; }

        public ICampaignTime DeathDay { get; set; }

        public IHero DeathMarkKillerHero { get; set; }

        public bool Detected { get; set; }

        public int Endurance { get; set; }

        public int Engineering { get; set; }
        public List<IEquipments> Equipments { get; set; }

        public IList<IHero> ExSpouses { get; set; }

        public IHero Father { get; set; }

        public string FirstName { get; set; }

        public int Gold { get; set; }

        public ITown GovernorOf { get; set; }

        public bool HasMet { get; set; }

        public ICharacterSkills HeroSkills { get; set; }


        public CharacterStates HeroState { get; set; }

        public ICharacterTraits HeroTraits { get; set; }


        public int HitPoints { get; set; }

        public ISettlement HomeSettlement { get; set; }
        public MBGUID Id { get; set; }

        public int Intelligence { get; set; }

        public bool IsActive { get; set; }

        public bool IsAlive { get; set; }

        public bool IsArtisan { get; set; }

        public bool IsChild { get; set; }

        public bool IsCommander { get; set; }

        public bool IsDead { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsFactionLeader { get; set; }

        public bool IsFemale { get; set; }

        public bool IsFertile { get; set; }

        public bool IsFugitive { get; set; }

        public bool IsGangLeader { get; set; }

        public bool IsHeadman { get; set; }

        public bool IsHealthFull { get; set; }

        public bool IsHumanPlayerCharacter { get; set; }

        public bool IsMainHeroIll { get; set; }

        public bool IsMercenary { get; set; }

        public bool IsMerchant { get; set; }

        public bool IsMinorFactionHero { get; set; }

        public bool IsNoble { get; set; }

        public bool IsNotable { get; set; }

        public bool IsNotSpawned { get; set; }

        public bool IsOccupiedByAnEvent { get; set; }

        public bool IsOutlaw { get; set; }

        public bool IsPartyLeader { get; set; }

        public bool IsPlayerCompanion { get; set; }

        public bool IsPreacher { get; set; }

        public bool IsPregnant { get; set; }

        public bool IsPrisoner { get; set; }

        public bool IsRebel { get; set; }

        public bool IsReleased { get; set; }

        public bool IsRuralNotable { get; set; }

        public bool IsSpecial { get; set; }


        public bool IsWanderer { get; set; }

        public bool IsWounded { get; set; }

        public ICampaignTime LastMeetingTimeWithPlayer { get; set; }

        public bool LastSeenInSettlement { get; set; }

        public ISettlement LastSeenPlace { get; set; }

        public ICampaignTime LastSeenTime { get; set; }

        public int Level { get; set; }

        public IFaction MapFaction { get; set; }

        public int MaxHitPoints { get; set; }

        public IHero Mother { get; set; }

        public string Name { get; set; }

        public bool NeverBecomePrisoner { get; set; }

        public bool Noncombatant { get; }

        public IMobileParty PartyBelongedTo { get; set; }

        public IPartyBase PartyBelongedToAsPrisoner { get; set; }

        public float PassedTimeAtHomeSettlement { get; set; }

        public float Power { get; set; }

        public float ProbabilityOfDeath { get; set; }

        public FactionRank Rank { get; set; }

        public float RelationScoreWithPlayer { get; set; }

        public IList<IHero> Siblings { get; set; }


        public IHero Spouse { get; set; }

        public ISettlement StayingInSettlementOfNotable { get; set; }

        public IClan SupporterOf { get; set; }


        public float UnmodifiedClanLeaderRelationshipScoreWithPlayer { get; set; }

        public int Vigor { get; set; }

        public float? Weight { get; set; }
    }
}