// Code written by Gabriel Mailhot, 30/08/2020.

namespace TalesEntities.TW
{
   #region

   using System.Collections.Generic;
   using TalesContract;
   using TalesEnums;

   #endregion

   public class Hero : IHero
   {
      public float Age { get; set; }

      public bool AlwaysUnconscious { get; set; }

      public IAttribute Attributes { get; set; }

      public bool AwaitingTrial { get; set; }

      public ICampaignTime BirthDay { get; set; }

      public ISettlement BornSettlement { get; set; }

      public bool CanBeCompanion { get; }

      public bool CanHaveRecruits { get; }

      public ICampaignTime CaptivityStartTime { get; set; }

      public ICharacterObject CharacterObject { get; set; }

      public IList<IHero> Children { get; set; } = new List<IHero>();

      public IClan Clan { get; set; }

      public IClan CompanionOf { get; set; }

      public IList<IHero> CompanionsInParty { get; set; } = new List<IHero>();

      public float Controversy { get; set; }

      public ICultureObject Culture { get; set; }

      public ISettlement CurrentSettlement { get; set; }

      public ICampaignTime DeathDay { get; set; }

      public IHero DeathMarkKillerHero { get; set; }

      public bool Detected { get; set; }

      public IList<IHero> ExSpouses { get; set; } = new List<IHero>();

      public IHero Father { get; set; }

      public string FirstName { get; set; }

      public int Gold { get; set; }

      public ITown GovernorOf { get; set; }

      public bool HasMet { get; set; }

      public ISkill HeroSkills { get; set; }

      public CharacterStates HeroState { get; set; }

      public ICharacterTraits HeroTraits { get; set; }

      public int HeroWoundedHealthLevel { get; set; }

      public int HitPoints { get; set; }

      public ISettlement HomeSettlement { get; set; }

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

      public bool IsTemplate { get; set; }

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

      public bool Noncombatant { get; set; }

      public IMobileParty PartyBelongedTo { get; set; }

      public IPartyBase PartyBelongedToAsPrisoner { get; set; }

      public float PassedTimeAtHomeSettlement { get; set; }

      public float Power { get; set; }

      public float ProbabilityOfDeath { get; set; }

      public FactionRank Rank { get; set; }

      public float RelationScoreWithPlayer { get; set; }

      public IList<IHero> Siblings { get; set; } = new List<IHero>();

      public ISkill Skills { get; set; }

      public IHero Spouse { get; set; }

      public ISettlement StayingInSettlementOfNotable { get; set; }

      public IClan SupporterOf { get; set; }

      public ICharacterTraits Traits { get; set; }

      public float UnmodifiedClanLeaderRelationshipScoreWithPlayer { get; set; }

      public float Weight { get; set; }
   }
}