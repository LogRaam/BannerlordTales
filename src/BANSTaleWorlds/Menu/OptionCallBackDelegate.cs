// Code written by Gabriel Mailhot, 03/10/2020.

#region

using System;
using System.Linq;
using TalesContract;
using TalesDAL;
using TalesEnums;
using TalesPersistence;
using TalesPersistence.Entities;
using TalesPersistence.Stories;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TraitObject = TaleWorlds.CampaignSystem.TraitObject;

#endregion

namespace TalesTaleWorlds.Menu
{
    public class OptionCallBackDelegate
    {
        private readonly Choice _choice;

        public OptionCallBackDelegate(IChoice choice)
        {
            _choice = new Choice(choice);
        }

        public bool OnConditionDelegate(MenuCallbackArgs args)
        {
            if (!IsChoiceShouldBeDisabled()) return true;

            args.Tooltip = new TextObject("condition not met"); //todo: may rebuilt eval and show it in tooltip
            args.IsEnabled = false;

            return false;
        }

        public void OnConsequenceDelegate(MenuCallbackArgs args)
        {
            foreach (var consequence in _choice.Consequences) ApplyConsequence(consequence);

            if (_choice.Triggers.Count > 0) PlayTriggers();
            else new MenuBroker().ShowCaptiveWaiting();
        }

        #region private

        private void ApplyAttributeConsequence(IEvaluation consequence)
        {
            if (consequence.Attribute == null) return;

            var value = new Evaluation(consequence).GetModifierValue();
            var hero = GetActorFrom(consequence).ToHero();

            if (consequence.Attribute is Attributes.UNKNOWN) throw new ApplicationException("consequence.Attribute unknown");

            if (consequence.Attribute is Attributes.VIGOR) SetAttribute(hero, CharacterAttributesEnum.Vigor, value);
            if (consequence.Attribute is Attributes.CONTROL) SetAttribute(hero, CharacterAttributesEnum.Control, value);
            if (consequence.Attribute is Attributes.ENDURANCE) SetAttribute(hero, CharacterAttributesEnum.Endurance, value);
            if (consequence.Attribute is Attributes.CUNNING) SetAttribute(hero, CharacterAttributesEnum.Cunning, value);
            if (consequence.Attribute is Attributes.SOCIAL) SetAttribute(hero, CharacterAttributesEnum.Social, value);
            if (consequence.Attribute is Attributes.INTELLIGENCE) SetAttribute(hero, CharacterAttributesEnum.Intelligence, value);
        }

        private void ApplyCharacteristicConsequence(IEvaluation consequence)
        {
            if (consequence.Characteristic == null) return;

            var value = new Evaluation(consequence).GetModifierValue();
            var hero = GetActorFrom(consequence).ToHero();


            if (consequence.Characteristic == Characteristics.UNKNOWN) throw new ApplicationException("consequence.Characteristic unknown");

            if (consequence.Characteristic == Characteristics.HEALTH) hero.HitPoints += value;
            if (consequence.Characteristic == Characteristics.GOLD) hero.Gold += value;
            if (consequence.Characteristic == Characteristics.RENOWN) hero.Clan.Renown += value;
        }

        private void ApplyConsequence(IEvaluation consequence)
        {
            //TODO: I should show a message with each applied consequence.
            ApplyPregnancyRiskConsequence(consequence);
            ApplyAttributeConsequence(consequence);
            ApplyCharacteristicConsequence(consequence);
            ApplyPersonalityTraitConsequence(consequence);
            ApplySkillConsequence(consequence);
        }

        private void ApplyPersonalityTraitConsequence(IEvaluation consequence)
        {
            if (consequence.PersonalityTrait == null) return;

            var value = new Evaluation(consequence).GetModifierValue();
            var hero = GetActorFrom(consequence).ToHero();

            if (consequence.PersonalityTrait == PersonalityTraits.MERCY) hero.SetTraitLevel(TraitObject.FindFirst(n => n.StringId.ToUpper() == "MERCY"), hero.GetHeroTraits().Mercy + value);
            if (consequence.PersonalityTrait == PersonalityTraits.GENEROSITY) hero.SetTraitLevel(TraitObject.FindFirst(n => n.StringId.ToUpper() == "GENEROSITY"), hero.GetHeroTraits().Generosity + value);
            if (consequence.PersonalityTrait == PersonalityTraits.HONOR) hero.SetTraitLevel(TraitObject.FindFirst(n => n.StringId.ToUpper() == "HONOR"), hero.GetHeroTraits().Honor + value);
            if (consequence.PersonalityTrait == PersonalityTraits.VALOR) hero.SetTraitLevel(TraitObject.FindFirst(n => n.StringId.ToUpper() == "VALOR"), hero.GetHeroTraits().Valor + value);
        }

        private void ApplyPregnancyRiskConsequence(IEvaluation consequence)
        {
            if (!consequence.PregnancyRisk) return;

            if (consequence.ValueIsPercentage)
            {
                if (TalesRandom.EvalPercentage(int.Parse(consequence.Value))) MakePregnant(consequence);

                return;
            }

            if (string.IsNullOrEmpty(consequence.Value) && consequence.RandomEnd > 0)
                if (TalesRandom.EvalPercentageRange(consequence.RandomStart, consequence.RandomEnd))
                    MakePregnant(consequence);
        }

        private void ApplySkillConsequence(IEvaluation consequence)
        {
            if (consequence.Skill == null) return;

            var value = new Evaluation(consequence).GetModifierValue();
            var hero = GetActorFrom(consequence).ToHero();

            SetSkill(hero, consequence.Skill.ToString(), value);
        }


        private Hero GetActorFrom(IEvaluation consequence)
        {
            var actor = consequence.Subject == Actor.NPC
                ? new Hero(GameData.Instance.GameContext.Captor)
                : new Hero(GameData.Instance.GameContext.Player);

            return actor;
        }


        private bool IsChoiceShouldBeDisabled()
        {
            var disabled = false;
            foreach (var condition in _choice.Conditions)
            {
                var eval = new Evaluation(condition);

                if (eval.IsAccepted()) continue;

                disabled = true;

                break;
            }

            return disabled;
        }

        private void MakePregnant(IEvaluation consequence)
        {
            var actor = GetActorFrom(consequence);

            if (!actor.IsPregnant) MakePregnantAction.Apply(actor.ToHero());
        }

        private void PlayHighestChanceToTrigger()
        {
            var t = _choice.Triggers[0];
            foreach (var trigger in _choice.Triggers)
            {
                if (trigger.ChanceToTrigger <= t.ChanceToTrigger) continue;

                t = trigger;
            }

            GameMenu.ActivateGameMenu(t.Link);
        }

        private void PlayTriggers()
        {
            var interval = _choice.Triggers.Sum(trigger => trigger.ChanceToTrigger);

            foreach (var trigger in _choice.Triggers)
            {
                if (!TalesRandom.EvalPercentage((trigger.ChanceToTrigger * interval) / 100)) continue;

                new MenuBroker().ShowMenuFor(trigger.Link);

                return;
            }

            PlayHighestChanceToTrigger();
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

        #endregion
    }
}