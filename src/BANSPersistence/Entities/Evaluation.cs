// Code written by Gabriel Mailhot, 02/12/2023.

#region

using _45_TalesGameState;
using _47_TalesMath;
using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using TalesBase.Stories.Evaluation;
using TalesContract;
using TalesDAL;
using TalesEnums;
using TalesPersistence.Context;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
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
            //ApplyAttributeConsequence(); //BUG: I don<t know how to change Attribute anymore.
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
            if (Persona.Attribute is Attributes.NotAssigned) return;

            var value = GetModifierValue();
            var hero = IdentifySubject().ToTwHero();

            if (Persona.Attribute is Attributes.Vigor) SetAttribute(hero, CharacterAttributesEnum.Vigor, value);
            if (Persona.Attribute is Attributes.Control) SetAttribute(hero, CharacterAttributesEnum.Control, value);
            if (Persona.Attribute is Attributes.Endurance) SetAttribute(hero, CharacterAttributesEnum.Endurance, value);
            if (Persona.Attribute is Attributes.Cunning) SetAttribute(hero, CharacterAttributesEnum.Cunning, value);
            if (Persona.Attribute is Attributes.Social) SetAttribute(hero, CharacterAttributesEnum.Social, value);
            if (Persona.Attribute is Attributes.Intelligence) SetAttribute(hero, CharacterAttributesEnum.Intelligence, value);
        }

        private void ApplyCharacteristicConsequence()
        {
            if (Persona.Characteristic == null) return;
            if (Persona.Characteristic == Characteristics.NotAssigned) return;

            var value = GetModifierValue();
            var hero = IdentifySubject().ToTwHero();

            if (Persona.Characteristic == Characteristics.Health) hero.HitPoints += value;
            if (Persona.Characteristic == Characteristics.Gold) hero.Gold += value;
            if (Persona.Characteristic == Characteristics.Renown) hero.Clan.Renown += value;
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
            else EndCaptivityAction.ApplyByEscape(new Hero(GameData.Instance.GameContext.Heroes.Player).ToTwHero()); //SetPrisonerFreeAction.Apply(p, new Hero(GameData.Instance.GameContext.Heroes.Player).ToTwHero());
        }

        private void ApplyPersonalityTraitConsequence()
        {
            if (Persona.PersonalityTrait == null) return;
            if (Persona.PersonalityTrait == PersonalityTraits.NotAssigned) return;

            var value = GetModifierValue();
            var hero = IdentifySubject().ToTwHero();

            if (Persona.PersonalityTrait == PersonalityTraits.Mercy) hero.SetTraitLevel(TraitObject.All.First(n => n.StringId.ToUpper() == "MERCY"), hero.GetHeroTraits().Mercy + value);
            if (Persona.PersonalityTrait == PersonalityTraits.Generosity) hero.SetTraitLevel(TraitObject.All.First(n => n.StringId.ToUpper() == "GENEROSITY"), hero.GetHeroTraits().Generosity + value);
            if (Persona.PersonalityTrait == PersonalityTraits.Honor) hero.SetTraitLevel(TraitObject.All.First(n => n.StringId.ToUpper() == "HONOR"), hero.GetHeroTraits().Honor + value);
            if (Persona.PersonalityTrait == PersonalityTraits.Valor) hero.SetTraitLevel(TraitObject.All.First(n => n.StringId.ToUpper() == "VALOR"), hero.GetHeroTraits().Valor + value);
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
            if (Persona.Skill == Skills.NotAssigned) return;

            var value = GetModifierValue();
            var hero = IdentifySubject().ToTwHero();

            SetSkill(hero, Persona.Skill.ToString(), value);
        }

        private bool AttributeAccepted()
        {
            switch (Persona.Attribute)
            {
                case Attributes.NotAssigned: return true;
                case Attributes.Vigor: return GameMath.IsEvaluationConform(this, IdentifySubject().Vigor);
                case Attributes.Control: return GameMath.IsEvaluationConform(this, IdentifySubject().Control);
                case Attributes.Endurance: return GameMath.IsEvaluationConform(this, IdentifySubject().Endurance);
                case Attributes.Cunning: return GameMath.IsEvaluationConform(this, IdentifySubject().Cunning);
                case Attributes.Social: return GameMath.IsEvaluationConform(this, IdentifySubject().Social);
                case Attributes.Intelligence: return GameMath.IsEvaluationConform(this, IdentifySubject().Intelligence);
                case null: throw new NullReferenceException("Attribute is null");
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private bool CharacteristicAccepted()
        {
            switch (Persona.Characteristic)
            {
                case Characteristics.NotAssigned: return true;
                case Characteristics.Age: return GameMath.IsEvaluationConform(this, IdentifySubject().Age);
                case Characteristics.Gender: return EvalGender(IdentifySubject());
                case Characteristics.Health: return GameMath.IsEvaluationConform(this, IdentifySubject().HitPoints);
                case Characteristics.Gold: return GameMath.IsEvaluationConform(this, IdentifySubject().Gold);
                case Characteristics.Renown: return GameMath.IsEvaluationConform(this, IdentifySubject().Clan.Renown);
                case Characteristics.Culture:
                    {
                        var b = Numbers.Value.Reformat();
                        var c = IdentifySubject().Culture.CultureCode.ToString();

                        return b == c;
                    }
                case null: throw new NullReferenceException("Characteristic is null");
                default: throw new ArgumentOutOfRangeException();
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
            return Persona.Subject == Actor.Npc
                ? new Hero(GameData.Instance.GameContext.Heroes.Captor)
                : new Hero(GameData.Instance.GameContext.Heroes.Player);
        }

        private void MakePregnant()
        {
            var actor = IdentifySubject();

            if (actor.IsPregnant) return;
            if (!actor.IsFemale) return;
            if (!actor.IsAlive) return;

            GameData.Instance.GameContext.Heroes.MakePregnant(actor);
        }


        private bool PartyTypeAccepted()
        {
            switch (PartyType)
            {
                case PartyType.Unknown: return true;
                case PartyType.Default: return true;

                default: throw new ArgumentOutOfRangeException();
            }
        }

        private bool PersonalityTraitAccepted()
        {
            switch (Persona.PersonalityTrait)
            {
                case PersonalityTraits.NotAssigned: return true;
                case PersonalityTraits.Mercy: return GameMath.IsEvaluationConform(this, IdentifySubject().Mercy);
                case PersonalityTraits.Generosity: return GameMath.IsEvaluationConform(this, IdentifySubject().Generosity);
                case PersonalityTraits.Honor: return GameMath.IsEvaluationConform(this, IdentifySubject().Honor);
                case PersonalityTraits.Valor: return GameMath.IsEvaluationConform(this, IdentifySubject().Valor);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private void SetAttribute(TaleWorlds.CampaignSystem.Hero hero, CharacterAttributesEnum attribute, int value)
        {
            //hero.SetAttributeValue(attribute, hero.GetAttributeValue(attribute) + value); //Deprecated in v1.1.0
        }


        private void SetSkill(TaleWorlds.CampaignSystem.Hero hero, string skill, int value)
        {
            //var s = SkillObject.FindFirst(n => n.StringId.ToUpper() == skill);
            var s = new SkillObject(skill);
            hero.SetSkillValue(s, hero.GetSkillValue(s) + value);
        }


        private bool SkillAccepted()
        {
            switch (Persona.Skill)
            {
                case Skills.NotAssigned: return true;
                case Skills.OneHanded: return GameMath.IsEvaluationConform(this, IdentifySubject().OneHanded);
                case Skills.TwoHanded: return GameMath.IsEvaluationConform(this, IdentifySubject().TwoHanded);
                case Skills.Polearm: return GameMath.IsEvaluationConform(this, IdentifySubject().Polearm);
                case Skills.Bow: return GameMath.IsEvaluationConform(this, IdentifySubject().Bow);
                case Skills.Crossbow: return GameMath.IsEvaluationConform(this, IdentifySubject().Crossbow);
                case Skills.Throwing: return GameMath.IsEvaluationConform(this, IdentifySubject().Throwing);
                case Skills.Riding: return GameMath.IsEvaluationConform(this, IdentifySubject().Riding);
                case Skills.Athletics: return GameMath.IsEvaluationConform(this, IdentifySubject().Athletics);
                case Skills.Crafting: return GameMath.IsEvaluationConform(this, IdentifySubject().Crafting);
                case Skills.Scouting: return GameMath.IsEvaluationConform(this, IdentifySubject().Scouting);
                case Skills.Tactics: return GameMath.IsEvaluationConform(this, IdentifySubject().Tactics);
                case Skills.Roguery: return GameMath.IsEvaluationConform(this, IdentifySubject().Roguery);
                case Skills.Charm: return GameMath.IsEvaluationConform(this, IdentifySubject().Charm);
                case Skills.Leadership: return GameMath.IsEvaluationConform(this, IdentifySubject().Leadership);
                case Skills.Trade: return GameMath.IsEvaluationConform(this, IdentifySubject().Trade);
                case Skills.Steward: return GameMath.IsEvaluationConform(this, IdentifySubject().Steward);
                case Skills.Medicine: return GameMath.IsEvaluationConform(this, IdentifySubject().Medicine);
                case Skills.Engineering: return GameMath.IsEvaluationConform(this, IdentifySubject().Engineering);
                default: throw new ArgumentOutOfRangeException();
            }
        }


        private bool TimeAccepted()
        {
            if (Time == GameTime.Anytime) return true;
            if (Time == GameTime.None) return true;

            return Time == GameData.Instance.GameContext.Time.GameTime;
        }

        #endregion
    }
}