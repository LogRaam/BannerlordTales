// Code written by Gabriel Mailhot, 12/10/2020.

#region

using System;
using TalesContract;
using TalesDAL;
using TalesEntities.Stories;
using TalesEnums;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;

#endregion

namespace TalesPersistence.Entities
{
    public class Evaluation : BaseEvaluation
    {
        public Evaluation(IEvaluation condition)
        {
            Subject = condition.Subject;

            Attribute = condition.Attribute;
            Characteristic = condition.Characteristic;
            PartyType = condition.PartyType;
            PersonalityTrait = condition.PersonalityTrait;
            Skill = condition.Skill;
            Time = condition.Time;

            PregnancyRisk = condition.PregnancyRisk;

            Operator = condition.Operator;

            RandomStart = condition.RandomStart;
            RandomEnd = condition.RandomEnd;

            Value = condition.Value;
            ValueIsPercentage = condition.ValueIsPercentage;
        }

        public Evaluation() { }


        public void ApplyConsequenceInGame()
        {
            ApplyPregnancyRiskConsequence();
            ApplyAttributeConsequence();
            ApplyCharacteristicConsequence();
            ApplyPersonalityTraitConsequence();
            ApplySkillConsequence();
        }

        public bool CanBePlayedInContext()
        {
            if (!AttributeAccepted()) return false;
            if (!CharacteristicAccepted()) return false;
            if (!PartyTypeAccepted()) return false;
            if (!PersonalityTraitAccepted()) return false;
            if (!SkillAccepted()) return false;
            if (!TimeAccepted()) return false;

            return true;
        }

        #region private

        private void ApplyAttributeConsequence()
        {
            if (Attribute == null) return;

            var value = GetModifierValue();
            var hero = IdentifySubject().ToHero();

            if (Attribute is Attributes.UNKNOWN) throw new ApplicationException("consequence.Attribute unknown");

            if (Attribute is Attributes.VIGOR) SetAttribute(hero, CharacterAttributesEnum.Vigor, value);
            if (Attribute is Attributes.CONTROL) SetAttribute(hero, CharacterAttributesEnum.Control, value);
            if (Attribute is Attributes.ENDURANCE) SetAttribute(hero, CharacterAttributesEnum.Endurance, value);
            if (Attribute is Attributes.CUNNING) SetAttribute(hero, CharacterAttributesEnum.Cunning, value);
            if (Attribute is Attributes.SOCIAL) SetAttribute(hero, CharacterAttributesEnum.Social, value);
            if (Attribute is Attributes.INTELLIGENCE) SetAttribute(hero, CharacterAttributesEnum.Intelligence, value);
        }

        private void ApplyCharacteristicConsequence()
        {
            if (Characteristic == null) return;

            var value = GetModifierValue();
            var hero = IdentifySubject().ToHero();

            if (Characteristic == Characteristics.UNKNOWN) throw new ApplicationException("consequence.Characteristic unknown");

            if (Characteristic == Characteristics.HEALTH) hero.HitPoints += value;
            if (Characteristic == Characteristics.GOLD) hero.Gold += value;
            if (Characteristic == Characteristics.RENOWN) hero.Clan.Renown += value;
        }

        private void ApplyPersonalityTraitConsequence()
        {
            if (PersonalityTrait == null) return;

            var value = GetModifierValue();
            var hero = IdentifySubject().ToHero();

            if (PersonalityTrait == PersonalityTraits.MERCY) hero.SetTraitLevel(TraitObject.FindFirst(n => n.StringId.ToUpper() == "MERCY"), hero.GetHeroTraits().Mercy + value);
            if (PersonalityTrait == PersonalityTraits.GENEROSITY) hero.SetTraitLevel(TraitObject.FindFirst(n => n.StringId.ToUpper() == "GENEROSITY"), hero.GetHeroTraits().Generosity + value);
            if (PersonalityTrait == PersonalityTraits.HONOR) hero.SetTraitLevel(TraitObject.FindFirst(n => n.StringId.ToUpper() == "HONOR"), hero.GetHeroTraits().Honor + value);
            if (PersonalityTrait == PersonalityTraits.VALOR) hero.SetTraitLevel(TraitObject.FindFirst(n => n.StringId.ToUpper() == "VALOR"), hero.GetHeroTraits().Valor + value);
        }


        private void ApplyPregnancyRiskConsequence()
        {
            if (!PregnancyRisk) return;

            if (ValueIsPercentage)
            {
                if (TalesRandom.EvalPercentage(int.Parse(Value))) MakePregnant();

                return;
            }

            if (string.IsNullOrEmpty(Value) && RandomEnd > 0)
                if (TalesRandom.EvalPercentageRange(RandomStart, RandomEnd))
                    MakePregnant();
        }

