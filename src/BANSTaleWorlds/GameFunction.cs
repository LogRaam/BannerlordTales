// Code written by Gabriel Mailhot, 12/08/2020.

namespace TalesTaleWorlds
{
   #region

   using TaleWorlds.Core;
   using TaleWorlds.MountAndBlade;

   #endregion

   public class GameFunction
   {
      public void PauseGame()
      {
         MBCommon.PauseGameEngine();
         Game.Current.GameStateManager.ActiveStateDisabledByUser = true;
      }

      public void UnPauseGame()
      {
         MBCommon.UnPauseGameEngine();
         Game.Current.GameStateManager.ActiveStateDisabledByUser = false;
      }
   }
}