// Code written by Gabriel Mailhot, 27/09/2020.

#region

using System;
using _47_TalesMath;
using TalesContract;
using TalesEntities.TW;
using TalesEnums;

#endregion

namespace TalesPersistence.Entities
{
    public class Hero : BaseHero
    {
        public Hero(IHero hero)
        {
            Age = hero.Age;
            AlwaysUnconscious = hero.AlwaysUnconscious;
            Athletics = hero.Athletics;
            AwaitingTrial = hero.AwaitingTrial;
            if (hero.BirthDay != null) BirthDay = hero.BirthDay;
            if (hero.BornSettlement != null) BornSettlement = hero.BornSettlement;
            Bow = hero.Bow;
            CanBeCompanion = hero.CanBeCompanion;
            CanHaveRecruits = hero.CanHaveRecruits;
            if (hero.CaptivityStartTime != null) CaptivityStartTime = hero.CaptivityStartTime;
            if (hero.CharacterObject != null) CharacterObject = hero.CharacterObject;
            Charm = hero.Charm;
            if (hero.Children != null) Children = hero.Children;
            if (hero.Clan != null) Clan = hero.Clan;
            if (hero.CompanionOf != null) CompanionOf = hero.CompanionOf;
            if (hero.CompanionsInParty != null) CompanionsInParty = hero.CompanionsInParty;
            Control = hero.Control;
            Controversy = hero.Controversy;
            Crafting = hero.Crafting;
            Calculating = hero.Calculating;
            Crossbow = hero.Crossbow;
            if (hero.Culture != null) Culture = hero.Culture;
            Cunning = hero.Cunning;
            if (hero.CurrentSettlement != null) CurrentSettlement = hero.CurrentSettlement;
            if (hero.DeathDay != null) DeathDay = hero.DeathDay;
            if (hero.DeathMarkKillerHero != null) DeathMarkKillerHero = hero.DeathMarkKillerHero;
            Detected = hero.Detected;
            Endurance = hero.Endurance;
            Engineering = hero.Engineering;
            if (hero.ExSpouses != null) ExSpouses = hero.ExSpouses;
            Father = hero.Father;
            FirstName = hero.FirstName;
            Gold = hero.Gold;
            if (hero.GovernorOf != null) GovernorOf = hero.GovernorOf;
            HasMet = hero.HasMet;
            if (hero.HeroSkills != null) HeroSkills = hero.HeroSkills;
            HeroState = hero.HeroState;
            if (hero.HeroTraits != null) HeroTraits = hero.HeroTraits;
            HitPoints = hero.HitPoints;
            if (hero.HomeSettlement != null) HomeSettlement = hero.HomeSettlement;
            if (hero.Id != null) Id = hero.Id;
            Intelligence = hero.Intelligence;
            IsActive = hero.IsActive;
            IsAlive = hero.IsAlive;
            IsArtisan = hero.IsArtisan;
            IsChild = hero.IsChild;
            IsCommander = hero.IsCommander;
            IsDead = hero.IsDead;
            IsDisabled = hero.IsDisabled;
            IsFactionLeader = hero.IsFactionLeader;
            IsFemale = hero.IsFemale;
            IsFertile = hero.IsFertile;
            IsFugitive = hero.IsFugitive;
            IsGangLeader = hero.IsGangLeader;
            IsHeadman = hero.IsHeadman;
            IsHealthFull = hero.IsHealthFull;
            IsHumanPlayerCharacter = hero.IsHumanPlayerCharacter;
            IsMainHeroIll = hero.IsMainHeroIll;
            IsMercenary = hero.IsMercenary;
            IsMerchant = hero.IsMerchant;
            IsMinorFactionHero = hero.IsMinorFactionHero;
            IsNoble = hero.IsNoble;
            IsNotable = hero.IsNotable;
            IsNotSpawned = hero.IsNotSpawned;
            IsOccupiedByAnEvent = hero.IsOccupiedByAnEvent;
            IsOutlaw = hero.IsOutlaw;
            IsPartyLeader = hero.IsPartyLeader;
            IsPlayerCompanion = hero.IsPlayerCompanion;
            IsPreacher = hero.IsPreacher;
            IsPregnant = hero.IsPregnant;
            IsPrisoner = hero.IsPrisoner;
            IsRebel = hero.IsRebel;
            IsReleased = hero.IsReleased;
            IsRuralNotable = hero.IsRuralNotable;
            IsSpecial = hero.IsSpecial;
            IsWanderer = hero.IsWanderer;
            IsWounded = hero.IsWounded;
            if (hero.LastMeetingTimeWithPlayer != null) LastMeetingTimeWithPlayer = hero.LastMeetingTimeWithPlayer;
            LastSeenInSettlement = hero.LastSeenInSettlement;
            if (hero.LastSeenPlace != null) LastSeenPlace = hero.LastSeenPlace;
            if (hero.LastSeenTime != null) LastSeenTime = hero.LastSeenTime;
            Level = hero.Level;
            if (hero.MapFaction != null) MapFaction = hero.MapFaction;
            MaxHitPoints = hero.MaxHitPoints;
            if (hero.Mother != null) Mother = hero.Mother;
            Name = hero.Name;
            NeverBecomePrisoner = hero.NeverBecomePrisoner;
            Noncombatant = hero.Noncombatant;
            if (hero.PartyBelongedTo != null) PartyBelongedTo = hero.PartyBelongedTo;
            if (hero.PartyBelongedToAsPrisoner != null) PartyBelongedToAsPrisoner = hero.PartyBelongedToAsPrisoner;
            PassedTimeAtHomeSettlement = hero.PassedTimeAtHomeSettlement;
            Power = hero.Power;
            ProbabilityOfDeath = hero.ProbabilityOfDeath;
            Rank = hero.Rank;
            RelationScoreWithPlayer = hero.RelationScoreWithPlayer;
            if (hero.Siblings != null) Siblings = hero.Siblings;
            if (hero.Spouse != null) Spouse = hero.Spouse;
            if (hero.StayingInSettlementOfNotable != null) StayingInSettlementOfNotable = hero.StayingInSettlementOfNotable;
            if (hero.SupporterOf != null) SupporterOf = hero.SupporterOf;
            UnmodifiedClanLeaderRelationshipScoreWithPlayer = hero.UnmodifiedClanLeaderRelationshipScoreWithPlayer;
            Vigor = hero.Vigor;
            Weight = hero.Weight;
        }