        private void ApplySkillConsequence()
        {
            if (Skill == null) return;

            var value = GetModifierValue();
            var hero = IdentifySubject().ToHero();

            SetSkill(hero, Skill.ToString(), value);
        }

        private bool AttributeAccepted()
        {
            switch (Attribute)
            {
                case Attributes.UNKNOWN:      throw new ApplicationException("Attribute unknown: " + Attribute);
                case Attributes.VIGOR:        return EvalOperation(IdentifySubject().Vigor);
                case Attributes.CONTROL:      return EvalOperation(IdentifySubject().Control);
                case Attributes.ENDURANCE:    return EvalOperation(IdentifySubject().Endurance);
                case Attributes.CUNNING:      return EvalOperation(IdentifySubject().Cunning);
                case Attributes.SOCIAL:       return EvalOperation(IdentifySubject().Social);
                case Attributes.INTELLIGENCE: return EvalOperation(IdentifySubject().Intelligence);
                case null:                    throw new NullReferenceException("Attribute is null");
                default:                      throw new ArgumentOutOfRangeException();
            }
        }

        private bool CharacteristicAccepted()
        {
            switch (Characteristic)
            {
                case Characteristics.UNKNOWN:    throw new ApplicationException("Characteristic Unknown");
                case Characteristics.AGE:        return EvalOperation((int)IdentifySubject().Age);
                case Characteristics.GENDER:     return EvalGender(IdentifySubject());
                case Characteristics.HEALTH:     return EvalOperation(IdentifySubject().HitPoints);
                case Characteristics.GOLD:       return EvalOperation(IdentifySubject().Gold);
                case Characteristics.RENOWN:     return EvalOperation((int)IdentifySubject().Clan.Renown);
                case Characteristics.CULTURE:    return Value.ToUpper() == IdentifySubject().Culture.CultureCode.ToString();
                case Characteristics.OCCUPATION: return EvalOccupation(IdentifySubject());
                case null:                       throw new NullReferenceException("Characteristic is null");
                default:                         throw new ArgumentOutOfRangeException();
            }
        }


        private bool EvalGender(IHero subject)
        {
            var t = Value.ToUpper();

            if (t == "ANY") return true;
            if (subject.IsFemale) return t == "FEMALE";

            return t == "MALE";
        }

        private bool EvalOccupation(IHero subject)
        {
            switch (Value.ToUpper())
            {
                case "ARTISAN":          return subject.IsArtisan;
                case "CHILD":            return subject.IsChild;
                case "COMMANDER":        return subject.IsCommander;
                case "FACTIONLEADER":    return subject.IsFactionLeader;
                case "FUGITIVE":         return subject.IsFugitive;
                case "GANGLEADER":       return subject.IsGangLeader;
                case "HEADMAN":          return subject.IsHeadman;
                case "MERCENARY":        return subject.IsMercenary;
                case "MERCHANT":         return subject.IsMerchant;
                case "MINORFACTIONHERO": return subject.IsMinorFactionHero;
                case "NOBLE":            return subject.IsNoble;
                case "NOTABLE":          return subject.IsNotable;
                case "OUTLAW":           return subject.IsOutlaw;
                case "PARTYLEADER":      return subject.IsPartyLeader;
                case "PLAYERCOMPANION":  return subject.IsPlayerCompanion;
                case "PREACHER":         return subject.IsPreacher;
                case "REBEL":            return subject.IsRebel;
                case "RURALNOTABLE":     return subject.IsRuralNotable;
                case "SPECIAL":          return subject.IsSpecial;
                case "WANDERER":         return subject.IsWanderer;
                default:                 throw new ArgumentOutOfRangeException();
            }
        }

        private bool EvalOperation(int n)
        {
            var value = int.Parse(Value);

            switch (Operator)
            {
                case Operator.UNKNOWN:     throw new ApplicationException("Operator unknown");
                case Operator.GREATERTHAN: return n > value;
                case Operator.LOWERTHAN:   return n < value;
                case Operator.EQUALTO:     return n == value;
                case Operator.NOTEQUALTO:  return n != value;
                default:                   throw new ArgumentOutOfRangeException();
            }
        }


        private int GetModifierValue()
        {
            return !string.IsNullOrEmpty(Value)
                ? int.Parse(Value)
                : TalesRandom.GenerateRandomNumber(RandomStart, RandomEnd);
        }

