// Code written by Gabriel Mailhot, 02/12/2023.

#region

using _47_TalesMath;
using Helpers;
using System;
using TalesBase.TW;
using TalesContract;
using TalesEnums;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

#endregion

namespace TalesPersistence.Entities
{
    public class Hero : BaseHero
    {
        public Hero(IHero hero)
        {
            Age = hero.Age;
            //AlwaysUnconscious = hero.AlwaysUnconscious;
            Athletics = hero.Athletics;

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

            Crafting = hero.Crafting;
            Calculating = hero.Calculating;
            Crossbow = hero.Crossbow;
            if (hero.Culture != null) Culture = hero.Culture;
            Cunning = hero.Cunning;
            if (hero.CurrentSettlement != null) CurrentSettlement = hero.CurrentSettlement;
            if (hero.DeathDay != null) DeathDay = hero.DeathDay;
            if (hero.DeathMarkKillerHero != null) DeathMarkKillerHero = hero.DeathMarkKillerHero;

            Endurance = hero.Endurance;
            Engineering = hero.Engineering;
            if (hero.ExSpouses != null) ExSpouses = hero.ExSpouses;
            Father = hero.Father;
            FirstName = hero.FirstName;
            Gold = hero.Gold;

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

            IsFugitive = hero.IsFugitive;
            IsGangLeader = hero.IsGangLeader;
            IsHeadman = hero.IsHeadman;
            IsHealthFull = hero.IsHealthFull;
            IsHumanPlayerCharacter = hero.IsHumanPlayerCharacter;
            IsMainHeroIll = hero.IsMainHeroIll;

            IsMerchant = hero.IsMerchant;
            IsMinorFactionHero = hero.IsMinorFactionHero;

            IsNotable = hero.IsNotable;
            IsNotSpawned = hero.IsNotSpawned;


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


            Level = hero.Level;
            if (hero.MapFaction != null) MapFaction = hero.MapFaction;
            MaxHitPoints = hero.MaxHitPoints;
            if (hero.Mother != null) Mother = hero.Mother;
            Name = hero.Name;


            PassedTimeAtHomeSettlement = hero.PassedTimeAtHomeSettlement;
            Power = hero.Power;
            ProbabilityOfDeath = hero.ProbabilityOfDeath;

            RelationScoreWithPlayer = hero.RelationScoreWithPlayer;
            if (hero.Siblings != null) Siblings = hero.Siblings;
            if (hero.Spouse != null) Spouse = hero.Spouse;

            if (hero.SupporterOf != null) SupporterOf = hero.SupporterOf;
            UnmodifiedClanLeaderRelationshipScoreWithPlayer = hero.UnmodifiedClanLeaderRelationshipScoreWithPlayer;
            Vigor = hero.Vigor;
            Weight = hero.Weight;
        }

        public Hero() { }

        public void GiveBodyArmor(string bodyArmorId)
        {
            var armor = MBObjectManager.Instance.GetObject<ItemObject>(bodyArmorId);
            var equipment = new Equipment();
            equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Body, new EquipmentElement(armor));

            EquipmentHelper.AssignHeroEquipmentFromEquipment(ToTwHero(), equipment);
        }

        public void GiveWeapon(string weaponId)
        {
            var weapon = MBObjectManager.Instance.GetObject<ItemObject>(weaponId);
            var equipment = new Equipment();
            equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon1, new EquipmentElement(weapon));

            EquipmentHelper.AssignHeroEquipmentFromEquipment(ToTwHero(), equipment);
        }

        public bool IsConsequenceConformFor(IEvaluation consequence)
        {
            if (consequence.Persona.PersonalityTrait != null) return IsPersonalityTraitConformFor(consequence);

            if (consequence.Persona.Attribute != null) return IsAttributeConformFor(consequence);

            if (consequence.Persona.Skill != null) return IsSkillConformFor(consequence);

            if (consequence.Persona.Characteristic != null) return IsCharacteristicConformFor(consequence);

            return true;
        }

        public TaleWorlds.CampaignSystem.Hero ToTwHero()
        {
            return TaleWorlds.CampaignSystem.Hero.FindFirst(n => n.Name.ToString() == Name && n.IsHumanPlayerCharacter == IsHumanPlayerCharacter && n.IsFemale == IsFemale);
        }

        public void Undress()
        {
            //TODO: keep equipment somewhere in case we want to return it to player.
            EquipmentHelper.AssignHeroEquipmentFromEquipment(ToTwHero(), new Equipment());
        }

        #region private

        private bool IsAttributeConformFor(IEvaluation consequence)
        {
            switch (consequence.Persona.Attribute)
            {
                case Attributes.Vigor: return GameMath.IsEvaluationConform(consequence, Vigor);
                case Attributes.Control: return GameMath.IsEvaluationConform(consequence, Control);
                case Attributes.Endurance: return GameMath.IsEvaluationConform(consequence, Endurance);
                case Attributes.Cunning: return GameMath.IsEvaluationConform(consequence, Cunning);
                case Attributes.Social: return GameMath.IsEvaluationConform(consequence, Social);
                case Attributes.Intelligence: return GameMath.IsEvaluationConform(consequence, Intelligence);
                case Attributes.NotAssigned: return true;
                case null: return true;

                default: return true;
            }
        }


        private bool IsCharacteristicConformFor(IEvaluation consequence)
        {
            switch (consequence.Persona.Characteristic)
            {
                case Characteristics.NotAssigned: return true;
                case Characteristics.Age: return GameMath.IsEvaluationConform(consequence, Age);
                case Characteristics.Gender: return IsGenderConform(consequence);
                case Characteristics.Health: return GameMath.IsEvaluationConform(consequence, HitPoints);
                case Characteristics.Gold: return GameMath.IsEvaluationConform(consequence, Gold);
                case Characteristics.Renown: return GameMath.IsEvaluationConform(consequence, Clan.Renown);
                case Characteristics.Culture: return IsCultureConform(consequence);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private bool IsCultureConform(IEvaluation consequence)
        {
            return Culture.Name.ToUpper() == consequence.Numbers.Value;
        }

        private bool IsGenderConform(IEvaluation consequence)
        {
            var female = consequence.Numbers.Value == "FEMALE";

            return female == IsFemale;
        }

        private bool IsPersonalityTraitConformFor(IEvaluation trait)
        {
            var ev = new Evaluation(trait);

            return ev.CanBePlayedInContext();
        }

        private bool IsSkillConformFor(IEvaluation skill)
        {
            var ev = new Evaluation(skill);

            return ev.CanBePlayedInContext();
        }

        #endregion
    }
}