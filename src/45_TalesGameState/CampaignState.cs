// Code written by Gabriel Mailhot, 14/09/2020.

#region

#endregion

#region

using TaleWorlds.CampaignSystem;

#endregion

namespace _45_TalesGameState
{
    public static class CampaignState
    {
        public static bool CurrentGameStarted()
        {
            return Campaign.Current != null && Campaign.Current.GameStarted;
        }
    }
}