        private Hero IdentifySubject()
        {
            return Subject == Actor.NPC
                ? new Hero(GameData.Instance.GameContext.Captor)
                : new Hero(GameData.Instance.GameContext.Player);
        }

        private void MakePregnant()
        {
            var actor = IdentifySubject();

            if (!actor.IsPregnant) MakePregnantAction.Apply(actor.ToHero());
        }


        private bool PartyTypeAccepted()
        {
            switch (PartyType)
            {
                case PartyType.UNKNOWN:       throw new ApplicationException("PartyType Trait unknown: " + PartyType);
                case PartyType.DEFAULT:       return EvalOperation(IdentifySubject().Mercy);
                case PartyType.LORD:          return EvalOperation(IdentifySubject().Generosity);
                case PartyType.BANDIT:        return EvalOperation(IdentifySubject().Honor);
                case PartyType.VILLAGER:      return EvalOperation(IdentifySubject().Valor);
                case PartyType.GARRISONPARTY: return EvalOperation(IdentifySubject().Valor);
                case PartyType.CARAVAN:       return EvalOperation(IdentifySubject().Valor);
                default:                      throw new ArgumentOutOfRangeException();
            }
        }

        private bool PersonalityTraitAccepted()
        {
            switch (PersonalityTrait)
            {
                case PersonalityTraits.UNKNOWN:    throw new ApplicationException("Personality Trait unknown: " + PersonalityTrait);
                case PersonalityTraits.MERCY:      return EvalOperation(IdentifySubject().Mercy);
                case PersonalityTraits.GENEROSITY: return EvalOperation(IdentifySubject().Generosity);
                case PersonalityTraits.HONOR:      return EvalOperation(IdentifySubject().Honor);
                case PersonalityTraits.VALOR:      return EvalOperation(IdentifySubject().Valor);
                default:                           throw new ArgumentOutOfRangeException();
            }
        }

        private void SetAttribute(TaleWorlds.CampaignSystem.Hero hero, CharacterAttributesEnum attribute, int value)
        {
            hero.SetAttributeValue(attribute, hero.GetAttributeValue(attribute) + value);
        }


        private void SetSkill(TaleWorlds.CampaignSystem.Hero hero, string skill, int value)
        {
            var s = SkillObject.FindFirst(n => n.StringId.ToUpper() == skill);
            hero.SetSkillValue(s, hero.GetSkillValue(s) + value);
        }

        private bool SkillAccepted()
        {
            switch (Skill)
            {
                case Skills.UNKNOWN:     throw new ApplicationException("Personality Trait unknown: " + PersonalityTrait);
                case Skills.ONEHANDED:   return EvalOperation(IdentifySubject().OneHanded);
                case Skills.TWOHANDED:   return EvalOperation(IdentifySubject().TwoHanded);
                case Skills.POLEARM:     return EvalOperation(IdentifySubject().Polearm);
                case Skills.BOW:         return EvalOperation(IdentifySubject().Bow);
                case Skills.CROSSBOW:    return EvalOperation(IdentifySubject().Crossbow);
                case Skills.THROWING:    return EvalOperation(IdentifySubject().Throwing);
                case Skills.RIDING:      return EvalOperation(IdentifySubject().Riding);
                case Skills.ATHLETICS:   return EvalOperation(IdentifySubject().Athletics);
                case Skills.CRAFTING:    return EvalOperation(IdentifySubject().Crafting);
                case Skills.SCOUTING:    return EvalOperation(IdentifySubject().Scouting);
                case Skills.TACTICS:     return EvalOperation(IdentifySubject().Tactics);
                case Skills.ROGUERY:     return EvalOperation(IdentifySubject().Roguery);
                case Skills.CHARM:       return EvalOperation(IdentifySubject().Charm);
                case Skills.LEADERSHIP:  return EvalOperation(IdentifySubject().Leadership);
                case Skills.TRADE:       return EvalOperation(IdentifySubject().Trade);
                case Skills.STEWARD:     return EvalOperation(IdentifySubject().Steward);
                case Skills.MEDICINE:    return EvalOperation(IdentifySubject().Medecine);
                case Skills.ENGINEERING: return EvalOperation(IdentifySubject().Engineering);
                default:                 throw new ArgumentOutOfRangeException();
            }
        }


        private bool TimeAccepted()
        {
            switch (Time)
            {
                case GameTime.ANYTIME:
                case GameTime.NONE:
                case GameTime.UNKNOWN:
                    return true;
            }

            return Time == GameData.Instance.GameContext.GameTime;
        }

        #endregion
    }
}