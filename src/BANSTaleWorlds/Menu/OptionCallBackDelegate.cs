// Code written by Gabriel Mailhot, 03/10/2020.

#region

using TalesContract;
using TalesPersistence;
using TalesPersistence.Stories;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.Engine.GauntletUI;

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
            //if (_choice.Triggers.Count == 0) args.optionLeaveType = GameMenuOption.LeaveType.Continue;


            return true;
        }

        public void OnConsequenceDelegate(MenuCallbackArgs args)
        {
            UIResourceManager.SpriteData.SpriteCategories["ui_fullbackgrounds"].SpriteSheets[13] = GameData.Instance.StoryContext.BackgroundImages.TextureList["LogCaptivePrisoner"];
            GameMenu.ExitToLast();
            new GameFunction().UnPauseGame();
        }
    }
}