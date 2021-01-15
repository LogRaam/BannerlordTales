// unset

#region

using _45_TalesGameState;
using _47_TalesMath;
using Helpers;
using System;
using System.Collections.Generic;
using TalesBase.Stories.Evaluation;
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
            Persona = condition.Persona;
            Numbers = condition.Numbers;
            Outcome = condition.Outcome;
            Equipments = condition.Equipments;
            PartyType = condition.PartyType;
        }

        public Evaluation() { }


        public void ApplyConsequenceInGame()
        {
            //BUG:Renown doesnt seems to work
            ApplyPregnancyRiskConsequence();
            ApplyAttributeConsequence();
            ApplyCharacteristicConsequence();
            ApplyPersonalityTraitConsequence();
            ApplySkillConsequence();
            ApplyEscapeConsequence();
            ApplyEquipmentConsequence();
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
            if (Persona.Attribute == null) return;
            if (Persona.Attribute is Attributes.UNKNOWN) return;

            var value = GetModifierValue();
            var hero = IdentifySubject().ToTwHero();

            if (Persona.Attribute is Attributes.VIGOR) SetAttribute(hero, CharacterAttributesEnum.Vigor, value);
            if (Persona.Attribute is Attributes.CONTROL) SetAttribute(hero, CharacterAttributesEnum.Control, value);
            if (Persona.Attribute is Attributes.ENDURANCE) SetAttribute(hero, CharacterAttributesEnum.Endurance, value);
            if (Persona.Attribute is Attributes.CUNNING) SetAttribute(hero, CharacterAttributesEnum.Cunning, value);
            if (Persona.Attribute is Attributes.SOCIAL) SetAttribute(hero, CharacterAttributesEnum.Social, value);
            if (Persona.Attribute is Attributes.INTELLIGENCE) SetAttribute(hero, CharacterAttributesEnum.Intelligence, value);
        }

        private void ApplyCharacteristicConsequence()
        {
            if (Persona.Characteristic == null) return;
            if (Persona.Characteristic == Characteristics.UNKNOWN) return;

            var value = GetModifierValue();
            var hero = IdentifySubject().ToTwHero();

            if (Persona.Characteristic == Characteristics.HEALTH) hero.HitPoints += value;
            if (Persona.Characteristic == Characteristics.GOLD) hero.Gold += value;
            if (Persona.Characteristic == Characteristics.RENOWN) hero.Clan.Renown += value;
        }

        private void ApplyEquipmentConsequence()
        {
            if (Outcome.ShouldUndress) ApplyShouldUndress();
            if (Outcome.ShouldEquip) ApplyShouldEquip();
        }

        private void ApplyEscapeConsequence()
        {
            if (!Outcome.Escaping) return;

            var p = IdentifySubject().ToTwHero();

            if (!p.IsPrisoner) return;

            if (p.IsHumanPlayerCharacter) PlayerCaptivity.EndCaptivity();
            else SetPrisonerFreeAction.Apply(p, new Hero(GameData.Instance.GameContext.Heroes.Player).ToTwHero());
        }

        private void ApplyPersonalityTraitConsequence()
        {
            if (Persona.PersonalityTrait == null) return;
            if (Persona.PersonalityTrait == PersonalityTraits.UNKNOWN) return;

            var value = GetModifierValue();
            var hero = IdentifySubject().ToTwHero();

            if (Persona.PersonalityTrait == PersonalityTraits.MERCY) hero.SetTraitLevel(TraitObject.FindFirst(n => n.StringId.ToUpper() == "MERCY"), hero.GetHeroTraits().Mercy + value);
            if (Persona.PersonalityTrait == PersonalityTraits.GENEROSITY) hero.SetTraitLevel(TraitObject.FindFirst(n => n.StringId.ToUpper() == "GENEROSITY"), hero.GetHeroTraits().Generosity + value);
            if (Persona.PersonalityTrait == PersonalityTraits.HONOR) hero.SetTraitLevel(TraitObject.FindFirst(n => n.StringId.ToUpper() == "HONOR"), hero.GetHeroTraits().Honor + value);
            if (Persona.PersonalityTrait == PersonalityTraits.VALOR) hero.SetTraitLevel(TraitObject.FindFirst(n => n.StringId.ToUpper() == "VALOR"), hero.GetHeroTraits().Valor + value);
        }


        private void ApplyPregnancyRiskConsequence()
        {
            if (!Outcome.PregnancyRisk) return;

            var age = GameData.Instance.GameContext.Heroes.Player.Age;

            if (age < 12) return;


            if (Numbers.ValueIsPercentage)
            {
                if (TalesRandom.EvalPercentage(int.Parse(Numbers.Value))) MakePregnant();

                return;
            }

            if (string.IsNullOrEmpty(Numbers.Value) && Numbers.RandomEnd > 0)
                if (TalesRandom.EvalPercentageRange(Numbers.RandomStart, Numbers.RandomEnd))
                {
                    MakePregnant();

                    return;
                }


            if (age < 30)
            {
                if (TalesRandom.EvalPercentage(15)) MakePregnant();

                return;
            }

            if (TalesRandom.EvalPercentage(15 - (age * 0.25f))) MakePregnant();
        }

        private void ApplyShouldEquip()
        {
            throw new NotImplementedException();
        }

        private void ApplyShouldUndress()
        {
            if (CampaignState.CurrentGameStarted()) EquipmentHelper.AssignHeroEquipmentFromEquipment(new Hero(GameData.Instance.GameContext.Heroes.Player).ToTwHero(), new Equipment(true));
            else GameData.Instance.GameContext.Heroes.Player.Equipments = new List<IEquipments>();
        }

        private void ApplySkillConsequence()
        {
            if (Persona.Skill == null) return;
            if (Persona.Skill == Skills.UNKNOWN) return;

            var value = GetModifierValue();
            var hero = IdentifySubject().ToTwHero();

            SetSkill(hero, Persona.Skill.ToString(), value);
        }

        private bool AttributeAccepted()
        {
            switch (Persona.Attribute)
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
            switch (Persona.Characteristic)
            {
                case Characteristics.UNKNOWN: return true;
                case Characteristics.AGE:     return GameMath.IsEvaluationConform(this, IdentifySubject().Age);
                case Characteristics.GENDER:  return EvalGender(IdentifySubject());
                case Characteristics.HEALTH:  return GameMath.IsEvaluationConform(this, IdentifySubject().HitPoints);
                case Characteristics.GOLD:    return GameMath.IsEvaluationConform(this, IdentifySubject().Gold);
                case Characteristics.RENOWN:  return GameMath.IsEvaluationConform(this, IdentifySubject().Clan.Renown);
                case Characteristics.CULTURE: return Numbers.Value.ToUpper() == IdentifySubject().Culture.CultureCode.ToString();
                case null:                    throw new NullReferenceException("Characteristic is null");
                default:                      throw new ArgumentOutOfRangeException();
            }
        }


        private bool EvalGender(IHero subject)
        {
            var t = Numbers.Value.ToUpper();

            if (t == "ANY") return true;
            if (subject.IsFemale) return t == "FEMALE";

            return t == "MALE";
        }

        private int GetModifierValue()
        {
            if (Numbers.Value.Contains("R ")) return TalesRandom.GenerateRandomNumber(Numbers.RandomStart, Numbers.RandomEnd);

            return int.Parse(Numbers.Value);
        }

        private Hero IdentifySubject()
        {
            return Persona.Subject == Actor.NPC
                ? new Hero(GameData.Instance.GameContext.Heroes.Captor)
                : new Hero(GameData.Instance.GameContext.Heroes.Player);
        }

        private void MakePregnant()
        {
            var actor = IdentifySubject();

            if (actor.IsPregnant) return;
            if (!actor.IsFemale) return;
            if (!actor.IsAlive) return;
            if (!actor.IsFertile) return;

            GameData.Instance.GameContext.Heroes.MakePregnant(actor);
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
            switch (Persona.PersonalityTrait)
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
            switch (Persona.Skill)
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

            return Time == GameData.Instance.GameContext.Time.GameTime;
        }

        #endregion
    }
}