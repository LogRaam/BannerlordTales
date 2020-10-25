// Code written by Gabriel Mailhot, 24/10/2020.

#region

using System.Diagnostics;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

#endregion

namespace TalesPersistence
{
    #region

    #endregion

    public class GameFunction
    {
        public static void Log(string message)
        {
            if (!GameData.Instance.IsDebug) return;

            Debug.WriteLine(message);
        }

        public void PauseGame()
        {
            if (MBCommon.IsPaused) return;

            MBCommon.PauseGameEngine();
            Game.Current.GameStateManager.ActiveStateDisabledByUser = true;
        }

        public void UnPauseGame()
        {
            if (!MBCommon.IsPaused) return;

            MBCommon.UnPauseGameEngine();
            Game.Current.GameStateManager.ActiveStateDisabledByUser = false;
        }
    }
}