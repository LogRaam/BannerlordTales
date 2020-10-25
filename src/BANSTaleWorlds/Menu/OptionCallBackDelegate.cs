﻿// Code written by Gabriel Mailhot, 03/10/2020.

#region

using System.Linq;
using TalesContract;
using TalesDAL;
using TalesPersistence.Entities;
using TalesPersistence.Stories;
using TaleWorlds.CampaignSystem.GameMenus;
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
            foreach (var consequence in _choice.Consequences) new Evaluation(consequence).ApplyConsequenceInGame();

            if (_choice.Triggers.Count > 0) PlayTriggers();
            else new MenuBroker().ExitToCaptiveWaitingMenu();
        }

        #region private

        private bool IsChoiceShouldBeDisabled()
        {
            var disabled = false;
            foreach (var condition in _choice.Conditions)
            {
                var eval = new Evaluation(condition);

                if (eval.CanBePlayedInContext()) continue;

                disabled = true;

                break;
            }

            return disabled;
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

        #endregion
    }
}