        public Hero()
        {
        }

        public bool IsConsequenceConformFor(IEvaluation consequence)
        {
            if (consequence.PersonalityTrait != null) return IsPersonalityTraitConformFor(consequence);

            if (consequence.Attribute != null) return IsAttributeConformFor(consequence);

            if (consequence.Skill != null) return IsSkillConformFor(consequence);

            if (consequence.Characteristic != null) return IsCharacteristicConformFor(consequence);

            return true;
        }

        public TaleWorlds.CampaignSystem.Hero ToHero()
        {
            return TaleWorlds.CampaignSystem.Hero.FindFirst(n => n.Name.ToString() == Name && n.IsHumanPlayerCharacter == IsHumanPlayerCharacter && n.IsFemale == IsFemale);
        }

        #region private

        private bool IsAttributeConformFor(IEvaluation consequence)
        {
            switch (consequence.Attribute)
            {
                case Attributes.VIGOR:        return GameMath.IsEvaluationConform(consequence, Vigor);
                case Attributes.CONTROL:      return GameMath.IsEvaluationConform(consequence, Control);
                case Attributes.ENDURANCE:    return GameMath.IsEvaluationConform(consequence, Endurance);
                case Attributes.CUNNING:      return GameMath.IsEvaluationConform(consequence, Cunning);
                case Attributes.SOCIAL:       return GameMath.IsEvaluationConform(consequence, Social);
                case Attributes.INTELLIGENCE: return GameMath.IsEvaluationConform(consequence, Intelligence);
                case Attributes.UNKNOWN:      return true;
                case null:                    return true;

                default: return true;
            }
        }


        private bool IsCharacteristicConformFor(IEvaluation consequence)
        {
            switch (consequence.Characteristic)
            {
                case Characteristics.UNKNOWN:    return true;
                case Characteristics.AGE:        return GameMath.IsEvaluationConform(consequence, Age);
                case Characteristics.GENDER:     return IsGenderConform(consequence);
                case Characteristics.HEALTH:     return GameMath.IsEvaluationConform(consequence, HitPoints);
                case Characteristics.GOLD:       return GameMath.IsEvaluationConform(consequence, Gold);
                case Characteristics.RENOWN:     return GameMath.IsEvaluationConform(consequence, Clan.Renown);
                case Characteristics.CULTURE:    return IsCultureConform(consequence);
                case Characteristics.OCCUPATION: return IsOccupationConform(consequence);
                default:                         throw new ArgumentOutOfRangeException();
            }
        }

        private bool IsCultureConform(IEvaluation consequence)
        {
            return Culture.Name.ToUpper() == consequence.Value;
        }

        private bool IsGenderConform(IEvaluation consequence)
        {
            var female = consequence.Value == "FEMALE";

            return female == IsFemale;
        }

        private bool IsOccupationConform(IEvaluation consequence)
        {
            return new BaseCharacterObject(this).Occupation.ToString() == consequence.Value.ToUpper();
        }


        private bool IsPersonalityTraitConformFor(IEvaluation trait)
        {
            throw new NotImplementedException();
        }

        private bool IsSkillConformFor(IEvaluation skill)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}