// Code written by Gabriel Mailhot, 03/10/2020.

#region

using System;
using TalesContract;
using TalesPersistence;
using TalesPersistence.Entities;
using TalesPersistence.Stories;
using TaleWorlds.CampaignSystem.GameMenus;
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
            //TODO: Must test conditions.  If conditions not met, should disable this option.
            foreach (var condition in _choice.Conditions)
            {
                var eval = new Evaluation(condition);
                if (eval.IsAccepted())
                {
                    throw new NotImplementedException();
                }
            }

            var disabled = false;
            if (disabled)
            {
                args.Tooltip = new TextObject("reason why it's disabled");
                args.IsEnabled = false;
            }

            return true;
        }

        public void OnConsequenceDelegate(MenuCallbackArgs args)
        {
            //TODO: Must apply consequences.
            //TODO: I must open new menu if there is a trigger.  May have to rand if multiple triggers.


            UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[13] = GameData.Instance.StoryContext.BackgroundImages.TextureList["LogCaptivePrisoner"];
            GameMenu.ExitToLast();
            new GameFunction().UnPauseGame();
        }
    }
}