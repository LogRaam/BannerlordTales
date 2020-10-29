// Code written by Gabriel Mailhot, 03/10/2020.

#region

using System.Linq;
using _47_TalesMath;
using TalesContract;
using TalesDAL;
using TalesPersistence.Entities;
using TalesPersistence.Stories;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.Localization;

#endregion

namespace TalesRuntime.Menu
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
            GameFunction.Log("OnConditionDelegate(MenuCallbackArgs args) args => " + args.MenuContext.GameMenu.StringId + ", image => " + args.MenuContext.CurrentBackgroundMeshName);

            if (!IsChoiceShouldBeDisabled()) return true;

            args.Tooltip = new TextObject("condition not met"); //todo: may rebuilt eval and show it in tooltip
            args.IsEnabled = false;

            return false;
        }

        public void OnConsequenceDelegate(MenuCallbackArgs args)
        {
            GameFunction.Log("OnConsequenceDelegate(MenuCallbackArgs args) args => " + args.MenuContext.GameMenu.StringId + ", image => " + args.MenuContext.CurrentBackgroundMeshName);

            foreach (var consequence in _choice.Consequences) new Evaluation(consequence).ApplyConsequenceInGame();

            if (_choice.Triggers.Count > 0)
            {
                GameFunction.Log(".. triggers count > 0, call => PlayTriggers()");
                PlayTriggers();
            }
            else
            {
                GameFunction.Log(".. call => ExitToCaptiveWaitingMenu()");
                args.optionLeaveType = GameMenuOption.LeaveType.Leave;
                new MenuBroker().ExitToCaptiveWaitingMenu();
            }
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
            GameFunction.Log("PlayHighestChanceToTrigger()...");

            var t = _choice.Triggers[0];
            foreach (var trigger in _choice.Triggers)
            {
                if (trigger.ChanceToTrigger < t.ChanceToTrigger) continue;

                t = trigger;
            }

            GameFunction.Log("... call => GotoMenuFor(t.Link) link => " + t.Link);
            new MenuBroker().GotoMenuFor(t.Link);
        }

        private void PlayTriggers()
        {
            GameFunction.Log("PlayTriggers()...");

            var interval = _choice.Triggers.Sum(trigger => trigger.ChanceToTrigger);

            foreach (var trigger in _choice.Triggers)
            {
                var test = TalesRandom.EvalPercentage((trigger.ChanceToTrigger * interval) / 100);

                if (!test) continue;

                GameFunction.Log("PlayTriggers() link => " + trigger.Link);
                new MenuBroker().GotoMenuFor(trigger.Link);

                return;
            }

            GameFunction.Log("...  call => PlayHighestChanceToTrigger()");
            PlayHighestChanceToTrigger();
        }

        #endregion
    }
}