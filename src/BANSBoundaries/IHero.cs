// Code written by Gabriel Mailhot, 02/12/2023.

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

        public int Athletics { get; set; }

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

        public int Crafting { get; set; }

        public int Crossbow { get; set; }

        public IBasicCultureObject Culture { get; set; }

        public int Cunning { get; set; }

        public ISettlement CurrentSettlement { get; set; }

        public ICampaignTime DeathDay { get; set; }

        public IHero DeathMarkKillerHero { get; set; }

        public int Endurance { get; set; }

        public int Engineering { get; set; }
        public List<IEquipments> Equipments { get; set; }

        public IList<IHero> ExSpouses { get; set; }

        public IHero Father { get; set; }

        public string FirstName { get; set; }

        public int Gold { get; set; }

        public bool HasMet { get; set; }

        public CharacterSkills HeroSkills { get; set; }

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

        public bool IsFugitive { get; set; }

        public bool IsGangLeader { get; set; }

        public bool IsHeadman { get; set; }

        public bool IsHealthFull { get; set; }

        public bool IsHumanPlayerCharacter { get; set; }

        public bool IsMainHeroIll { get; set; }

        public bool IsMerchant { get; set; }

        public bool IsMinorFactionHero { get; set; }

        public bool IsNotable { get; set; }

        public bool IsNotSpawned { get; set; }

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

        public int Level { get; set; }

        public IFaction MapFaction { get; set; }

        public int MaxHitPoints { get; set; }

        public IHero Mother { get; set; }

        public string Name { get; set; }

        public float PassedTimeAtHomeSettlement { get; set; }

        public float Power { get; set; }

        public float ProbabilityOfDeath { get; set; }

        public float RelationScoreWithPlayer { get; set; }

        public IList<IHero> Siblings { get; set; }

        public IHero Spouse { get; set; }

        public IClan SupporterOf { get; set; }

        public float UnmodifiedClanLeaderRelationshipScoreWithPlayer { get; set; }

        public int Vigor { get; set; }

        public float? Weight { get; set; }
    }
}