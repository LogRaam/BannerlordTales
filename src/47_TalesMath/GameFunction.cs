// Code written by Gabriel Mailhot, 02/12/2023.

#region

using System;
using System.Diagnostics;
using TaleWorlds.MountAndBlade;

#endregion

namespace _47_TalesMath
{
    #region

    #endregion

    public class GameFunction
    {
        private static readonly bool IsDebug = true;

        public static void Log(string message)
        {
            if (!IsDebug) return;

            Debug.WriteLine(message);
        }

        public static void LogMethod(string name, Guid id)
        {
            Log(name + " [" + id + "]");
        }

        public static void LogMethod(string name, Guid id, string action)
        {
            Log(name + " [" + id + "] => " + action);
        }

        public void PauseGame()
        {
            if (MBCommon.IsPaused) return;

            MBCommon.PauseGameEngine();
            //Game.Current.GameStateManager.ActiveStateDisabledByUser = true;
        }

        public void UnPauseGame()
        {
            if (!MBCommon.IsPaused) return;

            MBCommon.UnPauseGameEngine();
            //Game.Current.GameStateManager.ActiveStateDisabledByUser = false;
        }
    }
}