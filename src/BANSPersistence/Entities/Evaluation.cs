// Code written by Gabriel Mailhot, 26/10/2020.

#region

using System;
using _47_TalesMath;
using TalesBase.Stories;
using TalesContract;
using TalesDAL;
using TalesEnums;
using TalesPersistence.Context;
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
            Escaping = condition.Escaping;
        }

        public Evaluation() { }


        public void ApplyConsequenceInGame()
        {
            GameFunction.Log("ApplyConsequenceInGame()");
            //BUG:Renown doesnt seems to work
            ApplyPregnancyRiskConsequence();
            ApplyAttributeConsequence();
            ApplyCharacteristicConsequence();
            ApplyPersonalityTraitConsequence();
            ApplySkillConsequence();
            ApplyEscapeConsequence();
        }

        public bool CanBePlayedInContext()
        {
            if (!AttributeAccepted())
                return false;
            if (!CharacteristicAccepted())
                return false;
            if (!PartyTypeAccepted())
                return false;
            if (!PersonalityTraitAccepted())
                return false;
            if (!SkillAccepted())
                return false;
            if (!TimeAccepted())
                return false;

            return true;
        }

        #region private

        private void ApplyAttributeConsequence()
        {
            if (Attribute == null) return;
            if (Attribute is Attributes.UNKNOWN) return;

            var value = GetModifierValue();
            var hero = IdentifySubject().ToHero();

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
            if (Characteristic == Characteristics.UNKNOWN) return;

            var value = GetModifierValue();
            var hero = IdentifySubject().ToHero();

            if (Characteristic == Characteristics.HEALTH) hero.HitPoints += value;
            if (Characteristic == Characteristics.GOLD) hero.Gold += value;
            if (Characteristic == Characteristics.RENOWN) hero.Clan.Renown += value;
        }

        private void ApplyEscapeConsequence()
        {
            GameFunction.Log("ApplyEscapeConsequence() consequence Value => " + Value + ", Escaping => " + Escaping);

            if (!Escaping)
            {
                GameFunction.Log(".. not escaping => return");

                return;
            }


            var p = IdentifySubject().ToHero();

            GameFunction.Log("... hero is " + p.Name);

            if (!p.IsPrisoner)
            {
                GameFunction.Log("... hero is not a prisoner => return");

                return;
            }

            if (p.IsHumanPlayerCharacter)
            {
                GameFunction.Log("... call => EndCaptivity()");
                PlayerCaptivity.EndCaptivity();
            }
            else
            {
                GameFunction.Log("... call => SetPrisonerFreeAction.Apply(p, player)");
                SetPrisonerFreeAction.Apply(p, new Hero(GameData.Instance.GameContext.Player).ToHero());
            }
        }

        private void ApplyPersonalityTraitConsequence()
        {
            if (PersonalityTrait == null) return;
            if (PersonalityTrait == PersonalityTraits.UNKNOWN) return;

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
            if (Skill == Skills.UNKNOWN) return;

            var value = GetModifierValue();
            var hero = IdentifySubject().ToHero();

            SetSkill(hero, Skill.ToString(), value);
        }

        private bool AttributeAccepted()
        {
            switch (Attribute)
            {
                case Attributes.UNKNOWN:      return true;
                case Attributes.VIGOR:        return GameMath.IsEvaluationConform(this, IdentifySubject().Vigor);
                case Attributes.CONTROL:      return GameMath.IsEvaluationConform(this, IdentifySubject().Control);
                case Attributes.ENDURANCE:    return GameMath.IsEvaluationConform(this, IdentifySubject().Endurance);
                case Attributes.CUNNING:      return GameMath.IsEvaluationConform(this, IdentifySubject().Cunning);
                case Attributes.SOCIAL:       return GameMath.IsEvaluationConform(this, IdentifySubject().Social);
                case Attributes.INTELLIGENCE: return GameMath.IsEvaluationConform(this, IdentifySubject().Intelligence);
                case null:                    throw new NullReferenceException("Attribute is null");
                default:                      throw new ArgumentOutOfRangeException();
            }
        }

        private bool CharacteristicAccepted()
        {
            switch (Characteristic)
            {
                case Characteristics.UNKNOWN: return true;
                case Characteristics.AGE:     return GameMath.IsEvaluationConform(this, IdentifySubject().Age);
                case Characteristics.GENDER:  return EvalGender(IdentifySubject());
                case Characteristics.HEALTH:  return GameMath.IsEvaluationConform(this, IdentifySubject().HitPoints);
                case Characteristics.GOLD:    return GameMath.IsEvaluationConform(this, IdentifySubject().Gold);
                case Characteristics.RENOWN:  return GameMath.IsEvaluationConform(this, IdentifySubject().Clan.Renown);
                case Characteristics.CULTURE: return Value.ToUpper() == IdentifySubject().Culture.CultureCode.ToString();
                case null:                    throw new NullReferenceException("Characteristic is null");
                default:                      throw new ArgumentOutOfRangeException();
            }
        }


        private bool EvalGender(IHero subject)
        {
            var t = Value.ToUpper();

            if (t == "ANY") return true;
            if (subject.IsFemale) return t == "FEMALE";

            return t == "MALE";
        }

        private int GetModifierValue()
        {
            if (Value.Contains("R ")) return TalesRandom.GenerateRandomNumber(RandomStart, RandomEnd);

            return int.Parse(Value);
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
                case PartyType.UNKNOWN:       return true;
                case PartyType.DEFAULT:       return true;
                case PartyType.LORD:          return IdentifySubject().PartyBelongedTo.IsLordParty;
                case PartyType.BANDIT:        return IdentifySubject().PartyBelongedTo.IsBandit;
                case PartyType.VILLAGER:      return IdentifySubject().PartyBelongedTo.IsVillager;
                case PartyType.GARRISONPARTY: return IdentifySubject().PartyBelongedTo.IsGarrison;
                case PartyType.CARAVAN:       return IdentifySubject().PartyBelongedTo.IsCaravan;
                default:                      throw new ArgumentOutOfRangeException();
            }
        }

        private bool PersonalityTraitAccepted()
        {
            switch (PersonalityTrait)
            {
                case PersonalityTraits.UNKNOWN:    return true;
                case PersonalityTraits.MERCY:      return GameMath.IsEvaluationConform(this, IdentifySubject().Mercy);
                case PersonalityTraits.GENEROSITY: return GameMath.IsEvaluationConform(this, IdentifySubject().Generosity);
                case PersonalityTraits.HONOR:      return GameMath.IsEvaluationConform(this, IdentifySubject().Honor);
                case PersonalityTraits.VALOR:      return GameMath.IsEvaluationConform(this, IdentifySubject().Valor);
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
                case Skills.UNKNOWN:     return true;
                case Skills.ONEHANDED:   return GameMath.IsEvaluationConform(this, IdentifySubject().OneHanded);
                case Skills.TWOHANDED:   return GameMath.IsEvaluationConform(this, IdentifySubject().TwoHanded);
                case Skills.POLEARM:     return GameMath.IsEvaluationConform(this, IdentifySubject().Polearm);
                case Skills.BOW:         return GameMath.IsEvaluationConform(this, IdentifySubject().Bow);
                case Skills.CROSSBOW:    return GameMath.IsEvaluationConform(this, IdentifySubject().Crossbow);
                case Skills.THROWING:    return GameMath.IsEvaluationConform(this, IdentifySubject().Throwing);
                case Skills.RIDING:      return GameMath.IsEvaluationConform(this, IdentifySubject().Riding);
                case Skills.ATHLETICS:   return GameMath.IsEvaluationConform(this, IdentifySubject().Athletics);
                case Skills.CRAFTING:    return GameMath.IsEvaluationConform(this, IdentifySubject().Crafting);
                case Skills.SCOUTING:    return GameMath.IsEvaluationConform(this, IdentifySubject().Scouting);
                case Skills.TACTICS:     return GameMath.IsEvaluationConform(this, IdentifySubject().Tactics);
                case Skills.ROGUERY:     return GameMath.IsEvaluationConform(this, IdentifySubject().Roguery);
                case Skills.CHARM:       return GameMath.IsEvaluationConform(this, IdentifySubject().Charm);
                case Skills.LEADERSHIP:  return GameMath.IsEvaluationConform(this, IdentifySubject().Leadership);
                case Skills.TRADE:       return GameMath.IsEvaluationConform(this, IdentifySubject().Trade);
                case Skills.STEWARD:     return GameMath.IsEvaluationConform(this, IdentifySubject().Steward);
                case Skills.MEDICINE:    return GameMath.IsEvaluationConform(this, IdentifySubject().Medecine);
                case Skills.ENGINEERING: return GameMath.IsEvaluationConform(this, IdentifySubject().Engineering);
                default:                 throw new ArgumentOutOfRangeException();
            }
        }


        private bool TimeAccepted()
        {
            if (Time == GameTime.ANYTIME) return true;
            if (Time == GameTime.NONE) return true;
            if (Time == GameTime.UNKNOWN) return true;

            return Time == GameData.Instance.GameContext.GameTime;
        }

        #endregion
    }
}