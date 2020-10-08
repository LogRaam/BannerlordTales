// Code written by Gabriel Mailhot, 11/09/2020.

#region

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

#endregion

namespace TalesTaleWorlds
{
    #region

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