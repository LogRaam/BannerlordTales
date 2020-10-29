// Code written by Gabriel Mailhot, 01/10/2020.

#region

using System;
using _47_TalesMath;
using TalesContract;
using TalesPersistence.Stories;
using TaleWorlds.CampaignSystem.GameMenus;

#endregion

namespace TalesRuntime.Menu
{
    public class MenuCallBackDelegate
    {
        private readonly IAct _act;


        public MenuCallBackDelegate(IAct act)
        {
            _act = act;
        }

        public void ActMenuSetup(MenuCallbackArgs args)
        {
            GameFunction.Log("ActMenuSetup(MenuCallbackArgs args) args => " + args.MenuContext.GameMenu.StringId);
            //args.MenuContext.GameMenu.AllowWaitingAutomatically();
            //args.MenuContext.GameMenu.SetMenuAsWaitMenuAndInitiateWaiting();

            GameFunction.Log("... call => SetBackgroundImage(_act.Image) Image => " + _act.Image);
            new MenuBroker().SetBackgroundImage(_act.Image);
            //args.MenuContext.SetBackgroundMeshName(_act.Image);

            //TODO: Replace string flags by values (stringbuilder)
        }

        public int Index(string choiceId)
        {
            return -1;
        }

        public bool IsLeave(string choiceId)
        {
            var c = RetrieveChoice(choiceId);

            return c.Triggers.Count == 0;
        }

        public bool IsRepeatable(string choiceId)
        {
            return false;
        }

        #region private

        private Choice RetrieveChoice(string choiceId)
        {
            foreach (var choice in _act.Choices)
                if (choice.Id == choiceId)
                    return new Choice(choice);

            throw new Exception("ERROR: Choice not found!");
        }

        #endregion
    }
}