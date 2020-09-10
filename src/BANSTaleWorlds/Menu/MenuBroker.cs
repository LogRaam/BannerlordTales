// Code written by Gabriel Mailhot, 12/08/2020.

#region

using TalesContract;
using TalesEnums;
using TaleWorlds.CampaignSystem.GameMenus;

#endregion

namespace TalesTaleWorlds.Menu
{
   #region

   #endregion

   public class MenuBroker
   {
      public void ShowMenuFor(IEventRequest storyAction)
      {
         if (storyAction == null) return;

         if (storyAction.StoryType == StoryType.PLAYER_IS_CAPTIVE)
            if (storyAction.Action == StoryAction.START)
            {
               // GameMenu.SwitchToMenu("menuId");
               new GameFunction().PauseGame();
               GameMenu.ActivateGameMenu("menuId");
            }
      }
   }
}