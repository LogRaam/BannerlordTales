// Code written by Gabriel Mailhot, 03/10/2020.

#region

using System;
using TalesContract;
using TalesDAL;
using TalesEnums;
using TalesPersistence;
using TalesPersistence.Entities;
using TalesPersistence.Stories;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Localization;

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


            //TODO: I must open new menu if there is a trigger.  May have to rand if multiple triggers.


            UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[13] = GameData.Instance.StoryContext.BackgroundImages.TextureList["LogCaptivePrisoner"];
            GameMenu.ExitToLast();
            new GameFunction().UnPauseGame();
        }

        #region private

        private void ApplyAttributeConsequence(IEvaluation consequence)
        {
            if (consequence.Attribute == null) return;

            var value = new Evaluation(consequence).GetModifierValue();
            var hero = GetActorFrom(consequence).ToHero();

            if (consequence.Attribute is Attributes.UNKNOWN) throw new ApplicationException("consequence.Attribute unknown: " + consequence.Attribute);

            if (consequence.Attribute is Attributes.VIGOR) SetAttribute(hero, CharacterAttributesEnum.Vigor, value);
            if (consequence.Attribute is Attributes.CONTROL) SetAttribute(hero, CharacterAttributesEnum.Vigor, value);
            if (consequence.Attribute is Attributes.ENDURANCE) SetAttribute(hero, CharacterAttributesEnum.Vigor, value);
            if (consequence.Attribute is Attributes.CUNNING) SetAttribute(hero, CharacterAttributesEnum.Vigor, value);
            if (consequence.Attribute is Attributes.SOCIAL) SetAttribute(hero, CharacterAttributesEnum.Vigor, value);
            if (consequence.Attribute is Attributes.INTELLIGENCE) SetAttribute(hero, CharacterAttributesEnum.Vigor, value);
        }

        private void ApplyCharacteristicConsequence(IEvaluation consequence)
        {
            if (consequence.Attribute == null) return;

            throw new NotImplementedException();
        }

        private void ApplyConsequence(IEvaluation consequence)
        {
            ApplyPregnancyRiskConsequence(consequence);
            ApplyAttributeConsequence(consequence);
            ApplyCharacteristicConsequence(consequence);
            ApplyPersonalityTraitConsequence(consequence);
            ApplySkillConsequence(consequence);
        }

        private void ApplyPersonalityTraitConsequence(IEvaluation consequence)
        {
            if (consequence.Attribute == null) return;

            throw new NotImplementedException();
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
            if (consequence.Attribute == null) return;

            throw new NotImplementedException();
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

        private void SetAttribute(TaleWorlds.CampaignSystem.Hero hero, CharacterAttributesEnum attribute, int value)
        {
            hero.SetAttributeValue(attribute, hero.GetAttributeValue(attribute) + value);
        }

        #endregion
    }
}