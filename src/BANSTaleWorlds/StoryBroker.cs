// Code written by Gabriel Mailhot, 05/08/2020.

#region

using System;
using TalesContract;
using TalesPersistence;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using Location = TalesEnums.Location;

#endregion

namespace TalesTaleWorlds
{
   #region

   #endregion

   public class StoryBroker
   {
      public void AddStoriesToGame(CampaignGameStarter gameStarter)
      {
         //this.TestEvent(gameStarter);

         foreach (IStory story in GameData.Instance.StoryContext.Stories)
            CreateGameMenuFor(gameStarter, story);
      }

      #region private

      private bool Condition(MenuCallbackArgs args)
      {
         // todo
         return true;
      }

      private void Consequence(MenuCallbackArgs args)
      {
         // todo
         InformationManager.AddHintInformation("This is a hint information!");
         InformationManager.AddQuickInformation(new TextObject("This is a quick info"));
         InformationManager.DisplayMessage(new InformationMessage("This is an information", new Color(255, 0, 0)));
         GameMenu.ExitToLast();
         new GameFunction().UnPauseGame();
      }

      private void CreateGameMenuFor(CampaignGameStarter gameStarter, IStory story)
      {
         if (string.IsNullOrEmpty(story.Id)) story.Id = Guid.NewGuid().ToString();
         foreach (IAct act in story.Acts)
         {
            switch (act.Location)
            {
               case Location.UNKNOWN:
                  break;

               case Location.MAP:
                  break;

               case Location.SETTLEMENT:
                  break;

               case Location.VILLAGE:
                  break;

               case Location.DUNGEON:
                  break;

               case Location.CASTLE:
                  break;

               case Location.FORTIFICATION:
                  break;

               case Location.TOWN:
                  break;

               case Location.HIDEOUT:
                  break;

               default:
                  throw new ArgumentOutOfRangeException();
            }

            gameStarter.AddGameMenu(story.Id + act.Name, act.Intro, MyInitDelegate, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.none, "BANS");
         }
      }

      private void MyInitDelegate(MenuCallbackArgs args)
      {
         args.MenuContext.GameMenu.AllowWaitingAutomatically();
         args.MenuContext.GameMenu.SetMenuAsWaitMenuAndInitiateWaiting();
      }

      private void TestEvent(CampaignGameStarter gameStarter)
      {
         gameStarter.AddGameMenu("menuId", "You have surrendered to your aggressor.", MyInitDelegate, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.none, "BANS");

         gameStarter.AddGameMenuOption("menuId", "optionId", "You can choose this option...", Condition, Consequence, false, 0);
      }

      #endregion
   }
}